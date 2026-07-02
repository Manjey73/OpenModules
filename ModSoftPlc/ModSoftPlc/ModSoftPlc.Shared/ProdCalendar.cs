using System.Xml.Serialization;

namespace Scada.Server.Modules.ModSoftPlc
{
    [Serializable]
    public class calendar
    {
        public calendar()
        {
            holidays = new List<holiday>();     // Список праздников
            days = new List<day>();             // Список дней
        }

        [XmlAttribute]
        public string year { get; set; }

        [XmlAttribute]
        public string lang { get; set; }

        [XmlAttribute]
        public string date { get; set; }

        [XmlIgnore]
        public bool yearSpecified { get { return year != ""; } }
        public bool langSpecified { get { return lang != ""; } }
        public bool dateSpecified { get { return date != ""; } }

        public List<holiday> holidays { get; set; }
        public List<day> days { get; set; }

        public class holiday
        {
            [XmlAttribute]
            public string id { get; set; }

            [XmlAttribute]
            public string title { get; set; }

            [XmlIgnore]
            public bool idSpecified { get { return id != ""; } }
            public bool titleSpecified { get { return title != ""; } }
        }

        public class day
        {
            [XmlAttribute]
            public string d { get; set; }

            [XmlAttribute]
            public string t { get; set; }

            [XmlAttribute]
            public string h { get; set; }

            [XmlAttribute]
            public string f { get; set; }

            [XmlIgnore]
            public bool dSpecified { get { return d != ""; } }
            public bool tSpecified { get { return t != ""; } }

            public bool hSpecified { get { return h != ""; } }
            public bool fSpecified { get { return f != ""; } }
        }
    }
}
