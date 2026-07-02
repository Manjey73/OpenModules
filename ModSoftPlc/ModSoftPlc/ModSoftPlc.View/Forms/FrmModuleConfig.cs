using Scada.Data.Models;
using Scada.Forms;
using Scada.Lang;
using Scada.Server.Lang;
using Scada.Server.Modules.ModSoftPlc.Config;
using System.Reflection;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;


namespace Scada.Server.Modules.ModSoftPlc.View.Forms
{
    public partial class FrmModuleConfig : Form
    {
        #region Variables
        FrmParameters frmParameters; // child form properties Form();
        public Dictionary<string, object> tags;     // Словарь для сохранения параметров в родительском Ноде
        private string filePath;

        private readonly string configFileName;         // the configuration file name
        private string shortFileName = "";
        private string fileName;
        public bool modified;                           // indicates that the module configuration is modified
        private readonly AppDirs appDirs;

        private Dictionary<string, string> xml_node = new Dictionary<string, string>();

        private ContextMenuStrip contextMenu;
        public static Dictionary<string, string> CurLang = new Dictionary<string, string>();
        public static Dictionary<string, string> PrevLang = new Dictionary<string, string>();

        private TreeNode selectNode;
        ToolStripButton prodCalendar;
        private bool multiVal = false;
        // Словарь индексов изображений для интерфейса // Input, Output, Init, Retain - получится ли применить вместо Variable  или внутри для атрибута ????
        public static Dictionary<string, int> imageIndex = new Dictionary<string, int> // Словарь индексов изображений
        {
            { "Variable", 2 }, { "Program", 3 }, { "Calendar", 4 }, { "Day", 5 },  { "Week", 6 }, { "#text", 7 },
            {"#comment", 8 }, {"Delete", 9}, {"Input", 10}, {"Output", 11}, {"Init", 10}, {"Retain", 11}
        };

        #endregion Variables

        public FrmModuleConfig()
        {
            ModSoftPlcView.fConfig = this; // Присвоить ссылку на себя для доступа к переменным формы.
            Text = ModulePhrases.AppTitle;

            InitializeComponent();
            ConfigDataset = null;

            // Создать Menu
            CreateMenu();

            prodCalendar = tStrip.Items.OfType<ToolStripButton>().Where(b => b.Name == "tbCalendar").FirstOrDefault();
            prodCalendar.Enabled = false;

        }

        /// <summary>
        /// Gets or sets the configuration database.
        /// </summary>
        public ConfigDataset ConfigDataset { get; set; }

        public FrmModuleConfig(ConfigDataset configDataset, AppDirs appDirs)
            : this()
        {
            this.appDirs = appDirs ?? throw new ArgumentNullException(nameof(appDirs));
            configFileName = Path.Combine(appDirs.ConfigDir, ModuleConfig.DefaultFileName);
            modified = false;

            ArgumentNullException.ThrowIfNull(configDataset, nameof(configDataset));
            ConfigDataset = configDataset;
        }

        #region LoadXmlToTreeView  - Рабочий вариант
        private void LoadXmlToTreeView(XElement rootElement, TreeNodeCollection treeNodeCollection)
        {
            foreach (XElement xelement in rootElement.Elements())
            {
                int indexImage = 0;

                IEnumerable<XElement> elements = xelement.Elements();
                //Determine whether the element is a leaf element, that is, whether there are child elements below
                //If there are child elements, only add the element name, if it is a leaf element, add the element name and element content
                XName xname = xelement.Name;

                if (elements.Count() == 0) // Returns the number of elements in the incoming collection  // ReturnNumber(elements) == 0
                {
                    // Интерфейс - Иконки Нод TreeView
                    ReadXName(xelement, out indexImage, out xname);
                    // Интерфейс - Иконки Нод TreeView

                    TreeNode xnode = new TreeNode { Text = xname.ToString(), ImageIndex = indexImage, SelectedImageIndex = indexImage };
                    treeNodeCollection.Add(xnode);

                    tags = new Dictionary<string, object>();

                    ReadParameters(xelement, xnode);

                    xnode.Tag = tags;
                }
                else
                {
                    // Интерфейс - Иконки, Имена Нод TreeView
                    ReadXName(xelement, out indexImage, out xname);
                    // Интерфейс - Иконки, Имена Нод TreeView

                    TreeNode xnode = new TreeNode { Text = xname.ToString(), ImageIndex = indexImage, SelectedImageIndex = indexImage };
                    treeNodeCollection.Add(xnode);

                    tags = new Dictionary<string, object>();

                    ReadParameters(xelement, xnode);

                    xnode.Tag = tags;
                    LoadXmlToTreeView(xelement, xnode.Nodes);
                }
            }
        }
        #endregion LoadXmlToTreeView

