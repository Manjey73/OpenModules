/*
 * Copyright 2017-2018 Alexandr Kolodkin <alexandr.kolodkin@gmail.com>
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
 * Summary  : Server module user interface
 * 
 * Author   : Alexandr Kolodkin
 * Created  : 2017
 * Modified : 2018
 * Fork     : Andrey Burakhin
 * Modified : 2025
 */

namespace Scada.Server.Modules.ModAlarm.View
{
    partial class FrmAlarmConfig
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
            lblInfo = new Label();
            btnClose = new Button();
            btnSave = new Button();
            btnCancel = new Button();
            inputChannels = new ListView();
            columnChannel = new ColumnHeader();
            columnSoundFile = new ColumnHeader();
            btnAdd = new Button();
            btnRemove = new Button();
            columnEqual = new ColumnHeader();
            SuspendLayout();
            // 
            // lblInfo
            // 
            lblInfo.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            lblInfo.Location = new Point(11, 8);
            lblInfo.Margin = new Padding(4, 0, 4, 0);
            lblInfo.Name = "lblInfo";
            lblInfo.Size = new Size(554, 41);
            lblInfo.TabIndex = 0;
            lblInfo.Text = "Модуль воспроизводит выбранный звуковой сигнал пока значение указанного канала не равно 0";
            lblInfo.TextAlign = ContentAlignment.TopCenter;
            // 
            // btnClose
            // 
            btnClose.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnClose.DialogResult = DialogResult.Cancel;
            btnClose.Location = new Point(478, 365);
            btnClose.Margin = new Padding(4);
            btnClose.Name = "btnClose";
            btnClose.Size = new Size(88, 26);
            btnClose.TabIndex = 1;
            btnClose.Text = "Закрыть";
            btnClose.UseVisualStyleBackColor = true;
            // 
            // btnSave
            // 
            btnSave.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnSave.Location = new Point(289, 365);
            btnSave.Margin = new Padding(4);
            btnSave.Name = "btnSave";
            btnSave.Size = new Size(88, 26);
            btnSave.TabIndex = 2;
            btnSave.Text = "Сохранить";
            btnSave.UseVisualStyleBackColor = true;
            btnSave.Click += btnSave_Click;
            // 
            // btnCancel
            // 
            btnCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnCancel.Location = new Point(383, 365);
            btnCancel.Margin = new Padding(4);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new Size(88, 26);
            btnCancel.TabIndex = 3;
            btnCancel.Text = "Отменить";
            btnCancel.UseVisualStyleBackColor = true;
            btnCancel.Click += btnCancel_Click;
            // 
            // inputChannels
            // 
            inputChannels.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            inputChannels.Columns.AddRange(new ColumnHeader[] { columnChannel, columnSoundFile, columnEqual });
            inputChannels.FullRowSelect = true;
            inputChannels.GridLines = true;
            inputChannels.Location = new Point(10, 52);
            inputChannels.Name = "inputChannels";
            inputChannels.Size = new Size(555, 272);
            inputChannels.TabIndex = 5;
            inputChannels.UseCompatibleStateImageBehavior = false;
            inputChannels.View = System.Windows.Forms.View.Details;
            inputChannels.MouseDoubleClick += inputChannels_MouseDoubleClick;
            // 
            // columnChannel
            // 
            columnChannel.Text = "Канал";
            columnChannel.Width = 120;
            // 
            // columnSoundFile
            // 
            columnSoundFile.Text = "Аудиофайл";
            columnSoundFile.Width = 350;
            // 
            // btnAdd
            // 
            btnAdd.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnAdd.Location = new Point(478, 330);
            btnAdd.Name = "btnAdd";
            btnAdd.Size = new Size(88, 26);
            btnAdd.TabIndex = 7;
            btnAdd.Text = "Добавить";
            btnAdd.UseVisualStyleBackColor = true;
            btnAdd.Click += btnAdd_Click;
            // 
            // btnRemove
            // 
            btnRemove.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnRemove.Location = new Point(383, 330);
            btnRemove.Name = "btnRemove";
            btnRemove.Size = new Size(87, 26);
            btnRemove.TabIndex = 8;
            btnRemove.Text = "Удалить";
            btnRemove.UseVisualStyleBackColor = true;
            btnRemove.Click += btnRemove_Click;
            // 
            // columnEqual
            // 
            columnEqual.Text = "Выражение";
            columnEqual.Width = 80;
            // 
            // FrmAlarmConfig
            // 
            AcceptButton = btnSave;
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            CancelButton = btnClose;
            ClientSize = new Size(577, 404);
            Controls.Add(btnRemove);
            Controls.Add(btnAdd);
            Controls.Add(inputChannels);
            Controls.Add(btnCancel);
            Controls.Add(btnSave);
            Controls.Add(btnClose);
            Controls.Add(lblInfo);
            Margin = new Padding(4);
            MaximizeBox = false;
            MinimizeBox = false;
            MinimumSize = new Size(527, 237);
            Name = "FrmAlarmConfig";
            ShowIcon = false;
            ShowInTaskbar = false;
            SizeGripStyle = SizeGripStyle.Hide;
            StartPosition = FormStartPosition.CenterParent;
            Text = "Серверный модуль звуковой сигнализации";
            FormClosing += FrmAlarmConfig_FormClosing;
            Load += FrmAlarmConfig_Load;
            ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label lblInfo;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.ListView inputChannels;
        private System.Windows.Forms.ColumnHeader columnChannel;
        private System.Windows.Forms.ColumnHeader columnSoundFile;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.Button btnRemove;
        private ColumnHeader columnEqual;
    }
}
