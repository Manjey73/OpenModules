using Scada.Forms;
using Scada.Forms.Forms;
using System.Reflection;
using System.Xml.Linq;

namespace Scada.Server.Modules.ModSoftPlc.View.Forms
{
    public partial class FrmParameters : Form
    {
        #region Variables
        //public FrmModuleConfig frmParentGloabal;      // global general form
        public bool boolParent = false;                 // сhild startup flag
        private BindingSource bsAttribute = new BindingSource();
        private BindingSource bsCData = new BindingSource();
        Dictionary<string, object> tags;
        private IEnumerable<XAttribute> attr;
        private IEnumerable<XText> xtext;
        private IEnumerable<XCData> xcdata;
        private IEnumerable<TreeNode> query;
        Size dgvSize;
        Size rText;
        private int positionY = 10;
        int rowIndex = -1;
        int colIndex = -1;
        ToolStrip toolStrip;
        ToolStripButton findCnl;

        // Интерфейс
        private List<string> YesNo = new List<string> { "true", "false" }; // fill the drop down items.. Выпадающий список для ячейки с булевой переменной 
        public class AttributeOfVar()
        {
            public string VarType { get; set; }
            public string Description { get; set; }
            public string Data { get; set; }
        }

        private List<string> listOfVar;
        public Dictionary<string, AttributeOfVar> dictOfAttr = new Dictionary<string, AttributeOfVar>();

        // Список задач, которые требуется исключить из списка выбора. Должны быть доступны только те, программы которых можно выполнить.
        private List<string> ignoreTask = new List<string> { "SoftPlc", "FrmModuleConfig", "FrmParameters", "Calendar",  // "Func", "TON",
        "ProgramX", "DayX", "WeekX", "VariableX", "TitleX", "DayH","TimeSpanExtender", "ProgramzAttribute", "AttributeOfVar", 
        "CalendarFile", "Holiday", "CalendarDay"};
        private List<string> listOfTask;
        #endregion Variables

        public FrmParameters(ToolStrip _toolStrip) // С кнопкой работает ToolStripButton Search
        {
            InitializeComponent();

            rtbParam.TextChanged -= TxtParam_TextChanged; // TEST
            dgvAttribute.CellValueChanged -= DgvAttribute_CellValueChanged;
            dgvAttribute.CellClick -= DgvAttribute_CellClick;
            dgvAttribute.CurrentCellDirtyStateChanged -= DgvAttribute_CurrentCellDirtyStateChanged;
            dgvCData.CellValueChanged -= DgvCData_CellValueChanged;

            FormatWindow();

            rtbNode.ContentsResized += RtbNode_ContentsResized;

            toolStrip = _toolStrip;
            findCnl = toolStrip.Items.OfType<ToolStripButton>().Where(b => b.Name == "tbFind").FirstOrDefault();
            findCnl.Click += Search_Click;

            //prodCalendar = toolStrip.Items.OfType<ToolStripButton>().Where(b => b.Name == "tbCalendar").FirstOrDefault();
        }

        private void FormatWindow() // bool hasParent
        {
            FormBorderStyle = FormBorderStyle.None;
            Dock = DockStyle.Left | DockStyle.Top;
            TopLevel = false;
        }

        private void FrmParameters_Load(object sender, EventArgs e)
        {
            // translate the form
            FormTranslator.Translate(this, GetType().FullName);

            // translate menu
            //FormTranslator.Translate(cmnuLstVariable, GetType().FullName);

            ConfigToControls();

            // translate the listview
            //FormTranslator.Translate(lstVariables, GetType().FullName);
            Modified = frmParentGloabal.modified;
        }

