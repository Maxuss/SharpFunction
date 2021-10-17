using System.Collections.Generic;
using System.Xml.Serialization;

namespace SFLang.Language
{
	[XmlRoot(ElementName = "deployment")]
	public class Deployment
	{

		[XmlElement(ElementName = "name")] public string Name { get; set; }

		[XmlElement(ElementName = "author")] public string Author { get; set; }

		[XmlElement(ElementName = "group")] public string Group { get; set; }

		[XmlElement(ElementName = "upload")] public string Upload { get; set; }
	}

	[XmlRoot(ElementName = "file")]
	public class File
	{

		[XmlAttribute(AttributeName = "path")] public string Path { get; set; }
	}

	[XmlRoot(ElementName = "includes")]
	public class Includes
	{

		[XmlElement(ElementName = "file")] public List<File> File { get; set; }
	}

	[XmlRoot(ElementName = "mixin")]
	public class Mixin
	{
		[XmlAttribute(AttributeName = "assembly")]
		public string Assembly { get; set; }

		[XmlAttribute(AttributeName = "class")]
		public string Class { get; set; }
	}

	[XmlRoot(ElementName = "mixins")]
	public class Mixins
	{

		[XmlElement(ElementName = "mixin")] public List<Mixin> Stored { get; set; }
	}

	[XmlRoot(ElementName = "option")]
	public class Option
	{

		[XmlAttribute(AttributeName = "key")] public string Key { get; set; }

		[XmlText] public string Text { get; set; }
	}

	[XmlRoot(ElementName = "manifest")]
	public class Manifest
	{

		[XmlElement(ElementName = "option")] public List<Option> Option { get; set; }
	}

	[XmlRoot(ElementName = "scope")]
	public class Scope
	{

		[XmlAttribute(AttributeName = "provided")]
		public bool Provided { get; set; }
	}

	[XmlRoot(ElementName = "depends")]
	public class Depends
	{

		[XmlElement(ElementName = "name")] public string Name { get; set; }

		[XmlElement(ElementName = "group")] public string Group { get; set; }

		[XmlElement(ElementName = "fetch")] public string Fetch { get; set; }

		[XmlElement(ElementName = "scope")] public Scope Scope { get; set; }
	}

	[XmlRoot(ElementName = "dependencies")]
	public class Dependencies
	{

		[XmlElement(ElementName = "depends")] public List<Depends> Depends { get; set; }
	}

	[XmlRoot(ElementName = "project")]
	public class Project
	{

		[XmlElement(ElementName = "deployment")]
		public Deployment Deployment { get; set; }

		[XmlElement(ElementName = "includes")] public Includes Includes { get; set; }

		[XmlElement(ElementName = "mixins")] public Mixins Mixins { get; set; }

		[XmlElement(ElementName = "entrypoint")]
		public string Entrypoint { get; set; }

		[XmlElement(ElementName = "manifest")] public Manifest Manifest { get; set; }

		[XmlElement(ElementName = "dependencies")]
		public Dependencies Dependencies { get; set; }
	}
}