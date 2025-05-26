using System;
using System.Drawing;
using System.Windows.Forms;
using System.ComponentModel;


// ==============================================================
namespace MailNotifier
{
    // ============================================================== Главная точка входа для приложения.
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

    /*=============== Переопределение элементов формы ==============*/

    // ==============================================================
    #region =========  Параметры клика мыши на NotifyIcon  ==========
    // ------------
    public class MouseClicked
    {
        public int Count { get; set; } = 0;  //  Количество кликов мышью
        public MouseButtons Button { get; set; } = MouseButtons.Middle;  //  Кнопка клика мышью 

        // ====================================  Обнуление сведений о кликах мышью
        public void Reset()
        {
            Count = 0;
            Button = MouseButtons.Middle;
        }
    }
    // ------------
    #endregion


    // ==============================================================
    #region ======= Label с цветом, не зависящим от Enabled =========
    // ------------
    public partial class ColorLabel : Label
    {
        // ==================================== Конструкторы
        public ColorLabel() { }
        // ------------
        public ColorLabel(IContainer container)
        {
            container.Add(this);
            InitializeComponent();
        }
        // ------------
        private void InitializeComponent() { }

        // ==================================== Переопределение цвета OnPaint
        protected override void OnPaint(PaintEventArgs e)
        {
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
        // ==================================== Конструкторы
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

        // ==================================== Переопределение цвета OnPaint
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            if (Underline && this.Text.Length > 0)
            {
                SizeF textSize = e.Graphics.MeasureString(this.Text, this.Font);
                float gap = 11; // adjust the gap as needed              
                float underlineY = textSize.Height + gap;
                e.Graphics.DrawLine(Pens.White, 4, underlineY, textSize.Width + 2, underlineY);
            }
        }
    }
    // ------------
    #endregion
}
