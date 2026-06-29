// ------------ Не удалять, используется в Release !!!
using System.Threading.Tasks; // - НЕ УДАЛЯТЬ!!!
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
using System.Threading;
using System.Windows.Forms;

//////!!!!!! - Планируемые доработки
/// спросить ИИ о структуре проекта, переразместить методы по модулям для читаемости

// ==============================================================
namespace MailNotifier
{
    public partial class FormMain : Form
    {
        // ==========================================================================
        #region ====================    Глобальные переменные    ====================
        /* ------------ */
        // ------------ Cсылка на текущую форму для доступа из других классов 
        public static FormMain Form = null;

        // ------------ Параметры для реализации перемещения окна за заголовок 
        private Point MouseNow = new Point(); // - текущая позиция мыши для перемещения окна
        private MouseClicked TrayMouse = new MouseClicked(); // - сведения о кликах мышью по иконке в трее
        private ParametersMain Parameters = new ParametersMain(); // - глобальный объект Параметров для доступа к ним из других классов

        // ------------ Иконки для трея
        private Icon icTrayEmpty; // - иконка для трея при отсутствии непрочитанных писем
        private Icon icTrayOpen; // - иконка для трея при наличии непрочитанных писем
        private Icon icTrayClose; // - иконка для трея при закрытии окна
        private Icon icTrayUpdate; // - иконка для трея при обновлении

        // ------------ Переменные Drag-and-Drop панелей аккаунтов
        private int startMouseX = 0; // - начальная позиция мыши при начале перетаскивания
        private int originalPanelIndex = -1; // - исходный индекс панели в контейнере
        private Panel draggedPanel = null; // - ссылка на перетаскиваемую панель
        private bool isDraggingPanel = false; // - флаг, указывающий, что панель перетаскивается
        private WorkAccount draggedAccount = null; // - ссылка на перетаскиваемый аккаунт

        // ------------ Форма-прокси для отображения  Drag-and-Drop
        private Form dragProxyForm = null; // - форма-прокси для отображения скриншота перетаскиваемой панели
        
        // ------------
        #endregion


        // ==========================================================================
        #region ========================    Конструктор      ========================
        /* ------------ */
        public FormMain()
        {
            InitializeComponent();

            // ===============   Назначение стартовых настроек    ===================      
            Form = this;
            CaptionFormMain.Text = "Mail Notifier";

            // - выполнение стартовых функций для инициализации формы
            SetStartToolTips(); // - установить подсказки для элементов формы
            ErrorMessageClear(); // - очистить панель отображения ошибок
            SetStartParameters(); // - установить стартовые параметры из файла
            LoadParametersFromFile(); // - загрузить параметры из файла
            ShowAccountParameters(false); // - скрыть параметры аккаунта
            RebuildElementsByAutorization(); // - отобразить элементы в соответствии с правами доступа
        }

        // ------------
        #endregion


        // ==========================================================================
        #region ===================    Основные события формы     ===================
        /* ------------ */
        // ==================================== Событие При отрисовке панели формы (градиентная заливка)
        private void PanelFormMain_Paint(object sender, PaintEventArgs e)
        {
            // - создаем линейный градиент от темно-синего к черному по горизонтали и заливаем им всю панель формы
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
            // - запрет автоскрытия окна при старте в режиме debug
#if !DEBUG
            MinimazeAndMaximazeFormToTray();  
#endif

            // - установка двойной буферизации для всех основных элементов формы
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

            // - установка иконок для трея
            Rectangle Resolution = Screen.PrimaryScreen.Bounds;
            Location = new Point(
                Resolution.Right - Bounds.Width - 10,
                Resolution.Bottom - Bounds.Height - 55);

            // - запрет автопроверки почты при старте в режиме debug
#if !DEBUG
            // - задержка для корректного отображения иконки в трее при старте программы
            var tmp = Task.Run(async delegate { await Task.Delay(500); });
            tmp.Wait();

            // - загрузка иконок для трея из ресурсов           
            PanelSettingsAccount.Padding = new Padding(4, 44, 4, 11);
            PanelSettingsProgramm.Padding = new Padding(4, 44, 4, 11);
            UpdateAllAccountsPreProcessing();
#endif
        }

        // ==================================== Событие При нажатии мыши на заголовок Формы (старт перемещения) 
        private void PanelFormMainHeader_MouseDown(object sender, MouseEventArgs e)
        {
            // - если нажата не левая кнопка мыши, не начинать перемещение
            if (e.Button != MouseButtons.Left) return;

            // - сохранить текущую позицию мыши для расчета смещения при перемещении
            MouseNow = e.Location;
            Cursor = Cursors.SizeAll;
        }

        // ==================================== Событие При отпускании мыши заголовока Формы (финиш перемещения)
        private void PanelFormMainHeader_MouseUp(object sender, MouseEventArgs e)
        {
            // - если нажата не левая кнопка мыши, не заканчивать перемещение
            if (e.Button != MouseButtons.Left) return;

            // - сбросить позицию мыши и курсор
            Cursor = Cursors.Default;
        }

        // ==================================== Событие При перемещении мышью заголовока Формы (перемещение)
        private void PanelFormMainHeader_MouseMove(object sender, MouseEventArgs e)
        {
            // - если курсор не в режиме перемещения, или кнопка не леваяне перемещать окно
            if (Cursor != Cursors.SizeAll) return;
            if (e.Button != MouseButtons.Left) return;

            // - переместить окно на смещение от начальной позиции мыши до текущей позиции мыши
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

        // ==================================== Событие При покидании мышью переключателя закладок (цвет)
        private void RightLink_MouseLeave(object sender, EventArgs e)
        {
            // - если мышь покидает переключатель, снимем выделение
            bool Unsderline = ((UnderlinedLabel)sender).Underline;
            ((UnderlinedLabel)sender).LinkColor = (Unsderline) ? Color.WhiteSmoke : Color.LightBlue;
        }

        // ==================================== Событие При клике на переклчатель закладок (закладка)
        private void RightLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            // - снять выделение со всех переключателей и скрыть все панели
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

            // - выделить текущий переключатель
            UnderlinedLabel cLink = ((UnderlinedLabel)sender);
            // ------------
            cLink.Underline = true;
            cLink.LinkColor = Color.WhiteSmoke;

            // - отобразить панель, связанную с данным переключателем и обновить ее содержимое
            switch (cLink.Name)
            {
                case "RightLinkInfo": // - если переключатель "Информация о аккаунте"
                    PanelSetInfoAccount.Visible = true;
                    break;

                case "RightLinkProgramm": // - если переключатель "Настройки программы"
                    PanelSettingsProgramm.Visible = true;
                    break;

                case "RightLinkAccount": // - если переключатель "Настройки аккаунта"
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
            // - если нажат не Enter, то выйти
            if (e.KeyCode != Keys.Enter) {
                LoginErrorLabel.Visible = false;
                return; 
            }

            // - если введен пароль администратора, то авторизовать пользователя (режим редакнирования)
            if (LoginPassword.Text == Parameters.ParamWork.Settings.SavedSettings.Password) {
                Parameters.ParamWork.Settings.IsAdmin = true;
                ButtonFormHeaderAdmin_Click(sender, e);
                LoginPassword.Text = "";
            }

            // - иначе сообщить об ошибке авторизации
            else LoginErrorLabel.Visible = true;
        }

        // ==================================== Событие При клике на ссылку деавторизации
        private void UnloginLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            // - если есть несохраненные изменения, то спросить о сохранении
            if (CaptionFormMain.Text.EndsWith("*"))
            {
                DialogResult DlgRes = CustomMsgBox.ShowQuestion();
                
                // - если отмена, не выполняем никакие действия
                if (DlgRes == DialogResult.Cancel) return;

                // - если отказ, то перечитываем параметры из файла
                else if (DlgRes == DialogResult.No)
                {
                    LoadParametersFromFile();
                    ShowAccountParameters(false);

                    // - убираем признак изменений
                    ButtonFormHeaderSave.Visible = false;
                    CaptionFormMain.Text = "Mail Notifier";
                }

                // - если согласие, то сохраняем изменения
                else ButtonFormHeaderSave_Click(sender, e);
            }

            // - выходим из режима редактирования
            Parameters.ParamWork.Settings.IsAdmin = false;
            RebuildElementsByAutorization();
        }

