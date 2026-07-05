using System.Xml.Linq;

namespace Scada.Server.Modules.ModSoftPlc.View.Forms
{
    partial class FrmParameters
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;
        public FrmModuleConfig frmParentGloabal;        // global general form


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
            dgvCData = new DataGridView();
            rtbNode = new RichTextBox();
            dgvAttribute = new DataGridView();
            gbComment = new GroupBox();
            rtbParam = new RichTextBox();
            ((System.ComponentModel.ISupportInitialize)dgvCData).BeginInit();
            ((System.ComponentModel.ISupportInitialize)dgvAttribute).BeginInit();
            gbComment.SuspendLayout();
            SuspendLayout();
            // 
            // dgvCData
            // 
            dgvCData.BackgroundColor = SystemColors.Control;
            dgvCData.BorderStyle = BorderStyle.None;
            dgvCData.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvCData.Location = new Point(10, 308);
            dgvCData.Margin = new Padding(3, 2, 3, 2);
            dgvCData.Name = "dgvCData";
            dgvCData.RowHeadersWidth = 51;
            dgvCData.Size = new Size(500, 104);
            dgvCData.TabIndex = 9;
            // 
            // rtbNode
            // 
            rtbNode.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            rtbNode.BackColor = SystemColors.Info;
            rtbNode.BorderStyle = BorderStyle.None;
            rtbNode.Location = new Point(5, 20);
            rtbNode.Name = "rtbNode";
            rtbNode.Size = new Size(490, 103);
            rtbNode.TabIndex = 5;
            rtbNode.Text = "";
            // 
            // dgvAttribute
            // 
            dgvAttribute.BackgroundColor = SystemColors.Control;
            dgvAttribute.BorderStyle = BorderStyle.None;
            dgvAttribute.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvAttribute.Location = new Point(10, 38);
            dgvAttribute.Margin = new Padding(3, 2, 3, 2);
            dgvAttribute.Name = "dgvAttribute";
            dgvAttribute.RowHeadersWidth = 51;
            dgvAttribute.Size = new Size(500, 125);
            dgvAttribute.TabIndex = 2;
            // 
            // gbComment
            // 
            gbComment.Controls.Add(rtbNode);
            gbComment.Location = new Point(10, 225);
            gbComment.Margin = new Padding(3, 2, 3, 2);
            gbComment.Name = "gbComment";
            gbComment.Padding = new Padding(3, 2, 3, 2);
            gbComment.Size = new Size(501, 128);
            gbComment.TabIndex = 10;
            gbComment.TabStop = false;
            gbComment.Text = "gbComment";
            // 
            // rtbParam
            // 
            rtbParam.Location = new Point(10, 8);
            rtbParam.Margin = new Padding(3, 2, 3, 2);
            rtbParam.Name = "rtbParam";
            rtbParam.Size = new Size(501, 76);
            rtbParam.TabIndex = 11;
            rtbParam.Text = "";
            // 
            // FrmParameters
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            AutoScroll = true;
            ClientSize = new Size(531, 421);
            Controls.Add(rtbParam);
            Controls.Add(dgvCData);
            Controls.Add(dgvAttribute);
            Controls.Add(gbComment);
            Name = "FrmParameters";
            Text = "FormParameters";
            Load += FrmParameters_Load;
            ((System.ComponentModel.ISupportInitialize)dgvCData).EndInit();
            ((System.ComponentModel.ISupportInitialize)dgvAttribute).EndInit();
            gbComment.ResumeLayout(false);
            ResumeLayout(false);
        }

        private void FrmParameters_FormClosing(object sender, FormClosingEventArgs e)
        {
            findCnl.Click -= Search_Click; // Отписка от кнопки поиска, чтобы не появлялось окно несколько раз
            throw new NotImplementedException();
        }
        #endregion

        private DataGridView dgvCData;
        private RichTextBox rtbNode;
        private DataGridView dgvAttribute;
        private GroupBox gbComment;
        private RichTextBox rtbParam;
    }
}
