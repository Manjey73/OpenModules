using System.Xml.Serialization;

namespace Scada.Server.Modules.ModSoftPlc
{
    [Serializable]

    [XmlRoot("calendar")]
    public class CalendarFile
    {
        public CalendarFile()
        {
            holidays = new List<Holiday>();     // Список праздников
            days = new List<CalendarDay>();             // Список дней
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

        [XmlArrayItem("holiday")]
        public List<Holiday> holidays { get; set; }

        [XmlArrayItem("day")]
        public List<CalendarDay> days { get; set; }

        public class Holiday
        {
            [XmlAttribute]
            public string id { get; set; }

            [XmlAttribute]
            public string title { get; set; }

            [XmlIgnore]
            public bool idSpecified { get { return id != ""; } }
            public bool titleSpecified { get { return title != ""; } }
        }

        public class CalendarDay
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
