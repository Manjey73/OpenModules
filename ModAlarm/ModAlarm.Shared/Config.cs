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
using Scada.Server.Lang;
using System.Globalization;
using System.Xml;

namespace Scada.Server.Modules.ModAlarm
{
    /// <summary>
    /// Module configuration
    /// <para>Конфигурация модуля</para>
    /// </summary>
    internal class Config
    {
        /// <summary>
        /// Имя файла конфигурации
        /// </summary>
        private const string configFileName = "ModAlarm.xml";


        /// <summary>
        /// Список каналов и аудиофайлов
        /// </summary>
        public SortedDictionary<int, ChannelProp> channels = new SortedDictionary<int, ChannelProp>(); // TEST SortedDictionary<int,string>

        // TEST класс для добавления пути и выражения
        public class ChannelProp
        {
            public string path;
            public string expression;
            public string equal;    // занести тип сравнения
            public double val;      // занести значение для сравнения
        }

        /// <summary>
        /// Конструктор, ограничивающий создание объекта без параметров
        /// </summary>
        private Config()
        {
        }

        /// <summary>
        /// Конструктор
        /// </summary>
        public Config(string configDir)
        {
            fileName = ScadaUtils.NormalDir(configDir) + configFileName;
            SetToDefault();
        }


        /// <summary>
        /// Получить полное имя файла конфигурации
        /// </summary>
        public string fileName { get; private set; }


        /// <summary>
        /// Установить значения параметров конфигурации по умолчанию
        /// </summary>
        private void SetToDefault()
        {
            channels.Clear();
        }


        /// <summary>
        /// Добавить аварию
        /// </summary>
        public bool AddChannel(int channel, string path, string expression)
        {
            if (channel < 0) return false;
            if (channel > int.MaxValue) return false;
            if (path == "") return false;
            if (!File.Exists(path)) return false;
            if (channels.ContainsKey(channel)) return false;

            ChannelProp chProp = GetChannelProp(path, expression);
            channels.Add(channel, chProp); // new ChannelProp { path = path, expression = expression, equal = chProp.equal, val = chProp.val }
            return true;
        }


        /// <summary>
        /// Изменить аварию
        /// </summary>
        public bool UpdateChannel(int old_channel, int new_channel, string path, string expression)
        {
            if (new_channel < 0) return false;
            if (new_channel > int.MaxValue) return false;
            if (path == "") return false;
            if (!File.Exists(path)) return false;
            if (!channels.ContainsKey(old_channel)) return false;

            channels.Remove(old_channel);

            ChannelProp chProp = GetChannelProp(path, expression);
            channels.Add(new_channel, chProp); // new ChannelProp { path = path, expression = expression, equal = chProp.equal, val = chProp.val }
            return true;
        }


        /// <summary>
        /// Удалить аварию
        /// </summary>
        public bool RemoveChannel(int channel)
        {
            if (!channels.ContainsKey(channel)) return false;
            channels.Remove(channel);
            return true;
        }


        /// <summary>
        /// Загрузить конфигурацию модуля
        /// </summary>
        public bool Load(out string errMsg)
        {
            SetToDefault();

            try
            {
                errMsg = "";

                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(fileName);
                XmlNode root = xmlDoc.DocumentElement;

                foreach (XmlNode alert in root.SelectNodes("Alarm"))
                {
                    int channel = alert.GetChildAsInt("Channel", -1);
                    string soundFileName = alert.GetChildAsString("Sound", "");
                    string Expression = alert.GetChildAsString("Expression", "");

                    AddChannel(channel, soundFileName, Expression);
                }

                // Конвертация настроек модуля версии 1.1 и ниже
                int oldChannel = root.GetChildAsInt("Chanel", -1);
                string oldSound = root.GetChildAsString("Sound", "");
                string oldExpression = root.GetChildAsString("Expression", "");
                if (AddChannel(oldChannel, oldSound, oldExpression)) Save(out errMsg);

                return true;
            }
            catch (FileNotFoundException ex)
            {
                // Замена ModPhrases.LoadModSettingsError
                errMsg = ServerPhrases.LoadModuleConfigError + ": " + ex.Message +
                    Environment.NewLine + string.Format(Locale.IsRussian ?
                    "Сконфигурируйте модуль и перезапустите службу SCADA-Сервера" :
                    "Configure the module and restart SCADA-Server service"); // ModPhrases.ConfigureModule;
                return false;
            }
            catch (Exception ex)
            {
                errMsg = ServerPhrases.LoadModuleConfigError + ": " + ex.Message;
                return false;
            }
        }

