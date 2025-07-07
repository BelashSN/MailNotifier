// ------------ Не удалять, используется в Release !!!
using System.Threading.Tasks; //  Не удалять!!!
// ------------
using S22.Imap;
// ------------
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Windows.Forms;


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
            SetStartToolTips();
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
        /* ------------*/
        // ==================================== Событие При отрисовке панели формы (градиентная заливка)
        private void PanelFormMain_Paint(object sender, PaintEventArgs e)
        {
            using (var brush = new LinearGradientBrush(PanelFormMain.ClientRectangle,
                ColorTranslator.FromHtml("#000006"), ColorTranslator.FromHtml("#00002a"), LinearGradientMode.Horizontal))
                e.Graphics.FillRectangle(brush, PanelFormMain.ClientRectangle);
        }

        // ==================================== Событие При измененпии размеров формы (обновление компонентов)
        private void FormMain_Resize(object sender, EventArgs e)
        {
            Invalidate();
        }

        // ==================================== Событие При первом открытии формы (мерцание и автопроверка)
        private void FormMain_Shown(object sender, EventArgs e)
        {
            // ------------ Запрет автоскрытия окна в режиме debug
            #if !DEBUG  
                MinimazeAndMaximazeFormToTray();  
            #endif
            // ------------
            SetDoubleBuffered(this, true);
            SetDoubleBuffered(PanelFormMain, true);            
            SetDoubleBuffered(MainLeftPanel, true);
            SetDoubleBuffered(ToolsTabPanel, true);
            SetDoubleBuffered(ToolsAdminPanel, true);            
            SetDoubleBuffered(PanelAutorization, true);
            SetDoubleBuffered(MainSplitContainer, true);
            SetDoubleBuffered(PropertyGridAccount, true);
            SetDoubleBuffered(PanelSettingsAccount, true);
            SetDoubleBuffered(PanelSettingsProgramm, true);
            SetDoubleBuffered(PanelSettingSeparator, true);
            SetDoubleBuffered(MainSplitContainer.Panel1, true);
            SetDoubleBuffered(MainSplitContainer.Panel2, true);
            // ------------
            Rectangle Resolution = Screen.PrimaryScreen.Bounds;
            Location = new Point(
                Resolution.Right - Bounds.Width - 10,
                Resolution.Bottom - Bounds.Height - 55);
            // ------------  Запрет автопроверки почты в режиме debug
            #if !DEBUG
                Periods SavesPeriod = Parameters.ParamWork.Settings.SavedSettings.Period;
                TimerTrayMail.Interval = PeriodToMilliseconds(SavesPeriod);
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

        // ==================================== Событие При нажатии мыши на заголовок Формы (старт перемещения) 
        private void PanelFormMainHeader_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left) return;
            MouseNow = e.Location;
            Cursor = Cursors.SizeAll;
        }

        // ==================================== Событие При отпускании мыши заголовока Формы (финиш перемещения)
        private void PanelFormMainHeader_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left) return;
            Cursor = Cursors.Default;
        }

        // ==================================== Событие При перемещении мышью заголовока Формы (перемещение)
        private void PanelFormMainHeader_MouseMove(object sender, MouseEventArgs e)
        {
            if (Cursor != Cursors.SizeAll) return;
            if (e.Button != MouseButtons.Left) return;
            this.Location = new Point(
                this.Location.X + e.Location.X - MouseNow.X,
                this.Location.Y + e.Location.Y - MouseNow.Y);
        }

        // ==================================== Событие При наведении мыши на кнопку заголовока Формы (цвет)
        private void ButtonFormHeaderTray_MouseHover(object sender, EventArgs e)
        {
            ButtonFormHeaderTray.FlatAppearance.MouseOverBackColor = ColorTranslator.FromHtml("#007");
        }

        // ==================================== Событие При покидании мышью кнопки заголовока Формы (цвет)
        private void ButtonFormHeaderTray_MouseLeave(object sender, EventArgs e)
        {
            ButtonFormHeaderTray.FlatAppearance.MouseOverBackColor = Color.Transparent;
        }

        // ==================================== Событие При наведении мыши на переклчатель закладок (цвет)
        private void RightLink_MouseHover(object sender, EventArgs e)
        {
            ((UnderlinedLabel)sender).LinkColor = Color.DeepSkyBlue;
        }

        // ==================================== Событие При покидании мышью переклчателя закладок (цвет)
        private void RightLink_MouseLeave(object sender, EventArgs e)
        {
            bool Unsderline = ((UnderlinedLabel)sender).Underline;
            ((UnderlinedLabel)sender).LinkColor = (Unsderline) ? Color.WhiteSmoke : Color.LightBlue;
        }

        // ==================================== Событие При клике на переклчатель закладок (закладка)
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

        // ==================================== Событие При наведении мыши на ссылку (цвет)
        private void Link_MouseHover(object sender, EventArgs e)
        {
            ((LinkLabel)sender).LinkColor = Color.DeepSkyBlue;
        }

        // ==================================== Событие При покидании мышью ссылки (цвет)
        private void Link_MouseLeave(object sender, EventArgs e)
        {
            ((LinkLabel)sender).LinkColor = Color.LightBlue;
        }

        // ==================================== Событие При вводе пароля администратора (авторизация)
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

        // ==================================== Событие При клике на ссылку деавторизации
        private void UnloginLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Parameters.ParamWork.Settings.IsAdmin = false;
            RebuildElementsByAutorization();
        }

        // ==================================== Событие При клике на кнопку - скопировать сообщение ощибки
        private void ButtonErrorCopy_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(InfoError.Text.Trim());
        }

        // ==================================== Событие При клике на кнопку - очистить сообщение ощибки
        private void ButtonErrorClear_Click(object sender, EventArgs e)
        {
            if (Parameters.ParamWork.Settings.CurrentAccount != null)
            {
                Parameters.ParamWork.Settings.CurrentAccount.ErrorText = "";
                Parameters.ParamWork.Settings.CurrentAccount.IsError = false;
                // ------------
                Parameters.ParamWork.Settings.CurrentAccount.MenuPanel.SetAlert(false);
                Parameters.ParamWork.Settings.CurrentAccount.LeftButton.SetAlert(false);
                // ------------
                ShowAccountParameters(true);
            }
        }

        // ==================================== Событие При клике на кнопку заголовка - свернуть в трей
        private void ButtonFormHeaderToTray_Click(object sender, EventArgs e)
        {
            MinimazeAndMaximazeFormToTray();
            ButtonFormHeaderTray.FlatAppearance.MouseOverBackColor = Color.Transparent;
        }

        // ==================================== Событие При клике на кнопку заголовка - главное меню
        private void ButtonFormHeaderMenu_Click(object sender, EventArgs e)
        {
            Point Menulocation = new Point(Location.X + Width - 270, Location.Y + 35);
            MainMenuStrip.Show(Menulocation);
        }

        // ==================================== Событие При клике на кнопку заголовка - авторизация
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
                PanelAutorization.Visible = true;
                // ------------
                if (Parameters.ParamWork.Settings.SavedSettings.Password == "")
                {
                    bool IsAdmin = !Parameters.ParamWork.Settings.IsAdmin;
                    Parameters.ParamWork.Settings.IsAdmin = IsAdmin;
                    // ------------
                    if (IsAdmin) ButtonFormHeaderAdmin_Click(sender, e);
                    else RebuildElementsByAutorization();
                }
            }
        }
        // ------------
        #endregion


        // ==========================================================================
        #region ===============  Функции панелей настройки Editors   ================
        /*------------*/
        // ==================================== Получение размера периода в миллисекундах
        public int PeriodToMilliseconds(Periods Into)
        {
            return int.Parse(Into.ToString().Replace("m", ""));
        }

        // ==================================== Получение значения периода из миллисекунд
        public Periods MillisecondsToPeriod(int Into)
        {
            return (Periods)Enum.Parse(typeof(Periods), Into.ToString());
        }

        // ==================================== Обновление панели аккаунтов
        private void UpdateAccuuntPanel()
        {
            if (Parameters.ParamWork.Settings.CurrentAccount == null) return;
            PropertyGridAccount.SelectedObject = Parameters.ParamWork.Settings.CurrentAccount.Account;
            PropertyGridAccount.ViewForeColor = Color.Gainsboro;
            PropertyGridAccount.ForeColor = Color.Gainsboro;
        }

        // ==================================== Обновление панели настроек
        private void UpdateParametersPanel()
        {
            PropertyGridProgramm.SelectedObject = Parameters.ParamWork.Settings.SavedSettings;
            PropertyGridProgramm.ViewForeColor = Color.Gainsboro;
            PropertyGridProgramm.ForeColor = Color.Gainsboro;
        }
        // ------------
        #endregion


        // ==========================================================================
        #region ===============  Функции внешнего вызова из классов  ================
        /* ------------*/
        // ==================================== /////!!!!!
        private void ErrorMessageShow(String ErrorMessage)
        {
            InfoError.Visible = true;
            ButtonErrorCopy.Visible = true;
            ButtonErrorClear.Visible = true;
            ButtonErrorClear.Enabled = !Parameters.ParamWork.IsCriticalError;
            // ------------
            InfoError.Text = ErrorMessage;
        }

        // ==================================== /////!!!!!
        private void ErrorMessageClear()
        {
            InfoError.Visible = false;
            ButtonErrorCopy.Visible = false;
            ButtonErrorClear.Visible = false;
            // ------------
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
                    if (Parameters.ParamWork.Settings.CurrentAccount != null) ShowAccountParameters(true);
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
                        if (Parameters.ParamWork.Settings.CurrentAccount != null) {
                            WorkAccount CurrentAccount = Parameters.ParamWork.Settings.CurrentAccount;
                            CurrentAccount.LeftButton.BtnPanel.Controls["P_Caption"].ForeColor = Color.Gainsboro;
                            CurrentAccount.LeftButton.BtnPanel.BackColor = ColorTranslator.FromHtml("#00001a"); }
                        // ------------
                        if (mAccount != null) {
                            mAccount.LeftButton.BtnPanel.Controls["P_Caption"].ForeColor = Color.White;
                            mAccount.LeftButton.BtnPanel.BackColor = ColorTranslator.FromHtml("#005"); }
                        // ------------
                        bool IsCurrent = Parameters.ParamWork.Settings.CurrentAccount == mAccount;
                        Parameters.ParamWork.Settings.CurrentAccount = (IsCurrent) ? null : mAccount;
                        ShowAccountParameters(true);
                    }
                    break;
            }
        }
        // ------------
        #endregion


        // ==========================================================================
        #region =================  Обобщенные функции формы    ======================
        /* ------------*/
        // ------------ для скрытия иконки меню на панели задач
        [DllImport("User32.dll", ExactSpelling = true, CharSet = CharSet.Auto)]
        public static extern bool SetForegroundWindow(HandleRef hWnd);
        /* ------------*/

        // ==================================== Открытие меню от иконки в трее
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

        // ==================================== Сворачивание и разворачивание окна программы в трей 
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

        // ==================================== Прочитать почту выбранного аккаунта 
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

        // ==================================== Прочитать почту всех аккаунтов
        private void UpdateAllAccounts()
        {
            TimerTrayShow.Stop();
            NotifyIconMain.Icon = icTrayUpdate;
            Parameters.ParamWork.Settings.IsUpdate = true;
            // ------------
            Parameters.ParamWork.Settings.Count = 0;
            Parameters.ParamWork.Settings.CountBoxes = 0;
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
                Parameters.ParamWork.Settings.CountBoxes += (Account.Count == 0) ? 0 : 1;
                Parameters.ParamWork.Settings.IsError = Parameters.ParamWork.Settings.IsError || Account.IsError;
            }
            // ------------
            Parameters.ParamWork.Settings.LastCheck = DateTime.Now;
            StatusFormMainCount.Text = "Всего непрочитанных писем: " +  Parameters.ParamWork.Settings.Count.ToString();
            StatusFormMainTime.Text = "Последняя проверка почты: " +  Parameters.ParamWork.Settings.LastCheck.ToString();
            // ------------
            if (Parameters.ParamWork.Settings.Count == 0)
                NotifyIconMain.Icon = icTrayEmpty;
            else
            {
                NotifyIconMain.Icon = icTrayOpen;
                TimerTrayShow.Start();
            }
            // ------------
            Parameters.ParamWork.Settings.IsUpdate = false;
        }

        // ==================================== Получить название акаунта выбранного на панели
        public string GetCurrentButtonName()
        {
            bool IsCurrentAccount = (Parameters.ParamWork.Settings.CurrentAccount == null);
            return (IsCurrentAccount) ? "null" :  Parameters.ParamWork.Settings.CurrentAccount.Name;
        }
        // ------------
        #endregion


        // ==========================================================================
        #region ============== Динамическое изменение элементов формы  ==============
        /* ------------*/
        // ==================================== Создание обязательных кнпок меню
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
                IconColor = mAccount.Account.Color,
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
                IconColor =mAccount.Account.Color,
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

        // ==================================== Открыть выбранный аккаунт в браузере
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

        // ================================= Изменить доступные элементы формы при авторизации 
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
            if (Parameters.ParamWork.Settings.CurrentAccount != null)  RightLink_LinkClicked(RightLinkAccount, null);
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

        // ================================= Загрузить настройки программы из файла настроек 
        private void LoadParametersFromFile()
        {
            bool Res = Parameters.LoadSettings();
            // ------------
            RebuildElementsByParameters();
            UpdateParametersPanel();
            // ------------
            if (!Res) ErrorMessageShow( Parameters.ParamWork.Settings.ErrorText);
        }

        // ==================================== Установка ToolTip для кнопок формы
        private void SetStartToolTips()
        {
            ToolTip tButtonFormHeaderTray = new ToolTip();
            tButtonFormHeaderTray.SetToolTip(ButtonFormHeaderTray, "Свернуть в трей");
            // ------------
            ToolTip tButtonFormHeaderMenu = new ToolTip();
            tButtonFormHeaderMenu.SetToolTip(ButtonFormHeaderMenu, "Главное меню");
            // ------------
            ToolTip tButtonFormHeaderAdmin = new ToolTip();
            tButtonFormHeaderAdmin.SetToolTip(ButtonFormHeaderAdmin, "Режим администрирования");
            // ------------
            ToolTip tButtonErrorClear = new ToolTip();
            tButtonErrorClear.SetToolTip(ButtonErrorClear, "Очистить сообщение об ошибке");
            // ------------
            ToolTip tButtonErrorCopy = new ToolTip();
            tButtonErrorCopy.SetToolTip(ButtonErrorCopy, "Скопировать сообщение об ошибке");
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
            NotifyIconMain.Icon = icTrayEmpty;
        }
        // ------------
        #endregion


        // ==========================================================================
        #region ===================       События TrayIcon        ===================
        /* ------------*/
        // ==================================== Проверка двойного клика мыши по иконке в трее 
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

        // ==================================== Срабатывание таймера мигающей иконки при наличии сообщений
        private void TimerTrayShow_Tick(object sender, EventArgs e) 
        {
            NotifyIconMain.Icon = (NotifyIconMain.Icon == icTrayOpen) ? icTrayClose : icTrayOpen;
        }

        // ==================================== Срабатывание таймера проверки почты по периоду
        private void TimerTrayMail_Tick(object sender, EventArgs e)
        {
            UpdateAllAccounts();
        }

        // ==================================== Клики мыши по иконке в трее
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