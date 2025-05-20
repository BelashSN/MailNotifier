using System;
using System.Drawing;
using System.Windows.Forms;
using System.ComponentModel;
using System.Runtime.InteropServices;

// ==============================================================
namespace MailNotifier
{
	// ==============================================================
	#region ===============    Основная форма     ===================
	// ------------

	static class Program
	{
		/// <summary>
		/// Главная точка входа для приложения.
		/// </summary>
		[STAThread]
		static void Main()
		{
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			Application.Run(new FormMain());
		}
	}
	// ------------
	#endregion


	// ==============================================================
	#region =========  Параметры клика мыши на NotifyIcon  ==========
	// ------------

	public class MouseClicked
	{
		public int Count { get; set; } = 0;
		public MouseButtons Button { get; set; } = MouseButtons.Middle;

		// ======================== Обнуление =====
		public void Reset()
		{
			Count = 0;
			Button = MouseButtons.Middle;
		}
	}
	// ------------
	#endregion


	// ==============================================================
	#region ========  ComboBox с установкой цвета границы  ==========
	// ------------

	public partial class FlatComboBox : ComboBox
	{
		// ================ Конструктор ==============
		#region ==========  Конструктор   ============
		// ------------
		public FlatComboBox() { }
		// ------------
		public FlatComboBox(IContainer container)
		{
			container.Add(this);
			InitializeComponent();
		}
		// ------------      
		private void InitializeComponent() { }
		// ------------ ------------
		private Color borderColor = ColorTranslator.FromHtml("#000038");
		[DefaultValue(typeof(Color), "MidnightBlue")]
		// ------------
		public Color BorderColor
		{
			get { return borderColor; }
			set
			{
				if (borderColor != value)
				{
					borderColor = value;
					Invalidate();
				}
			}
		}
		// ------------ ------------
		private Color buttonColor = Color.DarkGray;
		[DefaultValue(typeof(Color), "Navy")]
		// ------------
		public Color ButtonColor
		{
			get { return buttonColor; }
			set
			{
				if (buttonColor != value)
				{
					buttonColor = value;
					Invalidate();
				}
			}
		}
		// ------------
		#endregion

		// ========== Переопределения WinAPI =========
		#region ====  Переопределения WinAPI   =======
		// ------------
		protected override void WndProc(ref Message m)
		{
			if (m.Msg == WM_PAINT && DropDownStyle != ComboBoxStyle.Simple)
			{
				var clientRect = ClientRectangle;
				var dropDownButtonWidth = SystemInformation.HorizontalScrollBarArrowWidth;
				var outerBorder = new Rectangle(clientRect.Location,
					new Size(clientRect.Width - 1, clientRect.Height - 1));
				var innerBorder = new Rectangle(outerBorder.X + 1, outerBorder.Y + 1,
					outerBorder.Width - dropDownButtonWidth - 2, outerBorder.Height - 2);
				var innerInnerBorder = new Rectangle(innerBorder.X + 1, innerBorder.Y + 1,
					innerBorder.Width - 2, innerBorder.Height - 2);
				var dropDownRect = new Rectangle(innerBorder.Right + 1, innerBorder.Y,
					dropDownButtonWidth, innerBorder.Height + 1);
				// ------------
				if (RightToLeft == RightToLeft.Yes)
				{
					innerBorder.X = clientRect.Width - innerBorder.Right;
					innerInnerBorder.X = clientRect.Width - innerInnerBorder.Right;
					dropDownRect.X = clientRect.Width - dropDownRect.Right;
					dropDownRect.Width += 1;
				}
				// ------------
				var innerBorderColor = Enabled ? BackColor : SystemColors.Control;
				var outerBorderColor = Enabled ? BorderColor : SystemColors.ControlDark;
				var buttonColor = Enabled ? ButtonColor : SystemColors.Control;
				var middle = new Point(dropDownRect.Left + dropDownRect.Width / 2,
					dropDownRect.Top + dropDownRect.Height / 2);
				// ------------
				var arrow = new Point[] {
				new Point(middle.X - 3, middle.Y - 2),
				new Point(middle.X + 4, middle.Y - 2),
				new Point(middle.X, middle.Y + 2) };
				// ------------
				var ps = new PAINTSTRUCT();
				bool shoulEndPaint = false;
				IntPtr dc;
				// ------------
				if (m.WParam == IntPtr.Zero)
				{
					dc = BeginPaint(Handle, ref ps);
					m.WParam = dc;
					shoulEndPaint = true;
				}
				else { dc = m.WParam; }
				// ------------
				var rgn = CreateRectRgn(innerInnerBorder.Left, innerInnerBorder.Top,
					innerInnerBorder.Right, innerInnerBorder.Bottom);
				// ------------
				SelectClipRgn(dc, rgn);
				DefWndProc(ref m);
				DeleteObject(rgn);
				// ------------
				rgn = CreateRectRgn(clientRect.Left, clientRect.Top,
					clientRect.Right, clientRect.Bottom);
				// ------------
				SelectClipRgn(dc, rgn);
				using (var g = Graphics.FromHdc(dc))
				{
					using (var b = new SolidBrush(buttonColor))
					{
						g.FillRectangle(b, dropDownRect);
					}
					// ------------
					using (var b = new SolidBrush(outerBorderColor))
					{
						g.FillPolygon(b, arrow);
					}
					// ------------
					using (var p = new Pen(innerBorderColor))
					{
						g.DrawRectangle(p, innerBorder);
						g.DrawRectangle(p, innerInnerBorder);
					}
					// ------------
					using (var p = new Pen(outerBorderColor))
					{
						g.DrawRectangle(p, outerBorder);
					}
				}
				// ------------
				if (shoulEndPaint) EndPaint(Handle, ref ps);
				DeleteObject(rgn);
			}
			else base.WndProc(ref m);
		}