        #region ReadParameters_AddParameters
        private void ReadParameters(XElement xelement, TreeNode xnode)
        {
            if (tags == null)
            {
                tags = new Dictionary<string, object>();
            }

            // Преобразовать в List и потом обратно в IEnumerable<XAttribute> после смены имен на локализованные ????
            IEnumerable<XAttribute> ieatr = xelement.Attributes();
            if (ieatr.Count() > 0)
            {
                List<XAttribute> lattr = ieatr.ToList();
                List<XAttribute> lattrNew = new List<XAttribute>();

                foreach (var atr in lattr) // in ieatr
                {
                    // Изменение имен Аттрибутов для интерфейса GetAttributeName(atr);
                    XAttribute xattr = new XAttribute(GetAttributeName(atr), atr.Value);

                    xnode.Nodes.Add(new TreeNode { Text = GetAttributeName(atr), Tag = xattr, ImageIndex = 1, SelectedImageIndex = 1 }); // TEST

                    lattrNew.Add(xattr);
                }

                IEnumerable<XAttribute> ieAttrNew = lattrNew.AsEnumerable();

                if (!tags.ContainsKey("Attributes")) // Тут Аттрибуты остаются английские
                {
                    tags.Add("Attributes", ieAttrNew); // "Attributes", xelement.Attributes()
                }
            }

            IEnumerable<XText> textsNode = xelement.Nodes().OfType<XText>().Where(t => t.NodeType != XmlNodeType.CDATA); // Выбирает все текстовые поля исключая CDATA
            if (textsNode.Any())
            {
                if (!tags.ContainsKey("Text"))
                {
                    tags.Add("Text", textsNode);
                }
                foreach (var text in textsNode)
                {
                    xnode.Nodes.Add(new TreeNode { Text = "#text", Tag = text.Value, ImageIndex = 7, SelectedImageIndex = 7 });
                }
            }

            IEnumerable<XComment> commentsNode = xelement.Nodes().OfType<XComment>();
            if (commentsNode.Any()) // 
            {
                if (!tags.ContainsKey("Comment"))
                {
                    tags.Add("Comment", commentsNode);
                }
                foreach (var comment in commentsNode)
                {
                    xnode.Nodes.Add(new TreeNode { Text = "#comment", Tag = comment, ImageIndex = 7, SelectedImageIndex = 7 });
                }
            }

            IEnumerable<XCData> cdataNode = xelement.Nodes().OfType<XCData>();
            if (cdataNode.Any()) // 
            {
                if (!tags.ContainsKey("CData"))
                {
                    tags.Add("CData", cdataNode);
                }
                foreach (var cdata in cdataNode)
                {
                    xnode.Nodes.Add(new TreeNode { Text = "#cdata-section", Tag = cdata });
                }
            }
        }
        #endregion ReadParameters_AddParameters

        #region GetAttributeName Интерфейсная часть
        // Изменение имен атрибутов для языкового вариианта интерфейса
        private string GetAttributeName(XAttribute attribute)
        {
            string aName = attribute.Name.ToString();

            // Поиск по имени xml Ноды имени параметра и подстановка индексов  изображений из словаря для отображения картинок.
            aName = CurLang[attribute.Name.ToString()].Replace(" ", "_").Replace("[", "").Replace("]", ""); // TEST Замена символов [ и ] для имен атрибутов при отсутствии в словаре

            if (!xml_node.ContainsKey(aName))
            {
                xml_node.Add(aName, attribute.Name.ToString());
            }
            return aName;
        }
        #endregion GetAttributeName

        // Создать какой-то список или словарь int для индексов Картинок и сжать код до проверки.
        #region ReadXName
        private void ReadXName(XElement xelement, out int indexImage, out XName xname)
        {
            indexImage = 0;
            xname = xelement.Name;
            string nameXmlNode = xelement.Name.ToString();

            string CurName = CurLang[xelement.Name.ToString()].Replace(" ", "_"); // TEST -остановка по несуществующим именам

            if (imageIndex.ContainsKey(nameXmlNode))
            {
                int idxImage = imageIndex[nameXmlNode];

                // TEST добавить определение Вход/Выход, чтобы добавить индекс иконки для переменной.

                indexImage = idxImage;
                xname = CurName;
                if (!xml_node.ContainsKey(xname.ToString()))        // Заносим в словарь в качестве ключа языковое название ноды, в качестве значения английский вариант
                    xml_node.Add(CurName, nameXmlNode);
            }
        }
        #endregion ReadXName

        #region SaveXmlItems  - Рабочий вариант, который допилился
        // Еше вариант
        public void SaveItems(XElement curNode, TreeNode item)
        {
            XAttribute xAttribute;
            XText xText;
            XComment xComment;
            XCData xCData;

            foreach (TreeNode itemloc in item.Nodes)
            {
                XElement newNode;

                if (itemloc.Text == "#text")
                {
                    xText = new XText(itemloc.Tag.ToString());
                    curNode.Add(xText);
                }
                else if (itemloc.Text == "#comment")
                {
                    xComment = new XComment(((XComment)itemloc.Tag).Value);
                    curNode.Add(xComment);
                }
                else if (itemloc.Text == "#cdata-section")
                {
                    xCData = new XCData((XCData)itemloc.Tag);
                    curNode.Add(xCData);
                }
                else if (itemloc.Nodes.Count == 0) // Проверка на отсутствующие ноды у дочерних, либо это атрибут, либо пустой элемент
                {
                    if (itemloc.Tag is XAttribute) // проверка на атрибут
                    {
                        if (xml_node.ContainsKey(itemloc.Text))
                            xAttribute = new XAttribute(xml_node[itemloc.Text], ((XAttribute)itemloc.Tag).Value);
                        else
                            xAttribute = new XAttribute(itemloc.Text, ((XAttribute)itemloc.Tag).Value);
                        curNode.Add(xAttribute);
                    }
                    else
                    {
                        if (xml_node.ContainsKey(itemloc.Text))
                            curNode.Add(new XElement(xml_node[itemloc.Text]));
                        else
                            curNode.Add(new XElement(itemloc.Text));
                    }
                }
                else
                {
                    if (xml_node.ContainsKey(itemloc.Text))
                        newNode = new XElement(xml_node[itemloc.Text]);
                    else
                        newNode = new XElement(itemloc.Text);
                    SaveItems(newNode, itemloc);
                    curNode.Add(newNode);
                }
            }
        }
        // Еше вариант
        #endregion SaveXmlItems

        #region Modify_when_AfterSelect
        private void trvRoot_AfterSelect(object sener, TreeViewEventArgs e)
        {
            trvRoot.SelectedNode = e.Node;
            if (trvRoot.SelectedNode.Tag == null) return;

            selectNode = trvRoot.SelectedNode; // TEST TreeNode selectNode
            LoadWindow(selectNode);
        }
        #endregion Modify_when_AfterSelect