        // ==================================== Событие При клике на кнопку - скопировать сообщение ошибки
        private void ButtonErrorCopy_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(InfoError.Text.Trim());
        }

        // ==================================== Событие При клике на кнопку - очистить сообщение ошибки
        private void ButtonErrorClear_Click(object sender, EventArgs e)
        {
            // - если текущий аккаунт выбран...
            if (Parameters.ParamWork.Settings.CurrentAccount != null)
            {
                // - очистить сообщение об ошибке
                Parameters.ParamWork.Settings.CurrentAccount.ErrorText = "";
                Parameters.ParamWork.Settings.CurrentAccount.IsError = false;

                // - сбросить отметки ошибок на панели меню
                Parameters.ParamWork.Settings.CurrentAccount.MenuPanel.SetAlert(false);
                Parameters.ParamWork.Settings.CurrentAccount.LeftButton.SetAlert(false);

                // - обновить данные панели параметров
                ShowAccountParameters(true);
            }
        }

        // ==================================== Событие При клике на кнопку заголовка - свернуть в трей
        private void ButtonFormHeaderToTray_Click(object sender, EventArgs e)
        {
            // - если есть признак изменений, то спросить о сохранении
            if (CaptionFormMain.Text.EndsWith("*"))
            {
                DialogResult DlgRes = CustomMsgBox.ShowQuestion();

                // - если отмена, не выполняем никакие действия
                if (DlgRes == DialogResult.Cancel) return;

                // - если отказ, то перечитываем параметры из файла
                else if (DlgRes == DialogResult.No)
                {
                    LoadParametersFromFile();
                    ShowAccountParameters(false);
                    RebuildElementsByAutorization();

                    // - убираем признак изменений
                    ButtonFormHeaderSave.Visible = false;
                    CaptionFormMain.Text = "Mail Notifier";
                } 

                // - если согласие, то сохраняем изменения
                else ButtonFormHeaderSave_Click(sender, e);
            }

            // - сворачиваем окно в трей, очищаем выделение кнопки
            MinimazeAndMaximazeFormToTray();
            ButtonFormHeaderTray.FlatAppearance.MouseOverBackColor = Color.Transparent;
        }

        // ==================================== Событие При клике на кнопку заголовка - главное меню
        private void ButtonFormHeaderMenu_Click(object sender, EventArgs e)
        {
            // - определим место для вывода меню и покажем его
            Point Menulocation = new Point(Location.X + Width - 270, Location.Y + 35);
            MainMenuStrip.Show(Menulocation);
        }

        // ==================================== Событие При клике на кнопку заголовка - авторизация
        private void ButtonFormHeaderAdmin_Click(object sender, EventArgs e)
        {
            // - если панель авторизации уже открыта, скроем ее
            if (PanelAutorization.Visible) {
                PanelAutorization.Visible = false;
                RebuildElementsByAutorization();
            }

            // - если панель авторизации закрыта, покажем ее
            else {
                // - предварительно скроем все панели
                ToolsTabPanel.Visible = false;
                LoginErrorLabel.Visible = false;
                PanelSetInfoAccount.Visible = false;
                PanelSettingsAccount.Visible = false;
                PanelSettingsProgramm.Visible = false;

                // - проверим, утановлен ли пароль, и установим признак администрирования
                bool IsAdmin = Parameters.ParamWork.Settings.IsAdmin;
                bool IsPassFree = Parameters.ParamWork.Settings.SavedSettings.Password == "";
                
                // - отобразим панель авторизации
                PanelAutorization.Visible = true;

                // - если пароль не установлен, то установим признак администрирования по умолчанию
                if (!IsAdmin && IsPassFree) {
                    Parameters.ParamWork.Settings.IsAdmin = true;
                    ButtonFormHeaderAdmin_Click(sender, e);
                }
            }
        }

        // ==================================== Событие При клике на кнопку заголовка - сохранить настройки
        private void ButtonFormHeaderSave_Click(object sender, EventArgs e)
        {
            // - если не установлен признак изменений, выходим
            if (!CaptionFormMain.Text.EndsWith("*")) return;

            // - сохраняем настройки
            bool SaveRes = Parameters.SaveSettings();

            // - если не удалось сохранить, выведем сообщение об ошибке
            if (!SaveRes)
                ErrorMessageShow(Parameters.ParamWork.Settings.ErrorText);

            // - если удалось, обновим элементы и снимем признак изменений
            else
            {               
                CaptionFormMain.Text = "Mail Notifier";
                ButtonFormHeaderSave.Visible = false;
                RebuildElementsByParameters();
            }
        }

        // ==================================== Событие При изменении значения в PropertyGrid настроек программы
        private void PropertyGridProgramm_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
        {
            // - установим признак изменений на форме
            CaptionFormMain.Text = "Mail Notifier *";
            ButtonFormHeaderSave.Visible = true;
        }

        // ------------
        #endregion


        // ==========================================================================
        #region ===============  Функции панелей настройки Editors   ================
        /* ------------ */
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
            // - если аккаунт не выбран, очистим панель и выйдем
            if (Parameters.ParamWork.Settings.CurrentAccount == null) return;

