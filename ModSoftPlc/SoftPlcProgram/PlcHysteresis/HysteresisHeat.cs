using ScadaCommFunc;

namespace Scada.Server.Modules.ModSoftPlc
{
    // Необходимый блок, чтобы добавлять CustomAttributes к описаниям переменных. По VarType определяется какая это переменная для Pro версии - Init = инициализация значения, Retain = Сохраняемая переменная, Input = Входная переменная, Output = Выходная переменная
    // Значения типов можно комбинировать, например: Init/Retain, Input-Init и так далее. Возможно не все реализовано в этом моменте, точно работают Init и Retain

    // A necessary block to add CustomAttributes to variable descriptions. The VarType determines which variable it is for the Pro version - Init = initialize the value, Retain = Saved variable, Input = Input variable, Output = Output variable
    // Type values can be combined, for example: Init/Retain, Input-Init, and so on. Perhaps not everything is implemented at this moment, Init and Retain work exactly.

    // Description - Переносится в комментарий переменной
    // Description - Is transferred to the variable comment.
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

    // Пример программы Гистерезиса c использованием подключенной библиотеки, в данном случае используется ScadaCommFunc.dll в которую я начал добавлять дополнительные функции
    // An example of a Hysteresis program using the connected library, in this case using ScadaCommFunc.dll which I started adding additional features to
    #region HysteresisHeat
    [Programz("Program", "Hysteresis Heater")] // Hysteresis Heater Нагреватель по Гистерезису
    public class HysteresisHeat
    {
        [Programz("Input", "Allowing the program to work")]
        public bool EN = true; // входная переменная разрешающая работу программы
        [Programz("Input", "Incoming measurement channel")]
        public double inCnl;
        [Programz("Input", "Minimum value")] // Минимальное значение Гистерезиса для включения в режиме нагревателя
        public double low;
        [Programz("Input", "Maximum value")] // Максимальное значение Гистерезиса для выключения в режиме нагревателя
        public double high;

        //[Programz("Input", "true = Heater, false = Cooler")]
        //public bool mode = true; // По умолчанию нагрев = true, охлаждение = false

        [Programz("Output/Retain", "Hysteresis output")]
        public bool Q; // Выход гистерезиса
        [Programz("Output", "Inverse Hysteresis output")]
        public bool Q_; // Выход гистерезиса инверсный

        [Programz("Input", "Task cycle in ms")]
        public ushort cycle; // Задание цикла выполнения программы в мс // Setting the program execution cycle in ms

        //Loop stop variable for stopping the program and/or closing the thread (Service)
        public bool terminated = false; // Переменная остановки цикла для остановки программы и(или) для закрытия потока (Служебная)

        // The default mode in the ScadaCommFunc library is true - the heater mode, you don't have to specify it.
        ControlMod.Hyst hyst1 = new() { mode = true }; // mode в библитотеке ScadaCommFunc по умолчанию true - режим нагревателя, можно не указывать

        public void Run()
        {
            // Копирование входных переменных во внутренние для программы
            // или инициализация временных переменных и т.д.
            bool res = false;

            do
            {
                if (EN)
                {
                    //// Копирование входных переменных во внутренние в начале цикла программы
                    //_mode = mode;
                    // Инициализация выходных переменных если требуется.

                    res = Q; // для возможности задания retain при перезапусках

                    // тело программы
                    hyst1.inCnl = inCnl;
                    hyst1.Low = low;
                    hyst1.High = high;
                    hyst1.Run();

                    res = hyst1.Q;

                    // тело программы
                    // Копирование выходных переменных из внутренних в конце цикла программы, для некоторых необязательно, будут обработаны программой
                    Q = res;
                    Q_ = !res;
                    // Задание времени цикла
                    Thread.Sleep(cycle);
                }
                else
                {
                    // Если останавливаем программу по переменной EN, выходы перевести в выключенное состояние независимо от требавания инверсии выхода.
                    Q = false; // При отключении программы выход перевести в fasle
                    Q_ = false; // а тоже false - типа безопасное состояние :)
                }
            }
            while (!terminated);
        }
    }
    #endregion HysteresisHeat