        #region Load Window
        private void LoadWindow(TreeNode selectNode)
        {
            if (trvRoot.SelectedNode == null)
            {
                return;
            }

            // checking tabs
            ValidateTabPage();

            frmParameters = new FrmParameters(tStrip); // Передаем весь ToolStrip чтобы вынимать из него нужные кнопки для подписки
            frmParameters.frmParentGloabal = this;

            try
            {
                TabPage tabPageNew = new TabPage(trvRoot.SelectedNode.Text);
                tabPageNew.Name = trvRoot.SelectedNode.Text;
                frmParameters.Name = trvRoot.SelectedNode.Text;

                tabPageNew.Text = trvRoot.SelectedNode.Text;
                tabPageNew.ImageKey = trvRoot.SelectedNode.ImageKey;
                tabPageNew.Controls.Add(frmParameters); // Добавление Формы в Контрол TabPage

                tabControl.ImageList = imgList; // Добавил картинки - что дальше ?
                tabControl.TabPages.Add(tabPageNew);
                tabControl.SelectedTab = tabPageNew;

                frmParameters.Dock = DockStyle.Fill;
                frmParameters.Show();
            }
            catch (ObjectDisposedException) { }
        }
        #endregion Load Window

        #region ValidateTabPage
        private void ValidateTabPage()
        {
            if (frmParameters != null)
            {
                frmParameters.DialogResult = DialogResult.OK;
                frmParameters.Close();
                frmParameters.Dispose();
            }
            tabControl.TabPages.Clear();
        }
        #endregion ValidateTabPage

        #region ButtonOpenClick
        private void btOpen_Click(object sender, EventArgs e)
        {
            OpenFileDialog OFD = new OpenFileDialog();
            OFD.Filter = ModulePhrases.FilterProject;

            OFD.FileName = "";
            if (OFD.ShowDialog() == DialogResult.OK)
            {
                filePath = Path.GetFileName(OFD.FileName);
            }

            trvRoot.Nodes.Clear();
            xml_node.Clear();

            trvRoot.ImageList = imgList;
            LoadTemplate(OFD.FileName);
        }
        #endregion ButtonOpenClick

        // Interface Создание меню и его иконки
        #region CreateMenu
        private void CreateMenu()
        {
            // Create the ContextMenuStrip.
            contextMenu = new ContextMenuStrip();

            ToolStripMenuItem addParameters = new ToolStripMenuItem();
            addParameters.Text = ModulePhrases.AddParameters; // "Add Parameters"
            addParameters.Image = imgList.Images[1];

            ToolStripMenuItem addVariable = new ToolStripMenuItem();
            addVariable.Text = ModulePhrases.AddVariable; // "Add Variable"
            addVariable.Image = imgList.Images[2];

            ToolStripMenuItem addProgram = new ToolStripMenuItem();
            addProgram.Text = ModulePhrases.AddProgram; // "Add Program"
            addProgram.Image = imgList.Images[3];

            ToolStripMenuItem addСalendar = new ToolStripMenuItem();
            addСalendar.Text = ModulePhrases.AddCalendar; // "Add Calendar"
            addСalendar.Image = imgList.Images[4];

            ToolStripMenuItem addDay = new ToolStripMenuItem();
            addDay.Text = ModulePhrases.AddDay; // "Add Day"
            addDay.Image = imgList.Images[5];

            ToolStripMenuItem addWeek = new ToolStripMenuItem();
            addWeek.Text = ModulePhrases.AddWeek; // "Add Week"
            addWeek.Image = imgList.Images[6];

            ToolStripMenuItem addText = new ToolStripMenuItem();
            addText.Text = ModulePhrases.AddText;
            addText.Image = imgList.Images[7];

            ToolStripMenuItem addComment = new ToolStripMenuItem();
            addComment.Text = ModulePhrases.AddComment;
            addComment.Image = imgList.Images[8];

            ToolStripMenuItem addDelete = new ToolStripMenuItem();
            addDelete.Text = ModulePhrases.Delete;
            addDelete.Image = imgList.Images[9];

            //Add the menu items to the menu.
            contextMenu.Items.AddRange([addParameters, addProgram, addСalendar, addDay, addWeek, addVariable, addText, addComment, addDelete]); // 

            contextMenu.ItemClicked += ContextMenu_ItemClicked;
        }

        #region CreateParameters
        // Метод добавления параметров для использования сразу при добавлении соответствующей ноды
        private void CreateParameters(string nodeText)
        {
            if (nodeText == ModulePhrases.Program)
            {
                SoftPlc.ProgramX program = new SoftPlc.ProgramX
                { TasksLibrary = "", Task = "", TaskCycle = "", Active = false, Variable = new List<SoftPlc.VariableX>() };

                XmlSerializer serializer = new XmlSerializer(typeof(SoftPlc.ProgramX));
                AddParameters(serializer, program);
            }
            //else if (nodeText == ModulePhrases.Calendar)
            //{
            //    SoftPlc.Calendar calendar = new SoftPlc.Calendar
            //    {
            //        Day = new List<SoftPlc.Calendar.DayX>(),
            //        Week = new List<SoftPlc.Calendar.WeekX>()
            //    };

            //    XmlSerializer serializer = new XmlSerializer(typeof(SoftPlc.Calendar));
            //    AddParameters(serializer, calendar);
            //}
            else if (nodeText == ModulePhrases.Day)
            {
                SoftPlc.Calendar.DayX day = new SoftPlc.Calendar.DayX
                {
                    DayNum = "",
                    Variable = new List<SoftPlc.VariableX>()
                };

                XmlSerializer serializer = new XmlSerializer(typeof(SoftPlc.Calendar.DayX));
                AddParameters(serializer, day);
            }
            else if (nodeText == ModulePhrases.Week)
            {
                SoftPlc.Calendar.WeekX week = new SoftPlc.Calendar.WeekX
                {
                    WeekNum = "",
                    Variable = new List<SoftPlc.VariableX>()
                };

                //rtb_Log.Text += $"{trvRoot.SelectedNode.Parent.Text}" + Environment.NewLine;
                //var rr = trvRoot.SelectedNode.Parent.Tag;
                //rtb_Log.Text += $"{rr.GetType().ToString()}" + Environment.NewLine;

                XmlSerializer serializer = new XmlSerializer(typeof(SoftPlc.Calendar.WeekX));
                AddParameters(serializer, week);
            }
            else if (nodeText == ModulePhrases.Variable) // Добавление переменных
            {
                SoftPlc.VariableX variable = new SoftPlc.VariableX
                {
                    VariableName = "",
                    CnlCode = "",
                    CnlNum = "",
                    Initialize = false,
                    Retain = false,
                    Data = "",
                    Command = ""
                };

                XmlSerializer serializer = new XmlSerializer(typeof(SoftPlc.VariableX));
                AddParameters(serializer, variable);
            }
            else if (nodeText == ModulePhrases.MainNode.Replace(" ", "_"))
            {
                SoftPlc softplc = new SoftPlc
                {
                    VisibleLogPrgFile = true,
                    SaveLogPrgTime = "2",
                    IterationTime = "300",
                };
                XmlSerializer serializer = new XmlSerializer(typeof(SoftPlc));
                AddParameters(serializer, softplc);
            }
        }
        #endregion CreateParameters

