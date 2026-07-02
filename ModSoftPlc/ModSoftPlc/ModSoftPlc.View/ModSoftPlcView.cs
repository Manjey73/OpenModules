using Scada.Forms;
using Scada.Lang;
using Scada.Server.Modules.ModSoftPlc.View.Forms;
using System.Reflection;

namespace Scada.Server.Modules.ModSoftPlc.View
{
    internal class ModSoftPlcView : ModuleView
    {
        public static FrmModuleConfig fConfig;

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public ModSoftPlcView()
        {
            CanShowProperties = true;
            RequireRegistration = true;

            RegistrationInfo = new RegistrationInfo
            {
                ProductCode = "ModSoftPlc",
                ProductName = Name,
                CompCodeFileName = "CompCodeBu.txt",
                PermanentKeyUrl = "https://krdburan.blogspot.com/2025/01/getlicense.html",
                TrialKeyUrl = "https://krdburan.blogspot.com/2025/01/getlicense.html"
            };
        }

        public override string Name => Locale.IsRussian? "Модуль SoftPlc" :
            "SoftPlc Module";

        /// <summary>
        /// Gets the module description.
        /// </summary>
        public override string Descr
        {
            get
            {
                return Locale.IsRussian ?
                    "Автор Бурахин Андрей email: aburakhin@bk.ru\n" +
                    "Модуль запуска программ пользователя\n" +
                    "Позволяет запускать программы, написанные на C#, имитирующие работу ПЛК" :

                    "Author Andrey Burakhin email: aburakhin@bk.ru\n" +
                    "User Program Launcher\n" +
                    "Allows you to run programs written in C# that simulate the operation of a PLC";
            }
        }

        /// <summary>
        /// Loads language dictionaries.
        /// </summary>
        public override void LoadDictionaries()
        {
            FrmModuleConfig.PrevLang.Clear();
            FrmModuleConfig.PrevLang = GetLangTo();

            if (!Locale.LoadDictionaries(AppDirs.LangDir, "ModSoftPlc", out string errMsg))
            {
                ScadaUiUtils.ShowError(errMsg);
            }
            else
            {
                ModulePhrases.Init();
            }
        }

        public static Dictionary<string, string> GetLangTo()
        {
            Dictionary<string, string> lang = new Dictionary<string, string>();
            PropertyInfo[] data = typeof(ModulePhrases).GetProperties();
            lang = data.ToDictionary(x => x.Name, x => x.GetValue(data)?.ToString() ?? "");
            return lang;
        }


        /// <summary>
        /// Shows a modal dialog box for editing module properties.
        /// </summary>
        public override bool ShowProperties()
        {
            return new FrmModuleConfig(ConfigDataset, AppDirs).ShowDialog() == DialogResult.OK;
        }
    }
}