            // - если аккаунт выбран, отобразим его настройки
            PropertyGridAccount.SelectedObject = Parameters.ParamWork.Settings.CurrentAccount.Account;
            PropertyGridAccount.ViewForeColor = Color.Gainsboro;
            PropertyGridAccount.ForeColor = Color.Gainsboro;
        }

        // ==================================== Обновление панели настроек
        private void UpdateParametersPanel()
        {
            
            // - выделим текущие настройки программы
            PropertyGridProgramm.SelectedObject = Parameters.ParamWork.Settings.SavedSettings;
            PropertyGridProgramm.ViewForeColor = Color.Gainsboro;
            PropertyGridProgramm.ForeColor = Color.Gainsboro;
        }

        // ------------
        #endregion


        // ==========================================================================
        #region ===============  Функции внешнего вызова из классов  ================
        /* ------------*/
        // ==================================== Показать сообщение об ошибке
        private void ErrorMessageShow(String ErrorMessage)
        {
            // - если нет ошибки, очистим панель и выйдем
            InfoError.Visible = true;
            ButtonErrorCopy.Visible = true;
            ButtonErrorClear.Visible = true;

            // - разлочим кнопку очистки ошибки только, если ошибка не критическая
            ButtonErrorClear.Enabled = !Parameters.ParamWork.IsCriticalError;

            // - отобразим текст ошибкина панели ошибок
            InfoError.Text = ErrorMessage;
        }

        // ==================================== Очистить сообщение об ошибке
        private void ErrorMessageClear()
        {
            // - очистим панель ошибок
            InfoError.Visible = false;
            ButtonErrorCopy.Visible = false;
            ButtonErrorClear.Visible = false;

            // - очистим текст ошибки на панели ошибок
            InfoError.Text = "";
        }

        // ==================================== Закрыть главное меню
        private void MainMenuStripClose()
        {
            MainMenuStrip.Close();
        }

        // ==================================== Обработка клика по панели кнопок
        public void BtnPanelClick(ButtonPanel sender, EventArgs e)
        {
            // - закрыть главное меню, если клик был из него 
            MainMenuStripClose();
            
            // - определить действие по имени кнопки
            switch (sender.Name)
            {
                // - если нажата кнопка выхода, проверим наличие несохраненных изменений
                case "Exit":
                    // - если есть несохраненные изменения, спросим, что делать
                    if (CaptionFormMain.Text.EndsWith("*"))
                    {
                        DialogResult DlgRes = CustomMsgBox.ShowQuestion();
                        
                        // - если пользователь отменил действие, не закрываем программу
                        if (DlgRes == DialogResult.Cancel) break;

                        // - если пользователь согласился, сэмулируем сохранение изменения
                        else if (DlgRes == DialogResult.Yes) ButtonFormHeaderSave_Click(sender, e);
                    }

                    // - закрыть программу
                    FormMain.Form.Close();
                    break;

                // - если нажата кнопка сворачивания в трей, свернуть / развернуть окно
                case "Tray":
                    MinimazeAndMaximazeFormToTray();
                    break;

                // - если нажата кнопка обновления, перечитать почту во всех аккаунтах и обновить отображение
                case "Update":
                    UpdateAllAccountsPreProcessing();
                    if (Parameters.ParamWork.Settings.CurrentAccount != null) ShowAccountParameters(true);
                    break;

                // - для остальных кнопок (кнопки аккаунтов в меню и на панели)
                default:

                    // - найти аккаунт, связанный с кнопкой, по имени
                    WorkAccount mAccount = Parameters.ParamWork.Accounts.Find(item => item.Name == sender.Name);

                    // - если клик был из меню, открыть аккаунт в браузере, 
                    if (sender.IsMenu) {
                        if (mAccount != null) OpenAccountInBrowser(mAccount);
                        return; }

                    //  - иначе (клик по левой панели) отобразить настройки аккаунта
                    else
                    {
                        // - если аккаунт уже выбран, сбросить цвет его кнопки на панели и в меню
                        if (Parameters.ParamWork.Settings.CurrentAccount != null) 
                        {
                            WorkAccount CurrentAccount = Parameters.ParamWork.Settings.CurrentAccount;
                            CurrentAccount.LeftButton.BtnPanel.Controls["P_Caption"].ForeColor = Color.Gainsboro;
                            CurrentAccount.LeftButton.BtnPanel.BackColor = ColorTranslator.FromHtml("#00001a"); 
                        }

                        // - установить цвет кнопки выбранного аккаунта на панели и в меню
                        if (mAccount != null) {
                            mAccount.LeftButton.BtnPanel.Controls["P_Caption"].ForeColor = Color.White;
                            mAccount.LeftButton.BtnPanel.BackColor = ColorTranslator.FromHtml("#005"); }

                        // - выделить / снять  выделение аккаунта, отобразить изменения на панели настроек
                        bool IsCurrent = Parameters.ParamWork.Settings.CurrentAccount == mAccount;
                        Parameters.ParamWork.Settings.CurrentAccount = (IsCurrent) ? null : mAccount;
                        ShowAccountParameters(true);
                    }
                    // ------------
                    break;
            }
        }
        
        // ------------
        #endregion


        // ==========================================================================
        #region =================  Обобщенные функции формы    ======================
        /* ------------ */

        // ---- для скрытия иконки меню на панели задач
        // ---- экземпляр класса для работы с окнами из dll
        [DllImport("User32.dll", ExactSpelling = true, CharSet = CharSet.Auto)]
        // - функция для установки активности окна
        public static extern bool SetForegroundWindow(HandleRef hWnd);
        /* ------------ */

        // ==================================== Реально убирает мерцания
        protected override CreateParams CreateParams
        {
            get
            {
                // - добавим стиль WS_EX_COMPOSITED для формы
                CreateParams handleParam = base.CreateParams;
                handleParam.ExStyle |= 0x02000000;   // WS_EX_COMPOSITED       
                return handleParam;
            }
        }

        // ==================================== Открытие меню от иконки в трее
        private void OpenTrayMenu()
        {
            // - активируем окно меню 
            SetForegroundWindow(new HandleRef(this, this.Handle));
            MainMenuStrip.Show(this, this.PointToClient(Cursor.Position));
        }

        // ==================================== Дополнительно убирает мерцания
        void SetDoubleBuffered(Control c, bool value)
        {
            // - получим доступ к защищенному свойству DoubleBuffered класса Control через рефлексию
            PropertyInfo pi = typeof(Control).GetProperty("DoubleBuffered",
                BindingFlags.SetProperty | BindingFlags.Instance | BindingFlags.NonPublic);

            // - если свойство найдено, установим его значение для данного контрола и обновим стили отрисовки
            if (pi != null)
            {
                // - установим значение свойства DoubleBuffered для данного контрола
                pi.SetValue(c, value, null);

                // - получим доступ к защищенному методу SetStyle класса Control через рефлексию
                MethodInfo mi = typeof(Control).GetMethod("SetStyle",
                    BindingFlags.Instance | BindingFlags.InvokeMethod | BindingFlags.NonPublic);

                // - установим стили отрисовки для данного контрола, включая двойную буферизацию и уменьшая мерцание
                if (mi != null)
                    mi.Invoke(c, new object[] {
                        ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer, true });

                // - получим доступ к защищенному методу UpdateStyles класса Control через рефлексию
                mi = typeof(Control).GetMethod("UpdateStyles",
                    BindingFlags.Instance | BindingFlags.InvokeMethod | BindingFlags.NonPublic);

                // - вызовем метод UpdateStyles для данного контрола, чтобы применить изменения стилей отрисовки
                if (mi != null) mi.Invoke(c, null);
            }
        }

        // ==================================== Сворачивание и разворачивание окна программы в трей 
        private void MinimazeAndMaximazeFormToTray()
        {
            // - скроем основной контейнер формы
            MainSplitContainer.Visible = false;

            // - скроем окно программы 
            Visible = !Visible;
            ShowInTaskbar = !ShowInTaskbar;

            // - найдем кнопку сворачивания в трей в меню, иначе - выход           
            ToolStripItem[] ThisMenuItem = MainMenuStrip.Items.Find("Tray", true);
            if (ThisMenuItem[0] == null) return;

            // - получим панель с кнопкой сворачивания в трей
            var tlPanel = ((ToolStripControlHost)ThisMenuItem[0]).Control;
            if (tlPanel == null) return;

            // - получим колонку панели
            var tlColumn = (TableLayoutPanel)tlPanel.Controls[0];
            if (tlColumn == null) return;

            // - получим картинку в колонке панели
            var tlpPicBox = (PictureBox)tlColumn.Controls[0];
            if (tlpPicBox == null) return;

            // - изменим картинку в колонке панели
            string MenuIcon = Visible ? "MenuToTray48.png" : "MenuFromTray48.png";
            tlpPicBox.Image = MainImageList.Images[MenuIcon];

            // - получим текст в колонке панели
            var tlpCapt = (ColorLabel)tlColumn.Controls[1];
            if (tlpCapt == null) return;

            // - изменим текст в колонке панели
            tlpCapt.Text = Visible ? "Сврнуть окно настроек" : "Открыть окно настроек";

            // - отобразим панель навигации
            ToolsTabPanel.Visible = true;
            PanelSetInfoAccount.Visible = true;

            // - если свернули окно...
            if (!Visible)
            {
                // - выйдем из режима администратора, сбросим текущий аккаунт
                Parameters.ParamWork.Settings.IsAdmin = false;
                Parameters.ParamWork.Settings.CurrentAccount = null;

                // - перерисуем все элементы формы (без авторизации)
                RebuildElementsByAutorization();
                ShowAccountParameters(true);

                // - сбросим цвет всех кнопок в левой панели аккаунтов
                foreach (WorkAccount mAccount in Parameters.ParamWork.Accounts)
                    mAccount.LeftButton.BtnPanel.BackColor = ColorTranslator.FromHtml("#00001a");
            }

            // - отобразим основной контейнер формы
            MainSplitContainer.Visible = true;
        }

        // ==================================== Установить текст tooltyp в трее > 64 символов
        private void UpdateAllAccountsPostProcessing()
        {
            // - заголовок tooltip в трее
            string NotifyIconText = "Mail Notifier\r\n";

            // - установим время последней проверки почты
            Parameters.ParamWork.Settings.LastCheck = DateTime.Now;
            StatusFormMainCount.Text = "Всего непрочитанных писем: " + Parameters.ParamWork.Settings.Count.ToString();
            StatusFormMainTime.Text = "Последняя проверка почты: " + Parameters.ParamWork.Settings.LastCheck.ToString();

            // - если нет непрочитанных писем
            if (Parameters.ParamWork.Settings.Count == 0)
            {
                // - заполним текст tooltip в трее и установим иконку в трее
                NotifyIconText += "Непрочитанных писем: 0";
                NotifyIconMain.Icon = icTrayEmpty;
            }

            // - иначе: есть непрочитанные письма
            else
            {
                // - сообщим о количестве непрочитанных писем, и количестве аккаунов               
                NotifyIconText += "В " + Parameters.ParamWork.Settings.CountBoxes.ToString();
                NotifyIconText += (Parameters.ParamWork.Settings.CountBoxes > 1) ? " аккаунтах" : " аккаунте";
                NotifyIconText += "\r\nНепрочитанных писем: " + Parameters.ParamWork.Settings.Count.ToString();

                // - установим иконку в трее, запустим таймер мигания иконки в трее
                NotifyIconMain.Icon = icTrayOpen;
                TimerTrayShow.Start();
            }

            // - сохраним время последней проверки почты в параметрах
            NotifyIconText += "\r\nНа:  " + Parameters.ParamWork.Settings.LastCheck.ToString();

            // - сбросим признак обновления почты
            Parameters.ParamWork.Settings.IsUpdate = false;

            // - установим текст tooltip в трее через рефлексию, обойдя ограничения NotifyIcon.Text в 64 символов
            Type tNF = typeof(NotifyIcon);
            BindingFlags hidden = BindingFlags.NonPublic | BindingFlags.Instance;
            tNF.GetField("text", hidden).SetValue(NotifyIconMain, NotifyIconText);

            // - обновим отображение иконки в трее
            if ((bool)tNF.GetField("added", hidden).GetValue(NotifyIconMain))
                tNF.GetMethod("UpdateIcon", hidden).Invoke(NotifyIconMain, new object[] { true });
        }

        // ==================================== Прочитать почту выбранного аккаунта 
        private void UpdateCurrentAccount(ref WorkAccount mAccount)
        {
            try
            {
                // - создадим клиента Imap с параметрами выбранного аккаунта
                using (ImapClient Client = new ImapClient(mAccount.Account.Host, mAccount.Account.Port,
                mAccount.Account.Login, mAccount.Account.Password, AuthMethod.Login, true)) 
                {
                    // - получим количество непрочитанных писем аккаунта и запомним время проверки
                    IEnumerable<uint> uids = Client.Search(SearchCondition.Unseen());
                    mAccount.LastCheck = DateTime.Now;
                    mAccount.Count = uids.Count();

                    // - закроем клиента Imap
                    Client.Dispose();
                }
            }

            // - если ошибка...
            catch (Exception e)
            {
                // - установим признак ошибки и запомним ее текст
                mAccount.IsError = true;
                mAccount.ErrorText = "Ошибка чтения почты:\r\n-----\r\n" + e.Message;
            }
        }

        // ==================================== Прочитать почту всех аккаунтов
        private void UpdateAllAccounts()
        {            
            // - остановим таймер мигания иконки в трее
            TimerTrayShow.Stop();

            // - если нет аккаунтов или не включена автопроверка - выйдем
            bool isAutoCheck = Parameters.ParamWork.Settings.SavedSettings.AutoCheck;
            isAutoCheck = isAutoCheck && Parameters.ParamWork.Accounts.Count > 0;
            if (!isAutoCheck) return;

            // - установим иконку проверки в трее
            NotifyIconMain.Icon = icTrayUpdate;
            Parameters.ParamWork.Settings.IsUpdate = true;

            // - обнулим все счетчики
            Parameters.ParamWork.Settings.Count = 0;
            Parameters.ParamWork.Settings.CountBoxes = 0;
            Parameters.ParamWork.Settings.IsError = false;

            // - цикл обхода гнастроенных аккаунтов
            foreach (WorkAccount Account in Parameters.ParamWork.Accounts)
            {
                // - Определим текущий аккаунт и обнулим счетчик
                WorkAccount mAccount = Account;
                mAccount.Count = -1;

                // - если установленоа настройка очищать ошибки
                if (Parameters.ParamWork.Settings.SavedSettings.ClearErrors)
                {
                    // - очистим признак ошибки и текст прощшлой ошибки аккаунта
                    mAccount.IsError = false;
                    mAccount.ErrorText = "";
                }

                // - запустим отдельный поток для чтения почты и вызовем проверку текущего аккаунта
                Thread thisThread = new Thread(() => {
                    UpdateCurrentAccount(ref mAccount);
                });

                // - запустим поток с ожиданием, ждем 10 сек и прервем его по таймауту, если не выполнен
                thisThread.Start();
                thisThread.Join(10000);
                thisThread.Abort();

                // - если за время ожидания не полученны данные, сообщим об ошибке по таймауту
                if (mAccount.Count < 0)
                {
                    mAccount.Count = 0;
                    mAccount.IsError = true;
                    mAccount.ErrorText = "Ошибка чтения почты:" 
                        + "\r\n-----\r\nПрервано по тайиауту...";
                }

                // - установим количество непрочитанных писем аккаунта в меню и боковую панель
                Account.MenuPanel.SetCount(Account.Count);
                Account.LeftButton.SetCount(Account.Count);

                // - установим признак ошибки в меню и боковую панель
                Account.MenuPanel.SetAlert(Account.IsError);
                Account.LeftButton.SetAlert(Account.IsError);

                // - суммируем общие счетчики по всем аккаунтам
                Parameters.ParamWork.Settings.Count += Account.Count;
                Parameters.ParamWork.Settings.CountBoxes += (Account.Count == 0) ? 0 : 1;
                Parameters.ParamWork.Settings.IsError = Parameters.ParamWork.Settings.IsError || Account.IsError;
            }

            // - финализируем проверку почты
            UpdateAllAccountsPostProcessing();          
        }

        // ==================================== Прочитать почту всех аккаунтов
        private void UpdateAllAccountsPreProcessing()
        {
            TimerTrayMail.Stop(); // - остановим таймер проверки почты
            UpdateAllAccounts(); // - запустим проверку почты
            TimerTrayMail_start(); // - запустим таймер проверки почты
        }

        // ==================================== Получить название акаунта выбранного на панели
        public string GetCurrentButtonName()
        {
            // - если текущий аккаунт установлен, вернем его наименование
            bool IsCurrentAccount = (Parameters.ParamWork.Settings.CurrentAccount == null);
            return (IsCurrentAccount) ? "null" : Parameters.ParamWork.Settings.CurrentAccount.Name;
        }

        // ------------
        #endregion


        // ==========================================================================
        #region ============== Динамическое изменение элементов формы  ==============
        /* ------------ */
        // ==================================== Создание обязательных кнпок меню
        private void CreateFixedMenuitems()
        {
            // - верхний разделитель
            MainMenuStrip.Items.Add("-");

            // - кнопка обновить почту
            ButtonPanel CurrentItemButton = new ButtonPanel(null, "Update")
            {
                IsMenu = true,
                IsCount = false,
                IconName = "TrayUpdate48.png",
                Caption = "Перечитать почту",
            };
            // ------------
            CurrentItemButton.Iinitialize();
            // ------------ панель на кнопке
            PanelMenuItem CurrentItem = new PanelMenuItem(CurrentItemButton, new Size(260, 28))
            {
                Margin = new Padding(-6, 0, 9, 0),
                Dock = DockStyle.Fill
            };
            // ------------ добавление пунта меню
            MainMenuStrip.Items.Add(CurrentItem);

            // - кнопка свернуть/развернуть форму настроек
            CurrentItemButton = new ButtonPanel(null, "Tray")
            {
                IsMenu = true,
                IsCount = false,
                IconName = "MenuToTray48.png",
                Caption = "Сврнуть окно настроек",
            };
            CurrentItemButton.Iinitialize();
            // ------------ панель на кнопке
            CurrentItem = new PanelMenuItem(CurrentItemButton, new Size(260, 28))
            {
                Margin = new Padding(-6, 0, 9, 0),
                Dock = DockStyle.Fill,
                Name = "Tray"
            };
            // ------------ добавление пунта меню
            MainMenuStrip.Items.Add(CurrentItem);

            // - средний разделитель
            MainMenuStrip.Items.Add("-");

            // - кнопка выхода
            ButtonPanel CurrentItemButton1 = new ButtonPanel(null, "Exit")
            {
                IsMenu = true,
                IsCount = false,
                IconName = "MunuExit48.png",
                Caption = "Выйти из программы",
            };
            // ------------
            CurrentItemButton1.Iinitialize();
            // - панель на кнопке
            CurrentItem = new PanelMenuItem(CurrentItemButton1, new Size(260, 28))
            {
                Margin = new Padding(-6, 0, 9, 0),
                Dock = DockStyle.Fill
            };
            // ------------ добавление пунта меню
            MainMenuStrip.Items.Add(CurrentItem);

            // - расчет высоты меню
            MainMenuStrip.Height = 102 + 28 * Parameters.ParamWork.Accounts.Count;
        }

        // ================================= Создание кнопки и меню Аккаутна
        private void CreateAccountsLinks(WorkAccount mAccount)
        {
            // - создание панели - контейнера
            Panel BtnContainer = new Panel
            {
                Name = "Cnt_" + mAccount.Name,
                Dock = DockStyle.Top,
                Height = 42
            };

            // - создание панели-кнопки для левой панели
            ButtonPanel BtnElement = new ButtonPanel(mAccount)
            {
                IsMenu = false,
                Count = mAccount.Count,
                IsAlert = mAccount.IsError,
                IconColor = mAccount.Account.Color,
            };
            BtnElement.Iinitialize();

            // - добавление панели-кнопки в контейнер
            BtnContainer.Controls.Add(BtnElement.BtnPanel);
            MainLeftPanel.Controls.Add(BtnContainer);
            MainLeftPanel.Controls.SetChildIndex(BtnContainer, 0);

            // ------------ ------------
            // - ривязка событий для поддержки Drag-and-Drop (только в режиме админа)
            BtnElement.BtnPanel.MouseMove += AccountPanel_MouseMove; ////!!!! - ??????
            BtnContainer.MouseMove += AccountPanel_MouseUp;
            // ------------
            BtnElement.BtnPanel.MouseDown += AccountPanel_MouseDown;
            BtnElement.BtnPanel.MouseUp += AccountPanel_MouseUp;
            BtnContainer.MouseUp += AccountPanel_MouseUp;

            // ------------ ------------
            // - добавление кнопки в левую панель
            mAccount.LeftButton = BtnElement;

            // - создание панели-кнопки для меню
            ButtonPanel MenuElement = new ButtonPanel(mAccount)
            {
                IsMenu = true,
                Count = mAccount.Count,
                IsAlert = mAccount.IsError,
                IconColor =mAccount.Account.Color,
            };
            MenuElement.Iinitialize();

            // - параметры пункта меню
            PanelMenuItem CurrentItem = new PanelMenuItem(MenuElement, new Size(260, 28))
            {
                Margin = new Padding(-6, 0, 9, 0),
                Dock = DockStyle.Fill
            };

            // - добавление пункта в меню
            MainMenuStrip.Items.Add(CurrentItem);

            // - моъранение ссылки напункт меню в аккаунт
            mAccount.MenuPanel = MenuElement;
        }

        // ================================= Пересоздать меню при Изменении параметров
        private void RebuildElementsByParameters()
        {
            // - очистка меню и левой панели
            MainLeftPanel.Controls.Clear();
            MainMenuStrip.Items.Clear();
            TimerTrayMail.Stop();
            
            // - создадим меню для каждого настроенного аккаунта          
            foreach (var MyAccount in Parameters.ParamWork.Accounts)
                CreateAccountsLinks(MyAccount);
            
            // - создадим фиксированные пункты меню
            CreateFixedMenuitems();
            TimerTrayMail_start();
            
            // - отображение счетчика настроенных аккаунтов
            ToolsAnonLabel.Text = "Всего настроено аккаунтов:  " +
                Parameters.ParamWork.Accounts.Count.ToString();
        }

        // ==================================== Открыть выбранный аккаунт в браузере
        private void OpenAccountInBrowser(WorkAccount mAccount)
        {
            try {
                // - попытаемся открыть в браузере из настроек приложения
                Process.Start(Parameters.ParamWork.Settings.SavedSettings.Browser, mAccount.Account.Url); }
            catch {
                try {
                    // - попытаемся открыть в браузере по умолчанию
                    Process.Start(mAccount.Account.Url); }
                catch {
                    // - если не удалось открыть в браузере, вывести сообщение об ошибке
                    CustomMsgBox.ShowAllert("Не удалось открыть аккаунт в браузере.");
                }
            }
        }

        // ================================= Изменить доступные элементы формы при авторизации 
        private void RebuildElementsByAutorization()
        {
            // - определить текущую панель настроек по флагу подчеркивания
            bool isInfo = RightLinkInfo.Underline;
            bool isAccount = RightLinkAccount.Underline;
            bool isProgramm = RightLinkProgramm.Underline;
            
            // - скрыть все панели с настройками
            ToolsTabPanel.Visible = true;            
            PanelSetInfoAccount.Visible = true;
            
            // - скрыть элементы авторизации
            LoginErrorLabel.Visible = false;
            PanelAutorization.Visible = false;
            
            // - включение / выключение элементов, зависимых от режима администрирования            
            LoginPassword.Visible = ! Parameters.ParamWork.Settings.IsAdmin;
            LoginInfoLabel.Visible = ! Parameters.ParamWork.Settings.IsAdmin;            
            ToolsAnonLabel.Visible = ! Parameters.ParamWork.Settings.IsAdmin;
            // ------------  
            UnloginLink.Visible =  Parameters.ParamWork.Settings.IsAdmin;          
            LeftLinkAdd.Visible =  Parameters.ParamWork.Settings.IsAdmin;
            LeftLinkDel.Visible =  Parameters.ParamWork.Settings.IsAdmin;
            RightLinkProgramm.Visible =  Parameters.ParamWork.Settings.IsAdmin;

            // - отображение элементов, зависимых от наличия текущего аккаунта
            RightLinkAccount.Visible =  Parameters.ParamWork.Settings.IsAdmin 
                && ( Parameters.ParamWork.Settings.CurrentAccount != null);
            
            // - если режим администрирования не авторизирован
            if (!Parameters.ParamWork.Settings.IsAdmin)
            {
                // - установим текст статуса на анонимный
                StatusFormMainMode.Text = "Режим: анонимный";
                
                // - скрыть все панели с настройками, установим статус
                PanelSettingsAccount.Visible = false;
                PanelSettingsProgramm.Visible = false;                
                
                // - эуляция нажатия ссылки: информация об аккаунте
                RightLink_LinkClicked(RightLinkInfo, null);
            }
            
            // - иначе: режим администрирования авторизирован
            else {
                // - установим текст статуса на администрироание
                StatusFormMainMode.Text = "Режим: администратор";
                
                //  - если аккаунт выбран - эмуляция нажатия ссылки: настройуи аккаунта
                if (Parameters.ParamWork.Settings.CurrentAccount != null)  RightLink_LinkClicked(RightLinkAccount, null);
                // - иначе если доступно: эмуляция нажатия ссылки: программа
                else if (!isInfo || !RightLinkProgramm.Visible) RightLink_LinkClicked(RightLinkProgramm, null);
                // - иначе: эмуляция нажатия ссылки: информация об аккаунте
                else RightLink_LinkClicked(RightLinkInfo, null);
            }
        }

        // ==================================== Установка параметров выбранного аккаунта
        private void ShowAccountParameters(bool ErrorClear)
        {
            // - если указан флаг очистки ошибок — очистить сообщение об ошибке
            if (ErrorClear) ErrorMessageClear();

            // - определить, выбран ли текущий аккаунт
            bool isAccount =  Parameters.ParamWork.Settings.CurrentAccount != null;

            // - если текущий аккаунт выбран
            if (isAccount)
            {
                // - обновить подпись аккаунта в интерфейсе (логин)
                InfoAccount.Text = "Аккаунт: " +  Parameters.ParamWork.Settings.CurrentAccount.Account.Login;
                // - обновить информацию о количестве непрочитанных писем
                InfoMessages.Text = "Всего непрочитанных писем: " +  Parameters.ParamWork.Settings.CurrentAccount.Count.ToString();

                // - если проверка почты когда-либо выполнялась — показать время последней проверки,
                // --- иначе показать сообщение, что проверка не производилась
                if ( Parameters.ParamWork.Settings.CurrentAccount.LastCheck == DateTime.MinValue)
                    InfoLastCheck.Text = "Проверка почты аккаунта не производилась...";
                else InfoLastCheck.Text = "Последняя провека почты аккаунта: "
                    +  Parameters.ParamWork.Settings.CurrentAccount.LastCheck.ToString();

                // - если для аккаунта есть флаг ошибки — показать текст ошибки в окне сообщений
                if ( Parameters.ParamWork.Settings.CurrentAccount.IsError)
                    ErrorMessageShow( Parameters.ParamWork.Settings.CurrentAccount.ErrorText);
            }

            // - оначе: аккаунт не выбран — вывести соответствующий текст
            else InfoAccount.Text = "Аккаунт не выбран...";

            // - показать/скрыть элементы с информацией о сообщениях и времени последней проверки
            InfoMessages.Visible = isAccount;
            InfoLastCheck.Visible = isAccount;
            LeftLinkDel.Links[0].Enabled = isAccount;

            // - обновить доступность и вид элементов интерфейса в зависимости от авторизации
            RebuildElementsByAutorization();
        }

        // ================================= Загрузить настройки программы из файла настроек 
        private void LoadParametersFromFile()
        {
            // - создать и загрузить параметры программы из файла настроек
            Parameters = new ParametersMain();
            bool LoadRes = Parameters.LoadSettings();
            
            // - обновить элементы меню и данных текущего аккаунта
            RebuildElementsByParameters();
            UpdateParametersPanel();
            
            // - если загрузка не удалась — вывести сообщение об ошибке
            if (!LoadRes) ErrorMessageShow( Parameters.ParamWork.Settings.ErrorText);
        }

        // ==================================== Установка ToolTip для кнопок формы
        private void SetStartToolTips()
        {
            // - tooltip для кнопки сворачивания в трей
            ToolTip tButtonFormHeaderTray = new ToolTip();
            tButtonFormHeaderTray.SetToolTip(ButtonFormHeaderTray, "Свернуть в трей");
            
            // - tooltip для кнопки открытия главного меню
            ToolTip tButtonFormHeaderMenu = new ToolTip();
            tButtonFormHeaderMenu.SetToolTip(ButtonFormHeaderMenu, "Главное меню");
            
            // - tooltip для кнопки авторизации в режим администрирования
            ToolTip tButtonFormHeaderAdmin = new ToolTip();
            tButtonFormHeaderAdmin.SetToolTip(ButtonFormHeaderAdmin, "Режим администрирования");
            
            // - tooltip для кнопки сохранения настроек
            ToolTip tButtonFormHeaderSave = new ToolTip();
            tButtonFormHeaderAdmin.SetToolTip(ButtonFormHeaderSave, "Сохранить настройки");
            
            // - tooltip для кнопки очистки сообщения об ошибке
            ToolTip tButtonErrorClear = new ToolTip();
            tButtonErrorClear.SetToolTip(ButtonErrorClear, "Очистить сообщение об ошибке");
            
            // - tooltip для кнопки копирования сообщения об ошибке
            ToolTip tButtonErrorCopy = new ToolTip();
            tButtonErrorCopy.SetToolTip(ButtonErrorCopy, "Скопировать сообщение об ошибке");
        }
        
        // ==================================== Установка параметров элементов по умолчанию
        private void SetStartParameters()
        {   
            // - снять выделение ссылок настроек
            RightLinkInfo.Underline = true;
            RightLinkInfo.LinkColor = Color.WhiteSmoke;
            // ------------
            RightLinkAccount.Underline = false;
            RightLinkProgramm.Underline = false;
            
            // - установить стили кнопок заголовка формы (трей)
            ButtonFormHeaderTray.FlatAppearance.BorderSize = 0;
            ButtonFormHeaderTray.FlatAppearance.MouseDownBackColor = ColorTranslator.FromHtml("#006");
            ButtonFormHeaderTray.FlatAppearance.MouseOverBackColor = ColorTranslator.FromHtml("#007");   
            // - главное меню
            ButtonFormHeaderMenu.FlatAppearance.BorderSize = 0;
            ButtonFormHeaderMenu.FlatAppearance.MouseDownBackColor = ColorTranslator.FromHtml("#006");
            ButtonFormHeaderMenu.FlatAppearance.MouseOverBackColor = ColorTranslator.FromHtml("#007");           
            // - режим администрирования
            ButtonFormHeaderAdmin.FlatAppearance.BorderSize = 0;
            ButtonFormHeaderAdmin.FlatAppearance.MouseDownBackColor = ColorTranslator.FromHtml("#006");
            ButtonFormHeaderAdmin.FlatAppearance.MouseOverBackColor = ColorTranslator.FromHtml("#007");            
            // - сохранить настройки
            ButtonFormHeaderSave.FlatAppearance.BorderSize = 0;
            ButtonFormHeaderSave.FlatAppearance.MouseDownBackColor = ColorTranslator.FromHtml("#006");
            ButtonFormHeaderSave.FlatAppearance.MouseOverBackColor = ColorTranslator.FromHtml("#007");
           
            // - установить иконки кнопок заголовка формы
            icTrayEmpty = Icon.FromHandle(((Bitmap)MainImageList.Images["MailNotifier48.png"]).GetHicon());
            icTrayOpen = Icon.FromHandle(((Bitmap)MainImageList.Images["TrayOpen48.png"]).GetHicon());
            icTrayClose = Icon.FromHandle(((Bitmap)MainImageList.Images["TrayClosed48.png"]).GetHicon());
            icTrayUpdate = Icon.FromHandle(((Bitmap)MainImageList.Images["TrayUpdate48.png"]).GetHicon());
            
            // - установить иконку в трее
            NotifyIconMain.Icon = icTrayEmpty;
        }

        // ------------
        #endregion


        // ==========================================================================
        #region ===================       События TrayIcon        ===================
        /* ------------ */       
        // ==================================== Срабатывание таймера мигающей иконки при наличии сообщений
        private void TimerTrayShow_Tick(object sender, EventArgs e) 
        {
            // - сменить иконку в трее (мигание)
            NotifyIconMain.Icon = (NotifyIconMain.Icon == icTrayOpen) ? icTrayClose : icTrayOpen;
        }

        // ==================================== Срабатывание таймера проверки почты по периоду
        private void TimerTrayMail_Tick(object sender, EventArgs e)
        {
            // - обновить все аккаунты
            UpdateAllAccounts();
        }
        
        // ==================================== Проверка двойного клика мыши по иконке в трее 
        private void TimerTrayClick_Tick(object sender, EventArgs e)
        {
            // - остановить таймер проверки даблклик
            TimerTrayClick.Stop();
            
            // - если еще не бвло клика или даблкик средней кнопой - возврат
            if (TrayMouse.Count < 1) return;
            if (TrayMouse.Button == MouseButtons.Middle) return;
            
            // - кнопка правая...
            if (TrayMouse.Button == MouseButtons.Right)
            {
                // - если даблклик - обновить все аккаунты
                if (TrayMouse.Count > 1) UpdateAllAccountsPreProcessing();
                // - иначе открыть меню
                else OpenTrayMenu();
            }
            
            // - кнопка левая...
            if (TrayMouse.Button == MouseButtons.Left)
            {
                // - если даблклик - свернуть / развернуть в трей
                if (TrayMouse.Count > 1) ButtonFormHeaderToTray_Click(sender, e);
                // - иначе открыть меню
                else OpenTrayMenu();
            }
            
            // - сбросить счетчик кликов
            TrayMouse.Reset();
        }

        // ==================================== Клики мыши по иконке в трее
        private void NotifyIconMain_MouseDown(object sender, MouseEventArgs e)
        {
            // - если средняя кнопка мыши - обновить все аккаунты
            if (e.Button == MouseButtons.Middle)
            {
                UpdateAllAccountsPreProcessing(); 
                return;
            }
            
            // - если работает тайме даблклика, и это второй клик по кнопке - возврат
            if (TimerTrayClick.Enabled && e.Button == TrayMouse.Button)
            {
                TrayMouse.Count++; 
                return;
            }
            
            // - перезапускаем таймер даблклика
            TimerTrayClick.Stop();
            TimerTrayClick.Start();
            
            // - запоминаем параметры клика
            TrayMouse.Count = 1;
            TrayMouse.Button = e.Button;
        }

        // ==================================== Запуск автопроверки почты
        private void TimerTrayMail_start()
        {
#if !DEBUG
                // - установить интервал проверки почты
                Periods SavesPeriod = Parameters.ParamWork.Settings.SavedSettings.Period;
                TimerTrayMail.Interval = PeriodToMilliseconds(SavesPeriod);
                
                // - запустить таймер проверки почты если он включен и есть настроенные аккаунты
                bool isAutoCheck = Parameters.ParamWork.Settings.SavedSettings.AutoCheck;
                isAutoCheck = isAutoCheck && Parameters.ParamWork.Accounts.Count > 0;
                if (isAutoCheck) TimerTrayMail.Start();
#endif
        }

        // ------------
        #endregion

        
        // ==========================================================================
        #region =================  Добавление и удаление аккаунтов  =================
        /* ------------ */
        // ==================================== Добавить новый аккаунт
        private void LeftLinkAdd_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            // - сгенерировать базовое имя из логина и обеспечить уникальность
            int idx = Parameters.ParamWork.Accounts.Count + 1;
            String name = "account_" + idx.ToString();

            // - проверить, есть ли уже аккаунт с таким именем, и если есть — увеличить индекс
            while (Parameters.ParamWork.Accounts.Exists(a => a.Name == name))
                name = "account_" + (++idx).ToString();

            // - создать новый аккаунт с параметрами по умолчанию
            var newSaveAccount = new SaveAccount{ Login = name };

            // - создать новый рабочий аккаунт с параметрами по умолчанию
            var newWorkAccount = new WorkAccount
            {
                Count = 0,
                Name = name,
                IsError = false,
                Account = newSaveAccount
            };

            // - добавить новый аккаунт в коллекцию
            Parameters.ParamWork.Accounts.Add(newWorkAccount);

            // - отметить изменения (показать что есть несохранённые параметры)
            CaptionFormMain.Text = "Mail Notifier *";
            ButtonFormHeaderSave.Visible = true;

            // - обновить левую панель и меню
            RebuildElementsByParameters();

            // - найти и "кликнуть" только что добавленную кнопку, чтобы открыть её в правой панели
            if (newWorkAccount.LeftButton != null)
                BtnPanelClick(newWorkAccount.LeftButton, EventArgs.Empty);

            // - открыть правую панель аккаунта на редактирование
            RightLink_LinkClicked(RightLinkAccount, null);
        }

        // ==================================== Удалить текущий аккаунт
        private void LeftLinkDel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e) 
        {
            // - проверить, выбран ли текущий аккаунт, если нет — выйти
            if (Parameters.ParamWork.Settings.CurrentAccount == null) return; 

            // - сформировать заголовок и текст диалога подтверждения удаления
            String str1 = "Удалить текущий аккаунт:";
            String str2 = Parameters.ParamWork.Settings.CurrentAccount.Account.Login + "?";

            // - показать диалог подтверждения (без кнопки Отмена) и получить результат
            DialogResult DlgRes = CustomMsgBox.ShowQuestion(str1, str2, false);

            // - если пользователь отказался — выйти
            if (DlgRes != DialogResult.Yes) return;

            // - если пользователь подтвердил — удалить текущий аккаунт из коллекции
            var toRemove = Parameters.ParamWork.Settings.CurrentAccount;
            Parameters.ParamWork.Accounts.Remove(toRemove);
            Parameters.ParamWork.Settings.CurrentAccount = null;

            // - обновить элементы интерфейса: левая панель и отображение правой панели
            RebuildElementsByParameters();
            ShowAccountParameters(true);

            // - пометить форму как изменённую и показать кнопку сохранения
            CaptionFormMain.Text = "Mail Notifier *";
            ButtonFormHeaderSave.Visible = true;
        }

        // ==================================== Обработчик изменения свойств аккаунта в PropertyGrid
        private void PropertyGridAccount_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
        {
            try
            {
                // - обработать только изменение свойства Login
                if (e.ChangedItem != null && e.ChangedItem.PropertyDescriptor != null &&
                    e.ChangedItem.PropertyDescriptor.Name == "Login")
                {
                    // - создание нового имени из измененного login аккаунта
                    string Newvalue = e.ChangedItem.Value?.ToString() ?? e.OldValue?.ToString();
                    string NewName = Newvalue.Replace("@", "").Replace(".", "");

                    // - получить текущий аккаунт
                    var curAccount = Parameters.ParamWork.Settings.CurrentAccount;

                    // - если аккаунт найден и он не пустой
                    if (curAccount != null && curAccount.Account != null)
                    {
                        // - обновить рабочее имя аккаунта и логин в сохраняемых параметрах
                        curAccount.Name = NewName;

                        // - обновить название и подпись левой кнопки, если она инициализирована
                        if (curAccount.LeftButton != null && curAccount.LeftButton.BtnPanel != null)
                        {
                            curAccount.LeftButton.BtnPanel.Name = NewName;
                            if (curAccount.LeftButton.BtnPanel.Controls.ContainsKey("P_Caption"))
                                curAccount.LeftButton.BtnPanel.Controls["P_Caption"].Text = Newvalue;

                            // - найти и обновить название кнопки в классе Account
                            var bpField = curAccount.LeftButton.GetType().GetField("<Name>k__BackingField"
                                , System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
                            if (bpField != null) bpField.SetValue(curAccount.LeftButton, NewName);
                        }

                        // - найти и обновить пункт меню (если есть)
                        if (curAccount.MenuPanel != null && curAccount.MenuPanel.BtnPanel != null)
                        {
                            curAccount.MenuPanel.BtnPanel.Name = NewName;
                            if (curAccount.MenuPanel.BtnPanel.Controls.ContainsKey("P_Caption"))
                                curAccount.MenuPanel.BtnPanel.Controls["P_Caption"].Text = Newvalue;
                        }

                        // - обновить отображение правой панели (информация об аккаунте)
                        ShowAccountParameters(true);

                        // - пометить форму как изменённую и показать кнопку сохранения
                        CaptionFormMain.Text = "Mail Notifier *";
                        ButtonFormHeaderSave.Visible = true;
                    }
                }
            }

            // - обработка исключений
            catch { }
        }

        // ------------
        #endregion

        
        // ==========================================================================
        #region =================  Drag-and-Drop панелей аккаунтов  ================
        /* ------------ */
        // ==================================== Начало: создание прокси и подготовка к перемещению
        private void AccountPanel_MouseDown(object sender, MouseEventArgs e)
        {
            // - функционал доступен только в режиме администратора
            if (!Parameters.ParamWork.Settings.IsAdmin) return;
            if (e.Button != MouseButtons.Right) return;
            if (!(sender is Panel panelInto)) return;

            // - получаем родительскую панель, которая содержит перетаскиваемую панель
            Panel panel = (Panel) panelInto.Parent;

            // - инициализируем переменные для перетаскивания
            isDraggingPanel = true;
            draggedPanel = panel;
            originalPanelIndex = MainLeftPanel.Controls.GetChildIndex(panel);

            // - создаем скриншот перетаскиваемой панели
            Bitmap bmp = new Bitmap(panel.Width, panel.Height);
            panel.DrawToBitmap(bmp, new Rectangle(0, 0, bmp.Width, bmp.Height));

            // - создаем форму-прокси для отображения скриншота рядом с курсором
            dragProxyForm = new Form
            {
                // - полупрозрачность для лучшего визуального восприятия
                Opacity = 0.85, 
                TopMost = true,
                Size = bmp.Size,
                BackgroundImage = bmp,
                ShowInTaskbar = false,
                FormBorderStyle = FormBorderStyle.None,
                StartPosition = FormStartPosition.Manual,
                BackgroundImageLayout = ImageLayout.Stretch
            };

            // - Ограничиваем указатель мыши в границах MainLeftPanel (экранные координаты)
            Point panelTopLeft = MainLeftPanel.PointToScreen(Point.Empty);
            Rectangle panelRect = new Rectangle(panelTopLeft, MainLeftPanel.Size);

            // - позиционируем прокси со смещением, чтобы он был "рядом" с курсором
            Point screenPt = Control.MousePosition;
            dragProxyForm.Location = new Point(panelRect.Left + 50, screenPt.Y + 15);

            // - показываем форму-прокси
            dragProxyForm.Show();

            // - захватываем мышь, чтобы событие MouseUp сработало даже при выходе за пределы панели
            panelInto.Capture = true; // - захватываем панель-отправитель
            Cursor = Cursors.SizeAll; // - меняем курсор на "перемещение"

            // - запоминаем начальную X позицию курсора для блокировки по X при перемещении
            startMouseX = screenPt.X; 

            // - если есть текущий аккаунт, скроем правую панель с информацией об аккаунте
            if (Parameters.ParamWork.Settings.CurrentAccount != null)
                BtnPanelClick(Parameters.ParamWork.Settings.CurrentAccount.LeftButton, EventArgs.Empty);

            // - визуально затемняем панели, чтобы показать, что они неактивны во время перетаскивания
            foreach (WorkAccount wAccount in Parameters.ParamWork.Accounts)
            {
                if (wAccount.LeftButton != null && wAccount.LeftButton.BtnPanel != null)
                {
                    // - определяем, является ли текущая панель той, которую мы перетаскиваем
                    bool isCurrent = (draggedPanel.Name == "Cnt_" + wAccount.LeftButton.Name);

                    // - затемняем все панели, кроме перетаскиваемой и запомним текущий аккаунт
                    if (isCurrent) draggedAccount = wAccount;
                    else wAccount.LeftButton.BtnPanel.BackColor = ColorTranslator.FromHtml("#00001a");

                    // - меняем цвет текста на кнопке: белый для перетаскиваемой панели, серый для остальных
                    foreach (Control ctrl in wAccount.LeftButton.BtnPanel.Controls)
                        if (ctrl is Label lbl) lbl.ForeColor = isCurrent ? Color.White : Color.Gray; 
                }
            }
        }

        // ==================================== Завершение: обработка отпускания мыши и обновление порядка
        private void AccountPanel_MouseUp(object sender, MouseEventArgs e)
        {
            // - если не перетаскиваем панель или нет необходимых объектов — выйти
            if (!isDraggingPanel || draggedPanel == null || e.Clicks == 0) return;

            // - очищаем форму-прокси
            if (dragProxyForm != null)
            {
                dragProxyForm.Hide();
                dragProxyForm.Dispose();
                dragProxyForm = null;
            }

            // - сброс захвата мыши и восстановление стандартного курсора
            Cursor = Cursors.Default;
            if (sender is Panel p) p.Capture = false;

            // - перебираем аккаунты для их настройки
            foreach (WorkAccount wAccount in Parameters.ParamWork.Accounts)
            {
                // - визуально убираем затемнение после перетаскивания
                if (wAccount.LeftButton != null && wAccount.LeftButton.BtnPanel != null)
                    foreach (Control ctrl in wAccount.LeftButton.BtnPanel.Controls)
                        if (ctrl is Label lbl) lbl.ForeColor = Color.Gainsboro;

                // - откроем настройки текущего аккаунта
                if (wAccount == draggedAccount)
                    BtnPanelClick(draggedAccount.LeftButton, EventArgs.Empty);
            }

            // - получаем новый индекс перетаскиваемой панели после перемещения
            int newIndex = MainLeftPanel.Controls.GetChildIndex(draggedPanel);
            Point clientPt = MainLeftPanel.PointToClient(Control.MousePosition);

            // - проверка: отпущена ли мышь в пределах родительского контейнера
            bool isInsideBounds = (clientPt.X >= 0 && clientPt.X <= MainLeftPanel.Width &&
                                   clientPt.Y >= 0 && clientPt.Y <= MainLeftPanel.Height);

            // - если успешное перемещение: синхронизируем коллекцию данных с визуальным порядком
            if (isInsideBounds && newIndex != originalPanelIndex) ReorderAccountsListByVisualOrder();

            // - иначе - перемещение не удалось или вылет за границы: возвращаем на исходное место
            else MainLeftPanel.Controls.SetChildIndex(draggedPanel, originalPanelIndex);

            // - сброс состояния перетаскивания
            startMouseX = 0;
            draggedPanel = null;
            draggedAccount = null;
            isDraggingPanel = false;
            originalPanelIndex = -1;          
        }

        // ==================================== Процесс перемещения: визуальное перемещение панели
        private void AccountPanel_MouseMove(object sender, MouseEventArgs e)
        {
            // - если не перетаскиваем панель или нет необходимых объектов — выйти
            if (!isDraggingPanel || draggedPanel == null || dragProxyForm == null) return;

            // - перемещаем форму-прокси вслед за курсором
            Point screenPt = Control.MousePosition;

            // - ограничиваем указатель мыши в границах MainLeftPanel (экранные координаты)
            Point panelTopLeft = MainLeftPanel.PointToScreen(Point.Empty);
            Rectangle panelRect = new Rectangle(panelTopLeft, MainLeftPanel.Size);

            // - убедимся, что заблокированная по X позиция и текущая по Y находятся в пределах панели
            int lockedY = Math.Max(panelRect.Top, Math.Min(panelRect.Bottom - 1, screenPt.Y));

            // - устанавливаем позицию курсора внутри панели
            Cursor.Position = new Point(startMouseX, lockedY);
          
            // - перемещаем прокси относительно новой позиции курсора
            dragProxyForm.Location = new Point(panelRect.Left + 50, lockedY + 10);

            // - определяем координаты мыши относительно родительского контейнера
            Point clientPt = MainLeftPanel.PointToClient(Control.MousePosition);

            // - найдем панель, над которой находится курсор мыши
            Panel targetPanel = null;
            foreach (Control ctrl in MainLeftPanel.Controls)
                if (ctrl is Panel p && p != draggedPanel)
                    if (clientPt.Y >= p.Top && clientPt.Y <= p.Bottom)
                    {
                        targetPanel = p;
                        break;
                    }

            // - если найдена панель и она отличается от текущей позиции перетаскиваемой
            if (targetPanel != null)
            {
                // - получим индексы целевой панели и перетаскиваемой панели
                int targetIndex = MainLeftPanel.Controls.GetChildIndex(targetPanel);
                int currentIndex = MainLeftPanel.Controls.GetChildIndex(draggedPanel);

                // - SetChildIndex автоматически сдвигает все остальные элементы вверх или вниз
                if (targetIndex != currentIndex) 
                    MainLeftPanel.Controls.SetChildIndex(draggedPanel, targetIndex);
            }
        }

        // ==================================== Синхронизация порядка аккаунтов в коллекции с визуальным порядком панелей
        private void ReorderAccountsListByVisualOrder()
        {
            // - создаем новый список аккаунтов в порядке следования панелей в MainLeftPanel.Controls
            List<WorkAccount> newOrder = new List<WorkAccount>();

            // - элементы в MainLeftPanel.Controls идут сверху вниз (индекс 0 - самый верхний из-за DockStyle.Top)
            foreach (Control ctrl in MainLeftPanel.Controls)
                if (ctrl is Panel panel)
                {
                    // - инициализируем аккаунт, связанный с этой панелью
                    WorkAccount matchedAccount = null;

                    // - найдем аккаунт, чья левая кнопка (BtnPanel) находится внутри этой панели
                    foreach (var acc in Parameters.ParamWork.Accounts)
                        if (acc.LeftButton != null && acc.LeftButton.BtnPanel != null)
                            if (panel.Controls.Contains(acc.LeftButton.BtnPanel))
                            {
                                matchedAccount = acc;
                                break;
                            }

                    // - если аккаунт найден, добавляем его в новый список в порядке следования панелей
                    if (matchedAccount != null) newOrder.Insert(0, matchedAccount);
                }

            // - полностью перезаписываем коллекцию в новом порядке
            Parameters.ParamWork.Accounts.Clear();
            Parameters.ParamWork.Accounts.AddRange(newOrder);

            // - помечаем форму как изменённую, чтобы при закрытии был запрос на сохранение
            CaptionFormMain.Text = "Mail Notifier *";
            ButtonFormHeaderSave.Visible = true;
        }

        // ------------
        #endregion
    }
    // ------------ End of FormMain class
}

