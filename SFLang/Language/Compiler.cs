using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using Newtonsoft.Json;
using SFLang.Exceptions;
using SFLang.Lexicon;
using SharpFunction.API;

using Files = System.IO.File;
using Formatting = Newtonsoft.Json.Formatting;

namespace SFLang.Language
{
    internal class TokenContainable
    {
        [JsonProperty("asm-name")] public string AssemblyName { get; set; }
        [JsonProperty("asm-ver")] public Dictionary<string, string> AssemblerVersion { get; set; }
        [JsonProperty("cmp")] public string Compiler { get; set; }
        [JsonProperty("tks")] public IEnumerable<string> Tokens { get; set; }
    }
    
    public static class Compiler
    {
        public static Lambda<Lambdas.Unit> ReadAssembly(string path, string name)
        {
            ZipFile.ExtractToDirectory(Path.Combine(path, name), Path.Combine(path, "runtime"), Encoding.UTF8);
            var mf = ReadManifest(Files.ReadAllText(Path.Combine(path, "runtime", "MANIFEST.MF")));
            Console.WriteLine($"Reading assembly of version {mf.Version}");
            Console.WriteLine($"Assembly {mf.ProjectName} by {mf.ProjectAuthor} on group {mf.ProjectGroup}");
            Manifests = GetAssemblies(path).ToList();
            if (!mf.ExpectedDependencies.All(asm => Manifests.Any(a => a.ProjectName == asm)))
                throw new CoreException($"Could not find all dependencies for assembly {mf.ProjectName}");

            var tf = mf.MainFile.Replace(".sf", ".sfc");
            var entry = ReadCompiledFile<Lambdas.Unit>(Path.Combine(path, "runtime", tf));
            var executables = 
                (from file in Directory.GetFiles(Path.Combine(path, "runtime"), "*.sfc") 
                    where Path.GetFileName(file) != tf 
                    select ReadCompiledFile<Lambdas.Unit>(file)).ToList();
            
            return (ctx, binder) =>
            {
                Console.WriteLine("Loading dependencies for project, please wait...");
                var lambdas = new List<Lambda<Lambdas.Unit>>();
                foreach (var req in Manifests.Where(req => mf.ExpectedDependencies.Contains(req.ProjectName)))
                {
                    lambdas.AddRange(ProcessDependencies(req, path, Path.Combine(path, "runtime")));
                }
                Console.WriteLine("Processed all dependencies successfully!");

                foreach (var l in lambdas) l(ctx, binder);
                foreach (var exec in executables) exec(ctx, binder);
                
                Console.WriteLine("Finished executing import-able projects");
                
                return entry(ctx, binder);
            };
        }
        
        private static Dictionary<string, List<Lambda<Lambdas.Unit>>> Loaded { get; set; }= new();
        private static List<ManifestInformation> Manifests { get; set; } = new();
        public static List<Lambda<Lambdas.Unit>> ProcessDependencies(ManifestInformation mf, string dir, string asmPath)
        {
            if (Loaded.ContainsKey(mf.ProjectName)) return Loaded[mf.ProjectName]; 
            Console.WriteLine($"Loading dependencies for {mf.ProjectName}");
            var gAssemblies = Manifests;
            var rtDir = Path.Combine(dir, "runtime");
            var intersect = gAssemblies.ToList().ConvertAll(o => o.ProjectName)
                .Intersect(mf.ExpectedDependencies).ToList();
            var lambdas = new List<Lambda<Lambdas.Unit>>();
            foreach (var asm in gAssemblies.Where(asm => intersect.Contains(asm.ProjectName)))
            {
                ZipFile.ExtractToDirectory(asmPath, rtDir, Encoding.UTF8);
                lambdas.AddRange(Directory.GetFiles(rtDir, "*.sfc").Select(ReadCompiledFile<Lambdas.Unit>));
            }
            Loaded.Add(mf.ProjectName, lambdas);
            return lambdas;
        }

        public static ManifestInformation GetAssemblyManifest(string path, string name)
        {
            var tempDirectory = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
            Directory.CreateDirectory(tempDirectory);
            ZipFile.ExtractToDirectory(Path.Combine(path, name), tempDirectory, Encoding.UTF8);
            var mf = ReadManifest(Files.ReadAllText(Path.Combine(path, "runtime", "MANIFEST.MF")));
            var dir = new DirectoryInfo(tempDirectory);
            dir.Delete(true);
            return mf;
        }
        
