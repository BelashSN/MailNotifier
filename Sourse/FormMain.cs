using System.Threading.Tasks; //  Не удалять!!!
// ------------ Не удалять, используется в Release !!!
using S22.Imap;
// ------------
using System;
using System.Linq;
using System.Drawing;
using System.Reflection;
using System.Diagnostics;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using System.Collections.Generic;
using System.Runtime.InteropServices;


// ==============================================================
namespace MailNotifier
{
    public partial class FormMain : Form
    {
        // ==========================================================================
        #region ====================    Глобальные переменные    ====================
        // ------------ 
        public static FormMain Form = null;
        private Point MouseNow = new Point();
        private MouseClicked TrayMouse = new MouseClicked();
        private ParametersMain Parameters = new ParametersMain();
        // ------------
        private Icon icTrayEmpty;
        private Icon icTrayOpen;
        private Icon icTrayClose;
        private Icon icTrayUpdate;
        // ------------
        #endregion


        // ==========================================================================
        #region ========================    Конструктор      ========================
        // ------------

        public FormMain()
        {
            InitializeComponent();

            // ===============   Назначение стартовых настроек    ===================      
            Form = this;
            CaptionFormMain.Text = "Mail Notifier";
            // ------------
            ErrorMessageClear();
            SetStartParameters();
            LoadParametersFromFile();
            ShowAccountParameters(false);
            RebuildElementsByAutorization();
        }
        // ------------
        #endregion


        // ==========================================================================
        #region ===================    Основные события формы     ===================
        // ------------
        
        // ==================================== /////!!!!!
        private void PanelFormMain_Paint(object sender, PaintEventArgs e)
        {
            using (var brush = new LinearGradientBrush(PanelFormMain.ClientRectangle,
                ColorTranslator.FromHtml("#000006"), ColorTranslator.FromHtml("#00002a"), LinearGradientMode.Horizontal))
                e.Graphics.FillRectangle(brush, PanelFormMain.ClientRectangle);
        }

        // ==================================== /////!!!!!
        private void FormMain_Resize(object sender, EventArgs e)
        {
            Invalidate();
        }

        // ==================================== /////!!!!!
        private void FormMain_Shown(object sender, EventArgs e)
        {
            // ------------ Запрет автоскрытия окна в режиме debug
            #if !DEBUG  
                MinimazeAndMaximazeFormToTray();  
            #endif
            // ------------
            SetDoubleBuffered(this, true);
            SetDoubleBuffered(PanelFormMain, true);
            SetDoubleBuffered(MainSplitContainer, true);
            SetDoubleBuffered(MainLeftPanel, true);
            SetDoubleBuffered(ToolsTabPanel, true);
            // ------------
            Rectangle Resolution = Screen.PrimaryScreen.Bounds;
            Location = new Point(
                Resolution.Right - Bounds.Width - 10,
                Resolution.Bottom - Bounds.Height - 55);
            // ------------  Запрет автопроверки почты в режиме debug
            #if !DEBUG
                TimerTrayMail.Interval =  Parameters.ParamWork.Settings.SavedSettings.Period;
                TimerTrayMail.Start();
                // ------------
                var tmp = Task.Run(async delegate { await Task.Delay(500); });
                tmp.Wait();
                // ------------
                UpdateAllAccounts();
                PanelSettingsAccount.Padding = new Padding(4, 44, 4, 11);
                PanelSettingsProgramm.Padding = new Padding(4, 44, 4, 11);
            #endif
        }