        #region ConfigToControls
        private void ConfigToControls()
        {
            rtbParam.Visible = false;
            dgvAttribute.Visible = false;
            dgvCData.Visible = false;

            findCnl.Enabled = false;
            //prodCalendar.Enabled = false;

            var currentNode = ModSoftPlcView.fConfig.trvRoot.SelectedNode;
            var currentNodes = ModSoftPlcView.fConfig.trvRoot.SelectedNode.Nodes;

            if (currentNode.Tag is Dictionary<string, object>)
            {
                tags = currentNode.Tag as Dictionary<string, object>;
                var count = 0;
                var dgvRH = 0;

                if (tags != null && tags.ContainsKey("Text"))
                {
                    xtext = tags["Text"] as IEnumerable<XText>;

                    if (xtext != null && xtext.Any())
                    {
                        if (tags.ContainsKey("Attributes"))
                        {
                            rtbParam.Location = new Point(rtbParam.Location.X, positionY);

                            int rtbSize = rtbParam.Size.Height + positionY;
                            dgvAttribute.Location = new Point(12, rtbSize + 10); // TEST

                            positionY = rtbSize + 10;
                        }

                        rtbParam.Visible = true; // TEST в richTextBox

                        XText txt = xtext.FirstOrDefault();

                        if (xtext is not XCData)
                        {
                            //result = ModSoftPlcView.fConfig.trvRoot.SelectedNode.Nodes.OfType<TreeNode>()
                            //        .FirstOrDefault(node => node.Text.Equals("#text"));
                            //lbText.Text = result.Text;

                            rtbParam.Text = txt.Value; // TEST в richTextBox
                        }
                    }
                }
                else
                {
                    dgvAttribute.Location = new Point(12, 10); // TEST
                }

                if (tags != null && tags.ContainsKey("Attributes")) // Тест аттрибутов на null
                {
                    attr = tags["Attributes"] as IEnumerable<XAttribute>;

                    count = attr.Count();

                    bsAttribute.DataSource = attr;
                    dgvAttribute.DataSource = bsAttribute;

                    dgvRH = dgvAttribute.Rows[0].Height;

                    dgvAttribute.Size = new Size(588, (count + 1) * dgvRH);

                    gbComment.Location = new Point(12, (count + 1) * dgvRH + positionY + 10);     // Расположение GroupBox при наличии Атрибутов

                    dgvAttribute.AllowUserToResizeRows = false;                         // Запретить изменять размер строк
                    dgvAttribute.Visible = true;
                    dgvAttribute.RowHeadersVisible = false;
                    dgvAttribute.Columns["IsNamespaceDeclaration"].Visible = false;     // Скрыть столбец со служебными переменными
                    dgvAttribute.Columns["NextAttribute"].Visible = false;              // Скрыть столбец со служебными переменными
                    dgvAttribute.Columns["NodeType"].Visible = false;                   // Скрыть столбец со служебными переменными
                    dgvAttribute.Columns["PreviousAttribute"].Visible = false;          // Скрыть столбец со служебными переменными
                    dgvAttribute.Columns["BaseUri"].Visible = false;                    // Скрыть столбец со служебными переменными
                    dgvAttribute.Columns["Document"].Visible = false;                   // Скрыть столбец со служебными переменными
                    dgvAttribute.Columns["Parent"].Visible = false;                     // Скрыть столбец со служебными переменными
                    dgvAttribute.Columns["Name"].HeaderText = ModulePhrases.dgvAttrName;
                    dgvAttribute.Columns["Value"].HeaderText = ModulePhrases.dgvAttrValue;

                    dgvSize = dgvAttribute.Size;

                    dgvAttribute.Columns["Name"].Width = dgvSize.Width / 3;
                    dgvAttribute.Columns["Value"].Width = dgvSize.Width - (dgvSize.Width / 3) - 18;

                    //События для DataGridView
                    dgvAttribute.DataError += new DataGridViewDataErrorEventHandler(dgvAttribute_DataError);
                }

                List<TreeNode> listquery = currentNodes.Cast<TreeNode>().Where(node => node.Text.Contains("#comment")).ToList();

                if (listquery != null && listquery.Count > 0)
                {
                    var cnt = listquery.Count;
                    foreach (TreeNode node in listquery)
                    {
                        rtbNode.Text += $"{((XComment)node.Tag).Value}"; // TEST Новая линия при выводе комментариев
                        if (cnt > 1) rtbNode.Text += Environment.NewLine;
                        cnt--;
                    }
                    gbComment.Size = new Size(568, rText.Height + 36);
                }
                else
                    gbComment.Visible = false;

                if (tags != null && tags.ContainsKey("CData"))
                {
                    xcdata = tags["CData"] as IEnumerable<XCData>;

                    if (xcdata != null && xcdata.Any())
                    {
                        bsCData.DataSource = xcdata;
                        dgvCData.DataSource = bsCData;

                        dgvCData.Location = new Point(12, positionY + gbComment.Height + 88);     // Расположение CDATA при наличии Атрибутов positionY + 68 +

                        dgvCData.Visible = true;
                        dgvCData.RowHeadersVisible = false;
                        dgvCData.ColumnHeadersVisible = false;
                        dgvCData.Columns["NextNode"].Visible = false;               // Скрыть столбец со служебными переменными
                        dgvCData.Columns["PreviousNode"].Visible = false;           // Скрыть столбец со служебными переменными
                        dgvCData.Columns["BaseUri"].Visible = false;                // Скрыть столбец со служебными переменными
                        dgvCData.Columns["Document"].Visible = false;               // Скрыть столбец со служебными переменными
                        dgvCData.Columns["Parent"].Visible = false;                 // Скрыть столбец со служебными переменными
                        dgvCData.Columns["NodeType"].Width = dgvSize.Width / 4;
                        dgvCData.Columns["Value"].Width = dgvSize.Width - (dgvSize.Width / 4) - 22;

                        dgvCData.Size = new Size(588, xcdata.Count() * dgvCData.Rows[0].Height + 3);
                    }
                }
            }
            else if (currentNode.Tag is string && currentNode.Tag is not XCData) // Зачем комментарий первого атрибута в текстовой метке ?
            {
                gbComment.Visible = false;
                rtbParam.Visible = true;
                rtbParam.Text = currentNode.Tag.ToString();
            }
            else if (currentNode.Tag is XComment)
            {
                gbComment.Visible = false;
                if (currentNode.Text == "#comment") // Для комментариев используем RichTextBox
                {
                    rtbParam.Visible = true;
                    rtbParam.Text = ((XComment)currentNode.Tag).Value;
                }
            }
            else if (currentNode.Tag is XCData)
            {
                gbComment.Visible = false;
                if (currentNode.Text == "#cdata-section")
                {
                    gbComment.Visible = false;

                    rtbParam.Visible = true;
                    rtbParam.Text = ((XCData)currentNode.Tag).Value;
                }
            }
            else if (currentNode.Tag is XAttribute)
            {
                XAttribute attribute = (XAttribute)currentNode.Tag;
                if (currentNode.Parent.Tag is Dictionary<string, object>)
                {
                    Dictionary<string, object> dict = currentNode.Parent.Tag as Dictionary<string, object>;

                    if (dict.ContainsKey("Comment"))
                    {
                        IEnumerable<XComment> comm = dict["Comment"] as IEnumerable<XComment>;
                        IEnumerable<XComment> xcomm = comm.Where(x => x.Value.StartsWith(attribute.Name.ToString()));

                        if (xcomm.Any())
                        {
                            if (xcomm.FirstOrDefault() != null)
                                rtbNode.Text = xcomm.FirstOrDefault().Value;
                            gbComment.Size = new Size(568, rText.Height + 36);  // Размер GroupBox - определить по количеству строк RichTextBox - собственно по queryComment.Count
                        }
                        else
                            gbComment.Visible = false;
                    }
                    else
                        gbComment.Visible = false;
                }
                rtbParam.Visible = true;
                rtbParam.Text = attribute.Value;
            }

            rowIndex = -1;

            try
            {
                DataGridViewRow row = dgvAttribute.Rows.Cast<DataGridViewRow>()
                     .Where(r => r.Cells["Name"].Value.ToString().Equals(ModulePhrases.CnlNum.Replace(" ", "_"))) // Equals("CnlNum")
                     .First();
                rowIndex = row.Index;
            }
            catch { }

            if (rowIndex > -1)
            {
                findCnl.Enabled = true;
            }

            rtbParam.TextChanged += TxtParam_TextChanged;
            dgvAttribute.CellValueChanged += DgvAttribute_CellValueChanged;
            dgvAttribute.CellClick += DgvAttribute_CellClick;
            dgvCData.CellValueChanged += DgvCData_CellValueChanged;
            dgvAttribute.CurrentCellDirtyStateChanged += DgvAttribute_CurrentCellDirtyStateChanged;
        }
        #endregion ConfigToControls