        public static IEnumerable<ManifestInformation> GetAssemblies(string file)
        {
            return Directory.GetFiles(file, "*.sfr").Select(s =>
            {
                var f = Path.GetFileName(s);
                var path = s.Replace(f, "");
                return GetAssemblyManifest(path, f);
            });
        }

        public static void CompileFile(string path, string assembly="sf-lib")
        {
            var data = Files.ReadAllText(path);
            var trueCode = Parser.UpgradeCode(data);
            var tokens = Parser.Default.Tokenize(trueCode);

            var cont = new TokenContainable();
            cont.AssemblerVersion = new Dictionary<string, string>
            {
                {"lexer", SFLang.LexerVersion},
                {"lang", SFLang.LanguageVersion}
            };
            cont.Compiler =
                $"Default SFLang Assembler: {typeof(Compiler).FullName}";
            cont.Tokens = tokens;
            cont.AssemblyName = assembly;
            
            var json = JsonConvert.SerializeObject(cont);

            var zipped = ByteUtils.Zip(json);
            Console.WriteLine("Compressed byte data...");
            var bpath = Path.Join(Directory.GetCurrentDirectory(), "build");
            if (!Directory.Exists(bpath)) Directory.CreateDirectory(bpath);
            var buildPath = Path.Join(Directory.GetCurrentDirectory(), "build", SFLang.Framework, "out");
            if (!Directory.Exists(buildPath)) Directory.CreateDirectory(buildPath);
            var asm = Path.Join(buildPath, $"{Path.GetFileNameWithoutExtension(path)}.sfc");
            var stream = new FileStream(asm, FileMode.CreateNew);
            var writer = new BinaryWriter(stream, Encoding.UTF8);
            
            // Magic number
            writer.Write((byte)0xF);
            
            // SFC version
            writer.Write($"sfc-ver: {SFLang.LanguageVersion}");
            writer.Write(ComputeSha256Hash(assembly));
            
            // Zipped bytes
            writer.Write7BitEncodedInt(zipped.Length);
            writer.Write(zipped);
            
            writer.Flush();
            writer.Close();

        }
        
        public static void CompileFiles(string executionPath, string assembly="sf-lib")
        {
            var project = ParseExecution(executionPath);
            foreach (var file in project.Includes.File)
            {
                var path = Path.Combine(Directory.GetCurrentDirectory(), file.Path);
                Console.WriteLine($"Compiling file {file.Path}");
                CompileFile(path, assembly);
            }
        }
        
        public static Project ParseExecution(string executionPath)
        {
            var serializer = new XmlSerializer(typeof(Project));
            using var reader = new XmlTextReader(executionPath);
            return (Project) serializer.Deserialize(reader);
        }

        public class ManifestInformation
        {
            [JsonProperty("Manifest-Version")] public string Version { get; set; }
            [JsonProperty("Main-File")] public string MainFile { get; set; }
            [JsonProperty("Project-Name")] public string ProjectName { get; set; }
            [JsonProperty("Project-Group")] public string ProjectGroup { get; set; }
            [JsonProperty("Project-Author")] public string ProjectAuthor { get; set; }
            [JsonProperty("Expect-Dependencies")] public List<string> ExpectedDependencies { get; set; } = new();
            [JsonProperty("Extra")] public Dictionary<string, string> NotParsed { get; set; } = new();
        }
        
        public static ManifestInformation ReadManifest(string manifestData)
        {
            return JsonConvert.DeserializeObject<ManifestInformation>(manifestData);
        }
        
