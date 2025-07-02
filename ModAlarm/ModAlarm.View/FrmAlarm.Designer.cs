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

namespace Scada.Server.Modules.ModAlarm.View
{
    partial class FrmAlarm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            btnOk = new Button();
            btnCansel = new Button();
            lblChannel = new Label();
            inputChannel = new NumericUpDown();
            lblPath = new Label();
            inputPath = new TextBox();
            btnBrowse = new Button();
            openFileDialog = new OpenFileDialog();
            btnTest = new CheckBox();
            inputExpression = new TextBox();
            lblEqual = new Label();
            ((System.ComponentModel.ISupportInitialize)inputChannel).BeginInit();
            SuspendLayout();
            // 
            // btnOk
            // 
            btnOk.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            btnOk.Location = new Point(106, 143);
            btnOk.Name = "btnOk";
            btnOk.Size = new Size(88, 26);
            btnOk.TabIndex = 0;
            btnOk.Text = "Ok";
            btnOk.UseVisualStyleBackColor = true;
            btnOk.Click += btnOk_Click;
            // 
            // btnCansel
            // 
            btnCansel.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            btnCansel.DialogResult = DialogResult.Cancel;
            btnCansel.Location = new Point(13, 143);
            btnCansel.Name = "btnCansel";
            btnCansel.Size = new Size(88, 26);
            btnCansel.TabIndex = 1;
            btnCansel.Text = "Отмена";
            btnCansel.UseVisualStyleBackColor = true;
            // 
            // lblChannel
            // 
            lblChannel.AutoSize = true;
            lblChannel.Location = new Point(10, 8);
            lblChannel.Name = "lblChannel";
            lblChannel.Size = new Size(143, 15);
            lblChannel.TabIndex = 2;
            lblChannel.Text = "Номер входного канала:";
            // 
            // inputChannel
            // 
            inputChannel.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            inputChannel.Location = new Point(13, 27);
            inputChannel.Maximum = new decimal(new int[] { int.MaxValue, 0, 0, 0 });
            inputChannel.Name = "inputChannel";
            inputChannel.Size = new Size(240, 23);
            inputChannel.TabIndex = 3;
            inputChannel.TextAlign = HorizontalAlignment.Center;
            inputChannel.ValueChanged += inputChannel_ValueChanged;
            // 
            // lblPath
            // 
            lblPath.AutoSize = true;
            lblPath.Location = new Point(10, 51);
            lblPath.Name = "lblPath";
            lblPath.Size = new Size(73, 15);
            lblPath.TabIndex = 4;
            lblPath.Text = "Аудиофайл:";
            // 
            // inputPath
            // 
            inputPath.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            inputPath.Location = new Point(13, 71);
            inputPath.Name = "inputPath";
            inputPath.Size = new Size(414, 23);
            inputPath.TabIndex = 5;
            inputPath.TextAlign = HorizontalAlignment.Center;
            inputPath.TextChanged += inputPath_TextChanged;
            // 
            // btnBrowse
            // 
            btnBrowse.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnBrowse.Location = new Point(339, 143);
            btnBrowse.Name = "btnBrowse";
            btnBrowse.Size = new Size(88, 26);
            btnBrowse.TabIndex = 6;
            btnBrowse.Text = "Обзор";
            btnBrowse.UseVisualStyleBackColor = true;
            btnBrowse.Click += btnBrowse_Click;
            // 
            // openFileDialog
            // 
            openFileDialog.Filter = "WAV аудио файл|*.wav";
            // 
            // btnTest
            // 
            btnTest.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnTest.Appearance = Appearance.Button;
            btnTest.Location = new Point(246, 143);
            btnTest.Name = "btnTest";
            btnTest.Size = new Size(88, 26);
            btnTest.TabIndex = 7;
            btnTest.Text = "Проверка";
            btnTest.TextAlign = ContentAlignment.MiddleCenter;
            btnTest.UseVisualStyleBackColor = true;
            btnTest.CheckedChanged += btnTest_CheckedChanged;
            // 
            // inputExpression
            // 
            inputExpression.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            inputExpression.Location = new Point(259, 27);
            inputExpression.Name = "inputExpression";
            inputExpression.Size = new Size(168, 23);
            inputExpression.TabIndex = 8;
            inputExpression.TextAlign = HorizontalAlignment.Center;
            inputExpression.TextChanged += inputExpression_TextChanged;
            // 
            // lblEqual
            // 
            lblEqual.AutoSize = true;
            lblEqual.Location = new Point(259, 8);
            lblEqual.Name = "lblEqual";
            lblEqual.Size = new Size(124, 15);
            lblEqual.TabIndex = 9;
            lblEqual.Text = "Значение сравнения:";
            // 
            // FrmAlarm
            // 
            AcceptButton = btnOk;
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            CancelButton = btnCansel;
            ClientSize = new Size(437, 187);
            Controls.Add(lblEqual);
            Controls.Add(inputExpression);
            Controls.Add(btnTest);
            Controls.Add(btnBrowse);
            Controls.Add(inputPath);
            Controls.Add(lblPath);
            Controls.Add(inputChannel);
            Controls.Add(lblChannel);
            Controls.Add(btnCansel);
            Controls.Add(btnOk);
            MaximizeBox = false;
            MinimizeBox = false;
            MinimumSize = new Size(440, 226);
            Name = "FrmAlarm";
            ShowIcon = false;
            SizeGripStyle = SizeGripStyle.Hide;
            StartPosition = FormStartPosition.CenterParent;
            Text = "Добавить аварию";
            FormClosing += FrmAlarm_FormClosing;
            Load += FrmAlarm_Load;
            Shown += FrmAlarm_Shown;
            ((System.ComponentModel.ISupportInitialize)inputChannel).EndInit();
            ResumeLayout(false);
            PerformLayout();

        }

        #endregion

        private Button btnOk;
        private Button btnCansel;
        private Label lblChannel;
        private NumericUpDown inputChannel;
        private Label lblPath;
        private TextBox inputPath;
        private Button btnBrowse;
        private OpenFileDialog openFileDialog;
        private CheckBox btnTest;
        private TextBox inputExpression;
        private Label lblEqual;
    }
}