        #region AddParameters_Void
        private void AddParameters(XmlSerializer serializer, object obj)
        {
            XDocument xdoc = new XDocument();
            //Create our own namespaces for the output
            XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
            //Add an empty namespace and empty value
            ns.Add("", "");

            using (MemoryStream w = new MemoryStream())
            {
                serializer.Serialize(w, obj, ns);
                w.Position = 0;
                xdoc = XDocument.Load(w);

                XElement rootElement = xdoc.Root;
                // Сперва читаем словарь объектов текущей ноды
                tags = trvRoot.SelectedNode.Tag as Dictionary<string, object>;

                ReadParameters(rootElement, trvRoot.SelectedNode);
                // Тут добавить в родительскую Ноду Список аттрибутов со значениями 
                trvRoot.SelectedNode.Tag = tags;
                w.Close();
            }

            if (!multiVal)
                trvRoot.SelectedNode.Expand(); // Раскрыть текущую ноду после добавления // TEST без раскрытия проверка

            modified = true;
            btOK.Enabled = modified;
            LoadWindow(trvRoot.SelectedNode);
        }
        #endregion AddParameters_Void

        #region AddNodes_Void
        private void AddNodes(XmlSerializer serializer, object obj)
        {
            XDocument xdoc = new XDocument();
            //Create our own namespaces for the output
            XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
            //Add an empty namespace and empty value
            ns.Add("", "");

            using (MemoryStream w = new MemoryStream())
            {
                serializer.Serialize(w, obj, ns);
                w.Position = 0;
                xdoc = XDocument.Load(w);

                XElement rootElement = xdoc.Root;
                // Сперва читаем словарь объектов текущей ноды
                tags = trvRoot.SelectedNode.Tag as Dictionary<string, object>;

                //ReadParameters(rootElement, trvRoot.SelectedNode);
                //Load XML into TreeView with recursion
                LoadXmlToTreeView(rootElement, trvRoot.SelectedNode.Nodes);

                // Тут добавить в родительскую Ноду Список аттрибутов со значениями 
                trvRoot.SelectedNode.Tag = tags;
                w.Close();
            }
            trvRoot.SelectedNode.Expand(); // Раскрыть текущую ноду после добавления
            modified = true;
            btOK.Enabled = modified;
            LoadWindow(trvRoot.SelectedNode);
        }
        #endregion AddNode_Void

        private void ContextMenu_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            // Сперва читаем словарь объектов текущей ноды
            tags = trvRoot.SelectedNode.Tag as Dictionary<string, object>;

            #region Delete_Attribute
            if (e.ClickedItem.Text == ModulePhrases.Delete) // "Delete"
            {
                Dictionary<string, object> TagDict = trvRoot.SelectedNode.Parent.Tag as Dictionary<string, object>;
                string nodeText = trvRoot.SelectedNode.Text;

                // Проверка удаляемой ноды на Атрибут
                bool isAttr = trvRoot.SelectedNode.Tag is XAttribute;
                if (isAttr)
                {
                    if (TagDict.ContainsKey("Attributes"))
                    {
                        List<XAttribute> lattr = TagDict["Attributes"] as List<XAttribute>;
                        XAttribute xattr = trvRoot.SelectedNode.Tag as XAttribute;

                        // Индекс удаляемого атрибута
                        XAttribute findXattr = lattr.Where(x => x.Name == xattr.Name).FirstOrDefault();
                        int attrIndex = lattr.IndexOf(findXattr); // xattr
                        lattr.RemoveAt(attrIndex);

                        // Записать все обратно в Tag родителя
                        TagDict["Attributes"] = lattr;
                        if (lattr.Count == 0)
                        {
                            TagDict.Remove("Attributes"); // Удалить ключ словаря
                        }
                        trvRoot.SelectedNode.Parent.Tag = TagDict;
                        // Записать все обратно в Tag родителя
                    }
                }

                // Удаление текстовой метки и словаря
                if (trvRoot.SelectedNode.Text == "#text")
                {
                    TagDict.Remove("Text");
                    trvRoot.SelectedNode.Parent.Tag = TagDict;
                }

                trvRoot.SelectedNode.Remove();
                modified = true;
                btOK.Enabled = modified;
            }
            #endregion Delete_Attribute

            #region AddParameters
            else if (e.ClickedItem.Text == ModulePhrases.AddParameters) // Добавить в словарь "Add Parameters"
            {
                CreateParameters(trvRoot.SelectedNode.Text);
            }
            #endregion AddParameters

            #region AddComment
            else if (e.ClickedItem.Text == ModulePhrases.AddComment) // "Add Comment"
            {
                if (tags == null)
                {
                    tags = new Dictionary<string, object>();
                }

                XComment comment = new XComment("");
                XElement xelement = new XElement("Comment");
                xelement.Add(comment);

                ReadParameters(xelement, trvRoot.SelectedNode);
                // Тут добавить в родительскую Ноду новые данные 
                trvRoot.SelectedNode.Tag = tags;
                trvRoot.SelectedNode.Expand(); // Раскрыть текущую ноду после добавления

                LoadWindow(trvRoot.SelectedNode); // TEST
            }
            #endregion AddComment

