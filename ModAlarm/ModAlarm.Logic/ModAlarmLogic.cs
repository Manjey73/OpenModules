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

using Scada.Data.Models;
using Scada.Lang;
using Scada.Log;
using Scada.Server.Lang;
using System.Text;
using static Scada.Server.Modules.ModAlarm.Config;

namespace Scada.Server.Modules.ModAlarm.Logic
{
    public class ModAlarmLogic : ModuleLogic
    {
        private const string InfoFileName = "ModAlarm.txt";
        private bool normalWork;          // признак нормальной работы модуля
        private string workState;         // строковая запись состояния работы
        private string infoFileName;      // полное имя файла информации
        private Config config;            // конфигурация модуля
        private SortedDictionary<int, bool> lastState = new SortedDictionary<int, bool>();          // предыдущее состояние сигнала аварии
        private Dictionary<int, string> fileWave = new Dictionary<int, string>();

        private bool terminated;

        Task PlayTask;
        private static CancellationTokenSource cts; // = new CancellationTokenSource();
        private CancellationToken token; // = cts.Token;

        /// <summary>
        /// The module log file name.
        /// </summary>
        private const string LogFileName = "ModAlarm.log";

        public ModAlarmLogic(IServerContext serverContext)
            : base(serverContext)
        {
            normalWork = true;
            workState = Locale.IsRussian ? "норма" : "normal";

            moduleLog = new LogFile(LogFormat.Simple)
            {
                FileName = Path.Combine(serverContext.AppDirs.LogDir, LogFileName),
                
                CapacityMB = serverContext.AppConfig.GeneralOptions.MaxLogSize
            };
        }

        public override string Code => "ModAlarm";
        private readonly ILog moduleLog;            // the module log

        #region OnServiceStart
        public override void OnServiceStart()
        {
            // write to log
            moduleLog.WriteBreak();
            moduleLog.WriteAction(ServerPhrases.StartModule, Code, Version);

            // определение полного имени файла информации
            infoFileName = ServerContext.AppDirs.LogDir + InfoFileName;

            // загрузка конфигурации
            config = new Config(ServerContext.AppDirs.ConfigDir);

            string errMsg;

            if (config.Load(out errMsg))
            {
                foreach (int channel in config.channels.Keys)
                {
                    lastState.Add(channel, false);
                }
                WriteInfo();

                cts = new CancellationTokenSource();
                token = cts.Token;
                PlayTask = new Task(() => Sound(), token); // Старт задачи воспроизведения звука

                StartTask(PlayTask);
            }
            else
            {
                normalWork = false;
                workState = Locale.IsRussian ? "ошибка" : "error";
                moduleLog.WriteAction(errMsg);
                // Замена NormalModExecImpossible
                moduleLog.WriteAction(Locale.IsRussian ? 
                   "Нормальная работа модуля невозможна" :
                   "Normal module execution is impossible");
            }
        }
        #endregion OnServiceStart

        #region OnServiceStop
        public override void OnServiceStop()
        {
            terminated = true;
            cts.Cancel();

            // write to log
            moduleLog.WriteAction(ServerPhrases.StopModule, Code);
            moduleLog.WriteBreak();
        }
        #endregion OnServiceStop

        public void StartTask(Task t)
        {
            if (t.Status == TaskStatus.Running)
                return;
            else
            {
                //moduleLog.WriteLine($"Running Task {PlayTask.Status}");
                if (t.IsFaulted)
                {
                    cts = new CancellationTokenSource();
                    token = cts.Token;
                    PlayTask = new Task(() => Sound(), token); // Старт новой задачи воспроизведения звука при сбое
                    StartTask(PlayTask);
                }
                else
                    t.Start();
            }
        }

        #region OnIteration
        public override void OnIteration()
        {
            if (normalWork)
            {
                StartTask(PlayTask);

                try
                {
                    foreach (int channel in config.channels.Keys)
                    {
                        CnlData curData = ServerContext.GetCurrentData(channel);

                        if (curData.IsDefined) // curSrez.GetCnlData(channel, out cnlData)
                        {
                            //bool state = Math.Abs(curData.Val) >= 1; // Как-то сюда добавить именно способ проверки - ветвление // TEST

                            //moduleLog.WriteLine($"TEST channel {channel} expression {config.channels[channel].expression}  equal {config.channels[channel].equal}  val {config.channels[channel].val}  laststate {lastState[channel]}");

                            bool state = CheckEqual(config.channels[channel], curData.Val);

                            if (lastState[channel] != state)
                            {
                                lastState[channel] = state;
                                if (state) StartAlarm(channel); else StopAlarm(channel);
                            }
                        }
                    }

                    if (token.IsCancellationRequested)
                    {
                        //moduleLog.WriteError("Задача умерла");
                        //moduleLog.WriteLine($"Task Status: {PlayTask.Status}");
                        return;
                    }
                }
                catch (Exception ex)
                {
                    moduleLog.WriteError(ex);
                }
            }
        }
        #endregion OnIteration

        #region OnCurrentDataProcessed
        /// <summary>
        /// Метод выполняется после вычисления дорасчётных каналов текущего среза (примерно каждые 100 мс)
        /// </summary>
        //public override void OnCurrentDataProcessed(Slice slice)
        //{
        //}
        #endregion OnCurrentDataProcessed