        // Меняет ячейку сразу после выбора в выпадающем списке DataGridViewComboboxCell
        void DgvAttribute_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            colIndex = dgvAttribute.CurrentCell.ColumnIndex;
            rowIndex = dgvAttribute.CurrentCell.RowIndex;
            int idClmName = dgvAttribute.Columns["Name"].Index;
            //int ciValue = dgvAttribute.Columns["Value"].Index;

            if (dgvAttribute.CurrentCell is DataGridViewComboBoxCell)
            {
                dgvAttribute.CommitEdit(DataGridViewDataErrorContexts.Commit);
                dgvAttribute.EndEdit();

                // Получить имя после редактирования для вывода
                string valClm = dgvAttribute[idClmName, rowIndex].Value.ToString().Replace(" ", "_");
                string cur = "";

                if (valClm == ModulePhrases.VariableName.Replace(" ", "_") || valClm == ModulePhrases.Task.Replace(" ", "_"))
                {
                    cur = dgvAttribute[colIndex, rowIndex].Value.ToString();


                }
                else
                {
                    // Надо проверять словарь PrevLang и CurLang  // Удалять пробелы
                    string nameKey = FrmModuleConfig.CurLang.Where(x => x.Value.Replace(" ", "_") == valClm).FirstOrDefault().Key;
                    cur = FrmModuleConfig.CurLang[nameKey];
                }
                //PrintDescription(cur);
            }
        }

        #region DataError 
        private void dgvAttribute_DataError(object sender, DataGridViewDataErrorEventArgs anErr) // Если ошиблись с вводом параметра то в случае отсутствия будет пустая строка
        {
            string cval = dgvAttribute[anErr.ColumnIndex, anErr.RowIndex].Value.ToString();
            if (cval != null)
            {
                dgvAttribute[anErr.ColumnIndex, anErr.RowIndex].Value = "";
                anErr.ThrowException = false;
            }
        }
        #endregion DataError

        private void RtbNode_ContentsResized(object sender, ContentsResizedEventArgs e)
        {
            RichTextBox richTextBox = (RichTextBox)sender;
            richTextBox.Width = 556;
            richTextBox.Height = e.NewRectangle.Height;
            rText = new Size(556, e.NewRectangle.Height);
        }

