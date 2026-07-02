

namespace Scada.Server.Modules.ModSoftPlc.View.Forms
{
    partial class FrmModuleConfig
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
            components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmModuleConfig));
            split1 = new SplitContainer();
            trvRoot = new TreeView();
            imgList = new ImageList(components);
            rtb_Log = new RichTextBox();
            tabControl = new TabControl();
            tStrip = new ToolStrip();
            tbNew = new ToolStripButton();
            btOpen = new ToolStripButton();
            tbSave = new ToolStripButton();
            tbSaveAs = new ToolStripButton();
            tbFind = new ToolStripButton();
            tbCalendar = new ToolStripButton();
            btCancel = new Button();
            btOK = new Button();
            OFD = new OpenFileDialog();
            lblFeedback = new Label();
            SFD = new SaveFileDialog();
            ((System.ComponentModel.ISupportInitialize)split1).BeginInit();
            split1.Panel1.SuspendLayout();
            split1.Panel2.SuspendLayout();
            split1.SuspendLayout();
            tStrip.SuspendLayout();
            SuspendLayout();
            // 
            // split1
            // 
            split1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            split1.Location = new Point(5, 41);
            split1.Name = "split1";
            // 
            // split1.Panel1
            // 
            split1.Panel1.Controls.Add(trvRoot);
            // 
            // split1.Panel2
            // 
            split1.Panel2.Controls.Add(rtb_Log);
            split1.Panel2.Controls.Add(tabControl);
            split1.Size = new Size(1088, 544);
            split1.SplitterDistance = 397;
            split1.TabIndex = 0;
            // 
            // trvRoot
            // 
            trvRoot.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            trvRoot.BackColor = SystemColors.Window;
            trvRoot.HideSelection = false;
            trvRoot.ImageIndex = 0;
            trvRoot.ImageList = imgList;
            trvRoot.LineColor = Color.White;
            trvRoot.Location = new Point(3, 4);
            trvRoot.Name = "trvRoot";
            trvRoot.SelectedImageIndex = 0;
            trvRoot.Size = new Size(391, 537);
            trvRoot.TabIndex = 0;
            trvRoot.AfterSelect += trvRoot_AfterSelect;
            trvRoot.NodeMouseClick += trvRoot_NodeMouseClick;
            trvRoot.MouseDown += trvRoot_MouseDown;
            // 
            // imgList
            // 
            imgList.ColorDepth = ColorDepth.Depth32Bit;
            imgList.ImageStream = (ImageListStreamer)resources.GetObject("imgList.ImageStream");
            imgList.TransparentColor = Color.Transparent;
            imgList.Images.SetKeyName(0, "pngegg.png");
            imgList.Images.SetKeyName(1, "settings.png");
            imgList.Images.SetKeyName(2, "program.png");
            imgList.Images.SetKeyName(3, "variable.png");
            imgList.Images.SetKeyName(4, "calendar.png");
            imgList.Images.SetKeyName(5, "day.png");
            imgList.Images.SetKeyName(6, "week.png");
            imgList.Images.SetKeyName(7, "text.png");
            imgList.Images.SetKeyName(8, "comment.png");
            imgList.Images.SetKeyName(9, "delete.png");
            imgList.Images.SetKeyName(10, "label-important.png");
            imgList.Images.SetKeyName(11, "label-out.png");
            // 
            // rtb_Log
            // 
            rtb_Log.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            rtb_Log.BackColor = SystemColors.Control;
            rtb_Log.Location = new Point(3, 411);
            rtb_Log.Name = "rtb_Log";
            rtb_Log.Size = new Size(677, 130);
            rtb_Log.TabIndex = 1;
            rtb_Log.Text = "";
            // 
            // tabControl
            // 
            tabControl.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            tabControl.Location = new Point(3, 4);
            tabControl.Name = "tabControl";
            tabControl.SelectedIndex = 0;
            tabControl.Size = new Size(681, 401);
            tabControl.TabIndex = 0;
            // 
            // tStrip
            // 
            tStrip.AutoSize = false;
            tStrip.ImageScalingSize = new Size(32, 32);
            tStrip.Items.AddRange(new ToolStripItem[] { tbNew, btOpen, tbSave, tbSaveAs, tbFind, tbCalendar });
            tStrip.Location = new Point(0, 0);
            tStrip.Name = "tStrip";
            tStrip.Size = new Size(1098, 38);
            tStrip.TabIndex = 1;
            tStrip.Text = "toolStrip1";
            // 
            // tbNew
            // 
            tbNew.DisplayStyle = ToolStripItemDisplayStyle.Image;
            tbNew.Image = (Image)resources.GetObject("tbNew.Image");
            tbNew.ImageTransparentColor = Color.Magenta;
            tbNew.Name = "tbNew";
            tbNew.Size = new Size(36, 35);
            tbNew.Text = "New Configuration";
            tbNew.Click += tbNew_Click;
            // 
            // btOpen
            // 
            btOpen.DisplayStyle = ToolStripItemDisplayStyle.Image;
            btOpen.Image = (Image)resources.GetObject("btOpen.Image");
            btOpen.ImageTransparentColor = Color.Magenta;
            btOpen.Name = "btOpen";
            btOpen.Size = new Size(36, 35);
            btOpen.Text = "Open";
            btOpen.Click += btOpen_Click;
            // 
            // tbSave
            // 
            tbSave.DisplayStyle = ToolStripItemDisplayStyle.Image;
            tbSave.Image = (Image)resources.GetObject("tbSave.Image");
            tbSave.ImageTransparentColor = Color.Magenta;
            tbSave.Name = "tbSave";
            tbSave.Size = new Size(36, 35);
            tbSave.Text = "Save";
            tbSave.Click += tbSave_Click;
            // 
            // tbSaveAs
            // 
            tbSaveAs.DisplayStyle = ToolStripItemDisplayStyle.Image;
            tbSaveAs.Image = (Image)resources.GetObject("tbSaveAs.Image");
            tbSaveAs.ImageTransparentColor = Color.Magenta;
            tbSaveAs.Name = "tbSaveAs";
            tbSaveAs.Size = new Size(36, 35);
            tbSaveAs.Text = "SaveAs";
            tbSaveAs.Click += tbSaveAs_Click;
            // 
            // tbFind
            // 
            tbFind.Alignment = ToolStripItemAlignment.Right;
            tbFind.DisplayStyle = ToolStripItemDisplayStyle.Image;
            tbFind.Image = (Image)resources.GetObject("tbFind.Image");
            tbFind.ImageTransparentColor = Color.Magenta;
            tbFind.Margin = new Padding(0, 1, 3, 2);
            tbFind.Name = "tbFind";
            tbFind.Size = new Size(36, 35);
            tbFind.Text = "Find Channel";
            // 
            // tbCalendar
            // 
            tbCalendar.Alignment = ToolStripItemAlignment.Right;
            tbCalendar.DisplayStyle = ToolStripItemDisplayStyle.Image;
            tbCalendar.Image = (Image)resources.GetObject("tbCalendar.Image");
            tbCalendar.ImageTransparentColor = Color.Magenta;
            tbCalendar.Margin = new Padding(0, 1, 3, 2);
            tbCalendar.Name = "tbCalendar";
            tbCalendar.Size = new Size(36, 35);
            tbCalendar.Text = "Production calendar";
            tbCalendar.Click += tbCalendar_Click;
            // 
            // btCancel
            // 
            btCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btCancel.Location = new Point(986, 596);
            btCancel.Name = "btCancel";
            btCancel.Size = new Size(100, 31);
            btCancel.TabIndex = 2;
            btCancel.Text = "Cancel";
            btCancel.UseVisualStyleBackColor = true;
            btCancel.Click += btCancel_Click;
            // 
            // btOK
            // 
            btOK.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btOK.Location = new Point(880, 596);
            btOK.Name = "btOK";
            btOK.Size = new Size(100, 31);
            btOK.TabIndex = 3;
            btOK.Text = "OK";
            btOK.UseVisualStyleBackColor = true;
            btOK.Click += btOK_Click;
            // 
            // OFD
            // 
            OFD.FileName = "openFileDialog";
            // 
            // lblFeedback
            // 
            lblFeedback.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            lblFeedback.AutoSize = true;
            lblFeedback.Location = new Point(12, 601);
            lblFeedback.Name = "lblFeedback";
            lblFeedback.Size = new Size(57, 15);
            lblFeedback.TabIndex = 4;
            lblFeedback.Text = "Feadback";
            // 
            // FrmModuleConfig
            // 
            ClientSize = new Size(1098, 639);
            Controls.Add(lblFeedback);
            Controls.Add(btOK);
            Controls.Add(btCancel);
            Controls.Add(tStrip);
            Controls.Add(split1);
            Icon = (Icon)resources.GetObject("$this.Icon");
            Name = "FrmModuleConfig";
            FormClosing += FrmModuleConfig_FormClosing;
            Load += FrmModuleConfig_Load;
            split1.Panel1.ResumeLayout(false);
            split1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)split1).EndInit();
            split1.ResumeLayout(false);
            tStrip.ResumeLayout(false);
            tStrip.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private SplitContainer split1;
        public TreeView trvRoot;
        private TabControl tabControl;
        public ToolStrip tStrip;
        private ToolStripButton btOpen;
        private ToolStripButton tbSave;
        private ToolStripButton tbSaveAs;
        private ToolStripButton tbNew;
        private Button btCancel;
        public Button btOK;
        private OpenFileDialog OFD;
        public ImageList imgList;
        private Label lblFeedback;
        private SaveFileDialog SFD;
        public RichTextBox rtb_Log;
        private ToolStripButton tbFind;
        private ToolStripButton tbCalendar;
    }
}
