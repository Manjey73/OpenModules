using Scada.Lang;

namespace Scada.Server.Modules.ModSoftPlc.View
{
    internal class ModulePhrases
    {
        // Scada.Server.Modules.ModDbExport.View.Forms.FrmModuleConfig
        public static string AppTitle { get; private set; } = "SoftPlc Module";
        public static string MainNode { get; private set; } = "SoftPlc";
        public static string TitleLoadProject { get; private set; } = "Load  project...";
        public static string FilterProject { get; private set; } = "XML (*.xml)|*.xml|All files (*.*)|*.*";

        public static string VisibleLogPrgFile { get; private set; } = "Visible Log Prg File";
        public static string SaveLogPrgTime { get; private set; } = "Save Log Prg Time"; 
        public static string IterationTime { get; private set; } = "Iteration Time";


        public static string Calendar { get; private set; } = "Calendar";
        public static string Title { get; private set; } = "Title";
        public static string id { get; private set; } = "id";
        public static string title { get; private set; } = "title";
        public static string Holyday { get; private set; } = "Holyday";
        public static string date { get; private set; } = "date";
        public static string h { get; private set; } = "h";
        public static string t { get; private set; } = "t";
        public static string f { get; private set; } = "f";
        public static string Day { get; private set; } = "Day";
        public static string Week { get; private set; } = "Week";


        public static string Program { get; private set; } = "Program";
        public static string Variable { get; private set; } = "Variable";
        public static string VariableType { get; private set; } = "Type of variable";
        public static string Description { get; private set; } = "Description";

        // Именование параметров (Аттрибутов) 
        public static string Active { get; private set; } = "Active";

        // Именование параметров Программы (Аттрибутов)
        public static string TasksLibrary { get; private set; } = "Tasks Library";

        public static string Task { get; private set; } = "Task";
        public static string Restart { get; private set; } = "Restart";
        public static string TaskCycle { get; private set; } = "Task Cycle";

        // Аттрибуты переменных
        public static string VariableName { get; private set; } = "Variable Name";
        public static string Data { get; private set; } = "Data Value";
        public static string Command { get; private set; } = "Data Command";
        public static string CnlNum { get; private set; } = "Channel Number";
        public static string CnlCode { get; private set; } = "Tag Channel";
        public static string Initialize { get; private set; } = "Initialization";
        public static string Retain { get; private set; } = "Retain Value";


        // в xml заменить DayNum на DayNumber и так далее в Календаре или все же наоборот в словарях?
        public static string DayNum { get; private set; } = "Day Number";
        //public static string Parameter { get; private set; } = "Parameter";
        public static string WeekNum { get; private set; } = "Week Number";

        // Названия столбцов Атрибутов
        public static string dgvAttrName { get; private set; } = "Parameter";
        public static string dgvAttrValue { get; private set; } = "Value";

        // Контекстное меню
        #region ContextMenu Nodes
        public static string AddParameters { get; private set; } = "Add Parameters";
        public static string AddCalendar { get; private set; } = "Add Calendar";
        public static string AddDay { get; private set; } = "Add Day";
        public static string AddWeek { get; private set; } = "Add Week";
        public static string AddProgram { get; private set; } = "Add Program";
        public static string AddVariable { get; private set; } = "Add Variable";
        public static string AddComment { get; private set; } = "Add Comment";
        public static string AddText { get; private set; } = "Add Text";
        public static string Delete { get; private set; } = "Delete";
        #endregion ContextMenu Nodes

        //// Для словаря FrmParameters
        //public static string Comment { get; private set; } = "Comment";

        public static void Init()
        {
            LocaleDict dict = Locale.GetDictionary("Scada.Server.Modules.ModSoftPlc.View.Forms.FrmModuleConfig");
            AppTitle = dict[nameof(AppTitle)];
            MainNode = dict[nameof(MainNode)];      // Главная Нода
            VisibleLogPrgFile = dict[nameof(VisibleLogPrgFile)];
            SaveLogPrgTime = dict[nameof(SaveLogPrgTime)];
            IterationTime = dict[nameof(IterationTime)];

            Calendar = dict[nameof(Calendar)];      // Календарь
            Day = dict[nameof(Day)];                // День
            Week = dict[nameof(Week)];              // День Недели
            Title = dict[nameof(Title)];            // Наименование праздника
            Holyday = dict[nameof(Holyday)];        // Дата праздника
            id = dict[nameof(id)];
            title = dict[nameof(title)];
            date = dict[nameof(date)];
            h = dict[nameof(h)];
            t = dict[nameof(t)];
            f = dict[nameof(f)];

            Program = dict[nameof(Program)];            // Программа
            Active = dict[nameof(Active)];            // Активность
            Variable = dict[nameof(Variable)];          // Переменная

            VariableType = dict[nameof(VariableType)];  // Тип Переменной
            Description = dict[nameof(Description)];    // Описание

            TitleLoadProject = dict[nameof(TitleLoadProject)];
            FilterProject = dict[nameof(FilterProject)];

            // Аттрибуты программы
            TasksLibrary = dict[nameof(TasksLibrary)];
            Task = dict[nameof(Task)];
            Restart = dict[nameof(Restart)];
            TaskCycle = dict[nameof(TaskCycle)];

            // Аттрибуты входов и выходов
            VariableName = dict[nameof(VariableName)];
            Data = dict[nameof(Data)];
            Command = dict[nameof(Command)];
            CnlNum = dict[nameof(CnlNum)];
            CnlCode = dict[nameof(CnlCode)];
            Initialize = dict[nameof(Initialize)];
            Retain = dict[nameof(Retain)];

            // Имя столбцов Атрибутов
            dgvAttrName = dict[nameof(dgvAttrName)];
            dgvAttrValue = dict[nameof(dgvAttrValue)];

            // Контекстное меню
            AddParameters = dict[nameof(AddParameters)];
            AddCalendar = dict[nameof(AddCalendar)];
            AddDay = dict[nameof(AddDay)];
            AddWeek = dict[nameof(AddWeek)];
            AddProgram = dict[nameof(AddProgram)];
            AddVariable = dict[nameof(AddVariable)];
            AddComment = dict[nameof(AddComment)];
            AddText = dict[nameof(AddText)];
            Delete = dict[nameof(Delete)];

            //LocaleDict dict1 = Locale.GetDictionary("Scada.Server.Modules.ModSoftPlc.View.Forms.FrmParameters");
            //Comment = dict1[nameof(Comment)];

        }
    }
}