        #region Form Control
        /// <summary>
        /// Gets or sets a value indicating whether the configuration was modified.
        /// </summary>
        private bool Modified
        {
            get
            {
                return frmParentGloabal.modified;
            }
            set
            {
                frmParentGloabal.modified = value; // modified = value;
                frmParentGloabal.btOK.Enabled = frmParentGloabal.modified;
            }
        }
        #endregion Form Control

        #region Search_Click
        private void Search_Click(object sender, EventArgs e)
        {
            if (frmParentGloabal.ConfigDataset != null)
            {
                string num = "";

                rowIndex = -1;
                try
                {
                    DataGridViewRow row = dgvAttribute.Rows.Cast<DataGridViewRow>()
                         .Where(r => r.Cells["Name"].Value.ToString().Equals(ModulePhrases.CnlNum.Replace(" ", "_"))) //  "CnlNum"
                         .First();
                    rowIndex = row.Index;
                }
                catch { }

                if (rowIndex > -1)
                {
                    num = dgvAttribute.Rows[rowIndex].Cells["Value"].Value.ToString();
                    int initCnl = 0;
                    bool isNum = int.TryParse(num, out initCnl);

                    FrmCnlSelect frmCnlSelect = new FrmCnlSelect(frmParentGloabal.ConfigDataset)
                    {
                        MultiSelect = false,
                        SelectedCnlNum = initCnl
                    };
                    if (frmCnlSelect.ShowDialog() == DialogResult.OK)
                    {
                        dgvAttribute.Rows[rowIndex].Cells["Value"].Value = frmCnlSelect.SelectedCnlNum.ToString(); // Было [3] вместо rowindex
                    }
                }
            }
            //else
            //{
            //    //frmParentGloabal.rtb_Log.Text += $"Нет данных" + Environment.NewLine; // TEST
            //}
        }
        #endregion Search_Click

        #region TxtParam_TextChanged
        private void SaveTextToTagDict(string boxText)
        {
            List<XText> text = (tags["Text"] as IEnumerable<XText>).ToList();

            XText xtext = text.FirstOrDefault();

            xtext.Value = boxText; // Строка 
            List<XText> lXtext = new List<XText>();
            lXtext.Add(xtext);

            IEnumerable<XText> ieXtext = lXtext.AsEnumerable<XText>();
            tags["Text"] = ieXtext;
        }