        /// <summary>
        /// Сохранить конфигурацию модуля
        /// </summary>
        public bool Save(out string errMsg)
        {
            try
            {
                XmlDocument xmlDoc = new XmlDocument();

                XmlDeclaration xmlDecl = xmlDoc.CreateXmlDeclaration("1.0", "utf-8", null);
                xmlDoc.AppendChild(xmlDecl);

                XmlElement root = xmlDoc.CreateElement("ModAlarm");
                xmlDoc.AppendChild(root);

                foreach (KeyValuePair<int, ChannelProp> channel in channels) // foreach(KeyValuePair<int, string> channel in channels)
                {
                    XmlElement alert = xmlDoc.CreateElement("Alarm");
                    alert.AppendElem("Channel", channel.Key);
                    alert.AppendElem("Sound", channel.Value.path);
                    alert.AppendElem("Expression", channel.Value.expression);
                    root.AppendChild(alert);
                }

                xmlDoc.Save(fileName);
                errMsg = "";
                return true;
            }
            catch (Exception ex)
            {
                // Замена ModPhrases.SaveModSettingsError 
                errMsg = ServerPhrases.SaveModuleConfigError + ": " + ex.Message;
                return false;
            }
        }

        /// <summary>
        /// Клонировать конфигурацию модуля
        /// </summary>
        public Config Clone()
        {
            Config configCopy = new Config();
            configCopy.fileName = fileName;
            configCopy.channels = channels;
            return configCopy;
        }

        #region Get Channel Propertie
        private ChannelProp GetChannelProp(string path, string expression)
        {
            ChannelProp channelProp = new ChannelProp();
            string express = expression.Replace(" ", ""); // Удалить пробелы
            string equal = ">=";    // по умолчанию
            double val = 1;         // по умолчанию
            string[] getEqual = new string[2];

            if (!string.IsNullOrEmpty(express))
            {
                getEqual = GetEqual(express);
                equal = getEqual[0];
                val = SToDouble(getEqual[1]);
            }

            channelProp.path = path;
            channelProp.expression = expression; // Для отображения с пробелами
            channelProp.equal = equal;
            channelProp.val = val;

            return channelProp;
        }
        #endregion Get Channel Propertie

        #region Get Equal
        private string[] GetEqual(string express)
        {
            string[] eq = new string[2];
            switch (express)
            {
                case string x when x.StartsWith("<") && !x.StartsWith("<="): // Проверка на <
                    {
                        eq[0] = "<";
                        eq[1] = x.Substring(1);
                        break;
                    }
                case string x when x.StartsWith("<="): // Проверка на <=
                    {
                        eq[0] = "<=";
                        eq[1] = x.Substring(2);
                        break;
                    }
                case string x when x.StartsWith(">") && !x.StartsWith(">="): // Проверка на >
                    {
                        eq[0] = ">";
                        eq[1] = x.Substring(1);
                        break;
                    }
                case string x when x.StartsWith(">="): // Проверка на >=
                    {
                        eq[0] = ">=";
                        eq[1] = x.Substring(2);
                        break;
                    }
                case string x when x.StartsWith("=") || x.StartsWith("=="): // Проверка на = или ==
                    {
                        eq[0] = "=";
                        eq[1] = x.StartsWith("==") ? x.Substring(2) : x.Substring(1);
                        break;
                    }
                case string x when x.StartsWith("<>") || x.StartsWith("!="): // Проверка на != или <>
                    {
                        eq[0] = "<>";
                        eq[1] = x.Substring(2);
                        break;
                    }
                default:
                    {
                        eq[0] = ">=";
                        eq[1] = "1";
                        break;
                    }
            }
            return eq;
        }
        #endregion Get Equal

        #region StringToDouble
        private double SToDouble(string s) // Позволяет в строке вводить дробные числа с ,(запятой) или .(точкой)
        {
            double result = 1;
            if (!double.TryParse(s, NumberStyles.Any, CultureInfo.GetCultureInfo("ru-RU"), out result))
            {
                if (!double.TryParse(s, NumberStyles.Any, CultureInfo.GetCultureInfo("en-US"), out result))
                {
                    return 1;
                }
            }
            return result;
        }
        #endregion StringToDouble

    }
}