        // ==================================== /////!!!!!
        private void PanelFormMainHeader_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left) return;
            MouseNow = e.Location;
            Cursor = Cursors.SizeAll;
        }

        // ==================================== /////!!!!!
        private void PanelFormMainHeader_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left) return;
            Cursor = Cursors.Default;
        }

        // ==================================== /////!!!!!
        private void PanelFormMainHeader_MouseMove(object sender, MouseEventArgs e)
        {
            if (Cursor != Cursors.SizeAll) return;
            if (e.Button != MouseButtons.Left) return;
            this.Location = new Point(
                this.Location.X + e.Location.X - MouseNow.X,
                this.Location.Y + e.Location.Y - MouseNow.Y);
        }

        // ==================================== /////!!!!!
        private void ButtonFormHeaderTray_MouseHover(object sender, EventArgs e)
        {
            ButtonFormHeaderTray.FlatAppearance.MouseOverBackColor = ColorTranslator.FromHtml("#007");
        }

        // ==================================== /////!!!!!
        private void ButtonFormHeaderTray_MouseLeave(object sender, EventArgs e)
        {
            ButtonFormHeaderTray.FlatAppearance.MouseOverBackColor = Color.Transparent;
        }

        // ==================================== /////!!!!!
        private void RightLink_MouseHover(object sender, EventArgs e)
        {
            ((UnderlinedLabel)sender).LinkColor = Color.DeepSkyBlue;
        }

        // ==================================== /////!!!!!
        private void RightLink_MouseLeave(object sender, EventArgs e)
        {
            bool Unsderline = ((UnderlinedLabel)sender).Underline;
            ((UnderlinedLabel)sender).LinkColor = (Unsderline) ? Color.WhiteSmoke : Color.LightBlue;
        }

        // ==================================== /////!!!!!
        private void RightLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            RightLinkInfo.Underline = false;
            RightLinkAccount.Underline = false;
            RightLinkProgramm.Underline = false;
            // ------------
            PanelSetInfoAccount.Visible = false;
            PanelSettingsAccount.Visible = false;
            PanelSettingsProgramm.Visible = false;
            // ------------
            RightLinkInfo.LinkColor = Color.LightBlue;
            RightLinkAccount.LinkColor = Color.LightBlue;
            RightLinkProgramm.LinkColor = Color.LightBlue;
            // ------------
            UnderlinedLabel cLink = ((UnderlinedLabel)sender);
            // ------------
            cLink.Underline = true;
            cLink.LinkColor = Color.WhiteSmoke;
            // ------------
            switch (cLink.Name)
            {
                case "RightLinkInfo":
                    PanelSetInfoAccount.Visible = true;
                    break;
                case "RightLinkProgramm":
                    PanelSettingsProgramm.Visible = true;
                    break;
                case "RightLinkAccount":
                    PanelSettingsAccount.Visible = true;
                    UpdateAccuuntPanel();
                    break;                
            }
        }

        // ==================================== /////!!!!!
        private void Link_MouseHover(object sender, EventArgs e)
        {
            ((LinkLabel)sender).LinkColor = Color.DeepSkyBlue;
        }

        // ==================================== /////!!!!!
        private void Link_MouseLeave(object sender, EventArgs e)
        {
            ((LinkLabel)sender).LinkColor = Color.LightBlue;
        }

        // ==================================== /////!!!!!
        private void LoginPassword_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Enter) {
                LoginErrorLabel.Visible = false;
                return; }
            // ------------
            if (LoginPassword.Text == Parameters.ParamWork.Settings.SavedSettings.Password) {
                Parameters.ParamWork.Settings.IsAdmin = true;
                ButtonFormHeaderAdmin_Click(sender, e); }
            // ------------
            else LoginErrorLabel.Visible = true;
        }

        // ==================================== /////!!!!!
        private void UnloginLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Parameters.ParamWork.Settings.IsAdmin = false;
            RebuildElementsByAutorization();
        }

        // ==================================== /////!!!!!
        private void ButtonFormHeaderToTray_Click(object sender, EventArgs e)
        {
            MinimazeAndMaximazeFormToTray();
            ButtonFormHeaderTray.FlatAppearance.MouseOverBackColor = Color.Transparent;
        }

        // ==================================== /////!!!!!
        private void ButtonFormHeaderMenu_Click(object sender, EventArgs e)
        {
            Point Menulocation = new Point(Location.X + Width - 270, Location.Y + 35);
            MainMenuStrip.Show(Menulocation);
        }

        // ==================================== /////!!!!!
        private void ButtonFormHeaderAdmin_Click(object sender, EventArgs e)
        {
            if (PanelAutorization.Visible) {                
                PanelAutorization.Visible = false;
                RebuildElementsByAutorization(); }
            // ------------
            else {            
                ToolsTabPanel.Visible = false;
                LoginErrorLabel.Visible = false;
                PanelSetInfoAccount.Visible = false;
                PanelSettingsAccount.Visible = false;
                PanelSettingsProgramm.Visible = false;
                // ------------
                PanelAutorization.Visible = true; }
        }
        // ------------