    // Пример программы Гистерезиса непосредственно в коде программы, без использования подключенной библиотеки
    // Example of a Hysteresis program directly in the program code, without using the attached library
    #region HisteresisTest
    [Programz("Program", "Hysteresis of the heater or cooler")] // Гистерезис нагревателя или охладителя
    public class Hysteresis()
    {
        [Programz("Input", "Allowing the program to work")]
        public bool EN; // входная переменная разрешающая работу программы
        [Programz("Input", "Incoming measurement channel")] 
        public double inCnl;
        [Programz("Input", "Minimum value")] // Минимальное значение Гистерезиса для включения в режиме нагревателя
        public double low;
        [Programz("Input", "Maximum value")] 
        public double high;

        [Programz("Input", "true = Heater, false = Cooler")] 
        public bool mode = true; // По умолчанию нагрев = true, охлаждение = false

        [Programz("Output", "Hysteresis output")] 
        public bool Q; // Выход гистерезиса
        [Programz("Output", "Inverse Hysteresis output")] 
        public bool Q_; // Выход гистерезиса

        [Programz("Input", "Task cycle in ms")] 
        public ushort cycle; // Задание цикла выполнения программы в мс

        //Loop stop variable for stopping the program and/or closing the thread (Service)
        public bool terminated = false; // Переменная остановки цикла для остановки программы и(или) для закрытия потока (Служебная)

        bool _mode;

        public void Run()
        {
            // Копирование входных переменных во внутренние при программы
            bool res = false;

            do
            {
                if (EN)
                {
                    // Копирование входных переменных во внутренние в начале цикла программы
                    _mode = mode;
                    // Инициализация выходных переменных если требуется.
                    res = Q; // для возможности задания retain при перезапусках
                    // тело программы
                    if (_mode)
                    {
                        if (inCnl < low) res = true;
                        if (inCnl > high) res = false;
                    }
                    else
                    {
                        if (inCnl < low) res = false;
                        if (inCnl > high) res = true;
                    }
                    // тело программы
                    // Копирование выходных переменных из внутренних в конце цикла программы, для некоторых необязательно, будут обработаны программой
                    Q = res;
                    Q_ = !Q;
                    // Задание времени цикла
                    Thread.Sleep(cycle);
                }
                else
                {
                    Q = false; // При отключении программы выход перевести в fasle
                    Q_ = !Q;
                }
            }
            while (!terminated);
        }
    }
    #endregion HisteresisTest

    #region Test Exception
    [Programz("Program", "TestException Program")] // Гистерезис нагревателя или охладителя
    public class TestException
    {
        [Programz("Input", "Allowing the program to work")]
        public bool EN = true; // входная переменная разрешающая работу программы

        [Programz("Output", "An output variable showing the result of extracting a value from an array")] // Выходная переменная, показывающая результат извлечения значения из массива
        public int Q;

        [Programz("Input", "Task cycle in ms")]
        public ushort cycle = 2000; // Задание цикла выполнения программы в мс

        //Loop stop variable for stopping the program and/or closing the thread (Service)
        public bool terminated = false; // Переменная остановки цикла для остановки программы и(или) для закрытия потока

        private int[] mass = [1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20];
        public void Run()
        {
            // Копирование входных переменных во внутренние при программы
            Q = 0;

            do
            {
                if (EN)
                {
                    // тело программы
                    for (int i = 0; i < 23; i++)
                    {

                        Q = mass[i];
                        Thread.Sleep(cycle); // Пауза = время цикла, чтобы быстро не перебирался массив
                    }

                    // тело программы
                    // Копирование выходных переменных из внутренних в конце цикла программы, для некоторых необязательно, будут обработаны программой
                    // Задание времени цикла
                    Thread.Sleep(cycle);
                }
                else
                {
                    Q = 0; // При отключении программы выход перевести в fasle
                }
            }
            while (!terminated);
        }
    }
    #endregion Test Exception

}
