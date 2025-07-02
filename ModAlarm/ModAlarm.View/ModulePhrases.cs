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

using Scada.Lang;

namespace Scada.Server.Modules.ModAlarm.View
{
    internal class ModulePhrases
    {
        // Scada.Server.Modules.ModAlarm.View.FrmModuleConfig
        public static string msgAlreadyExists { get; private set; }
        public static string msgWarning { get; private set; }
        public static string isEdit { get; private set; }
        public static string columnChannel { get; private set; }
        public static string columnSoundFile { get; private set; }
        public static string columnEqual { get; private set; }


        public static void Init()
        {
            LocaleDict dict = Locale.GetDictionary("Scada.Server.Modules.ModAlarm.View.FrmAlarmConfig");
            LocaleDict dict1 = Locale.GetDictionary("Scada.Server.Modules.ModAlarm.View.FrmAlarm");

            msgAlreadyExists = dict[nameof(msgAlreadyExists)];
            msgWarning = dict[nameof(msgWarning)];
            columnChannel = dict[nameof(columnChannel)];
            columnSoundFile = dict[nameof(columnSoundFile)];
            columnEqual = dict[nameof(columnEqual)];

            isEdit = dict1[nameof(isEdit)];
        }
    }
}