		// ========= ========= =========
		private const int WM_PAINT = 0xF;
		[StructLayout(LayoutKind.Sequential)]
		
		// ========= ========= =========
		public struct RECT
		{
			public int L, T, R, B;
		}
		
		// ========= ========= =========
		[StructLayout(LayoutKind.Sequential)]
		public struct PAINTSTRUCT
		{
			public IntPtr hdc;
			public bool fErase;
			public int rcPaint_left;
			public int rcPaint_top;
			public int rcPaint_right;
			public int rcPaint_bottom;
			public bool fRestore;
			public bool fIncUpdate;
			public int reserved1;
			public int reserved2;
			public int reserved3;
			public int reserved4;
			public int reserved5;
			public int reserved6;
			public int reserved7;
			public int reserved8;
		}
		
		// ========= ========= =========
		[DllImport("user32.dll")]
		private static extern IntPtr BeginPaint(IntPtr hWnd,
			[In, Out] ref PAINTSTRUCT lpPaint);
		
		// ========= ========= =========
		[DllImport("user32.dll")]
		private static extern bool EndPaint(IntPtr hWnd, ref PAINTSTRUCT lpPaint);
		
		// ========= ========= =========
		[DllImport("gdi32.dll")]
		public static extern int SelectClipRgn(IntPtr hDC, IntPtr hRgn);
		
		// ========= ========= =========
		[DllImport("user32.dll")]
		public static extern int GetUpdateRgn(IntPtr hwnd, IntPtr hrgn, bool fErase);
		public enum RegionFlags
		{
			ERROR = 0,
			NULLREGION = 1,
			SIMPLEREGION = 2,
			COMPLEXREGION = 3,
		}
		
		// ========= ========= =========
		[DllImport("gdi32.dll")]
		internal static extern bool DeleteObject(IntPtr hObject);
		
		// ========= ========= =========
		[DllImport("gdi32.dll")]
		private static extern IntPtr CreateRectRgn(int x1, int y1, int x2, int y2);
		// ------------
		#endregion
	}
    // ------------
    #endregion


	// ==============================================================
	#region ======= Label с цветом, не зависящим от Enabled =========
	// ------------

	public partial class ColorLabel : Label
	{
		// ================ Конструктор ==============
		public ColorLabel() { }
		public ColorLabel(IContainer container)
		{
			container.Add(this);
			InitializeComponent();
		}
		// ------------
		private void InitializeComponent() { }

		// ================ Переопределение цвета ====
		protected override void OnPaint(PaintEventArgs e)
		{
			var textSize = TextRenderer.MeasureText(Text, Font);
			TextRenderer.DrawText(e.Graphics, Text, Font, e.ClipRectangle, ForeColor,
				(TextAlign == ContentAlignment.MiddleLeft) ?
					TextFormatFlags.Left | TextFormatFlags.VerticalCenter :
					(TextAlign == ContentAlignment.MiddleRight) ?
						TextFormatFlags.Right | TextFormatFlags.VerticalCenter :
						TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
		}
	}
	// ------------
	#endregion


	// ==============================================================
	#region =======  Linkabel с подчеркиванием выбранного   =========
	// ------------
	public partial class UnderlinedLabel : LinkLabel
	{
		public bool Underline { get; set; } = false;
		// ------------
		public UnderlinedLabel() { }
		// ------------
		public UnderlinedLabel(IContainer container)
		{
			container.Add(this);
			InitializeComponent();
		}
		// ------------
		private void InitializeComponent() { }
		// ------------
		protected override void OnPaint(PaintEventArgs e)
		{
			base.OnPaint(e);
			if (Underline && this.Text.Length > 0) {
				SizeF textSize = e.Graphics.MeasureString(this.Text, this.Font);
				float gap = 11; // adjust the gap as needed              
				float underlineY = textSize.Height + gap;
				e.Graphics.DrawLine(Pens.White, 4, underlineY, textSize.Width + 2, underlineY); }
		}
	}
	// ------------
	#endregion