#endregion


        // ==========================================================================
        #region ===============  Функции панелей настройки Editors   ================
        // ------------

        // ==================================== /////!!!!!
        private void ParamLabel_Click(object sender, EventArgs e)
        {
            Label curLabel = (Label)sender;
            string namelabel = curLabel.Name;
            bool isName = namelabel.IndexOf("nameL_") >= 0;
            string fullNameParam = namelabel.Replace(isName ? "nameL_" : "valueL_", "");
            // ------------
            var arrNamePram = fullNameParam.Split('_');
            if (arrNamePram.Length != 2) return;
            // ------------           
            string nameParam = arrNamePram[1];
            string ObjectName = arrNamePram[0] + "Editor";
            // ------------
            var PsInfo = Parameters.ParamWork.GetType().GetProperty(ObjectName);
            ParamContainers pSetting = (ParamContainers)PsInfo.GetValue(Parameters.ParamWork, null);
            if (pSetting == null) return;
            // ------------
            Color SelectColor = ColorTranslator.FromHtml("#000038");
            foreach (ParamElement pElement in pSetting.Elements)
            {
                bool isCurLabel = (pElement.LabellName == curLabel) || (pElement.LabellValue == curLabel);
                pElement.LabellValue.BackColor = isCurLabel ? SelectColor : Color.Transparent;
                pElement.LabellName.BackColor = isCurLabel ? SelectColor : Color.Transparent;
            }
            // ------------
            if (isName) return;
            // ------------ ------------ /////!!!!! вызов замены элемента формы 

        }

        // ==================================== /////!!!!!
        private void UpdateAccuuntPanel()
        {
            // -----------
            if ( Parameters.ParamWork.Settings.CurrentAccount == null) return;
            // -----------
            string pString =  Parameters.ParamWork.AccountParams;
            ParamContainers pSetElement = Parameters.ParamWork.AccountEditor;
            SplitContainer Container = SplitContainerParamAccount;
            SaveAccount Account =  Parameters.ParamWork.Settings.CurrentAccount.Account;
            // ------------
            var arrPram = pString.Split((char)9679);
            foreach (var gridRowParam in arrPram)
            {
                var arrRowPram = gridRowParam.Split((char)9678);
                if (arrRowPram.Length != 4) continue;
                // ------------
                ParamElement Element = pSetElement.Elements.First(obj => obj.Name == arrRowPram[0]);
                // ------------
                var PsInfo = Account.GetType().GetProperty(arrRowPram[0]);
                string vString = PsInfo.GetValue(Account, null).ToString();
                if (vString == null) vString = "Undefined";
                // ------------
                switch (arrRowPram[2])
                {
                    case "Password":
                        Element.LabellValue.Text = new string((char)9679, vString.Length);
                        break;
                    default:
                        Element.LabellValue.Text = vString;
                        break;
                }
                // ------------
                if (arrRowPram[3] == "Color") 
                    Element.LabellValue.ForeColor = ColorTranslator.FromHtml(vString);
            }
        }

        // ==================================== /////!!!!!
        private void CreateParamPanel(string ContainerName, string gridRowParam, ParamContainers pSetElement)
        {
            var arrRowPram = gridRowParam.Split((char)9678);
            if (arrRowPram.Length != 4) { return; } /////!!!!! message about error
            // ------------
            ParamElement curElement = new ParamElement();
            curElement.ParamString = gridRowParam;
            curElement.Name = arrRowPram[0];
            // ------------ ------------
            Label NameLabel = new Label();
            NameLabel.Tag = arrRowPram[3];
            NameLabel.Margin = new Padding(0);
            NameLabel.Size = new Size(100, 30);
            NameLabel.Text = arrRowPram[1] + ":";
            NameLabel.ForeColor = Color.Gainsboro;
            NameLabel.BackColor = Color.Transparent;
            NameLabel.Padding = new Padding(3, 0, 0, 0);
            NameLabel.TextAlign = ContentAlignment.MiddleLeft;
            NameLabel.Name = "nameL_" + ContainerName + "_" + arrRowPram[0];
            // ------------
            pSetElement.Container.Panel1.Controls.Add(NameLabel);
            NameLabel.Dock = DockStyle.Top;
            NameLabel.Click += ParamLabel_Click;
            // ------------
            curElement.LabellName = NameLabel;
            // ------------ ------------
            Label ValueLabel = new Label();
            ValueLabel.Tag = arrRowPram[3];
            ValueLabel.Margin = new Padding(0);
            ValueLabel.Size = new Size(100, 30);
            ValueLabel.ForeColor = Color.Gainsboro;
            ValueLabel.BackColor = Color.Transparent;
            NameLabel.Padding = new Padding(3, 0, 3, 0);
            ValueLabel.TextAlign = ContentAlignment.MiddleLeft;
            ValueLabel.Name = "valueL_" + ContainerName + "_" + arrRowPram[0];
            // ------------
            var PInfo =  Parameters.ParamWork.Settings.SavedSettings.GetType().GetProperty(arrRowPram[0]);
            var pValue = PInfo == null ? "" : PInfo.GetValue(Parameters.ParamWork.Settings.SavedSettings, null);
            // ------------
            switch (arrRowPram[2])
            {
                case "Bool":
                    ValueLabel.Text = ((bool)pValue) ? "Да" : "Нет";
                    break;
                case "Password":
                    ValueLabel.Text = new string((char)9679, ((string)pValue).Length);
                    break;
                case "Period":
                    ValueLabel.Text = ((int)pValue / 60000).ToString() + " мин.";
                    break;
                default:
                    ValueLabel.Text = pValue.ToString();
                    break;
            }
            // ------------
            pSetElement.Container.Panel2.Controls.Add(ValueLabel);
            ValueLabel.Dock = DockStyle.Top;
            ValueLabel.Click += ParamLabel_Click;
            // ------------
            curElement.LabellValue = ValueLabel;
            // ------------ ------------
            pSetElement.Elements.Add(curElement);
        }

        // ==================================== /////!!!!!
        public void CreateParametersPanels(string ContainerName)
        {
            // ------------
            String varName = "SplitContainerParam" + ContainerName;
            var Containers = this.Controls.Find(varName, true);
            if (Containers == null) { return; } /////!!!!! message about error
            if (Containers.Length < 1) { return; } /////!!!!! message about error
            SplitContainer Container = (SplitContainer)Containers[0];
            // ------------
            varName = ContainerName + "Params";
            var PsInfo =  Parameters.ParamWork.GetType().GetProperty(varName);
            string pString = (string)PsInfo.GetValue( Parameters.ParamWork, null);
            if (pString == null) { return; } /////!!!!! message about error
            // ------------
            ParamContainers pSetElement = new ParamContainers(Container);
            pSetElement.Elements = new List<ParamElement>();
            pSetElement.EditEnable = false;
            pSetElement.EditControl = null;
            // ------------
            var arrPram = pString.Split((char)9679);
            foreach (var gridRowParam in arrPram)
                CreateParamPanel(ContainerName, gridRowParam, pSetElement);
            // ------------
            var PrInfo = Parameters.ParamWork.GetType().GetProperty(ContainerName + "Editor");
            PrInfo.SetValue(Parameters.ParamWork, pSetElement);
        }
        // ------------
        #endregion


        // ==========================================================================
        #region ===============  Функции внешнего вызова из классов  ================
        // ------------

        // ==================================== /////!!!!!
        private void ErrorMessageShow(String ErrorMessage)
        {
            InfoError.Visible = true;
            InfoError.Text = ErrorMessage;
        }

        // ==================================== /////!!!!!
        private void ErrorMessageClear()
        {
            InfoError.Visible = false;
            InfoError.Text = "";
        }

        // ==================================== /////!!!!!
        private void MainMenuStripClose()
        {
            MainMenuStrip.Close();
        }

        // ==================================== /////!!!!!
        public void BtnPanelClick(ButtonPanel sender, EventArgs e)
        {
            MainMenuStripClose();
            // ------------
            switch (sender.Name)
            {
                case "Exit":
                    FormMain.Form.Close();  /////!!!!!?????
                    break;
                case "Tray":
                    MinimazeAndMaximazeFormToTray();
                    break;
                case "Update":
                    UpdateAllAccounts();
                    break;
                default:
                    WorkAccount mAccount = Parameters.ParamWork.Accounts.Find(item => item.Name == sender.Name);
                    // ------------
                    if (sender.IsMenu) {
                        if (mAccount != null) OpenAccountInBrowser(mAccount);
                        return; }
                    // ------------
                    else
                    {
                        if ( Parameters.ParamWork.Settings.CurrentAccount != null) {
                             Parameters.ParamWork.Settings.CurrentAccount.LeftButton.BtnPanel.Controls["P_Caption"].ForeColor = Color.Gainsboro;
                             Parameters.ParamWork.Settings.CurrentAccount.LeftButton.BtnPanel.BackColor = ColorTranslator.FromHtml("#00001a"); }
                        // ------------
                        if (mAccount != null) {
                            mAccount.LeftButton.BtnPanel.Controls["P_Caption"].ForeColor = Color.White;
                            mAccount.LeftButton.BtnPanel.BackColor = ColorTranslator.FromHtml("#005"); }
                        // ------------
                         Parameters.ParamWork.Settings.CurrentAccount = ( Parameters.ParamWork.Settings.CurrentAccount == mAccount) ? null : mAccount;
                        ShowAccountParameters(true);
                    }
                    break;
            }
        }
        // ------------
        #endregion


        // ==========================================================================
        #region =================  Обобщенные функции формы    ======================
        // ------------

        // ------------ для скрытия иконки меню на панели задач
        [DllImport("User32.dll", ExactSpelling = true, CharSet = CharSet.Auto)]
        public static extern bool SetForegroundWindow(HandleRef hWnd);
        // ------------

        // ==================================== /////!!!!!
        private void OpenTrayMenu()
        {
            SetForegroundWindow(new HandleRef(this, this.Handle));
            MainMenuStrip.Show(this, this.PointToClient(Cursor.Position));
        }

        // ==================================== Реально убирает мерцания
        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams handleParam = base.CreateParams;
                handleParam.ExStyle |= 0x02000000;   // WS_EX_COMPOSITED       
                return handleParam;
            }
        }

        // ==================================== Дополнительно убирает мерцания
        void SetDoubleBuffered(Control c, bool value)
        {
            PropertyInfo pi = typeof(Control).GetProperty("DoubleBuffered",
                BindingFlags.SetProperty | BindingFlags.Instance | BindingFlags.NonPublic);
            // ------------
            if (pi != null)
            {
                pi.SetValue(c, value, null);
                // ------------
                MethodInfo mi = typeof(Control).GetMethod("SetStyle",
                    BindingFlags.Instance | BindingFlags.InvokeMethod | BindingFlags.NonPublic);
                // ------------
                if (mi != null)
                {
                    mi.Invoke(c, new object[] {
                        ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer, true });
                }
                // ------------
                mi = typeof(Control).GetMethod("UpdateStyles",
                    BindingFlags.Instance | BindingFlags.InvokeMethod | BindingFlags.NonPublic);
                // -----------
                if (mi != null) mi.Invoke(c, null);
            }
        }

        // ==================================== /////!!!!!
        private void MinimazeAndMaximazeFormToTray()
        {
            MainSplitContainer.Visible = false;
            // ------------
            Visible = !Visible;
            ShowInTaskbar = !ShowInTaskbar;
            // ------------           
            ToolStripItem[] ThisMenuItem = MainMenuStrip.Items.Find("Tray", true);
            // ------------
            if (ThisMenuItem[0] == null) return;
            // ------------
            var tlPanel = ((ToolStripControlHost)ThisMenuItem[0]).Control;
            if (tlPanel == null) return;
            // ------------
            var tlColumn = (TableLayoutPanel)tlPanel.Controls[0];
            if (tlColumn == null) return;
            // ------------
            var tlpPicBox = (PictureBox)tlColumn.Controls[0];
            if (tlpPicBox == null) return;
            // ------------
            string MenuIcon = Visible ? "MenuToTray48.png" : "MenuFromTray48.png";
            tlpPicBox.Image = MainImageList.Images[MenuIcon];
            // ------------
            var tlpCapt = (ColorLabel)tlColumn.Controls[1];
            if (tlpCapt == null) return;
            // ------------
            tlpCapt.Text = Visible ? "Сврнуть окно настроек" : "Открыть окно настроек";
            // ------------
            ToolsTabPanel.Visible = true;
            PanelSetInfoAccount.Visible = true;
            // ------------
            if (!Visible)
            {
                 Parameters.ParamWork.Settings.IsAdmin = false;
                 Parameters.ParamWork.Settings.CurrentAccount = null;
                RebuildElementsByAutorization();
                ShowAccountParameters(true);
                // ------------
                foreach (WorkAccount mAccount in Parameters.ParamWork.Accounts)
                    mAccount.LeftButton.BtnPanel.BackColor = ColorTranslator.FromHtml("#00001a");
            }
            // ------------
            MainSplitContainer.Visible = true;
        }

        // ==================================== /////!!!!!
        private void UpdateCurrentAccount(ref WorkAccount mAccount)
        {
            try
            {
                using (ImapClient Client = new ImapClient(mAccount.Account.Host, mAccount.Account.Port,
                mAccount.Account.Login, mAccount.Account.Password, AuthMethod.Login, true))
                {
                    IEnumerable<uint> uids = Client.Search(SearchCondition.Unseen());
                    mAccount.LastCheck = DateTime.Now;
                    mAccount.Count = uids.Count();
                    // ------------
                    Client.Dispose();
                }
            }
            // ------------
            catch (Exception e)
            {
                mAccount.IsError = true;
                mAccount.ErrorText = "Ошибка чтения почты:\r\n-----\r\n" + e.Message;
            }
        }

        // ==================================== /////!!!!!
        private void UpdateAllAccounts()
        {
            TimerTrayShow.Stop();
            NotifyIconMain.Icon = icTrayUpdate;
             Parameters.ParamWork.Settings.IsUpdate = true;
            // ------------
             Parameters.ParamWork.Settings.Count = 0;
             Parameters.ParamWork.Settings.IsError = false;
            // ------------
            foreach (WorkAccount Account in Parameters.ParamWork.Accounts)
            {
                WorkAccount mAccount = Account;
                UpdateCurrentAccount(ref mAccount);
                // ------------
                Account.MenuPanel.SetCount(Account.Count);
                Account.LeftButton.SetCount(Account.Count);
                // ------------
                Account.MenuPanel.SetAlert(Account.IsError);
                Account.LeftButton.SetAlert(Account.IsError);
                // ------------
                 Parameters.ParamWork.Settings.Count += Account.Count;
                 Parameters.ParamWork.Settings.IsError =  Parameters.ParamWork.Settings.IsError || Account.IsError;
            }
            // ------------
             Parameters.ParamWork.Settings.LastCheck = DateTime.Now;
            StatusFormMainCount.Text = "Всего непрочитанных писем: " +  Parameters.ParamWork.Settings.Count.ToString();
            StatusFormMainTime.Text = "Последняя проверка почты: " +  Parameters.ParamWork.Settings.LastCheck.ToString();
            // ------------
            if ( Parameters.ParamWork.Settings.Count == 0)
                NotifyIconMain.Icon = icTrayEmpty;
            else
            {
                NotifyIconMain.Icon = icTrayOpen;
                TimerTrayShow.Start();
            }
            // ------------
             Parameters.ParamWork.Settings.IsUpdate = false;
        }

        // ==================================== /////!!!!!
        public string GetCurrentButtonName()
        {
            return ( Parameters.ParamWork.Settings.CurrentAccount == null) ? "null" :  Parameters.ParamWork.Settings.CurrentAccount.Name;
        }
        // ------------
        #endregion


        // ==========================================================================
        #region ============== Динамическое изменение элементов формы  ==============
        // ------------

        // ==================================== /////!!!!!
        private void CreateFixedMenuitems()
        {
            // ------------ ------------
            MainMenuStrip.Items.Add("-");
            // ------------ ------------
            ButtonPanel CurrentItemButton = new ButtonPanel(null, "Update")
            {
                IsMenu = true,
                IsCount = false,
                IconName = "TrayUpdate48.png",
                Caption = "Перечитать почту",
            };
            CurrentItemButton.Iinitialize();
            // ------------
            PanelMenuItem CurrentItem = new PanelMenuItem(CurrentItemButton, new Size(260, 28));
            CurrentItem.Margin = new Padding(-6, 0, 9, 0);
            CurrentItem.Dock = DockStyle.Fill;
            // ------------
            MainMenuStrip.Items.Add(CurrentItem);
            // ------------ ------------
            CurrentItemButton = new ButtonPanel(null, "Tray")
            {
                IsMenu = true,
                IsCount = false,
                IconName = "MenuToTray48.png",
                Caption = "Сврнуть окно настроек",
            };
            CurrentItemButton.Iinitialize();
            // ------------
            CurrentItem = new PanelMenuItem(CurrentItemButton, new Size(260, 28));
            CurrentItem.Margin = new Padding(-6, 0, 9, 0);
            CurrentItem.Dock = DockStyle.Fill;
            CurrentItem.Name = "Tray";
            // ------------
            MainMenuStrip.Items.Add(CurrentItem);
            // ------------ ------------
            MainMenuStrip.Items.Add("-");
            // ------------ ------------
            ButtonPanel CurrentItemButton1 = new ButtonPanel(null, "Exit")
            {
                IsMenu = true,
                IsCount = false,
                IconName = "MunuExit48.png",
                Caption = "Выйти из программы",
            };
            CurrentItemButton1.Iinitialize();
            // ------------
            CurrentItem = new PanelMenuItem(CurrentItemButton1, new Size(260, 28));
            CurrentItem.Margin = new Padding(-6, 0, 9, 0);
            CurrentItem.Dock = DockStyle.Fill;
            // ------------
            MainMenuStrip.Items.Add(CurrentItem);
            // ------------ ------------
            MainMenuStrip.Height = 102 + 28 * Parameters.ParamWork.Accounts.Count;
        }

        // ================================= Создание кнопки и меню Аккаутна
        private void CreateAccountsLinks(WorkAccount mAccount)
        {
            Panel BtnContainer = new Panel();
            BtnContainer.Dock = DockStyle.Top;
            BtnContainer.Height = 42;
            // ------------
            ButtonPanel BtnElement = new ButtonPanel(mAccount)
            {
                IsMenu = false,
                Count = mAccount.Count,
                IsAlert = mAccount.IsError,
                IconColor = ColorTranslator.FromHtml(mAccount.Account.Color),
            };
            BtnElement.Iinitialize();
            // ------------
            BtnContainer.Controls.Add(BtnElement.BtnPanel);
            MainLeftPanel.Controls.Add(BtnContainer);
            MainLeftPanel.Controls.SetChildIndex(BtnContainer, 0);
            // ------------ ------------
            mAccount.LeftButton = BtnElement;
            // ------------ ------------
            ButtonPanel MenuElement = new ButtonPanel(mAccount)
            {
                IsMenu = true,
                Count = mAccount.Count,
                IsAlert = mAccount.IsError,
                IconColor = ColorTranslator.FromHtml(mAccount.Account.Color),
            };
            MenuElement.Iinitialize();
            // ------------
            PanelMenuItem CurrentItem = new PanelMenuItem(MenuElement, new Size(260, 28));
            CurrentItem.Margin = new Padding(-6, 0, 9, 0);
            CurrentItem.Dock = DockStyle.Fill;
            // ------------
            MainMenuStrip.Items.Add(CurrentItem);
            // ------------ ------------
            mAccount.MenuPanel = MenuElement;
            // ------------ ------------
        }

        // ================================= Перерисовать при Изменении параметров
        private void RebuildElementsByParameters()
        {
            MainLeftPanel.Controls.Clear();
            MainMenuStrip.Items.Clear();
            // ------------           
            foreach (var MyAccount in Parameters.ParamWork.Accounts)
                CreateAccountsLinks(MyAccount);
            // ------------
            CreateFixedMenuitems();
            // ------------ ------------
            ToolsAnonLabel.Text = "Всего настроено аккаунтов:  " +
                Parameters.ParamWork.Accounts.Count.ToString();
        }

        // ==================================== /////!!!!!
        private void OpenAccountInBrowser(WorkAccount mAccount)
        {
            try {
                Process.Start(Parameters.ParamWork.Settings.SavedSettings.Browser, mAccount.Account.Url); }
            catch {
                try {
                    Process.Start(mAccount.Account.Url); }
                catch {
                    /////Process.Start("https://tv.yandex.ru/"); //////!!!!!!
                }
            }
        }

        // ================================= /////!!!!! 
        private void RebuildElementsByAutorization()
        {
            bool isProgrammVisible = RightLinkProgramm.Visible;
            // ------------
            bool isInfo = RightLinkInfo.Underline;
            bool isAccount = RightLinkAccount.Underline;
            bool isProgramm = RightLinkProgramm.Underline;
            // ------------
            ToolsTabPanel.Visible = true;            
            PanelSetInfoAccount.Visible = true;
            // ------------
            LoginErrorLabel.Visible = false;
            PanelAutorization.Visible = false;
            // ------------
            UnloginLink.Visible =  Parameters.ParamWork.Settings.IsAdmin;
            LoginPassword.Visible = ! Parameters.ParamWork.Settings.IsAdmin;
            LoginInfoLabel.Visible = ! Parameters.ParamWork.Settings.IsAdmin;
            // ------------
            ToolsAnonLabel.Visible = ! Parameters.ParamWork.Settings.IsAdmin;
            // ------------            
            LeftLinkAdd.Visible =  Parameters.ParamWork.Settings.IsAdmin;
            LeftLinkDel.Visible =  Parameters.ParamWork.Settings.IsAdmin;
            // ------------
            RightLinkProgramm.Visible =  Parameters.ParamWork.Settings.IsAdmin;
            RightLinkAccount.Visible =  Parameters.ParamWork.Settings.IsAdmin && ( Parameters.ParamWork.Settings.CurrentAccount != null);
            // ------------
            if (! Parameters.ParamWork.Settings.IsAdmin)
            {
                PanelSettingsAccount.Visible = false;
                PanelSettingsProgramm.Visible = false;
                StatusFormMainMode.Text = "Режим: анонимный";
                RightLink_LinkClicked(RightLinkInfo, null);
                return;
            }
            // ------------
            StatusFormMainMode.Text = "Режим: администратор";
            // ------------
            if ( Parameters.ParamWork.Settings.CurrentAccount != null)  RightLink_LinkClicked(RightLinkAccount, null);
            else if (!isInfo || !isProgrammVisible) RightLink_LinkClicked(RightLinkProgramm, null);
            else RightLink_LinkClicked(RightLinkInfo, null);
        }

        // ==================================== Установка параметров выбранного аккаунта
        private void ShowAccountParameters(bool ErrorClear)
        {
            if (ErrorClear) ErrorMessageClear();
            bool isAccount =  Parameters.ParamWork.Settings.CurrentAccount != null;
            // ------------
            if (isAccount)
            {
                InfoAccount.Text = "Аккаунт: " +  Parameters.ParamWork.Settings.CurrentAccount.Account.Login;
                InfoMessages.Text = "Всего непрочитанных писем: " +  Parameters.ParamWork.Settings.CurrentAccount.Count.ToString();
                // ------------
                if ( Parameters.ParamWork.Settings.CurrentAccount.LastCheck == DateTime.MinValue)
                    InfoLastCheck.Text = "Проверка почты аккаунта не производилась...";
                else InfoLastCheck.Text = "Последняя провека почты аккаунта: "
                    +  Parameters.ParamWork.Settings.CurrentAccount.LastCheck.ToString();
                // ------------
                if ( Parameters.ParamWork.Settings.CurrentAccount.IsError)
                    ErrorMessageShow( Parameters.ParamWork.Settings.CurrentAccount.ErrorText);
            }
            // ------------
            else InfoAccount.Text = "Аккаунт не выбран...";
            // ------------
            InfoMessages.Visible = isAccount;
            InfoLastCheck.Visible = isAccount;
            // ------------
            RebuildElementsByAutorization();
        }

        // ================================= /////!!!!! 
        private void LoadParametersFromFile()
        {
            bool Res = Parameters.LoadSettings();
            // ------------
            RebuildElementsByParameters();
            CreateParametersPanels("Programm");
            CreateParametersPanels("Account");
            // ------------
            if (!Res) ErrorMessageShow( Parameters.ParamWork.Settings.ErrorText);
        }

        // ==================================== Установка параметров элементов по умолчанию
        private void SetStartParameters()
        {   
            RightLinkInfo.Underline = true;
            RightLinkInfo.LinkColor = Color.WhiteSmoke;
            // ------------
            RightLinkAccount.Underline = false;
            RightLinkProgramm.Underline = false;
            // ------------
            ButtonFormHeaderTray.FlatAppearance.BorderSize = 0;
            ButtonFormHeaderTray.FlatAppearance.MouseDownBackColor = ColorTranslator.FromHtml("#006");
            ButtonFormHeaderTray.FlatAppearance.MouseOverBackColor = ColorTranslator.FromHtml("#007");
            // ------------
            ButtonFormHeaderMenu.FlatAppearance.BorderSize = 0;
            ButtonFormHeaderMenu.FlatAppearance.MouseDownBackColor = ColorTranslator.FromHtml("#006");
            ButtonFormHeaderMenu.FlatAppearance.MouseOverBackColor = ColorTranslator.FromHtml("#007");
            // ------------
            ButtonFormHeaderAdmin.FlatAppearance.BorderSize = 0;
            ButtonFormHeaderAdmin.FlatAppearance.MouseDownBackColor = ColorTranslator.FromHtml("#006");
            ButtonFormHeaderAdmin.FlatAppearance.MouseOverBackColor = ColorTranslator.FromHtml("#007");
            // ------------
            icTrayEmpty = Icon.FromHandle(((Bitmap)MainImageList.Images["MailNotifier48.png"]).GetHicon());
            icTrayOpen = Icon.FromHandle(((Bitmap)MainImageList.Images["TrayOpen48.png"]).GetHicon());
            icTrayClose = Icon.FromHandle(((Bitmap)MainImageList.Images["TrayClosed48.png"]).GetHicon());
            icTrayUpdate = Icon.FromHandle(((Bitmap)MainImageList.Images["TrayUpdate48.png"]).GetHicon());
            // ------------
            EditComboBoxPanel.Visible = false; /////!!!!!
            // ------------
            NotifyIconMain.Icon = icTrayEmpty;
        }
        // ------------
        #endregion


        // ==========================================================================
        #region ===================       События TrayIcon        ===================
        // ------------

        // ==================================== /////!!!!!
        private void TimerTrayClick_Tick(object sender, EventArgs e)
        {
            TimerTrayClick.Stop();
            // ------------
            if (TrayMouse.Count < 1) return;
            if (TrayMouse.Button == MouseButtons.Middle) return;
            // ------------
            if (TrayMouse.Button == MouseButtons.Right)
            {
                if (TrayMouse.Count > 1) UpdateAllAccounts();
                else OpenTrayMenu();
            }
            // ------------
            if (TrayMouse.Button == MouseButtons.Left)
            {
                if (TrayMouse.Count > 1) MinimazeAndMaximazeFormToTray();               
                else  {
                    /////!!!!! UpdateAllAccounts(); /////!!!!!
                    OpenTrayMenu(); }
            }
            // ------------
            TrayMouse.Reset();
        }

        // ==================================== /////!!!!!
        private void TimerTrayShow_Tick(object sender, EventArgs e)
        {
            NotifyIconMain.Icon = (NotifyIconMain.Icon == icTrayOpen) ? icTrayClose : icTrayOpen;
        }

        // ==================================== /////!!!!!
        private void TimerTrayMail_Tick(object sender, EventArgs e)
        {
            UpdateAllAccounts();
        }

        // ==================================== /////!!!!!
        private void NotifyIconMain_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Middle)
            {
                UpdateAllAccounts(); return;
            }
            // ------------
            if (TimerTrayClick.Enabled && e.Button == TrayMouse.Button)
            {
                TrayMouse.Count++; return;
            }
            // ------------
            TimerTrayClick.Stop();
            TimerTrayClick.Start();
            // ------------
            TrayMouse.Count = 1;
            TrayMouse.Button = e.Button;
        }
        // ------------
        #endregion
    }
}