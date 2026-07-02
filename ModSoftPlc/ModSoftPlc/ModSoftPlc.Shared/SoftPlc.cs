using System.Xml;
using System.Xml.Serialization;

namespace Scada.Server.Modules.ModSoftPlc
{
    #region ProgramAttribute
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Class | AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class ProgramzAttribute : Attribute
    {
        public string VarType { get; private set; }
        public string Description { get; private set; }


        public ProgramzAttribute(string vartype, string description)
        {
            VarType = vartype;
            Description = description;
        }
    }
    #endregion ProgramAttribute

    [Serializable]
    public class SoftPlc
    {
        #region ClassSoftPlc
        public SoftPlc()
        {
            Program = new List<ProgramX>();
        }
        
        [Programz("Option", "Show the program save log")]
        [XmlAttribute]  public bool VisibleLogPrgFile { get; set; }

        [Programz("Option", "Time to save program parameters in minutes")]
        [XmlAttribute] public string SaveLogPrgTime { get; set; }

        [Programz("Option", "Iteration time for calling task parameters in milliseconds")]
        [XmlAttribute] public string IterationTime { get; set; }

        [XmlElement]
        public List<ProgramX> Program { get; set; } // это позволяет сделать сериализацию без создания соответствующих групп если они пустые
        [XmlIgnore]
        public bool ProgramSpecified { get { return Program.Count != 0; } }
        #endregion ClassSoftPlc

        #region Program Описание класса Program
        public class ProgramX
        {
            public ProgramX()
            {
            }

            public ProgramX(string TasksLibrary, string Task, string TaskCycle, bool Active, bool Restart) // string TaskPriority,
            {
                this.TasksLibrary = TasksLibrary;
                this.Task = Task;
                this.Active = Active;
                this.Restart = Restart;
                this.TaskCycle = TaskCycle;
                Variable = new List<VariableX>();
            }

            [Programz("Program Option", "Choosing a program library")]
            [XmlAttribute] public string TasksLibrary { get; set; }

            [Programz("Program Option", "Task Class Name")]
            [XmlAttribute] public string Task { get; set; }

            [Programz("Program Option", "Task cycle in milliseconds")]
            [XmlAttribute] public string TaskCycle { get; set; }

            [Programz("Program Option", "Changing program activity")]
            [XmlAttribute] public bool Active { get; set; }

            [Programz("Program Option", "Restarting the program when the module is restarted")]
            [XmlAttribute] public bool Restart { get; set; }

            [XmlElement]
            public List<VariableX> Variable { get; set; }
            //Игнорировать при записи если строки пустые и bool = false
            [XmlIgnore]
            public bool VariableSpecified { get { return Variable.Count != 0; } }

            public Calendar Calendar;
        }
        #endregion Program

        #region ClassCalendar Описание класса Calendar для сериализации
        public class Calendar
        {
            public Calendar()
            {
                Title = new List<TitleX>();
                Holyday = new List<DayH>();
                Day = new List<DayX>();
                Week = new List<WeekX>();
            }

            [XmlElement]
            public List<TitleX> Title { get; set; } 
            [XmlIgnore]
            public bool TitleSpecified { get { return Title.Count != 0; } } // это позволяет сделать сериализацию без создания соответствующих групп если они пустые

            [XmlElement]
            public List<DayH> Holyday { get; set; } 
            [XmlIgnore]
            public bool HolydaySpecified { get { return Holyday.Count != 0; } } // это позволяет сделать сериализацию без создания соответствующих групп если они пустые


            [XmlElement]
            public List<DayX> Day { get; set; }
            [XmlIgnore]
            public bool DaySpecified { get { return Day.Count != 0; } } // это позволяет сделать сериализацию без создания соответствующих групп если они пустые

            [XmlElement]
            public List<WeekX> Week { get; set; }
            [XmlIgnore]
            public bool WeekSpecified { get { return Week.Count != 0; } } // это позволяет сделать сериализацию без создания соответствующих групп если они пустые

