/*
 * Copyright 2017-2020 Oleksandr Kolodkin <alexandr.kolodkin@gmail.com>
 * 
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 * 
 *     http://www.apache.org/licenses/LICENSE-2.0
 * 
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 * 
 * 
 * Product  : Rapid SCADA
 * Module   : ModAlarm
 * Summary  : Module configuration
 * 
 * Author   : Alexandr Kolodkin
 * Created  : 2017
 * Modified : 2020
 * Fork     : Andrey Burakhin
 * Modified : 2025
 */

using Scada.Forms;
using Scada.Lang;

namespace Scada.Server.Modules.ModAlarm.View
{
    public class ModAlarmView : ModuleView
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public ModAlarmView()
        {
            CanShowProperties = true;
        }

        public override string Name => Locale.IsRussian ? "Модуль Alarm" :
            "Alarm Module";
        /// <summary>
        /// Gets the module description.
        /// </summary>
        public override string Descr
        {
            get
            {
                return Locale.IsRussian ?
                    "Серверный модуль для звуковой сигнализации." :
                    "Server module for the sound alarm.";
            }
        }

        /// <summary>
        /// Loads language dictionaries.
        /// </summary>
        public override void LoadDictionaries()
        {
            if (!Locale.LoadDictionaries(AppDirs.LangDir, "ModAlarm", out string errMsg))
                ScadaUiUtils.ShowError(errMsg);

            ModulePhrases.Init();
        }

        /// <summary>
        /// Shows a modal dialog box for editing module properties.
        /// </summary>
        public override bool ShowProperties()
        {
            return new FrmAlarmConfig(ConfigDataset, AppDirs).ShowDialog() == DialogResult.OK;
        }
    }
}
