using System;
using System.Drawing;
using System.Windows.Forms;


// ==============================================================
namespace MailNotifier
{
    // ==============================================================
    #region =========   Кнопка панели меню и main-кнопок   =========
    /* ------------ */
    public class ButtonPanel
    {
        // ==============================================================
        #region =============   Глобальные переменные    ================
        /* ------------ */
        // - панель кнопки
        public TableLayoutPanel BtnPanel { get; }
        // - свойства кнопки
        public int Count { get; set; } = 0;
        public string Name { get; } = "DefaultName";
        public bool IsMenu { get; set; } = false;
        public bool IsCount { get; set; } = true;
        public bool IsAlert { get; set; } = false;
        public string Caption { get; set; } = "Caption";
        public string IconName { get; set; } = "Account48.png";
        public Color IconColor { get; set; } = Color.Transparent;
        public ImageList ImgList { get; set; } = FormMain.Form.MainImageList;

        // ------------
        #endregion


        // ==============================================================
        #region ==================  Конструктор   =======================
        /* ------------ */
        public ButtonPanel(WorkAccount mAccount, string cName = "")
        {
            // - инициализация свойств кнопки
            Name = (mAccount != null) ? mAccount.Name : cName;
            Caption = (mAccount != null) ? mAccount.Account.Login : cName;

            // ------------ ------------ Основная панель кнопки
            BtnPanel = new TableLayoutPanel
            {
                Name = Name,
                RowCount = 1,
                ColumnCount = 4,
                Dock = DockStyle.Fill,
                BackColor = Color.Transparent,
                Padding = new Padding(4, 0, 0, 2)
            };
            BtnPanel.BackColor = ColorTranslator.FromHtml("#00001c");

            // - добавление стилей колонок
            BtnPanel.ColumnStyles.Add(new ColumnStyle());
            BtnPanel.ColumnStyles.Add(new ColumnStyle());
            BtnPanel.ColumnStyles.Add(new ColumnStyle());
            BtnPanel.ColumnStyles.Add(new ColumnStyle());

            // - установка стилей колонок
            BtnPanel.ColumnStyles[0].SizeType = SizeType.Absolute;
            BtnPanel.ColumnStyles[0].Width = 18;
            BtnPanel.ColumnStyles[1].SizeType = SizeType.Percent;
            BtnPanel.ColumnStyles[1].Width = 100;
            BtnPanel.ColumnStyles[2].SizeType = SizeType.Absolute;
            BtnPanel.ColumnStyles[2].Width = 18;
            BtnPanel.ColumnStyles[3].SizeType = SizeType.Absolute;
            BtnPanel.ColumnStyles[3].Width = 30;

            // ------------ ------------ Иконка цвета аккаунта
            PictureBox PanIcon = new PictureBox
            {
                Name = "P_Icon",
                Enabled = false,
                Anchor = AnchorStyles.None,
                Margin = new Padding(0, 2, 0, 0),
                Size = new Size(17, 17),
                Image = ImgList.Images["MyErr.png"],
                SizeMode = PictureBoxSizeMode.StretchImage,
                BackColor = Color.Transparent
            };

            // - добавление иконки на панель кнопки
            BtnPanel.Controls.Add(PanIcon);

            // ------------ ------------ Заголовок - имя аккаунта
            ColorLabel BtnCaption = new ColorLabel
            {
                Name = "P_Caption",
                Enabled = false,
                Text = Caption,
                Dock = DockStyle.Fill,
                ForeColor = Color.Gainsboro,
                Padding = new Padding(2, 0, 0, 4),
                TextAlign = ContentAlignment.MiddleLeft,
                Font = new Font("Microsoft Sans Serif", 9),
                BackColor = IconColor
            };

            // - добавление заголовка на панель кнопки
            BtnPanel.Controls.Add(BtnCaption);

            // ------------ ------------ Иконка предупреждения об ошибке
            PictureBox PanAlert = new PictureBox
            {
                Name = "P_Alert",
                Enabled = false,
                Anchor = AnchorStyles.None,
                Margin = new Padding(0, 3, 0, 0),
                Size = new Size(17, 17),
                Image = ImgList.Images["MyErr.png"],
                SizeMode = PictureBoxSizeMode.StretchImage,
                BackColor = Color.Transparent
            };

            // - добавление иконки предупреждения на панель кнопки
            BtnPanel.Controls.Add(PanAlert);

            // ------------ ------------ Указатель количества непрочитанных писем
            ColorLabel BtnCount = new ColorLabel
            {
                Name = "P_Count",
                Enabled = false,
                Text = Count.ToString(),
                Dock = DockStyle.Fill,
                ForeColor = Color.Gainsboro,
                Margin = new Padding(1, 0, 0, 0),
                TextAlign = ContentAlignment.MiddleRight,
                Font = new Font("Microsoft Sans Serif", 9),
                BackColor = Color.Transparent
            };

            // - добавление указателя на панель кнопки
            BtnPanel.Controls.Add(BtnCount);

            // ------------ ------------ Инициализация событий основной панели
            BtnPanel.Click += BtnPanelOnClick;
            BtnPanel.MouseHover += BtnPanel_MouseHover;
            BtnPanel.MouseLeave += BtnPanel_MouseLeave;
        }
        
