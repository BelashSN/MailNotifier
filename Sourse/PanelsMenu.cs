using System;
using System.Drawing;
using System.Windows.Forms;


// ==============================================================
namespace MailNotifier
{
    // ==============================================================
    #region =========   Кнопка панели меню и main-кнопок   =========
    // ------------
    public class ButtonPanel
    {
        // ==============================================================
        #region =============   Глобальные переменные    ================
        // ------------  
        public TableLayoutPanel BtnPanel { get; }
        // ------------
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
        // ------------
        public ButtonPanel(WorkAccount mAccount, string cName = "")
        {
            Name = (mAccount != null) ? mAccount.Name : cName;
            Caption = (mAccount != null) ? mAccount.Account.Login : cName;

            // ------------ ------------ Основная панель кнопки
            BtnPanel = new TableLayoutPanel();
            BtnPanel.Name = Name;
            BtnPanel.RowCount = 1;
            BtnPanel.ColumnCount = 4;
            BtnPanel.Dock = DockStyle.Fill;
            BtnPanel.BackColor = Color.Transparent;
            BtnPanel.Padding = new Padding(4, 0, 0, 2);
            BtnPanel.BackColor = ColorTranslator.FromHtml("#00001c");
            // ------------
            BtnPanel.ColumnStyles.Add(new ColumnStyle());
            BtnPanel.ColumnStyles.Add(new ColumnStyle());
            BtnPanel.ColumnStyles.Add(new ColumnStyle());
            BtnPanel.ColumnStyles.Add(new ColumnStyle());
            // ------------
            BtnPanel.ColumnStyles[0].SizeType = SizeType.Absolute;
            BtnPanel.ColumnStyles[0].Width = 18;
            BtnPanel.ColumnStyles[1].SizeType = SizeType.Percent;
            BtnPanel.ColumnStyles[1].Width = 100;
            BtnPanel.ColumnStyles[2].SizeType = SizeType.Absolute;
            BtnPanel.ColumnStyles[2].Width = 18;
            BtnPanel.ColumnStyles[3].SizeType = SizeType.Absolute;
            BtnPanel.ColumnStyles[3].Width = 30;

            // ------------ ------------ Иконка цвета аккаунта
            PictureBox PanIcon = new PictureBox();
            PanIcon.Name = "P_Icon";
            PanIcon.Enabled = false;
            PanIcon.Anchor = AnchorStyles.None;
            PanIcon.Margin = new Padding(0, 2, 0, 0);
            PanIcon.Size = new Size(17, 17);
            PanIcon.Image = ImgList.Images["MyErr.png"];
            PanIcon.SizeMode = PictureBoxSizeMode.StretchImage;
            PanIcon.BackColor = Color.Transparent;
            // -------
            BtnPanel.Controls.Add(PanIcon);

            // ------------ ------------ Заголовок - имя аккаунта
            ColorLabel BtnCaption = new ColorLabel();
            BtnCaption.Name = "P_Caption";
            BtnCaption.Enabled = false;
            BtnCaption.Text = Caption;
            BtnCaption.Dock = DockStyle.Fill;
            BtnCaption.ForeColor = Color.Gainsboro;
            BtnCaption.Padding = new Padding(2, 0, 0, 4);
            BtnCaption.TextAlign = ContentAlignment.MiddleLeft;
            BtnCaption.Font = new Font("Microsoft Sans Serif", 9);
            BtnCaption.BackColor = IconColor;
            // -------
            BtnPanel.Controls.Add(BtnCaption);

            // ------------ ------------ Иконка предупреждения об ошибке
            PictureBox PanAlert = new PictureBox();
            PanAlert.Name = "P_Alert";
            PanAlert.Enabled = false;
            PanAlert.Anchor = AnchorStyles.None;
            PanAlert.Margin = new Padding(0, 3, 0, 0);
            PanAlert.Size = new Size(17, 17);
            PanAlert.Image = ImgList.Images["MyErr.png"];
            PanAlert.SizeMode = PictureBoxSizeMode.StretchImage;
            PanAlert.BackColor = Color.Transparent;
            // -------
            BtnPanel.Controls.Add(PanAlert);

            // ------------ ------------ Указатель количества непрочитанных писем
            ColorLabel BtnCount = new ColorLabel();
            BtnCount.Name = "P_Count";
            BtnCount.Enabled = false;
            BtnCount.Text = Count.ToString();
            BtnCount.Dock = DockStyle.Fill;
            BtnCount.ForeColor = Color.Gainsboro;
            BtnCount.Margin = new Padding(1, 0, 0, 0);
            BtnCount.TextAlign = ContentAlignment.MiddleRight;
            BtnCount.Font = new Font("Microsoft Sans Serif", 9);
            BtnCount.BackColor = Color.Transparent;
            // -------
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
        /*---------*/
        // ==================================== Стартовая инициализация 
        public void Iinitialize()
        {
            BtnPanel.Controls["P_Caption"].Text = Caption;
            BtnPanel.Controls["P_Count"].Text = IsCount ? Count.ToString() : "";
            ((PictureBox)BtnPanel.Controls["P_Icon"]).BackColor = IconColor;
            ((PictureBox)BtnPanel.Controls["P_Icon"]).Image = ImgList.Images[IconName];
            ((PictureBox)BtnPanel.Controls["P_Alert"]).Image = IsAlert ? ImgList.Images["MyErr.png"] : null;
        }

        // ==================================== Собитие при покидании мышью панели кнопки
        private void BtnPanel_MouseLeave(object sender, EventArgs e)
        {
            BtnPanel.Cursor = Cursors.Default;
            // ------------ ------------ Это - боковая кнопка панели
            bool IsCurrentButton = FormMain.Form.GetCurrentButtonName() == this.Name;
            IsCurrentButton = IsCurrentButton && !this.IsMenu;
            // ------------
            string panColor = (IsCurrentButton) ? "#000038" : "#00001a";
            BtnPanel.BackColor = ColorTranslator.FromHtml(panColor);
        }

        // ==================================== Собитие при наведении мышью на панель кнопки
        private void BtnPanel_MouseHover(object sender, EventArgs e)
        {
            BtnPanel.Cursor = Cursors.Hand;
            BtnPanel.BackColor = ColorTranslator.FromHtml("#005");
        }

        // ==================================== Собитие при клике мышью на панель кнопки
        private void BtnPanelOnClick(object sender, EventArgs e)
        {
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
    // ------------
    public class PanelMenuItem : ToolStripControlHost
    {
        // ==================================== Глобальные переменные
        public Panel PanelMenu;

        // ==================================== Конструктор
        public PanelMenuItem(ButtonPanel Container, Size cntSize) : base(new Panel())
        {
            PanelMenu = Control as Panel;
            // ------------
            PanelMenu.Size = cntSize;
            PanelMenu.Left = -10;
            PanelMenu.Dock = DockStyle.Fill;
            PanelMenu.MinimumSize = cntSize;
            PanelMenu.MaximumSize = cntSize;
            PanelMenu.Margin = new Padding(0, 0, 0, 0);
            PanelMenu.Padding = new Padding(0, 0, 0, 0);
            PanelMenu.BackColor = Color.Transparent;
            // ------------
            PanelMenu.Controls.Add(Container.BtnPanel);
        }
    }
    // ------------
    #endregion
}
