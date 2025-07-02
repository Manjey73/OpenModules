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

namespace Scada.Server.Modules.ModAlarm.View
{
    public partial class FrmAlarm : Form
    {
        public string SoundFilePath;
        public int Channel;
        public bool isEdit = false;
        public string Expession = "";

        private AppDirs appDirs;         // директории приложения

        public FrmAlarm(AppDirs appDirs)
        {
            this.appDirs = appDirs;
            InitializeComponent();
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                inputPath.Text = openFileDialog.FileName;
            }
        }

        private void inputChannel_ValueChanged(object sender, EventArgs e)
        {
            Channel = Decimal.ToInt32(inputChannel.Value); // 

            // обновление состояние кнопки ОK
            UpdateOkButton();
        }

        private void inputPath_TextChanged(object sender, EventArgs e)
        {
            SoundFilePath = inputPath.Text;
            // обновление состояние кнопки ОK
            UpdateOkButton();
        }

        private void inputExpression_TextChanged(object sender, EventArgs e)
        {
            Expession = inputExpression.Text;
            UpdateOkButton();
        }


        private void btnOk_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void FrmAlarm_Load(object sender, EventArgs e)
        {
            // translate the form
            FormTranslator.Translate(this, GetType().FullName);

            if (isEdit) // else if
            {
                Text = ModulePhrases.isEdit; // "Изменить аварию"
            }
        }

        private void btnTest_CheckedChanged(object sender, EventArgs e)
        {
            if (btnTest.Checked)
            {
                try
                {
                    Wav.Play(inputPath.Text, Wav.SoundFlags.SND_LOOP | Wav.SoundFlags.SND_ASYNC);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error");
                }
            }
            else
            {
                try
                {
                    Wav.Play(null, Wav.SoundFlags.SND_PURGE);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error");
                }
            }
        }

        private void FrmAlarm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Wav.Play(null, Wav.SoundFlags.SND_PURGE | Wav.SoundFlags.SND_MEMORY);
        }

        private void UpdateOkButton()
        {
            btnOk.Enabled = (File.Exists(SoundFilePath) && (Channel >= 0) && (Channel <= int.MaxValue));
        }

        private void FrmAlarm_Shown(object sender, EventArgs e)
        {
            // задание начальных значений
            inputChannel.Value = Channel;
            inputPath.Text = SoundFilePath;
            inputExpression.Text = Expession;

            // обновление состояние кнопки ОK
            UpdateOkButton();
        }

    }
}