        // ------------
        #endregion


        // ==============================================================
        #region ======== Реализацияе методов класса ButtonPanel  ========
        /* --------- */
        // ==================================== Стартовая инициализация 
        public void Iinitialize()
        {
            BtnPanel.Controls["P_Caption"].Text = Caption;
            BtnPanel.Controls["P_Count"].Text = IsCount ? Count.ToString() : "";
            ((PictureBox)BtnPanel.Controls["P_Icon"]).BackColor = IconColor;
            ((PictureBox)BtnPanel.Controls["P_Icon"]).Image = ImgList.Images[IconName];
            ((PictureBox)BtnPanel.Controls["P_Alert"]).Image = IsAlert ? ImgList.Images["MyErr.png"] : null;
        }

        // ==================================== Событие при покидании мышью панели кнопки
        private void BtnPanel_MouseLeave(object sender, EventArgs e)
        {
            // - установка курсора по умолчанию
            BtnPanel.Cursor = Cursors.Default;

            // - определение, является ли эта кнопка текущей (активной) в данный момент
            bool IsCurrentButton = FormMain.Form.GetCurrentButtonName() == this.Name;
            IsCurrentButton = IsCurrentButton && !this.IsMenu;

            // - установка цвета панели кнопки в зависимости от активности
            string panColor = (IsCurrentButton) ? "#000038" : "#00001a";
            BtnPanel.BackColor = ColorTranslator.FromHtml(panColor);
        }

        // ==================================== Событие при наведении мышью на панель кнопки
        private void BtnPanel_MouseHover(object sender, EventArgs e)
        {
            // - установка курсора и цвета панели кнопки при наведении мышью
            BtnPanel.Cursor = Cursors.Hand;
            BtnPanel.BackColor = ColorTranslator.FromHtml("#005");
        }

        // ==================================== Событие при клике мышью на панель кнопки
        private void BtnPanelOnClick(object sender, EventArgs e)
        {
            // - вызов метода обработки клика из главной формы приложения
            if (((MouseEventArgs)e).Button == MouseButtons.Left)
                FormMain.Form.BtnPanelClick(this, e);
        }

        // ==================================== Установка значения Count - кол-во непрочитанных писем
        public void SetCount(int NewCount)
        {
            Count = NewCount;
            BtnPanel.Controls["P_Count"].Text = IsCount ? Count.ToString() : "";
        }

        // ==================================== Установка иконки предупреждения об ошибке - Alert
        public void SetAlert(bool NewAlert)
        {
            IsAlert = NewAlert;
            ((PictureBox)BtnPanel.Controls["P_Alert"]).Image = IsAlert ? ImgList.Images["MyErr.png"] : null;
        }
        
        // ------------
        #endregion
    }
    
    // ------------
    #endregion

    
    // ==============================================================
    #region ========  Панель встраиваемых компонентов меню  =========
    /* ------------ */
    public class PanelMenuItem : ToolStripControlHost
    {
        // ==================================== Глобальные переменные
        // - панель меню, в которую будут добавляться кнопки
        public Panel PanelMenu;

        // ==================================== Конструктор
        public PanelMenuItem(ButtonPanel Container, Size cntSize) : base(new Panel())
        {
            // - инициализация панели меню
            PanelMenu = Control as Panel;

            // - установка свойств панели меню
            PanelMenu.Size = cntSize;
            PanelMenu.Left = -10;
            PanelMenu.Dock = DockStyle.Fill;
            PanelMenu.MinimumSize = cntSize;
            PanelMenu.MaximumSize = cntSize;
            PanelMenu.Margin = new Padding(0, 0, 0, 0);
            PanelMenu.Padding = new Padding(0, 0, 0, 0);
            PanelMenu.BackColor = Color.Transparent;

            // - добавление панели кнопки в панель меню
            PanelMenu.Controls.Add(Container.BtnPanel);
        }
    }
    
    // ------------
    #endregion
}