            #region AddText
            else if (e.ClickedItem.Text == ModulePhrases.AddText) // "Add Text"
            {
                if (tags == null)
                {
                    tags = new Dictionary<string, object>();
                }

                XText xtext = new XText("");
                XElement xelement = new XElement("Text");
                xelement.Add(xtext);

                ReadParameters(xelement, trvRoot.SelectedNode);
                // Тут добавить в родительскую Ноду новые данные 
                trvRoot.SelectedNode.Tag = tags;
                trvRoot.SelectedNode.Expand(); // Раскрыть текущую ноду после добавления

                LoadWindow(trvRoot.SelectedNode); // TEST
            }
            #endregion AddText

            #region AddNode // TEST добавление НОД
            // Собрать все варианты добавления просто Нод в одну кучу, определив имя элемента через словари соответствий
            else if (e.ClickedItem.Text == ModulePhrases.AddCalendar || e.ClickedItem.Text == ModulePhrases.AddDay || e.ClickedItem.Text == ModulePhrases.AddWeek
                || e.ClickedItem.Text == ModulePhrases.AddProgram || e.ClickedItem.Text == ModulePhrases.AddVariable)
            {
                // TEST TEST
                multiVal = false;

                if (e.ClickedItem.Text == ModulePhrases.AddVariable)
                {
                    //rtb_Log.Text += $"Добавление параметра через Context menu" + Environment.NewLine; // TEST TEST
                    // Тут надо определить имя задачи Parent  - Папа Father
                    Dictionary<string, object> father = trvRoot.SelectedNode.Tag as Dictionary<string, object>;

                    if (father != null && father.ContainsKey("Attributes"))
                    {
                        List<XAttribute> lattr = father["Attributes"] as List<XAttribute>;

                        try
                        {
                            string taskName = lattr.Where(x => x.Name == ModulePhrases.Task.Replace(" ", "_")).FirstOrDefault().Value.ToString();

                            //rtb_Log.Text += $"taskName   {taskName}" + Environment.NewLine; // TEST TEST TEST

                            if (!string.IsNullOrEmpty(taskName)) // Если есть имя задачи, то создаем все переменные  // taskName != null ||
                            {
                                multiVal = true;
                                // Тут обращаемся к получению списка переменных задачи для добавления в список listOfVar
                                frmParameters.GetTaskValName(taskName, false);

                                // Сперва читаем словарь объектов текущей ноды
                                tags = trvRoot.SelectedNode.Tag as Dictionary<string, object>;

                                foreach (var valu in frmParameters.dictOfAttr)
                                {
                                    if (valu.Value != null)
                                    {
                                        //rtb_Log.Text += $"{valu.Key} {valu.Value.VarType} {valu.Value.Description}" + Environment.NewLine; // TEST TEST TEST

                                        bool ret = valu.Value.VarType.ToLower().Contains("retain"); // должно содержать
                                        bool init = valu.Value.VarType.ToLower().Contains("init"); // должно содержать

                                        SoftPlc.VariableX variable = new SoftPlc.VariableX
                                        {
                                            VariableName = valu.Key,
                                            CnlCode = "",
                                            CnlNum = "",
                                            Initialize = init,
                                            Retain = ret,
                                            Data = "",
                                            Command = ""
                                        };

                                        int indexImage = 0;
                                        XName xname = "AnyName";
                                        XElement xelement = new XElement(xname);

                                        string nameKey = CurLang.Where(x => x.Value == e.ClickedItem.Text).FirstOrDefault().Key;
                                        xelement = new XElement(PrevLang[nameKey].Replace("Add ", ""));

                                        ReadXName(xelement, out indexImage, out xname); // Тут Индекс изображения самой ветки Переменная {X}
                                        TreeNode xnode = new TreeNode { Text = xname.ToString(), ImageIndex = indexImage, SelectedImageIndex = indexImage }; // Добавление Ноды Переменная
                                        trvRoot.SelectedNode.Nodes.Add(xnode);

                                        //trvRoot.SelectedNode.Expand(); // Раскрыть текущую ноду после добавления
                                        trvRoot.SelectedNode = trvRoot.SelectedNode.LastNode;
                                        trvRoot.EndUpdate();

                                        tags = new Dictionary<string, object>();

                                        ReadParameters(xelement, trvRoot.SelectedNode);
                                        trvRoot.SelectedNode.Tag = tags;

                                        XmlSerializer serializer = new XmlSerializer(typeof(SoftPlc.VariableX));
                                        AddParameters(serializer, variable);

                                        if (!string.IsNullOrEmpty(valu.Value.Description))
                                        {
                                            // Добавление комментария
                                            XComment comment = new XComment(valu.Value.Description);
                                            XElement xelementСomment = new XElement("Comment");
                                            xelementСomment.Add(comment);
                                            // TEST Добавление комментария

                                            ReadParameters(xelementСomment, trvRoot.SelectedNode);
                                            // Тут добавить в родительскую Ноду новые данные 
                                            trvRoot.SelectedNode.Tag = tags;
                                        }

                                        // Вернуться в родительскую ноду для следующего цикла
                                        trvRoot.SelectedNode = trvRoot.SelectedNode.Parent;
                                    }
                                }
                            }
                            else
                            {
                                CreateNode(e);
                            }
                        }
                        catch
                        {
                            //rtb_Log.Text += $"Мы в catch taskName" + Environment.NewLine; // TEST
                        }
                    }
                    // TEST TEST
                }
                else // Если не добавление переменной, то исполняем старый код
                {
                    CreateNode(e);
                }
                #endregion AddNode
            }
        }
        #endregion CreateMenu