        private void TxtParam_TextChanged(object sender, EventArgs e)
        {
            string boxText = "";

            try
            {
                TextBox tb = (TextBox)sender;
                boxText = tb.Text;
            }
            catch
            {
                try
                {
                    RichTextBox rtb = (RichTextBox)sender;
                    boxText = rtb.Text;
                }
                catch { }
            }

            TreeNode treeNode = null;
            if (ModSoftPlcView.fConfig.trvRoot.SelectedNode.Tag is Dictionary<string, object>)
            {
                tags = ModSoftPlcView.fConfig.trvRoot.SelectedNode.Tag as Dictionary<string, object>;
                TreeNode result = null;

                result = ModSoftPlcView.fConfig.trvRoot.SelectedNode.Nodes.OfType<TreeNode>()
                    .FirstOrDefault(node => node.Text.Equals("#text"));

                if (result != null)
                {
                    ModSoftPlcView.fConfig.trvRoot.SelectedNode.Nodes[result.Index].Tag = boxText; // Запись текста в ноду #text
                    SaveTextToTagDict(boxText);    // Необходимо еще записать в себя, в свой словарь текста
                }
            }
            else if (ModSoftPlcView.fConfig.trvRoot.SelectedNode.Tag is string) // Изменение строки если редактируем саму строку
            {
                ModSoftPlcView.fConfig.trvRoot.SelectedNode.Tag = boxText;

                treeNode = ModSoftPlcView.fConfig.trvRoot.SelectedNode.Parent; // perentNode

                if (treeNode != null)
                {
                    if (treeNode.Tag is Dictionary<string, object>)
                    {
                        tags = treeNode.Tag as Dictionary<string, object>;
                        if (tags.ContainsKey("Text") && ModSoftPlcView.fConfig.trvRoot.SelectedNode.Text == "#text")
                        {
                            SaveTextToTagDict(boxText);
                        }
                    }
                }
            }
            else if (ModSoftPlcView.fConfig.trvRoot.SelectedNode.Tag is XComment)
            {
                ModSoftPlcView.fConfig.trvRoot.SelectedNode.Tag = new XComment(boxText);

                treeNode = ModSoftPlcView.fConfig.trvRoot.SelectedNode.Parent; // perentNode

                if (treeNode != null)
                {
                    if (treeNode.Tag is Dictionary<string, object>)
                    {
                        tags = treeNode.Tag as Dictionary<string, object>;
                        if (tags.ContainsKey("Comment") && ModSoftPlcView.fConfig.trvRoot.SelectedNode.Text == "#comment")
                        {
                            query = treeNode.Nodes.Cast<TreeNode>().Where(node => node.Text.Contains("#comment"));
                            List<XComment> lXComment = new List<XComment>();

                            foreach (TreeNode comment in query)
                            {
                                XComment xcomment = new XComment(((XComment)comment.Tag).Value);
                                lXComment.Add(xcomment);
                            }
                            IEnumerable<XComment> ieXcomment = lXComment.AsEnumerable<XComment>();
                            tags["Comment"] = ieXcomment;
                        }
                    }
                }
            }
            else if (ModSoftPlcView.fConfig.trvRoot.SelectedNode.Tag is XCData)
            {
                ModSoftPlcView.fConfig.trvRoot.SelectedNode.Tag = new XCData(boxText);
                treeNode = ModSoftPlcView.fConfig.trvRoot.SelectedNode.Parent;  // perentNode

                if (treeNode != null)
                {
                    if (treeNode.Tag is Dictionary<string, object>)
                    {
                        tags = treeNode.Tag as Dictionary<string, object>;
                        if (tags.ContainsKey("CData") && ModSoftPlcView.fConfig.trvRoot.SelectedNode.Text == "#cdata-section")
                        {
                            query = treeNode.Nodes.Cast<TreeNode>().Where(node => node.Text.Contains("#cdata-section"));
                            List<XCData> lCdata = new List<XCData>();

                            foreach (TreeNode cdata in query)
                            {
                                XCData xdata = new XCData(((XCData)cdata.Tag).Value);
                                lCdata.Add(xdata);
                            }
                            IEnumerable<XCData> ieCData = lCdata.AsEnumerable<XCData>();
                            tags["CData"] = ieCData;
                        }
                    }
                }
            }
            else if (ModSoftPlcView.fConfig.trvRoot.SelectedNode.Tag is XAttribute)
            {
                XAttribute attribute = (XAttribute)ModSoftPlcView.fConfig.trvRoot.SelectedNode.Tag;
                attribute.Value = boxText;

                treeNode = ModSoftPlcView.fConfig.trvRoot.SelectedNode.Parent; // perentNode

                if (treeNode != null)
                {
                    if (treeNode.Tag is Dictionary<string, object>)
                    {
                        tags = treeNode.Tag as Dictionary<string, object>;
                        if (tags.ContainsKey("Attributes") && ModSoftPlcView.fConfig.trvRoot.SelectedNode.Tag is XAttribute)
                        {
                            query = treeNode.Nodes.Cast<TreeNode>().Where(node => node.Tag is XAttribute);
                            List<XAttribute> lAttribute = new List<XAttribute>();

                            foreach (var attr in query)
                            {
                                XAttribute xattr = new XAttribute(attr.Text, ((XAttribute)attr.Tag).Value); //  attr.Tag
                                lAttribute.Add(xattr);
                            }
                            IEnumerable<XAttribute> ieAttribute = lAttribute.AsEnumerable<XAttribute>();
                            tags["Attributes"] = ieAttribute;
                        }
                    }
                }
            }
            Modified = true;
        }
        #endregion TxtParam_TextChanged

        #region ListIndexSelect


        #endregion ListIndexSelect

        #region DgvAttribute_CellClick
        private void DgvAttribute_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            rowIndex = e.RowIndex;
            colIndex = e.ColumnIndex;
            int idClmName = dgvAttribute.Columns["Name"].Index;
            int ciValue = dgvAttribute.Columns["Value"].Index;

