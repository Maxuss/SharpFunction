using System.Xml.Serialization;

namespace SFLang.Language
{
    [XmlRoot(ElementName="deployment", Namespace="")]
	public class Deployment { 

		[XmlElement(ElementName="name", Namespace="")] 
		public string Name; 

		[XmlElement(ElementName="author", Namespace="")] 
		public string Author; 

		[XmlElement(ElementName="group", Namespace="")] 
		public string Group; 

		[XmlElement(ElementName="upload", Namespace="")] 
		public string Upload; 
	}

	[XmlRoot(ElementName="file", Namespace="")]
	public class File { 

		[XmlAttribute(AttributeName="path", Namespace="")] 
		public string Path; 
	}

	[XmlRoot(ElementName="includes", Namespace="")]
	public class Includes { 

		[XmlElement(ElementName="file", Namespace="")] 
		public List<File> File; 
	}

	[XmlRoot(ElementName="option", Namespace="")]
	public class Option { 

		[XmlAttribute(AttributeName="key", Namespace="")] 
		public string Key; 

		[XmlText] 
		public string Text; 
	}

	[XmlRoot(ElementName="manifest", Namespace="")]
	public class Manifest { 

		[XmlElement(ElementName="option", Namespace="")] 
		public List<Option> Option; 
	}

	[XmlRoot(ElementName="scope", Namespace="")]
	public class Scope { 

		[XmlAttribute(AttributeName="provided", Namespace="")] 
		public bool Provided;
	}

	[XmlRoot(ElementName="depends", Namespace="")]
	public class Depends { 

		[XmlElement(ElementName="name", Namespace="")] 
		public string Name; 

		[XmlElement(ElementName="group", Namespace="")] 
		public string Group; 

		[XmlElement(ElementName="fetch", Namespace="")] 
		public string Fetch; 

		[XmlElement(ElementName="scope", Namespace="")] 
		public Scope Scope; 
	}

	[XmlRoot(ElementName="dependencies", Namespace="")]
	public class Dependencies { 

		[XmlElement(ElementName="depends", Namespace="")] 
		public List<Depends> Depends; 
	}

	[XmlRoot(ElementName="project", Namespace="")]
	public class Project { 

		[XmlElement(ElementName="deployment", Namespace="")] 
		public Deployment Deployment; 

		[XmlElement(ElementName="includes", Namespace="")] 
		public Includes Includes; 

		[XmlElement(ElementName="entrypoint", Namespace="")] 
		public string Entrypoint; 

		[XmlElement(ElementName="manifest", Namespace="")] 
		public Manifest Manifest; 

		[XmlElement(ElementName="dependencies", Namespace="")] 
		public Dependencies Dependencies; 
	}
}