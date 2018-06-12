using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Yme.Code
{
    [Serializable]
    [XmlRoot(ElementName = "MsgContentCollection")]
    public class MsgContentCollection
    {
        [XmlElement("MsgDescription")]
        public List<MsgDescription> MsgDescriptions
        {
            get;
            set;
        }
    }

    [Serializable]
    public class MsgDescription
    {
        [XmlElement("MsgKey")]
        public string MsgKey
        {
            get;
            set;
        }

        [XmlElement("MsgFormat")]
        public string MsgFormat
        {
            get;
            set;
        }

        [XmlElement("MsgStatus")]
        public string MsgStatus
        {
            get;
            set;
        }

        [XmlElement("MsgExamples")]
        public string MsgExamples
        {
            get;
            set;
        }
    }
}
