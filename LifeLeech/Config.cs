using System;
using System.Xml.Serialization;

namespace LifeLeech {
	[Serializable()]
	[XmlRoot(ElementName = "Config")]
	public class Config {
		[XmlElement(ElementName = "StaticHealing")]
		public StaticHealing StaticHealing { get; set; }
		[XmlElement(ElementName = "MultiplerHealing")]
		public MultiplerHealing MultiplerHealing { get; set; }
		[XmlElement(ElementName = "ExcludeCavalry")]
		public bool ExcludeCavalry { get; set; }
		[XmlElement(ElementName = "PlayerOnly")]
		public bool PlayerOnly { get; set; }
		[XmlElement(ElementName = "KillBased")]
		public bool KillBased { get; set; }
		[XmlElement(ElementName = "LimitedByMaxHealth")]
		public bool LimitedByMaxHealth { get; set; }
		[XmlElement(ElementName = "OnlyNamedCharacters")]
		public bool OnlyNamedCharacters { get; set; }
		[XmlElement(ElementName = "DebugOutput")]
		public bool DebugOutput { get; set; }
	}

	[Serializable()]
	[XmlRoot(ElementName = "StaticHealing")]
	public class StaticHealing {
		[XmlAttribute(AttributeName = "enabled")]
		public bool Enabled { get; set; }
		[XmlText]
		public float Value { get; set; }
	}

	[Serializable()]
	[XmlRoot(ElementName = "MultiplerHealing")]
	public class MultiplerHealing {
		[XmlAttribute(AttributeName = "enabled")]
		public bool Enabled { get; set; }
		[XmlText]
		public decimal Value { get; set; }
	}
}