        public static void GenerateManifest(string executionPath, string manifestPath)
        {
            var project = ParseExecution(executionPath);
            var path = Path.Combine(manifestPath, "MANIFEST.mf");
            if(Files.Exists(path)) Files.Delete(path);

            var obj = new ManifestInformation
            {
                Version = SFLang.Version,
                MainFile = project?.Entrypoint ?? "NULL",
                ProjectName = project?.Deployment?.Name ?? "NULL",
                ProjectGroup = project?.Deployment?.Group ?? "NULL",
                ProjectAuthor = project?.Deployment?.Author ?? "NULL"
            };

            foreach (var entry in project?.Dependencies?.Depends?.Where(s => s.Scope?.Provided ?? false) ?? new List<Depends>())
            {
                obj.ExpectedDependencies.Add(entry.Name);
            }

            foreach (var option in project?.Manifest?.Option ?? new List<Option>())
            {
                obj.NotParsed[option.Key] = option.Text;
            }

            var text = JsonConvert.SerializeObject(obj, Formatting.Indented);
            Files.WriteAllText(path, text);
        }
        
        public static Lambda<TContext> ReadCompiledFile<TContext>(string path)
        {
            var start = DateTime.Now;
            var stream = new FileStream(path, FileMode.Open);

            var reader = new BinaryReader(stream, Encoding.UTF8);

            Console.WriteLine($"Started decompiling file at {path}");
            
            var magic = reader.ReadByte();
            if (magic != 0xF) throw new IOException("The specified assembly is written in wrong/obsolete format!");
            var len = reader.Read7BitEncodedInt();
            var str = reader.ReadBytes(len);
            var hashlen = reader.Read7BitEncodedInt();
            var hash = reader.ReadBytes(hashlen);
            var sstr = Encoding.UTF8.GetString(str);
            var hhash = Encoding.UTF8.GetString(hash);
            Console.WriteLine($"SFC file SHA256 signature: {hhash}\n SFC Signature: {sstr}");
            var zlen = reader.Read7BitEncodedInt();
            var zipped = reader.ReadBytes(zlen);
            Console.WriteLine($"Read {zlen} zipped bytes successfully...");
            var unzipped = ByteUtils.Unzip(zipped);
            var tokens = JsonConvert.DeserializeObject<TokenContainable>(unzipped) ?? throw new IOException("Error deserializing data!");
            var asm = tokens.AssemblerVersion;
            Console.WriteLine($"Assembly compiled with {tokens.Compiler}");
            reader.Close();
            var tok = tokens.Tokens;
            var tstr = string.Join("", tok);
            var compiled = Lexer.Compile<TContext>(Parser.Default, tstr);
            var diff = DateTime.Now - start;
            Console.WriteLine($"Finished disassembly and interpretation of .sfc file in {diff.TotalMilliseconds} ms!");
            return compiled;
        }
        
        public static void Compile(string path, string assemblyName="sf-lib")
        {
            var start = DateTime.Now;
            Console.WriteLine("Started compiling code...");

            var exec = Path.Combine(path, "execution.xml");
            var bpath = Path.Join(Directory.GetCurrentDirectory(), "build");
            if (!Directory.Exists(bpath)) Directory.CreateDirectory(bpath);
            var buildPath = Path.Join(Directory.GetCurrentDirectory(), "build", SFLang.Framework);
            if (!Directory.Exists(buildPath)) Directory.CreateDirectory(buildPath);

            var libPath = Path.Join(buildPath, "lib");
            var outPath = Path.Join(buildPath, "out");
            if (!Directory.Exists(libPath)) Directory.CreateDirectory(libPath);
            if (!Directory.Exists(outPath)) Directory.CreateDirectory(outPath);
            var libFiles = Directory.EnumerateFiles(libPath);
            var outFiles = Directory.EnumerateFiles(outPath);
            
            foreach (var file in libFiles)
            {
                Files.Delete(file);
            }

            foreach (var file in outFiles)
            {
                Files.Delete(file);
            }
            
            CompileFiles(exec, assemblyName);
            GenerateManifest(exec, outPath);
            
            ZipFile.CreateFromDirectory(
                outPath, Path.Combine(libPath, $"{assemblyName}-{SFLang.Framework}.sfr"), CompressionLevel.Fastest, false, Encoding.UTF8);

            var diff = DateTime.Now - start;
            Console.WriteLine($"Finished compilation of project in {diff.TotalMilliseconds} ms!");
        }

        private static string ComputeSha256Hash(string rawData)
        {
            using var sha256Hash = SHA256.Create();
            var bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(rawData));  
            
            var builder = new StringBuilder();  
            foreach (var t in bytes)
            {
                builder.Append(t.ToString("x2"));
            }  
            return builder.ToString();
        }  
    }
}