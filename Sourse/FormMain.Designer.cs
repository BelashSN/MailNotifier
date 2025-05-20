using System.Windows.Forms;

namespace MailNotifier
{
    partial class FormMain
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        public ImageList GetMainImageList()
        {
            return MainImageList;
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormMain));
            this.NotifyIconMain = new System.Windows.Forms.NotifyIcon(this.components);
            this.MainMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.NotifyIconMenuStripBaseExit = new System.Windows.Forms.ToolStripMenuItem();
            this.PanelFormMainHeader = new System.Windows.Forms.Panel();
            this.ButtonFormHeaderAdmin = new System.Windows.Forms.Button();
            this.ButtonFormHeaderMenu = new System.Windows.Forms.Button();
            this.ButtonFormHeaderTray = new System.Windows.Forms.Button();
            this.PictureFormMainHeader = new System.Windows.Forms.PictureBox();
            this.StatusFormMain = new System.Windows.Forms.StatusStrip();
            this.StatusFormMainMode = new System.Windows.Forms.ToolStripStatusLabel();
            this.StatusFormMainTime = new System.Windows.Forms.ToolStripStatusLabel();
            this.StatusFormMainCount = new System.Windows.Forms.ToolStripStatusLabel();
            this.PanelFormMain = new System.Windows.Forms.Panel();
            this.MainSplitContainer = new System.Windows.Forms.SplitContainer();
            this.ToolsAdminPanel = new System.Windows.Forms.TableLayoutPanel();
            this.LeftLinkAdd = new System.Windows.Forms.LinkLabel();
            this.LeftLinkDel = new System.Windows.Forms.LinkLabel();
            this.ToolsAnonLabel = new System.Windows.Forms.Label();
            this.MainLeftPanel = new System.Windows.Forms.Panel();
            this.EditComboBoxPanel = new System.Windows.Forms.Panel();
            this.PanelSetInfoAccount = new System.Windows.Forms.Panel();
            this.InfoError = new System.Windows.Forms.TextBox();
            this.InfoLastCheck = new System.Windows.Forms.Label();
            this.InfoMessages = new System.Windows.Forms.Label();
            this.InfoAccount = new System.Windows.Forms.Label();
            this.PanelSettingsAccount = new System.Windows.Forms.Panel();
            this.SplitContainerParamAccount = new System.Windows.Forms.SplitContainer();
            this.PanelSettingsProgramm = new System.Windows.Forms.Panel();
            this.SplitContainerParamProgramm = new System.Windows.Forms.SplitContainer();
            this.PanelAutorization = new System.Windows.Forms.Panel();
            this.LoginPassword = new System.Windows.Forms.TextBox();
            this.UnloginLink = new System.Windows.Forms.LinkLabel();
            this.LoginInfoLabel = new System.Windows.Forms.Label();
            this.LoginErrorLabel = new System.Windows.Forms.Label();
            this.LoginLabel = new System.Windows.Forms.Label();
            this.PanelSettingSeparator = new System.Windows.Forms.Panel();
            this.ToolsTabPanel = new System.Windows.Forms.TableLayoutPanel();
            this.MainImageList = new System.Windows.Forms.ImageList(this.components);
            this.MainContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.TimerTrayClick = new System.Windows.Forms.Timer(this.components);
            this.TimerTrayShow = new System.Windows.Forms.Timer(this.components);
            this.TimerTrayMail = new System.Windows.Forms.Timer(this.components);
            this.flatComboBox1 = new MailNotifier.FlatComboBox(this.components);
            this.RightLinkInfo = new MailNotifier.UnderlinedLabel(this.components);
            this.RightLinkProgramm = new MailNotifier.UnderlinedLabel(this.components);
            this.RightLinkAccount = new MailNotifier.UnderlinedLabel(this.components);
            this.CaptionFormMain = new MailNotifier.ColorLabel(this.components);
            this.PanelFormMainHeader.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PictureFormMainHeader)).BeginInit();
            this.StatusFormMain.SuspendLayout();
            this.PanelFormMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.MainSplitContainer)).BeginInit();
            this.MainSplitContainer.Panel1.SuspendLayout();
            this.MainSplitContainer.Panel2.SuspendLayout();
            this.MainSplitContainer.SuspendLayout();
            this.ToolsAdminPanel.SuspendLayout();
            this.EditComboBoxPanel.SuspendLayout();
            this.PanelSetInfoAccount.SuspendLayout();
            this.PanelSettingsAccount.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.SplitContainerParamAccount)).BeginInit();
            this.SplitContainerParamAccount.SuspendLayout();
            this.PanelSettingsProgramm.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.SplitContainerParamProgramm)).BeginInit();
            this.SplitContainerParamProgramm.SuspendLayout();
            this.PanelAutorization.SuspendLayout();
            this.ToolsTabPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // NotifyIconMain
            // 
            this.NotifyIconMain.BalloonTipText = "Проверка почты не производилась...";
            this.NotifyIconMain.BalloonTipTitle = "Mail Notifier";
            this.NotifyIconMain.Icon = ((System.Drawing.Icon)(resources.GetObject("NotifyIconMain.Icon")));
            this.NotifyIconMain.Text = "Mail Notifier";
            this.NotifyIconMain.Visible = true;
            this.NotifyIconMain.MouseDown += new System.Windows.Forms.MouseEventHandler(this.NotifyIconMain_MouseDown);
            // 
            // MainMenuStrip
            // 
            this.MainMenuStrip.AutoSize = false;
            this.MainMenuStrip.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(35)))));
            this.MainMenuStrip.Name = "MainMenuStrip";
            this.MainMenuStrip.ShowImageMargin = false;
            this.MainMenuStrip.ShowItemToolTips = false;
            this.MainMenuStrip.Size = new System.Drawing.Size(265, 26);
            // 
            // NotifyIconMenuStripBaseExit
            // 
            this.NotifyIconMenuStripBaseExit.Name = "NotifyIconMenuStripBaseExit";
            this.NotifyIconMenuStripBaseExit.Size = new System.Drawing.Size(32, 19);
            // 
            // PanelFormMainHeader
            // 
            this.PanelFormMainHeader.BackColor = System.Drawing.Color.Transparent;
            this.PanelFormMainHeader.Controls.Add(this.ButtonFormHeaderAdmin);
            this.PanelFormMainHeader.Controls.Add(this.ButtonFormHeaderMenu);
            this.PanelFormMainHeader.Controls.Add(this.ButtonFormHeaderTray);
            this.PanelFormMainHeader.Controls.Add(this.CaptionFormMain);
            this.PanelFormMainHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.PanelFormMainHeader.Location = new System.Drawing.Point(0, 0);
            this.PanelFormMainHeader.Name = "PanelFormMainHeader";
            this.PanelFormMainHeader.Size = new System.Drawing.Size(818, 36);
            this.PanelFormMainHeader.TabIndex = 1;
            this.PanelFormMainHeader.MouseDown += new System.Windows.Forms.MouseEventHandler(this.PanelFormMainHeader_MouseDown);
            this.PanelFormMainHeader.MouseMove += new System.Windows.Forms.MouseEventHandler(this.PanelFormMainHeader_MouseMove);
            this.PanelFormMainHeader.MouseUp += new System.Windows.Forms.MouseEventHandler(this.PanelFormMainHeader_MouseUp);
            // 
            // ButtonFormHeaderAdmin
            // 
            this.ButtonFormHeaderAdmin.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.ButtonFormHeaderAdmin.Cursor = System.Windows.Forms.Cursors.Hand;
            this.ButtonFormHeaderAdmin.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ButtonFormHeaderAdmin.Image = ((System.Drawing.Image)(resources.GetObject("ButtonFormHeaderAdmin.Image")));
            this.ButtonFormHeaderAdmin.Location = new System.Drawing.Point(702, 2);
            this.ButtonFormHeaderAdmin.Margin = new System.Windows.Forms.Padding(0);
            this.ButtonFormHeaderAdmin.Name = "ButtonFormHeaderAdmin";
            this.ButtonFormHeaderAdmin.Size = new System.Drawing.Size(36, 32);
            this.ButtonFormHeaderAdmin.TabIndex = 4;
            this.ButtonFormHeaderAdmin.UseVisualStyleBackColor = true;
            this.ButtonFormHeaderAdmin.Click += new System.EventHandler(this.ButtonFormHeaderAdmin_Click);
            // 
            // ButtonFormHeaderMenu
            // 
            this.ButtonFormHeaderMenu.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.ButtonFormHeaderMenu.Cursor = System.Windows.Forms.Cursors.Hand;
            this.ButtonFormHeaderMenu.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ButtonFormHeaderMenu.Image = ((System.Drawing.Image)(resources.GetObject("ButtonFormHeaderMenu.Image")));
            this.ButtonFormHeaderMenu.Location = new System.Drawing.Point(741, 2);
            this.ButtonFormHeaderMenu.Margin = new System.Windows.Forms.Padding(0);
            this.ButtonFormHeaderMenu.Name = "ButtonFormHeaderMenu";
            this.ButtonFormHeaderMenu.Size = new System.Drawing.Size(36, 32);
            this.ButtonFormHeaderMenu.TabIndex = 1;
            this.ButtonFormHeaderMenu.UseVisualStyleBackColor = true;
            this.ButtonFormHeaderMenu.Click += new System.EventHandler(this.ButtonFormHeaderMenu_Click);
            // 
            // ButtonFormHeaderTray
            // 
            this.ButtonFormHeaderTray.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.ButtonFormHeaderTray.Cursor = System.Windows.Forms.Cursors.Hand;
            this.ButtonFormHeaderTray.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ButtonFormHeaderTray.Image = ((System.Drawing.Image)(resources.GetObject("ButtonFormHeaderTray.Image")));
            this.ButtonFormHeaderTray.Location = new System.Drawing.Point(780, 2);
            this.ButtonFormHeaderTray.Margin = new System.Windows.Forms.Padding(0);
            this.ButtonFormHeaderTray.Name = "ButtonFormHeaderTray";
            this.ButtonFormHeaderTray.Size = new System.Drawing.Size(36, 32);
            this.ButtonFormHeaderTray.TabIndex = 0;
            this.ButtonFormHeaderTray.UseVisualStyleBackColor = true;
            this.ButtonFormHeaderTray.Click += new System.EventHandler(this.ButtonFormHeaderToTray_Click);
            this.ButtonFormHeaderTray.MouseLeave += new System.EventHandler(this.ButtonFormHeaderTray_MouseLeave);
            this.ButtonFormHeaderTray.MouseHover += new System.EventHandler(this.ButtonFormHeaderTray_MouseHover);
            // 
            // PictureFormMainHeader
            // 
            this.PictureFormMainHeader.BackColor = System.Drawing.Color.Transparent;
            this.PictureFormMainHeader.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("PictureFormMainHeader.BackgroundImage")));
            this.PictureFormMainHeader.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.PictureFormMainHeader.Enabled = false;
            this.PictureFormMainHeader.Location = new System.Drawing.Point(10, 12);
            this.PictureFormMainHeader.Name = "PictureFormMainHeader";
            this.PictureFormMainHeader.Size = new System.Drawing.Size(38, 38);
            this.PictureFormMainHeader.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.PictureFormMainHeader.TabIndex = 2;
            this.PictureFormMainHeader.TabStop = false;
            // 
            // StatusFormMain
            // 
            this.StatusFormMain.AutoSize = false;
            this.StatusFormMain.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(32)))));
            this.StatusFormMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.StatusFormMainMode,
            this.StatusFormMainTime,
            this.StatusFormMainCount});
            this.StatusFormMain.Location = new System.Drawing.Point(0, 418);
            this.StatusFormMain.Name = "StatusFormMain";
            this.StatusFormMain.Size = new System.Drawing.Size(818, 30);
            this.StatusFormMain.TabIndex = 3;
            this.StatusFormMain.Text = "statusStrip1";
            // 
            // StatusFormMainMode
            // 
            this.StatusFormMainMode.Font = new System.Drawing.Font("Segoe UI", 8.5F);
            this.StatusFormMainMode.ForeColor = System.Drawing.Color.Gainsboro;
            this.StatusFormMainMode.Margin = new System.Windows.Forms.Padding(0, 3, 12, 2);
            this.StatusFormMainMode.Name = "StatusFormMainMode";
            this.StatusFormMainMode.Size = new System.Drawing.Size(117, 25);
            this.StatusFormMainMode.Text = "Режим: анонимный";
            // 
            // StatusFormMainTime
            // 
            this.StatusFormMainTime.Font = new System.Drawing.Font("Segoe UI", 8.5F);
            this.StatusFormMainTime.ForeColor = System.Drawing.Color.Gainsboro;
            this.StatusFormMainTime.Margin = new System.Windows.Forms.Padding(0, 3, 12, 2);
            this.StatusFormMainTime.Name = "StatusFormMainTime";
            this.StatusFormMainTime.Size = new System.Drawing.Size(211, 25);
            this.StatusFormMainTime.Text = "Проверка почты не производилась...";
            // 
            // StatusFormMainCount
            // 
            this.StatusFormMainCount.Font = new System.Drawing.Font("Segoe UI", 8.5F);
            this.StatusFormMainCount.ForeColor = System.Drawing.Color.Gainsboro;
            this.StatusFormMainCount.Margin = new System.Windows.Forms.Padding(0, 3, 12, 2);
            this.StatusFormMainCount.Name = "StatusFormMainCount";
            this.StatusFormMainCount.Size = new System.Drawing.Size(178, 25);
            this.StatusFormMainCount.Text = "Всего непрочитанных писем: 0";
            // 
            // PanelFormMain
            // 
            this.PanelFormMain.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.PanelFormMain.BackColor = System.Drawing.Color.MidnightBlue;
            this.PanelFormMain.Controls.Add(this.PictureFormMainHeader);
            this.PanelFormMain.Controls.Add(this.MainSplitContainer);
            this.PanelFormMain.Controls.Add(this.StatusFormMain);
            this.PanelFormMain.Controls.Add(this.PanelFormMainHeader);
            this.PanelFormMain.ForeColor = System.Drawing.Color.Gainsboro;
            this.PanelFormMain.Location = new System.Drawing.Point(1, 1);
            this.PanelFormMain.Name = "PanelFormMain";
            this.PanelFormMain.Size = new System.Drawing.Size(818, 448);
            this.PanelFormMain.TabIndex = 0;
            this.PanelFormMain.Paint += new System.Windows.Forms.PaintEventHandler(this.PanelFormMain_Paint);
            // 
            // MainSplitContainer
            // 
            this.MainSplitContainer.BackColor = System.Drawing.Color.Transparent;
            this.MainSplitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.MainSplitContainer.ForeColor = System.Drawing.Color.Gainsboro;
            this.MainSplitContainer.Location = new System.Drawing.Point(0, 36);
            this.MainSplitContainer.Name = "MainSplitContainer";
            // 
            // MainSplitContainer.Panel1
            // 
            this.MainSplitContainer.Panel1.BackColor = System.Drawing.Color.Transparent;
            this.MainSplitContainer.Panel1.Controls.Add(this.ToolsAdminPanel);
            this.MainSplitContainer.Panel1.Controls.Add(this.MainLeftPanel);
            this.MainSplitContainer.Panel1MinSize = 260;
            // 
            // MainSplitContainer.Panel2
            // 
            this.MainSplitContainer.Panel2.BackColor = System.Drawing.Color.Transparent;
            this.MainSplitContainer.Panel2.Controls.Add(this.EditComboBoxPanel);
            this.MainSplitContainer.Panel2.Controls.Add(this.PanelSetInfoAccount);
            this.MainSplitContainer.Panel2.Controls.Add(this.PanelSettingsAccount);
            this.MainSplitContainer.Panel2.Controls.Add(this.PanelSettingsProgramm);
            this.MainSplitContainer.Panel2.Controls.Add(this.PanelAutorization);
            this.MainSplitContainer.Panel2.Controls.Add(this.PanelSettingSeparator);
            this.MainSplitContainer.Panel2.Controls.Add(this.ToolsTabPanel);
            this.MainSplitContainer.Panel2MinSize = 400;
            this.MainSplitContainer.Size = new System.Drawing.Size(818, 382);
            this.MainSplitContainer.SplitterDistance = 280;
            this.MainSplitContainer.SplitterWidth = 3;
            this.MainSplitContainer.TabIndex = 6;
            // 
            // ToolsAdminPanel
            // 
            this.ToolsAdminPanel.BackColor = System.Drawing.Color.Transparent;
            this.ToolsAdminPanel.ColumnCount = 4;
            this.ToolsAdminPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 60F));
            this.ToolsAdminPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.ToolsAdminPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.ToolsAdminPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.ToolsAdminPanel.Controls.Add(this.LeftLinkAdd, 1, 0);
            this.ToolsAdminPanel.Controls.Add(this.LeftLinkDel, 2, 0);
            this.ToolsAdminPanel.Controls.Add(this.ToolsAnonLabel, 3, 0);
            this.ToolsAdminPanel.Location = new System.Drawing.Point(0, 0);
            this.ToolsAdminPanel.Margin = new System.Windows.Forms.Padding(0);
            this.ToolsAdminPanel.Name = "ToolsAdminPanel";
            this.ToolsAdminPanel.RowCount = 1;
            this.ToolsAdminPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.ToolsAdminPanel.Size = new System.Drawing.Size(298, 28);
            this.ToolsAdminPanel.TabIndex = 2;
            // 
            // LeftLinkAdd
            // 
            this.LeftLinkAdd.ActiveLinkColor = System.Drawing.Color.WhiteSmoke;
            this.LeftLinkAdd.AutoSize = true;
            this.LeftLinkAdd.Dock = System.Windows.Forms.DockStyle.Fill;
            this.LeftLinkAdd.LinkBehavior = System.Windows.Forms.LinkBehavior.NeverUnderline;
            this.LeftLinkAdd.LinkColor = System.Drawing.Color.LightBlue;
            this.LeftLinkAdd.Location = new System.Drawing.Point(63, 0);
            this.LeftLinkAdd.Name = "LeftLinkAdd";
            this.LeftLinkAdd.Padding = new System.Windows.Forms.Padding(4, 0, 0, 0);
            this.LeftLinkAdd.Size = new System.Drawing.Size(74, 28);
            this.LeftLinkAdd.TabIndex = 0;
            this.LeftLinkAdd.TabStop = true;
            this.LeftLinkAdd.Text = "Добавить";
            this.LeftLinkAdd.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.LeftLinkAdd.UseMnemonic = false;
            this.LeftLinkAdd.MouseLeave += new System.EventHandler(this.Link_MouseLeave);
            this.LeftLinkAdd.MouseHover += new System.EventHandler(this.Link_MouseHover);
            // 
            // LeftLinkDel
            // 
            this.LeftLinkDel.ActiveLinkColor = System.Drawing.Color.WhiteSmoke;
            this.LeftLinkDel.AutoSize = true;
            this.LeftLinkDel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.LeftLinkDel.LinkBehavior = System.Windows.Forms.LinkBehavior.NeverUnderline;
            this.LeftLinkDel.LinkColor = System.Drawing.Color.LightBlue;
            this.LeftLinkDel.Location = new System.Drawing.Point(143, 0);
            this.LeftLinkDel.Name = "LeftLinkDel";
            this.LeftLinkDel.Padding = new System.Windows.Forms.Padding(4, 0, 0, 0);
            this.LeftLinkDel.Size = new System.Drawing.Size(66, 28);
            this.LeftLinkDel.TabIndex = 1;
            this.LeftLinkDel.TabStop = true;
            this.LeftLinkDel.Text = "Удалить";
            this.LeftLinkDel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.LeftLinkDel.MouseLeave += new System.EventHandler(this.Link_MouseLeave);
            this.LeftLinkDel.MouseHover += new System.EventHandler(this.Link_MouseHover);
            // 
            // ToolsAnonLabel
            // 
            this.ToolsAnonLabel.AutoSize = true;
            this.ToolsAnonLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ToolsAnonLabel.ForeColor = System.Drawing.Color.Gainsboro;
            this.ToolsAnonLabel.Location = new System.Drawing.Point(215, 0);
            this.ToolsAnonLabel.Name = "ToolsAnonLabel";
            this.ToolsAnonLabel.Padding = new System.Windows.Forms.Padding(4, 0, 0, 0);
            this.ToolsAnonLabel.Size = new System.Drawing.Size(80, 28);
            this.ToolsAnonLabel.TabIndex = 0;
            this.ToolsAnonLabel.Text = "Всего: 0";
            this.ToolsAnonLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // MainLeftPanel
            // 
            this.MainLeftPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.MainLeftPanel.AutoScroll = true;
            this.MainLeftPanel.AutoScrollMargin = new System.Drawing.Size(20, 0);
            this.MainLeftPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(22)))));
            this.MainLeftPanel.Location = new System.Drawing.Point(3, 37);
            this.MainLeftPanel.Margin = new System.Windows.Forms.Padding(0);
            this.MainLeftPanel.Name = "MainLeftPanel";
            this.MainLeftPanel.Size = new System.Drawing.Size(270, 337);
            this.MainLeftPanel.TabIndex = 4;
            // 
            // EditComboBoxPanel
            // 
            this.EditComboBoxPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.EditComboBoxPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(56)))));
            this.EditComboBoxPanel.Controls.Add(this.flatComboBox1);
            this.EditComboBoxPanel.Location = new System.Drawing.Point(22, 328);
            this.EditComboBoxPanel.Name = "EditComboBoxPanel";
            this.EditComboBoxPanel.Padding = new System.Windows.Forms.Padding(0, 3, 0, 0);
            this.EditComboBoxPanel.Size = new System.Drawing.Size(355, 30);
            this.EditComboBoxPanel.TabIndex = 0;
            // 
            // PanelSetInfoAccount
            // 
            this.PanelSetInfoAccount.BackColor = System.Drawing.Color.Transparent;
            this.PanelSetInfoAccount.Controls.Add(this.InfoError);
            this.PanelSetInfoAccount.Controls.Add(this.InfoLastCheck);
            this.PanelSetInfoAccount.Controls.Add(this.InfoMessages);
            this.PanelSetInfoAccount.Controls.Add(this.InfoAccount);
            this.PanelSetInfoAccount.Dock = System.Windows.Forms.DockStyle.Fill;
            this.PanelSetInfoAccount.Location = new System.Drawing.Point(0, 39);
            this.PanelSetInfoAccount.Margin = new System.Windows.Forms.Padding(3, 3, 13, 3);
            this.PanelSetInfoAccount.Name = "PanelSetInfoAccount";
            this.PanelSetInfoAccount.Size = new System.Drawing.Size(535, 343);
            this.PanelSetInfoAccount.TabIndex = 7;
            // 
            // InfoError
            // 
            this.InfoError.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.InfoError.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(34)))));
            this.InfoError.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.InfoError.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.InfoError.Location = new System.Drawing.Point(6, 132);
            this.InfoError.Multiline = true;
            this.InfoError.Name = "InfoError";
            this.InfoError.ReadOnly = true;
            this.InfoError.Size = new System.Drawing.Size(518, 204);
            this.InfoError.TabIndex = 6;
            // 
            // InfoLastCheck
            // 
            this.InfoLastCheck.AutoSize = true;
            this.InfoLastCheck.Location = new System.Drawing.Point(6, 93);
            this.InfoLastCheck.Name = "InfoLastCheck";
            this.InfoLastCheck.Size = new System.Drawing.Size(312, 16);
            this.InfoLastCheck.TabIndex = 5;
            this.InfoLastCheck.Text = "Проверка почты аккаунта не производилась...";
            // 
            // InfoMessages
            // 
            this.InfoMessages.AutoSize = true;
            this.InfoMessages.Location = new System.Drawing.Point(6, 52);
            this.InfoMessages.Name = "InfoMessages";
            this.InfoMessages.Size = new System.Drawing.Size(206, 16);
            this.InfoMessages.TabIndex = 4;
            this.InfoMessages.Text = "Всего непрочитанных писем: 0";
            // 
            // InfoAccount
            // 
            this.InfoAccount.AutoSize = true;
            this.InfoAccount.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.InfoAccount.Location = new System.Drawing.Point(6, 10);
            this.InfoAccount.Name = "InfoAccount";
            this.InfoAccount.Size = new System.Drawing.Size(244, 18);
            this.InfoAccount.TabIndex = 3;
            this.InfoAccount.Text = "Аккаунт: test_account@test-mail.ru";
            // 
            // PanelSettingsAccount
            // 
            this.PanelSettingsAccount.BackColor = System.Drawing.Color.Transparent;
            this.PanelSettingsAccount.Controls.Add(this.SplitContainerParamAccount);
            this.PanelSettingsAccount.Dock = System.Windows.Forms.DockStyle.Fill;
            this.PanelSettingsAccount.Location = new System.Drawing.Point(0, 39);
            this.PanelSettingsAccount.Margin = new System.Windows.Forms.Padding(3, 3, 13, 3);
            this.PanelSettingsAccount.Name = "PanelSettingsAccount";
            this.PanelSettingsAccount.Padding = new System.Windows.Forms.Padding(4, 45, 4, 11);
            this.PanelSettingsAccount.Size = new System.Drawing.Size(535, 343);
            this.PanelSettingsAccount.TabIndex = 9;
            // 
            // SplitContainerParamAccount
            // 
            this.SplitContainerParamAccount.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SplitContainerParamAccount.Location = new System.Drawing.Point(4, 45);
            this.SplitContainerParamAccount.Name = "SplitContainerParamAccount";
            this.SplitContainerParamAccount.Panel1MinSize = 30;
            this.SplitContainerParamAccount.Panel2MinSize = 70;
            this.SplitContainerParamAccount.Size = new System.Drawing.Size(527, 287);
            this.SplitContainerParamAccount.SplitterDistance = 68;
            this.SplitContainerParamAccount.TabIndex = 0;
            // 
            // PanelSettingsProgramm
            // 
            this.PanelSettingsProgramm.BackColor = System.Drawing.Color.Transparent;
            this.PanelSettingsProgramm.Controls.Add(this.SplitContainerParamProgramm);
            this.PanelSettingsProgramm.Dock = System.Windows.Forms.DockStyle.Fill;
            this.PanelSettingsProgramm.Location = new System.Drawing.Point(0, 39);
            this.PanelSettingsProgramm.Margin = new System.Windows.Forms.Padding(3, 3, 13, 3);
            this.PanelSettingsProgramm.Name = "PanelSettingsProgramm";
            this.PanelSettingsProgramm.Padding = new System.Windows.Forms.Padding(4, 45, 4, 11);
            this.PanelSettingsProgramm.Size = new System.Drawing.Size(535, 343);
            this.PanelSettingsProgramm.TabIndex = 8;
            // 
            // SplitContainerParamProgramm
            // 
            this.SplitContainerParamProgramm.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SplitContainerParamProgramm.Location = new System.Drawing.Point(4, 45);
            this.SplitContainerParamProgramm.Name = "SplitContainerParamProgramm";
            this.SplitContainerParamProgramm.Panel1MinSize = 40;
            this.SplitContainerParamProgramm.Panel2MinSize = 80;
            this.SplitContainerParamProgramm.Size = new System.Drawing.Size(527, 287);
            this.SplitContainerParamProgramm.SplitterDistance = 169;
            this.SplitContainerParamProgramm.TabIndex = 0;
            // 
            // PanelAutorization
            // 
            this.PanelAutorization.BackColor = System.Drawing.Color.Transparent;
            this.PanelAutorization.Controls.Add(this.LoginPassword);
            this.PanelAutorization.Controls.Add(this.UnloginLink);
            this.PanelAutorization.Controls.Add(this.LoginInfoLabel);
            this.PanelAutorization.Controls.Add(this.LoginErrorLabel);
            this.PanelAutorization.Controls.Add(this.LoginLabel);
            this.PanelAutorization.Dock = System.Windows.Forms.DockStyle.Fill;
            this.PanelAutorization.Location = new System.Drawing.Point(0, 39);
            this.PanelAutorization.Name = "PanelAutorization";
            this.PanelAutorization.Size = new System.Drawing.Size(535, 343);
            this.PanelAutorization.TabIndex = 10;
            // 
            // LoginPassword
            // 
            this.LoginPassword.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.LoginPassword.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(34)))));
            this.LoginPassword.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.LoginPassword.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.LoginPassword.ForeColor = System.Drawing.Color.Gainsboro;
            this.LoginPassword.Location = new System.Drawing.Point(126, 108);
            this.LoginPassword.Margin = new System.Windows.Forms.Padding(3, 14, 3, 3);
            this.LoginPassword.Name = "LoginPassword";
            this.LoginPassword.PasswordChar = '*';
            this.LoginPassword.Size = new System.Drawing.Size(276, 21);
            this.LoginPassword.TabIndex = 2;
            this.LoginPassword.UseSystemPasswordChar = true;
            this.LoginPassword.WordWrap = false;
            this.LoginPassword.KeyDown += new System.Windows.Forms.KeyEventHandler(this.LoginPassword_KeyDown);
            // 
            // UnloginLink
            // 
            this.UnloginLink.ActiveLinkColor = System.Drawing.Color.WhiteSmoke;
            this.UnloginLink.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.UnloginLink.LinkBehavior = System.Windows.Forms.LinkBehavior.NeverUnderline;
            this.UnloginLink.LinkColor = System.Drawing.Color.LightBlue;
            this.UnloginLink.Location = new System.Drawing.Point(97, 69);
            this.UnloginLink.Name = "UnloginLink";
            this.UnloginLink.Padding = new System.Windows.Forms.Padding(0, 0, 4, 0);
            this.UnloginLink.Size = new System.Drawing.Size(348, 20);
            this.UnloginLink.TabIndex = 1;
            this.UnloginLink.TabStop = true;
            this.UnloginLink.Text = "Выйти из режима администрирования";
            this.UnloginLink.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.UnloginLink.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.UnloginLink_LinkClicked);
            this.UnloginLink.MouseLeave += new System.EventHandler(this.Link_MouseLeave);
            this.UnloginLink.MouseHover += new System.EventHandler(this.Link_MouseHover);
            // 
            // LoginInfoLabel
            // 
            this.LoginInfoLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.LoginInfoLabel.Location = new System.Drawing.Point(6, 59);
            this.LoginInfoLabel.Name = "LoginInfoLabel";
            this.LoginInfoLabel.Size = new System.Drawing.Size(518, 39);
            this.LoginInfoLabel.TabIndex = 0;
            this.LoginInfoLabel.Text = "Для изменения настроек необходимо ввести пароль:";
            this.LoginInfoLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // LoginErrorLabel
            // 
            this.LoginErrorLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.LoginErrorLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.LoginErrorLabel.Location = new System.Drawing.Point(9, 141);
            this.LoginErrorLabel.Name = "LoginErrorLabel";
            this.LoginErrorLabel.Size = new System.Drawing.Size(518, 27);
            this.LoginErrorLabel.TabIndex = 4;
            this.LoginErrorLabel.Text = "Прароль администратора не корректен...";
            this.LoginErrorLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // LoginLabel
            // 
            this.LoginLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.LoginLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.LoginLabel.Location = new System.Drawing.Point(5, 34);
            this.LoginLabel.Name = "LoginLabel";
            this.LoginLabel.Size = new System.Drawing.Size(522, 18);
            this.LoginLabel.TabIndex = 3;
            this.LoginLabel.Text = "Авторизация";
            this.LoginLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // PanelSettingSeparator
            // 
            this.PanelSettingSeparator.BackColor = System.Drawing.Color.Transparent;
            this.PanelSettingSeparator.Dock = System.Windows.Forms.DockStyle.Top;
            this.PanelSettingSeparator.Location = new System.Drawing.Point(0, 28);
            this.PanelSettingSeparator.Name = "PanelSettingSeparator";
            this.PanelSettingSeparator.Size = new System.Drawing.Size(535, 11);
            this.PanelSettingSeparator.TabIndex = 6;
            // 
            // ToolsTabPanel
            // 
            this.ToolsTabPanel.BackColor = System.Drawing.Color.Transparent;
            this.ToolsTabPanel.ColumnCount = 3;
            this.ToolsTabPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.ToolsTabPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.ToolsTabPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.ToolsTabPanel.Controls.Add(this.RightLinkInfo, 0, 0);
            this.ToolsTabPanel.Controls.Add(this.RightLinkProgramm, 1, 0);
            this.ToolsTabPanel.Controls.Add(this.RightLinkAccount, 2, 0);
            this.ToolsTabPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.ToolsTabPanel.Location = new System.Drawing.Point(0, 0);
            this.ToolsTabPanel.Margin = new System.Windows.Forms.Padding(0);
            this.ToolsTabPanel.Name = "ToolsTabPanel";
            this.ToolsTabPanel.RowCount = 1;
            this.ToolsTabPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.ToolsTabPanel.Size = new System.Drawing.Size(535, 28);
            this.ToolsTabPanel.TabIndex = 5;
            // 
            // MainImageList
            // 
            this.MainImageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("MainImageList.ImageStream")));
            this.MainImageList.TransparentColor = System.Drawing.Color.Transparent;
            this.MainImageList.Images.SetKeyName(0, "MailNotifier48.png");
            this.MainImageList.Images.SetKeyName(1, "Account48.png");
            this.MainImageList.Images.SetKeyName(2, "MunuExit48.png");
            this.MainImageList.Images.SetKeyName(3, "MenuFromTray48.png");
            this.MainImageList.Images.SetKeyName(4, "MenuToTray48.png");
            this.MainImageList.Images.SetKeyName(5, "TrayUpdate48.png");
            this.MainImageList.Images.SetKeyName(6, "TrayClosed48.png");
            this.MainImageList.Images.SetKeyName(7, "TrayOpen48.png");
            this.MainImageList.Images.SetKeyName(8, "MyQst.png");
            this.MainImageList.Images.SetKeyName(9, "MyInf.png");
            this.MainImageList.Images.SetKeyName(10, "MyErr.png");
            // 
            // MainContextMenu
            // 
            this.MainContextMenu.Name = "contextMenuStrip1";
            this.MainContextMenu.Size = new System.Drawing.Size(61, 4);
            // 
            // TimerTrayClick
            // 
            this.TimerTrayClick.Interval = 250;
            this.TimerTrayClick.Tick += new System.EventHandler(this.TimerTrayClick_Tick);
            // 
            // TimerTrayShow
            // 
            this.TimerTrayShow.Interval = 600;
            this.TimerTrayShow.Tag = "TrayClosed48.png";
            this.TimerTrayShow.Tick += new System.EventHandler(this.TimerTrayShow_Tick);
            // 
            // TimerTrayMail
            // 
            this.TimerTrayMail.Interval = 300000;
            this.TimerTrayMail.Tick += new System.EventHandler(this.TimerTrayMail_Tick);
            // 
            // flatComboBox1
            // 
            this.flatComboBox1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(56)))));
            this.flatComboBox1.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(56)))));
            this.flatComboBox1.ButtonColor = System.Drawing.Color.DarkGray;
            this.flatComboBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flatComboBox1.ForeColor = System.Drawing.Color.Gainsboro;
            this.flatComboBox1.FormattingEnabled = true;
            this.flatComboBox1.Location = new System.Drawing.Point(0, 3);
            this.flatComboBox1.Name = "flatComboBox1";
            this.flatComboBox1.Size = new System.Drawing.Size(355, 24);
            this.flatComboBox1.TabIndex = 0;
            // 
            // RightLinkInfo
            // 
            this.RightLinkInfo.ActiveLinkColor = System.Drawing.Color.WhiteSmoke;
            this.RightLinkInfo.AutoSize = true;
            this.RightLinkInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.RightLinkInfo.LinkBehavior = System.Windows.Forms.LinkBehavior.NeverUnderline;
            this.RightLinkInfo.LinkColor = System.Drawing.Color.LightBlue;
            this.RightLinkInfo.Location = new System.Drawing.Point(3, 0);
            this.RightLinkInfo.Name = "RightLinkInfo";
            this.RightLinkInfo.Size = new System.Drawing.Size(61, 28);
            this.RightLinkInfo.TabIndex = 2;
            this.RightLinkInfo.TabStop = true;
            this.RightLinkInfo.Text = "Аккаунт";
            this.RightLinkInfo.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.RightLinkInfo.Underline = false;
            this.RightLinkInfo.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.RightLink_LinkClicked);
            this.RightLinkInfo.MouseLeave += new System.EventHandler(this.RightLink_MouseLeave);
            this.RightLinkInfo.MouseHover += new System.EventHandler(this.RightLink_MouseHover);
            // 
            // RightLinkProgramm
            // 
            this.RightLinkProgramm.ActiveLinkColor = System.Drawing.Color.WhiteSmoke;
            this.RightLinkProgramm.AutoSize = true;
            this.RightLinkProgramm.Dock = System.Windows.Forms.DockStyle.Fill;
            this.RightLinkProgramm.LinkBehavior = System.Windows.Forms.LinkBehavior.NeverUnderline;
            this.RightLinkProgramm.LinkColor = System.Drawing.Color.LightBlue;
            this.RightLinkProgramm.Location = new System.Drawing.Point(70, 0);
            this.RightLinkProgramm.Name = "RightLinkProgramm";
            this.RightLinkProgramm.Size = new System.Drawing.Size(154, 28);
            this.RightLinkProgramm.TabIndex = 3;
            this.RightLinkProgramm.TabStop = true;
            this.RightLinkProgramm.Text = "Настройки программы";
            this.RightLinkProgramm.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.RightLinkProgramm.Underline = true;
            this.RightLinkProgramm.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.RightLink_LinkClicked);
            this.RightLinkProgramm.MouseLeave += new System.EventHandler(this.RightLink_MouseLeave);
            this.RightLinkProgramm.MouseHover += new System.EventHandler(this.RightLink_MouseHover);
            // 
            // RightLinkAccount
            // 
            this.RightLinkAccount.ActiveLinkColor = System.Drawing.Color.WhiteSmoke;
            this.RightLinkAccount.AutoSize = true;
            this.RightLinkAccount.Dock = System.Windows.Forms.DockStyle.Fill;
            this.RightLinkAccount.LinkBehavior = System.Windows.Forms.LinkBehavior.NeverUnderline;
            this.RightLinkAccount.LinkColor = System.Drawing.Color.LightBlue;
            this.RightLinkAccount.Location = new System.Drawing.Point(230, 0);
            this.RightLinkAccount.Name = "RightLinkAccount";
            this.RightLinkAccount.Size = new System.Drawing.Size(302, 28);
            this.RightLinkAccount.TabIndex = 4;
            this.RightLinkAccount.TabStop = true;
            this.RightLinkAccount.Text = "Настройки аккаунта";
            this.RightLinkAccount.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.RightLinkAccount.Underline = true;
            this.RightLinkAccount.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.RightLink_LinkClicked);
            this.RightLinkAccount.MouseLeave += new System.EventHandler(this.RightLink_MouseLeave);
            this.RightLinkAccount.MouseHover += new System.EventHandler(this.RightLink_MouseHover);
            // 
            // CaptionFormMain
            // 
            this.CaptionFormMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.CaptionFormMain.Enabled = false;
            this.CaptionFormMain.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.CaptionFormMain.ForeColor = System.Drawing.Color.Gainsboro;
            this.CaptionFormMain.Location = new System.Drawing.Point(0, 0);
            this.CaptionFormMain.Name = "CaptionFormMain";
            this.CaptionFormMain.Size = new System.Drawing.Size(818, 36);
            this.CaptionFormMain.TabIndex = 3;
            this.CaptionFormMain.Text = "Mail Notifier";
            this.CaptionFormMain.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.DimGray;
            this.ClientSize = new System.Drawing.Size(820, 450);
            this.Controls.Add(this.PanelFormMain);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.ForeColor = System.Drawing.Color.Gainsboro;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormMain";
            this.Text = "Mail Notifier";
            this.Shown += new System.EventHandler(this.FormMain_Shown);
            this.Resize += new System.EventHandler(this.FormMain_Resize);
            this.PanelFormMainHeader.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.PictureFormMainHeader)).EndInit();
            this.StatusFormMain.ResumeLayout(false);
            this.StatusFormMain.PerformLayout();
            this.PanelFormMain.ResumeLayout(false);
            this.MainSplitContainer.Panel1.ResumeLayout(false);
            this.MainSplitContainer.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.MainSplitContainer)).EndInit();
            this.MainSplitContainer.ResumeLayout(false);
            this.ToolsAdminPanel.ResumeLayout(false);
            this.ToolsAdminPanel.PerformLayout();
            this.EditComboBoxPanel.ResumeLayout(false);
            this.PanelSetInfoAccount.ResumeLayout(false);
            this.PanelSetInfoAccount.PerformLayout();
            this.PanelSettingsAccount.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.SplitContainerParamAccount)).EndInit();
            this.SplitContainerParamAccount.ResumeLayout(false);
            this.PanelSettingsProgramm.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.SplitContainerParamProgramm)).EndInit();
            this.SplitContainerParamProgramm.ResumeLayout(false);
            this.PanelAutorization.ResumeLayout(false);
            this.PanelAutorization.PerformLayout();
            this.ToolsTabPanel.ResumeLayout(false);
            this.ToolsTabPanel.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.NotifyIcon NotifyIconMain;
        private System.Windows.Forms.Panel PanelFormMainHeader;
        private System.Windows.Forms.Button ButtonFormHeaderMenu;
        private System.Windows.Forms.Button ButtonFormHeaderTray;
        private System.Windows.Forms.PictureBox PictureFormMainHeader;
        private System.Windows.Forms.StatusStrip StatusFormMain;
        private System.Windows.Forms.ToolStripStatusLabel StatusFormMainMode;
        private System.Windows.Forms.ToolStripStatusLabel StatusFormMainTime;
        private System.Windows.Forms.ToolStripStatusLabel StatusFormMainCount;
        private System.Windows.Forms.Panel PanelFormMain;
        private System.Windows.Forms.ToolStripMenuItem NotifyIconMenuStripBaseExit;
        public System.Windows.Forms.ImageList MainImageList;
        private ContextMenuStrip MainMenuStrip;
        private TableLayoutPanel ToolsTabPanel;
        private SplitContainer MainSplitContainer;
        private Label ToolsAnonLabel;
        private TableLayoutPanel ToolsAdminPanel;
        private LinkLabel LeftLinkAdd;
        private LinkLabel LeftLinkDel;
        private Panel MainLeftPanel;
        private ContextMenuStrip MainContextMenu;
        private ColorLabel CaptionFormMain;
        private Timer TimerTrayClick;
        private Timer TimerTrayShow;
        private Timer TimerTrayMail;
        private UnderlinedLabel RightLinkInfo;
        private UnderlinedLabel RightLinkProgramm;
        private UnderlinedLabel RightLinkAccount;
        private Panel PanelSettingSeparator;
        private Panel PanelSetInfoAccount;
        private Panel PanelSettingsProgramm;
        private Panel PanelSettingsAccount;
        private Label LoginInfoLabel;
        private LinkLabel UnloginLink;
        private TextBox LoginPassword;
        private Label InfoLastCheck;
        private Label InfoMessages;
        private Label InfoAccount;
        private TextBox InfoError;
        private Button ButtonFormHeaderAdmin;
        private Panel PanelAutorization;
        private Label LoginLabel;
        private Label LoginErrorLabel;
        private SplitContainer SplitContainerParamProgramm;
        private Panel EditComboBoxPanel;
        private FlatComboBox flatComboBox1;
        private SplitContainer SplitContainerParamAccount;
    }
}