            if (idClmName != -1 && rowIndex != -1 && colIndex == ciValue) // Проверка что такое поле есть, соответственно есть и поле Value
            {
                string valClm = dgvAttribute[idClmName, rowIndex].Value.ToString();
                string curl = valClm;
                string cur = dgvAttribute[colIndex, rowIndex].Value.ToString(); // string cur = dgvAttribute[colIndex, rowIndex].Value as string;

                string nameKey = FrmModuleConfig.CurLang.Where(x => x.Value.Replace(" ", "_") == valClm).FirstOrDefault().Key; // TEST Значение приводить к беспробельному варианту

                if (nameKey != null) // Если в словарь добавлен перевод, то будет использоваться он, иначе просто имя столбца
                {
                    curl = FrmModuleConfig.CurLang[nameKey];
                    GetOptionValName(valClm);
                }
                else GetOptionValName(curl);


                PrintDescription(curl);

                if (valClm == ModulePhrases.Active.Replace(" ", "_") || valClm == ModulePhrases.Retain.Replace(" ", "_") || valClm == ModulePhrases.Restart.Replace(" ", "_") ||
                    valClm == ModulePhrases.Initialize.Replace(" ", "_") || valClm == ModulePhrases.VisibleLogPrgFile.Replace(" ", "_"))
                {
                    DataGridViewComboBoxCell c = new DataGridViewComboBoxCell();
                    c.DisplayStyle = DataGridViewComboBoxDisplayStyle.Nothing; // убрать серый стиль ComboBoxCell, приводит к глюку лишних нажатий мыши
                    c.Style.BackColor = Color.White;
                    c.DataSource = YesNo;
                    dgvAttribute[e.ColumnIndex, rowIndex] = c;  // change the cell
                }
                else if (valClm == ModulePhrases.Task.Replace(" ", "_"))
                {
                    Type[] typelist = Assembly.GetExecutingAssembly().GetExportedTypes(); // Получение списка только интересующего класса

                    dictOfAttr.Clear();
                    listOfTask = new List<string> { "" };
                    // Отключить из добавления в список внутренних классов
                    foreach (Type type in typelist)
                    {
                        int idT = -1;
                        idT = ignoreTask.IndexOf(type.Name);
                        if (idT == -1)
                        {
                            listOfTask.Add(type.Name);
                            // Добавить в словарь атрибутов пользовательские атрибуты, если есть
                            AddTaskToDict(type);
                        }
                    }

                    // Добавить сюда возможность прописывать свои dll в качестве программ, а по умолчанию SoftPlc.dll
                    try
                    {
                        string libraryName = dgvAttribute[colIndex, rowIndex-1].Value as string;
                        string libName = string.IsNullOrEmpty(libraryName) ? "SoftPlcPrg" : libraryName;

                        Type[] asmlist = Assembly.Load(libName).GetExportedTypes();
                        foreach (var asmtype in asmlist)
                        {
                            int idT = -1;
                            idT = ignoreTask.IndexOf(asmtype.Name);
                            if (idT == -1)
                            {
                                listOfTask.Add(asmtype.Name);
                                // Добавить в словарь атрибутов пользовательские атрибуты из сторонних библиотек, если есть
                                AddTaskToDict(asmtype);
                            }
                        }

                    }
                    catch { }

                    if (listOfTask != null)
                    {
                        var ix = listOfTask.IndexOf(cur);
                        if (ix == -1 && cur != null)
                        {
                            dgvAttribute[e.ColumnIndex, rowIndex].Value = "";
                        }
                        DataGridViewComboBoxCell lT = new DataGridViewComboBoxCell();
                        lT.DisplayStyle = DataGridViewComboBoxDisplayStyle.Nothing;
                        lT.Style.BackColor = Color.White;
                        lT.DataSource = listOfTask;
                        dgvAttribute[colIndex, rowIndex] = lT;  // change the cell
                    }
                }
                else if (valClm == ModulePhrases.TasksLibrary.Replace(" ", "_"))
                {
                }
                else if (valClm == ModulePhrases.TaskCycle.Replace(" ", "_"))
                {
                }
                else if (valClm == ModulePhrases.VariableName.Replace(" ", "_"))
                {
                    // Тут надо определить имя задачи Parent  - Папа Father
                    Dictionary<string, object> grandFather = ModSoftPlcView.fConfig.trvRoot.SelectedNode.Parent.Tag as Dictionary<string, object>;

                    if (grandFather != null && grandFather.ContainsKey("Attributes"))
                    {
                        List<XAttribute> lattr = grandFather["Attributes"] as List<XAttribute>;
                        try
                        {
                            string taskName = lattr.Where(x => x.Name == ModulePhrases.Task.Replace(" ", "_")).FirstOrDefault().Value.ToString();

                            if (taskName != null)
                            {
                                // Тут обращаемся к получению списка переменных задачи для добавления в список listOfVar
                                GetTaskValName(taskName);
                                // Как проверить, что ячейка содержит значение из списка и если нет, обнулить перед переводов в ComboBox?
                                listOfVar = dictOfAttr.Keys.ToList();

                                if (listOfVar != null)
                                {
                                    var ix = listOfVar.IndexOf(cur);
                                    if (ix == -1 && cur != null)
                                    {
                                        dgvAttribute[colIndex, rowIndex].Value = "";
                                    }
                                    DataGridViewComboBoxCell l = new DataGridViewComboBoxCell();
                                    l.DisplayStyle = DataGridViewComboBoxDisplayStyle.Nothing; // убрать серый стиль ComboBoxCell, приводит к глюку лишних нажатий мыши
                                    l.Style.BackColor = Color.White;
                                    l.DataSource = listOfVar;
                                    dgvAttribute[colIndex, rowIndex] = l;  // change the cell
                                }
                            }
                        }
                        catch { }

                        // TEST


                        PrintDescription(cur);


                        //frmParentGloabal.rtb_Log.Text += Environment.NewLine + $"ValueName = {cur}   {dictOfAttr[cur].VarType}   {dictOfAttr[cur].Description}" + Environment.NewLine; // TEST
                        // TEST



                    }
                }
            }
        }
        #endregion DgvAttribute_CellClick

        #region AddTask_To_Dictionary
        private void AddTaskToDict(Type type)
        {
            AttributeOfVar attributOfVar = null;
            CustomAttributeData data = type.GetCustomAttributesData().Where(x => x.AttributeType.ToString().EndsWith("ProgramzAttribute")).FirstOrDefault();

            if (data != null && data.ConstructorArguments.Count > 0)
            {
                attributOfVar = new AttributeOfVar
                {
                    VarType = data.ConstructorArguments[0].Value.ToString(),
                    Description = data.ConstructorArguments[1].Value.ToString(),
                };
            }

            if (!dictOfAttr.ContainsKey(type.Name))
            {
                dictOfAttr.Add(type.Name, attributOfVar);
            }
        }
        #endregion AddTask_To_Dictionary

        #region DgvAttribute_CellValueChanged
        private void DgvAttribute_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            int ri = e.RowIndex;

            if (ri != -1)
            {
                int ciName = dgvAttribute.Columns["Name"].Index;
                int ciValue = dgvAttribute.Columns["Value"].Index;

                TreeNode result = ModSoftPlcView.fConfig.trvRoot.SelectedNode.Nodes.OfType<TreeNode>()
                            .FirstOrDefault(node => node.Text.Equals(dgvAttribute[ciName, ri].Value.ToString()));

                if (result.Tag is XAttribute)
                {
                    ModSoftPlcView.fConfig.trvRoot.SelectedNode.Nodes[result.Index].Tag = new XAttribute(ModSoftPlcView.fConfig.trvRoot.SelectedNode.Nodes[result.Index].Text, ModSoftPlcView.fConfig.trvRoot.SelectedNode.Nodes[result.Index].Tag = dgvAttribute[ciValue, ri].Value);
                }
                Modified = true;
            }
        }
        #endregion DgvAttribute_CellValueChanged

        #region DgvCData_CellValueChanged
        private void DgvCData_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            rowIndex = e.RowIndex;
            TreeNode current = ModSoftPlcView.fConfig.trvRoot.SelectedNode; // Данные выбранной Ноды

            if (rowIndex != -1)
            {
                List<TreeNode> lNode = current.Nodes.OfType<TreeNode>().Where(x => x.Text == "#cdata-section").ToList(); // Список нод cdata-section для определения индекса редактируемой Ноды
                int ciName = dgvCData.Columns["NodeType"].Index;
                int ciValue = dgvCData.Columns["Value"].Index;
                current.Nodes[lNode[rowIndex].Index].Tag = new XCData(dgvCData[ciValue, rowIndex].Value.ToString()); // Изменение данных ноды cdata-section при редактировании данных в DataGreedView
                Modified = true;
            }
        }
        #endregion DgvCData_CellValueChanged

        #region GetTaskValue
        public void GetTaskValName(string task, bool grand = true)
        {
            dictOfAttr.Clear();
            dictOfAttr.Add("", null);

            string libraryName = "SoftPlcPrg";

            Type typelist = Assembly.GetExecutingAssembly().GetExportedTypes().Where(x => x.Name == task).FirstOrDefault(); // Получение списка только интересующего класса
            Type type = null;

            if (typelist == null)
            {
                Dictionary<string, object> Father = new Dictionary<string, object>();
                // Вероятно тут надо вместо Родителя, определять уже себя, если мы стоим на программе и хотим добавить все переменные сразу
                // Тут надо определить имя задачи Parent  - Папа Father
                if (grand)
                {
                    Father = ModSoftPlcView.fConfig.trvRoot.SelectedNode.Parent.Tag as Dictionary<string, object>;
                }
                else
                {
                    Father = ModSoftPlcView.fConfig.trvRoot.SelectedNode.Tag as Dictionary<string, object>;
                }

                if (Father != null && Father.ContainsKey("Attributes"))
                {
                    List<XAttribute> lattr = Father["Attributes"] as List<XAttribute>;
                    try
                    {
                        string libName = lattr.Where(x => x.Name == ModulePhrases.TasksLibrary.Replace(" ", "_")).FirstOrDefault().Value.ToString();

                        if (libName != null)
                        {
                            libraryName = libName;
                        }
                    }
                    catch { }
                }

                typelist = Assembly.Load(libraryName).GetExportedTypes().Where(x => x.Name == task).FirstOrDefault(); // Добавить возможность выбора любой dll, по умолчанию SoftPlcPrg
                if (typelist != null)
                {
                    type = typelist;
                }
            }
            else
            {
                type = typelist;
            }

            object instance = null;
            try
            {
                instance = Activator.CreateInstance(type); // Падает тут, потому что пытается загрузить переменную из ScadaCommFunc - Как их игнорировать?
            }
            catch (TargetInvocationException ex)
            {
                frmParentGloabal.rtb_Log.Text += Environment.NewLine + $"Ошибка: {ex.Message}" + Environment.NewLine; // TEST TEST TEST TEST
            }

            // Добавляем имена переменных в список - Как сделать проверку уже добавленных в параметры входов, выходов имен и удалять из списка для нового добавления
            // и обратно возвращать, если имя удалили ?
            foreach (FieldInfo mi in instance.GetType().GetFields())
            {
                // Добавляем все, кроме "terminated" для использования.
                if (mi.Name != "terminated")
                {
                    AttributeOfVar attributOfVar = null;
                    CustomAttributeData data = mi.GetCustomAttributesData().Where(x => x.AttributeType.ToString().EndsWith("ProgramzAttribute")).FirstOrDefault(); // field

                    if (data != null && data.ConstructorArguments.Count > 0)
                    {
                        string midata = "";
                        try
                        {
                            midata = mi.GetValue(instance).ToString();
                        }
                        catch { }

                        attributOfVar = new AttributeOfVar
                        {
                            VarType = data.ConstructorArguments[0].Value.ToString(),
                            Description = data.ConstructorArguments[1].Value.ToString(),

                            Data = midata // TEST TEST TEST ----------------------РАБОТАЕТ--------------- на чем-то падает....
                        };

                        //frmParentGloabal.rtb_Log.Text += $"{mi.Name}  --  {attributOfVar.Data}" + Environment.NewLine; // TEST TEST TEST TEST

                    }
                    dictOfAttr.Add(mi.Name, attributOfVar);
                }
            }
        }
        #endregion GetTaskValue

        #region GetOptionValue
        private void GetOptionValName(string valname)
        {
            dictOfAttr.Clear();


            Type typelist = Assembly.GetExecutingAssembly().GetExportedTypes().Where(x => x.Name == "SoftPlc").FirstOrDefault(); // Получение списка только интересующего класса
            if (valname == ModulePhrases.Active.Replace(" ", "_") || valname == ModulePhrases.Restart.Replace(" ", "_") || valname == ModulePhrases.TasksLibrary.Replace(" ", "_") ||
                valname == ModulePhrases.Task.Replace(" ", "_") || valname == ModulePhrases.TaskCycle.Replace(" ", "_"))
            {
                typelist = Assembly.GetExecutingAssembly().GetExportedTypes().Where(x => x.Name == "ProgramX").FirstOrDefault(); // Получение списка только интересующего класса
            }
            else if (valname == ModulePhrases.Retain.Replace(" ", "_") || valname == ModulePhrases.Initialize.Replace(" ", "_") || valname == ModulePhrases.CnlNum.Replace(" ", "_") ||
                valname == ModulePhrases.Command.Replace(" ", "_") || valname == ModulePhrases.Data.Replace(" ", "_") || valname == ModulePhrases.CnlCode.Replace(" ", "_"))
            {
                typelist = Assembly.GetExecutingAssembly().GetExportedTypes().Where(x => x.Name == "VariableX").FirstOrDefault(); // Получение списка только интересующего класса
            }
            else if (valname == ModulePhrases.date.Replace(" ", "_") || valname == ModulePhrases.h.Replace(" ", "_") || 
                valname == ModulePhrases.t.Replace(" ", "_") ||  valname == ModulePhrases.f.Replace(" ", "_"))
            {
                typelist = Assembly.GetExecutingAssembly().GetExportedTypes().Where(x => x.Name == "DayH").FirstOrDefault(); // Получение списка только интересующего класса
            }
            // .Replace("[", "").Replace("]", "") - запрашивает typelist без словаря, код ниже запрашивает атрибуты
            // ProgramzAttribute если данные о перепенных классов есть в словаре

            if (typelist != null)
            {
                object instance = Activator.CreateInstance(typelist);

                foreach (PropertyInfo prop in instance.GetType().GetProperties())
                {
                    AttributeOfVar attributOfVar = null;
                    ProgramzAttribute propi = (ProgramzAttribute)prop.GetCustomAttribute(typeof(ProgramzAttribute), true);
                    if (propi != null)
                    {
                        attributOfVar = new AttributeOfVar { VarType = propi.VarType, Description = propi.Description };
                        dictOfAttr.Add(FrmModuleConfig.CurLang[prop.Name], attributOfVar); // TEST Тут так же менять между словарями Lang на текущий ???? FrmModuleConfig.CurLang[prop.Name] // prop.Name
                    }
                }
            }
        }

        #endregion GetOptionValue

        #region PrintDescription
        private void PrintDescription(string varName)
        {
            frmParentGloabal.rtb_Log.Clear(); // Test - потом вернуть
            try
            {
                frmParentGloabal.rtb_Log.Text += $"{ModulePhrases.VariableName}: {varName}";
                frmParentGloabal.rtb_Log.Text += Environment.NewLine + $"{ModulePhrases.VariableType}: {dictOfAttr[varName].VarType}" +
                    Environment.NewLine + $"{ModulePhrases.Description}: {dictOfAttr[varName].Description}";
            }
            catch { }
        }
        #endregion PrintDescription
    }
}