        #region Create Node кроме Переменных
        private void CreateNode(ToolStripItemClickedEventArgs e)
        {
            trvRoot.BeginUpdate();

            int indexImage = 0;
            XName xname = "AnyName";
            XElement xelement = new XElement(xname);

            string nameKey = CurLang.Where(x => x.Value == e.ClickedItem.Text).FirstOrDefault().Key;
            xelement = new XElement(PrevLang[nameKey].Replace("Add ", ""));

            ReadXName(xelement, out indexImage, out xname); // Тут Индекс изображения самой ветки Переменная {X}
            TreeNode xnode = new TreeNode { Text = xname.ToString(), ImageIndex = indexImage, SelectedImageIndex = indexImage }; // Добавление Ноды Переменная
            trvRoot.SelectedNode.Nodes.Add(xnode);

            // TEST Вероятно из цикла необходимо исключить раскрытие, хотя непонятно как это будет работать тогда????
            trvRoot.SelectedNode.Expand(); // Раскрыть текущую ноду после добавления
            trvRoot.SelectedNode = trvRoot.SelectedNode.LastNode;
            trvRoot.EndUpdate();

            tags = new Dictionary<string, object>();
            ReadParameters(xelement, trvRoot.SelectedNode);
            trvRoot.SelectedNode.Tag = tags;

            CreateParameters(trvRoot.SelectedNode.Text); // TEST Надо после Add перейти и выбрать добавленную ноду
            LoadWindow(trvRoot.SelectedNode); // TEST
                                              // Как сделать обновление ноды ?, типа выбора
        }
        #endregion Create Node кроме Переменных

        #region CreateEmptyRootNode
        private void CreateRootNode()
        {
            XDocument document = new XDocument();
            XElement rootElement = new XElement(ModulePhrases.MainNode.Replace(" ", "_"));
            TreeNode rootNode = new TreeNode { Text = rootElement.Name.ToString(), ImageIndex = 0, SelectedImageIndex = 0 };

            trvRoot.Nodes.Add(rootNode); // Наш TreeView
        }
        #endregion CreateEmptyRootNode

        #region LoadTemplate
        private void LoadTemplate(string filepath)
        {
            if (filepath != null)
            {
                var start = DateTime.Now; // Запуск измерения времени

                //Use xDocument to read xml file
                XDocument document = XDocument.Load(filepath);
                //Remove the root node
                XElement rootElement = document.Root;

                XName name = rootElement.Name = ModulePhrases.MainNode.Replace(" ", "_"); // Интерфейс

                if (!xml_node.ContainsKey(name.ToString()))
                    xml_node.Add(name.ToString(), "SoftPlc");

                //Load the root element of the xml file to the root node of the treeview
                TreeNode rootNode = new TreeNode { Text = name.ToString(), ImageIndex = 0, SelectedImageIndex = 0 }; // Index 0 = Device image

                trvRoot.Nodes.Add(rootNode); // Наш TreeView

                tags = new Dictionary<string, object>();

                ReadParameters(rootElement, rootNode);

                rootNode.Tag = tags;

                //Load XML into TreeView with recursion
                LoadXmlToTreeView(rootElement, rootNode.Nodes);

                #region EmptyRoot если не требуется атрибутов в root директории можно упростить
                //// Когда в рут директории не нужны атрибуты, можно упростить до такого кода
                //TreeNode rootNode = new TreeNode { Text = rootElement.Name.ToString(), ImageIndex = 0, SelectedImageIndex = 0 }; // Index 0 = Device image
                //trvRoot.Nodes.Add(rootNode); // TreeNode rootNode = trvRoot.Nodes.Add(rootElement.Name.ToString());
                //LoadxmlToTreeView(rootElement, rootNode.Nodes);
                #endregion EmptyRoot

                rootNode.Expand();
                trvRoot.SelectedNode = rootNode; // TEST

                // Измерение времени и вывод затраченного времени
                var elapsed = DateTime.Now.Subtract(start);
                lblFeedback.Text = string.Format("TreeViewLoad: {0:N0} ms ({1})", elapsed.TotalMilliseconds, elapsed.Display());
            }
        }
        #endregion LoadTemplate

        #region Form_Load
        private void FrmModuleConfig_Load(object sender, EventArgs e)
        {
            // translate the form
            FormTranslator.Translate(this, GetType().FullName);

            // translate TreeNode
            //FormTranslator.Translate(cmnuListMain, GetType().FullName);

            CurLang.Clear();
            CurLang = ModSoftPlcView.GetLangTo();

            if (File.Exists(configFileName))
            {
                trvRoot.Nodes.Clear();
                xml_node.Clear();

                trvRoot.ImageList = imgList;
                LoadTemplate(configFileName);
            }
        }
        #endregion Form_Load

        #region NewButtonClick
        private void tbNew_Click(object sender, EventArgs e)
        {
            trvRoot.Nodes.Clear();
            xml_node.Clear();
            trvRoot.ImageList = imgList;
            CreateRootNode();
            // Очистить FrmParameters
            ValidateTabPage();
        }
        #endregion NewButtonClick

        #region Button OK Click
        private void btOK_Click(object sender, EventArgs e)
        {
            fileName = configFileName;
            tbSave_Click(sender, e);
            modified = false;
            btOK.Enabled = modified;
        }
        #endregion Button OK Click

        #region Button SaveAs Click
        private void tbSaveAs_Click(object sender, EventArgs e)
        {
            SFD.InitialDirectory = appDirs.ConfigDir;
            SFD.Title = ModulePhrases.TitleLoadProject;
            SFD.Filter = ModulePhrases.FilterProject;

            if (shortFileName == "")
            {
                if (SFD.ShowDialog() == DialogResult.OK)
                {
                    fileName = SFD.FileName;
                }
                else return;
            }

            // сохранение шаблона устройства в новый файл
            tbSave_Click(sender, e);
        }
        #endregion Button SaveAs Click