            #region Holyday Title Prodact Calendar
            public class TitleX
            {
                public TitleX()
                {
                }

                public TitleX(string id, string title)
                {
                    this.id = id;
                    this.title = title;
                }

                [XmlAttribute] public string id { get; set; }
                [XmlAttribute] public string title { get; set; }
            }
            #endregion Holyday Title Prodact Calendar

            #region Day Holiday
            public class DayH
            {
                public DayH()
                {
                }

                public DayH(string date, string h, string t, string f)
                {
                    this.date = date;
                    this.h = h;
                    this.t = t;
                    this.f = f;
                }

                [Programz("Производственный календарь", "День (формат ММ.ДД)")]
                [XmlAttribute] public string date { get; set; }

                [Programz("Производственный календарь", "Номер праздника (ссылка на атрибут id тэга holiday)")]
                [XmlAttribute] public string h { get; set; }

                [Programz("Производственный календарь", "тип дня:\n1 - выходной день\n2 - рабочий и сокращенный (может быть использован для любого дня недели)\n3 - рабочий день (суббота/воскресенье)")]
                [XmlAttribute] public string t { get; set; }

                [Programz("Производственный календарь", "Дата с которой был перенесен выходной день\nсуббота и воскресенье считаются выходными, если нет тегов day с атрибутом t=2 и t=3 за этот день")]
                [XmlAttribute] public string f { get; set; }
            }
            #endregion Day Holiday

            #region DayX
            public class DayX
            {
                public DayX()
                {
                }

                public DayX(string DayNum)
                {
                    this.DayNum = DayNum;
                    Variable = new List<VariableX>();
                }

                [XmlAttribute]
                public string DayNum { get; set; }

                [XmlElement]
                public List<VariableX> Variable { get; set; }
                //Игнорировать при записи если строки пустые и bool = false
                [XmlIgnore]
                public bool VariableSpecified { get { return Variable.Count != 0; } }
            }
            #endregion DayX

            #region WeekX
            public class WeekX
            {
                public WeekX()
                {
                }

                public WeekX(string WeekNum)
                {
                    this.WeekNum = WeekNum;
                    Variable = new List<VariableX>();
                }

                [XmlAttribute]
                public string WeekNum { get; set; }

                [XmlElement]
                public List<VariableX> Variable { get; set; }
                //Игнорировать при записи если строки пустые и bool = false
                [XmlIgnore]
                public bool VariableSpecified { get { return Variable.Count != 0; } }
            }
            #endregion WeekX
        }
        #endregion ClassCalendar

        #region Variables
        public class VariableX
        {
            public VariableX()
            {
            }

            public VariableX(string VariableName, bool Initialize, bool Retain, string Data, string Command, string CnlNum, string CnlCode)
            {
                this.VariableName = VariableName;
                this.Initialize = Initialize;
                this.Retain = Retain;
                this.Data = Data;
                this.Command = Command;
                this.CnlNum = CnlNum;
                this.CnlCode = CnlCode;
            }

            [XmlAttribute]
            public string VariableName { get; set; }

            [Programz("Variable Option", "Initialization of a variable at the start of the program.\nAvailable in the Pro version.")]
            [XmlAttribute] public bool Initialize { get; set; }

            [Programz("Variable Option", "Saving a variable during program restarts.\nAvailable in the Pro version.")]
            [XmlAttribute] public bool Retain { get; set; }

            [Programz("Variable Option", "The value of the program variable.")]
            [XmlAttribute] public string Data { get; set; }

            [Programz("Variable Option", "Data for sending the command.")]
            [XmlAttribute] public string Command { get; set; }

            [Programz("Variable Option", "The channel number associated with the program variable")]
            [XmlAttribute] public string CnlNum { get; set; }

            [Programz("Variable Option", "The full tag code for the information is an Object.Device.TagСode")]
            [XmlAttribute] public string CnlCode { get; set; }
        }
        #endregion Variables

    }
}
