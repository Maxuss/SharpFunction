using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Xml;
using Newtonsoft.Json;
using SharpFunction.API;

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
        // public static void GenerateManifest(string executionPath)
        // {
        //     var xml = new XmlDocument();
        //     xml.Load(executionPath);
        //     xml.Schemas.Contai
        // }
        
        public static Lambda<TContext> ReadAssembly<TContext>(string path)
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
            Console.WriteLine($"Assembly SHA256 signature: {hhash}\n SFC Signature: {sstr}");
            var zlen = reader.Read7BitEncodedInt();
            var zipped = reader.ReadBytes(zlen);
            Console.WriteLine($"Read {zlen} worth of zipped bytes successfully...");
            var unzipped = ByteUtils.Unzip(zipped);
            Console.WriteLine("Successfully unzipped byte data!");
            var tokens = JsonConvert.DeserializeObject<TokenContainable>(unzipped) ?? throw new IOException("Error deserializing data!");
            Console.WriteLine("Successfully deserialized tokenized data!");
            var asm = tokens.AssemblerVersion;
            Console.WriteLine($"Assembly Lexer version: {asm["lexer"]}\nAssembly SFLang version: {asm["lang"]}");
            Console.WriteLine($"Assembly compiled with {tokens.Compiler}");
            Console.WriteLine($"Assembly name: {tokens.AssemblyName}");
            reader.Close();
            var tok = tokens.Tokens;
            var tstr = string.Join("", tok);
            var compiled = Lexer.Compile<TContext>(Parser.Default, tstr);
            var diff = DateTime.Now - start;
            Console.WriteLine($"Finished disassembly and interpretation of .sfc file in {diff.TotalMilliseconds} ms!");
            return compiled;
        }
        
        public static void Compile(string code, string assemblyName="sf-lib")
        {
            var start = DateTime.Now;
            Console.WriteLine("Started compiling code...");
            var trueCode = Parser.UpgradeCode(code);
            var tokens = Parser.Default.Tokenize(trueCode);

            var cont = new TokenContainable();
            cont.AssemblerVersion = new Dictionary<string, string>
            {
                {"lexer", SFLang.LexerVersion},
                {"lang", SFLang.Version}
            };
            cont.Compiler =
                $"Default SFLang Assembler: {typeof(Compiler).FullName}";
            cont.Tokens = tokens;
            cont.AssemblyName = assemblyName;
            
            var json = JsonConvert.SerializeObject(cont);

            var zipped = ByteUtils.Zip(json);
            Console.WriteLine("Compressed byte data...");
            var bpath = Path.Join(Directory.GetCurrentDirectory(), "build");
            if (!Directory.Exists(bpath)) Directory.CreateDirectory(bpath);
            var buildPath = Path.Join(Directory.GetCurrentDirectory(), "build", SFLang.Framework);
            if (!Directory.Exists(buildPath)) Directory.CreateDirectory(buildPath);
            var asm = Path.Join(buildPath, $"{assemblyName}-{SFLang.Framework}.sfc");
            var stream = new FileStream(asm, FileMode.CreateNew);
            var writer = new BinaryWriter(stream, Encoding.UTF8);
            
            // Magic number
            writer.Write((byte)0xF);
            
            // SFC version
            writer.Write($"sfc-ver: {SFLang.Version}");
            writer.Write(ComputeSha256Hash(assemblyName));
            
            // Zipped bytes
            writer.Write7BitEncodedInt(zipped.Length);
            writer.Write(zipped);
            
            writer.Flush();
            writer.Close();
            var diff = DateTime.Now - start;
            Console.WriteLine($"Finished compilation of file in {diff.TotalMilliseconds} ms!");
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