        #region SaveButtion
        private void tbSave_Click(object sender, EventArgs e)
        {
            var start = DateTime.Now; // TEST

            if (fileName == null) fileName = configFileName;

            XElement root;
            // Сохраняем файл
            if (xml_node.ContainsKey(trvRoot.Nodes[0].Text.ToString()))
                root = new XElement(xml_node[trvRoot.Nodes[0].Text.ToString()]);
            else
                root = new XElement(trvRoot.Nodes[0].Text.ToString());

            foreach (TreeNode item in trvRoot.Nodes)
                SaveItems(root, item);
            root.Save(fileName); //  root.Save(configFileName);

            modified = false;
            btOK.Enabled = modified;
            // Измерение времени и вывод затраченного времени
            var elapsed = DateTime.Now.Subtract(start); // TEST
            lblFeedback.Text = string.Format("TreeViewSave: {0:N0} ms ({1})", elapsed.TotalMilliseconds, elapsed.Display()); // TEST
        }
        #endregion SaveButtion

        // Выбор ноды при правой кнопке мыши
        #region MouseRight
        private void trvRoot_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            // Если нажата правая кнопка мыши, проверяем наличие уже добавленных элементов для отключения возможности добавить.
            if (e.Button == MouseButtons.Right)
            {
                trvRoot.SelectedNode = e.Node;
                tags = e.Node.Tag as Dictionary<string, object>;
                List<TreeNode> treeNodeList = trvRoot.SelectedNode.Nodes.Cast<TreeNode>().ToList();

                if (e.Node.Text == ModulePhrases.Program.Replace(" ", "_"))
                {
                    // Проверить наличие Календаря в Программе
                    TreeNode tnl = treeNodeList.Where(x => x.Text == ModulePhrases.Calendar.Replace(" ", "_")).FirstOrDefault();
                    if (tnl != null)
                        contextMenu.Items[2].Enabled = false;
                    else
                        contextMenu.Items[2].Enabled = true;
                }

                if (tags != null && tags.ContainsKey("Attributes"))
                {
                    // Закрыть параметр AddParameters
                    contextMenu.Items[0].Enabled = false;
                }
                else contextMenu.Items[0].Enabled = true; // Открыть меню добавления Параметров

                //if (tags != null && tags.ContainsKey("Text"))
                //{
                //    // Закрыть параметр AddText
                //    contextMenu.Items[12].Enabled = false;
                //}
                //else contextMenu.Items[12].Enabled = true; // Открыть меню добавления Текста
                // TEST почему в атрибутах закрытие Текста, а индекс Параметров?
            }
        }
        #endregion MouseRight

        #region MouseDown
        private void trvRoot_MouseDown(object sender, MouseEventArgs e)
        {
            try
            {
                TreeViewHitTestInfo htInfo = trvRoot.HitTest(e.X, e.Y);
                selectNode = htInfo.Node;
                if (selectNode != null)
                {
                    trvRoot.SelectedNode = selectNode;
                    trvRoot.SelectedNode.ContextMenuStrip = null;
                    string nodeText = trvRoot.SelectedNode.Text;

                    // [0] addParameter, [1] addProgram, [2] addСalendar, [3] addDay, [4] Week,
                    // [5] Variable, [6] addText, [7] addComment, [8] addDelete

                    contextMenu.Items[0].Available = false;     // Parameter
                    contextMenu.Items[1].Available = false;     // Program
                    contextMenu.Items[2].Available = false;     // Сalendar
                    contextMenu.Items[3].Available = false;     // Day
                    contextMenu.Items[4].Available = false;     // Week
                    contextMenu.Items[5].Available = false;     // Variable
                    contextMenu.Items[6].Available = false;     // Text
                    contextMenu.Items[7].Available = true;     // Comment
                    contextMenu.Items[8].Available = true;     // Delete

                    prodCalendar.Enabled = false; // Закрыть кнопку Производственного календаря

                    if (nodeText == ModulePhrases.MainNode.Replace(" ", "_")) // string.Join("", ModulePhrases.MainNode.Split(default(string[]), StringSplitOptions.RemoveEmptyEntries)); // ModulePhrases.MainNode.Replace(" ", "")
                    {
                        contextMenu.Items[0].Available = true;     // Параметры
                        contextMenu.Items[1].Available = true;      // Программы
                        contextMenu.Items[8].Available = false;     // Удалить
                    }
                    else if (nodeText == ModulePhrases.Calendar)
                    {
                        contextMenu.Items[3].Available = true;
                        contextMenu.Items[4].Available = true;

                        prodCalendar.Enabled = true; // Открыть кнопку производственного календаря, Возможно если уже есть Календарь, то закрывать

                    }
                    else if (nodeText == ModulePhrases.Day)
                    {
                        contextMenu.Items[0].Available = true;     // Parameter
                        contextMenu.Items[5].Available = true;
                    }
                    else if (nodeText == ModulePhrases.Week)
                    {
                        contextMenu.Items[0].Available = true;     // Parameter
                        contextMenu.Items[5].Available = true;
                    }
                    else if (nodeText == ModulePhrases.Program)
                    {
                        contextMenu.Items[0].Available = true;     // Parameter
                        contextMenu.Items[2].Available = true;
                        contextMenu.Items[5].Available = true;
                        //prodCalendar.Enabled = true; // Открыть кнопку производственного календаря, Возможно если уже есть Календарь, то закрывать
                    }
                    else if (nodeText == ModulePhrases.Variable)
                    {
                        contextMenu.Items[0].Available = true;
                    }
                    else if (nodeText == "#comment" || nodeText == "#text" || nodeText == "#cdata-section" || selectNode.Tag is XAttribute)
                    {
                        contextMenu.Items[7].Available = false;    // Comment
                    }
                    trvRoot.SelectedNode.ContextMenuStrip = contextMenu;
                }
            }
            catch { }
        }
        #endregion MouseDown

        #region Button_Cancel
        private void btCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        #endregion Button_Cancel

        #region Form_Closing
        private void FrmModuleConfig_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (modified)
            {
                DialogResult result = MessageBox.Show(ServerPhrases.SaveModuleConfigConfirm,
                    CommonPhrases.QuestionCaption, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);

                switch (result)
                {
                    case DialogResult.Yes:
                        fileName = configFileName;
                        tbSave_Click(sender, e);
                        break;

                    case DialogResult.No:
                        break;

                    default:
                        e.Cancel = true;
                        break;
                }
            }
        }
        #endregion Form_Closing

        #region TraceObject
        void TraceObjectFields(object obj) // выводит список полей, их имена и значения:
        {
            foreach (FieldInfo mi in obj.GetType().GetFields())
            {
                rtb_Log.Text += $"{mi.FieldType.Name} {mi.Name} = {mi.GetValue(obj)} {mi.FieldType.ToString()}" + Environment.NewLine;
            }
            //moduleLog.WriteLine(mi.FieldType.Name + " " + mi.Name + " = " + mi.GetValue(obj) + mi.FieldType.ToString());
        }
        void TraceObjectProperties(object obj) // выводит список свойств объекта:
        {
            foreach (PropertyInfo pi in obj.GetType().GetProperties())
            {
                rtb_Log.Text += $"{pi.PropertyType.Name} {pi.Name} = {pi.GetValue(obj, null)}" + Environment.NewLine;
            }
            //moduleLog.WriteLine(pi.PropertyType.Name + " " + pi.Name + " = " + pi.GetValue(obj, null));
        }
        void TraceObjectMethods(object obj)
        {
            foreach (MethodInfo mi in obj.GetType().GetMethods())
            {
                rtb_Log.Text += $"{mi.Name} {mi.IsPublic}" + Environment.NewLine;
            }
            //moduleLog.WriteLine(mi.Name + " " + mi.IsPublic);
        }
        #endregion TraceObject

        #region Button Calendar
        private void tbCalendar_Click(object sender, EventArgs e)
        {
            //rtb_Log.Text += $"Нажата кнопка ProdCalendar" + Environment.NewLine;
            calendar prodCal = new calendar();

            try
            {
                string URLString = "https://xmlcalendar.ru/data/ru/2026/calendar.xml";

                using (HttpClient hc = new HttpClient())
                {
                    HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, URLString);
                    HttpResponseMessage response = hc.Send(request);
                    Stream st = response.Content.ReadAsStream();

                    st.Position = 0;
                    XmlSerializer serializer = new XmlSerializer(typeof(calendar));
                    prodCal = serializer.Deserialize(st) as calendar;
                    st.Close();
                }

                //rtb_Log.Text += $"Что-то считалось" + Environment.NewLine;
            }
            catch (FileNotFoundException ex)
            {
                rtb_Log.Text += $"Error: {ex.Message}" + Environment.NewLine;

                //moduleLog.WriteLine(ex.Message);
                //return; // false
            }
            catch (Exception err)
            {
                rtb_Log.Text += $"Error: {err.Message}" + Environment.NewLine;

                //moduleLog.WriteLine(err.Message);
                //return false;
            }

            if (prodCal != null)
            {
                SoftPlc.Calendar calendar = new SoftPlc.Calendar
                {
                    Title = new List<SoftPlc.Calendar.TitleX>(),
                    Holyday = new List<SoftPlc.Calendar.DayH>(),
                    Day = new List<SoftPlc.Calendar.DayX>(),
                    Week = new List<SoftPlc.Calendar.WeekX>()
                };

                foreach (var tit in prodCal.holidays)
                {
                    //rtb_Log.Text += $"id={tit.id} title={tit.title}" + Environment.NewLine;

                    var ti = new SoftPlc.Calendar.TitleX { id = string.IsNullOrEmpty(tit.id.ToString()) ? "" : tit.id.ToString(), title = string.IsNullOrEmpty(tit.title) ? "" : tit.title };
                    calendar.Title.Add(ti);
                }


                foreach (var days in prodCal.days)
                {
                    //rtb_Log.Text += $"d={days.d} h={days.h} t={days.t} f={days.f}" + Environment.NewLine;

                    var pd = new SoftPlc.Calendar.DayH
                    {
                        date = string.IsNullOrEmpty(days.d) ? "" : days.d,
                        h = string.IsNullOrEmpty(days.h) ? "" : days.h,
                        t = string.IsNullOrEmpty(days.t) ? "" : days.t,
                        f = string.IsNullOrEmpty(days.f) ? "" : days.f
                    };
                    calendar.Holyday.Add(pd);
                }

                XmlSerializer serializer = new XmlSerializer(typeof(SoftPlc.Calendar));

                AddNodes(serializer, calendar);  // TEST   Пока отключить
            }
        }
        #endregion Button Calendar

    }


    #region TimeSpanExpander - В последствии не нужен, чисто для измерения времени чтения/записи
    public static class TimeSpanExtender
    {
        /// <summary>
        /// Present timespan in friendly form adequate for the size.
        /// </summary>
        public static string Display(this TimeSpan timeSpan)
        {
            if (timeSpan.TotalDays >= 1)
                return string.Format("{0:N1} days", timeSpan.TotalDays);
            if (timeSpan.TotalHours >= 1)
                return string.Format("{0:N1} h", timeSpan.TotalHours);
            if (timeSpan.TotalMinutes >= 1)
                return string.Format("{0:N1} min", timeSpan.TotalMinutes);
            if (timeSpan.TotalSeconds >= 1)
                return string.Format("{0:N1} s", timeSpan.TotalSeconds);
            if (timeSpan.TotalMilliseconds >= 10)
                return string.Format("{0:N0} ms", timeSpan.TotalMilliseconds);
            if (timeSpan.TotalMilliseconds >= 1)
                return string.Format("{0:N1} ms", timeSpan.TotalMilliseconds);
            double totalMicroseconds = timeSpan.TotalMilliseconds * 1000;
            if (totalMicroseconds >= 10)
                return string.Format("{0:N0} μs", totalMicroseconds);
            return string.Format("{0:N1} μs", totalMicroseconds);
        }
    }
    #endregion TimeSpanExpander
}