        #region OnCommand
        /// <summary>
        /// Performs actions after receiving and before enqueuing a telecontrol command.
        /// </summary>
        //public override void OnCommand(TeleCommand command, CommandResult commandResult)
        //{
        //}
        #endregion OnCommand

        #region Write Info
        /// <summary>
        /// Записать в файл информацию о работе модуля
        /// </summary>
        private void WriteInfo()
        {
            try
            {
                // формирование текста
                StringBuilder sbInfo = new StringBuilder();

                if (Locale.IsRussian)
                {
                    sbInfo
                        .AppendLine("Модуль аварийной сигнализации")
                        .AppendLine("-----------------------------")
                        .Append("Состояние: ").AppendLine(workState).AppendLine();

                    foreach (KeyValuePair<int, Config.ChannelProp> channel in config.channels) // foreach (KeyValuePair<int, string> channel in config.channels)
                    {
                        sbInfo
                            .Append("Канал: ").Append(channel.Key)
                            .Append(", Аудио файл: ").Append(channel.Value.path)
                            .Append(", Выражение: ").AppendLine(channel.Value.expression);

                        if (!File.Exists(channel.Value.path))
                        {
                            moduleLog.WriteError(string.Format("Ошибка файл '{0}' не найден.", channel.Value.path));
                        }
                    }
                }
                else
                {
                    sbInfo
                        .AppendLine("Sound Alarm Module")
                        .AppendLine("------------------")
                        .Append("State: ").AppendLine(workState).AppendLine();

                    foreach (KeyValuePair<int, Config.ChannelProp> channel in config.channels) // foreach (KeyValuePair<int, string> channel in config.channels)
                    {
                        sbInfo
                            .Append("Channel: ").Append(channel.Key)
                            .Append(", Sound file: ").Append(channel.Value.path)
                            .Append(", Expression: ").AppendLine(channel.Value.expression);

                        if (!File.Exists(channel.Value.path))
                        {
                            moduleLog.WriteError(string.Format("Error file '{0}' not found.", channel.Value.path));
                        }
                    }
                }

                // вывод в файл
                using (StreamWriter writer = new StreamWriter(infoFileName, false, Encoding.UTF8))
                    writer.Write(sbInfo.ToString());
            }
            catch (ThreadAbortException)
            {
            }
            catch (Exception ex)
            {
                // Замена ModPhrases.WriteInfoError - ActTypes ???
                moduleLog.WriteError(ServerPhrases.WriteFileError + ": " + ex.Message);
            }
        }
        #endregion Write Info

        #region Start Alarm
        /// <summary>
        /// Запустить воспроизведение звука (поддерживается wav формат)
        /// </summary>
        private void StartAlarm(int channel)
        {
            try
            {
                if (!fileWave.ContainsKey(channel))
                {
                    fileWave.Add(channel, config.channels[channel].path);
                }
            }
            catch (Exception ex)
            {
                moduleLog.WriteAction(string.Format(Locale.IsRussian ?
                    "Ошибка при воспроизведении аудиофайла {0}: {1}" :
                    "Error playing audio file {0}: {1}", config.channels[channel], ex.Message));
            }
        }
        #endregion Start Alarm

        #region Stop Alarm
        /// <summary>
        /// Остановить воспроизведение звука
        /// </summary>
        private void StopAlarm(int channel)
        {
            try
            {
                if (fileWave.ContainsKey(channel) && (fileWave[channel] != null))
                {
                    moduleLog.WriteAction(string.Format("Delete: {0}", channel));
                    fileWave.Remove(channel);
                }
            }
            catch (Exception ex)
            {
                moduleLog.WriteAction(string.Format(Locale.IsRussian ?
                    "Ошибка при остановке воспроизведения аудиофайла '{0}': {1}" :
                    "Error while stoping audio file '{0}': {1}", config.channels[channel], ex.Message));
            }
        }
        #endregion Stop Alarm

        #region Play Sound
        private void Sound()
        {
            while (!terminated)
            {
                Dictionary<int, string> WaveNew = new Dictionary<int, string>(fileWave);

                if (WaveNew.Count > 0)
                {
                    foreach (var s in WaveNew)
                    {
                        Wav.Play(s.Value, Wav.SoundFlags.SND_SYNC | Wav.SoundFlags.SND_FILENAME | Wav.SoundFlags.SND_SYSTEM);
                        Thread.Sleep(1000); // Сделать настройку паузы в конфиге.
                    }
                }
            }
        }
        #endregion Play Sound

        #region Check Equal
        private bool CheckEqual(ChannelProp channelProps, double curDataVal)
        {
            bool state = false;
            switch (channelProps.equal)
            {
                case "<":
                    {
                        state = curDataVal < channelProps.val; 
                        break;
                    }
                case "<=":
                    {
                        state = curDataVal <= channelProps.val;
                        break;
                    }
                case ">":
                    {
                        state = curDataVal > channelProps.val;
                        break;
                    }
                case ">=":
                    {
                        state = curDataVal >= channelProps.val;
                        break;
                    }
                case "=":
                    {
                        state = curDataVal == channelProps.val;
                        break;
                    }
                case "<>":
                    {
                        state = curDataVal != channelProps.val;
                        break;
                    }
            }
            return state;
        }
        #endregion Check Equal
    }
}