	// ==============================================================
	#region ========== Панель встроенных компонентов меню ===========
	// ------------
	public class PanelMenuItem : ToolStripControlHost
	{
		public Panel panel;

		// ================ Конструктор ==============
		public PanelMenuItem(ButtonPanel Container, Size cntSize) : base(new Panel())
		{

			panel = Control as Panel;
			panel.Size = cntSize;
			panel.Left = -10;
			panel.Dock = DockStyle.Fill;
			panel.MinimumSize = cntSize;
			panel.MaximumSize = cntSize;
			panel.Margin = new Padding(0, 0, 0, 0);
			panel.Padding = new Padding(0, 0, 0, 0);
			panel.BackColor = Color.Transparent;
			// ------------
			panel.Controls.Add(Container.BtnPanel);
		}
	}
	// ------------
	#endregion

	
	// ==============================================================
	#region ======== Панель компонентов меню и main-кнопок ==========
	// ------------
	public class ButtonPanel
	{
		// ================ Определения  ==============
		#region ==========  Определения   =============
		// ------------
		public string Name { get; } = "DefaultName";
		// ------------
		public TableLayoutPanel BtnPanel { get; }
		public int Count { get; set; } = 0;
		public bool isMenu { get; set; } = false;
		public bool isCount { get; set; } = true;
		public bool isAlert { get; set; } = false;       
		public string Caption { get; set; } = "Caption";
		public string IconName { get; set; } = "Account48.png";
		public Color IconColor { get; set; } = Color.Transparent;       
		public ImageList ImgList { get; set; } = FormMain.Form.MainImageList;
		// ------------
		#endregion

		// ================ Конструктор ==============
		#region ==========  Конструктор   ============
		// ------------
		public ButtonPanel(WorkAccount mAccount, string cName = "")
		{
			Name = (mAccount != null) ? mAccount.Name : cName;
			Caption = (mAccount != null) ? mAccount.Account.Login : cName;
			// ------------
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
			// ------------ ------------
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
			// ------------
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
			// ------------
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
			// ------------
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
			// ------------ ------------
			BtnPanel.Click += BtnPanelOnClick;
			BtnPanel.MouseHover += BtnPanel_MouseHover;
			BtnPanel.MouseLeave += BtnPanel_MouseLeave;
		}
		// ------------
		#endregion

		// ======= Функкции класса ButtonPanel =======
		#region ==== Функкции класса ButtonPanel =====
		// ---------
		// ========= Стартовая инициализация =========
		public void initialize()
		{
			BtnPanel.Controls["P_Caption"].Text = Caption;          
			BtnPanel.Controls["P_Count"].Text = isCount ? Count.ToString() : "";
			((PictureBox)BtnPanel.Controls["P_Icon"]).BackColor = IconColor;
			((PictureBox)BtnPanel.Controls["P_Icon"]).Image = ImgList.Images[IconName];
			((PictureBox)BtnPanel.Controls["P_Alert"]).Image = isAlert ? ImgList.Images["MyErr.png"] : null;                      
		}

		// ========= aaa ========= /////!!!!!
		private void BtnPanel_MouseLeave(object sender, EventArgs e)
		{
			BtnPanel.Cursor = Cursors.Default;
			// ------------
			string panColor = (FormMain.Form.GetCurrentButtonName() == this.Name) ? "#000038" : "#00001a";
			BtnPanel.BackColor = ColorTranslator.FromHtml(panColor);
		}

		// ========= aaa ========= /////!!!!!
		private void BtnPanel_MouseHover(object sender, EventArgs e)
		{
			BtnPanel.Cursor = Cursors.Hand;
			BtnPanel.BackColor = ColorTranslator.FromHtml("#005");
		}

		// ======= Установка значения Count ==========
		public void SetCount(int NewCount)
		{
			Count = NewCount;
			BtnPanel.Controls["P_Count"].Text = isCount ? Count.ToString() : "";
		}

		// ======= Установка значения Alert ==========
		public void SetAlert(bool NewAlert)
		{
			isAlert = NewAlert;
			((PictureBox)BtnPanel.Controls["P_Alert"]).Image = isAlert ? ImgList.Images["MyErr.png"] : null;
		}

		// ======= Установка значения Alert ==========
		public void SetIcon(string NewIconName)
		{
			IconName = NewIconName;
			((PictureBox)BtnPanel.Controls["P_Icon"]).Image = ImgList.Images[IconName];
		}

		// ========= aaa ========= /////!!!!!
		private void BtnPanelOnClick(object sender, EventArgs e)
		{
			FormMain.Form.BtnPanelClick(this, e);
		}
		// ------------
		#endregion
	}
	// ------------
	#endregion

}
