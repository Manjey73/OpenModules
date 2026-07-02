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

    // Программа передачи переменной
    [Programz("Program", "The program for sending the value")] 

    public class PlcForwardValue
    {
        [Programz("Input", "Allowing the program to work")] // TEST свои атрибуты для переменных - Как их переводить в словарях ?
        public bool EN = true; // входная переменная разрешающая работу программы

        [Programz("Input", "Input value")]
        public double InVal; // 

        [Programz("Output", "Output value")]
        public double OutVal; // 

        [Programz("Input", "Input value")]
        public ushort cycle = 500; // Задание цикла выполнения программы в мс
        public bool terminated = false; // Переменная остановки цикла для закрытия потока


        public void Run()
        {
            while (!terminated)
            {
                if (EN)
                {
                    OutVal = InVal; // Примитивная операция назначения входа выходу

                    // Задание времени цикла
                    Thread.Sleep(cycle);
                }
            }
        }
    }
}
