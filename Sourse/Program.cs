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

    /* =============== Переопределение элементов формы ============== */

    // ==============================================================
    #region =========  Параметры клика мыши на NotifyIcon  ==========
    /* ------------ */
    public class MouseClicked
    {
        public int Count { get; set; } = 0;  // - количество кликов мышью
        public MouseButtons Button { get; set; } = MouseButtons.Middle;  // - кнопка клика мышью 

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
    /* ------------ */
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

        // - пустой метод для поддержки конструктора с контейнером
        private void InitializeComponent() { }

        // ==================================== Переопределение цвета OnPaint
        protected override void OnPaint(PaintEventArgs e)
        {
            // - вызываем базовый метод для обеспечения правильного отображения элементов
            TextRenderer.DrawText(e.Graphics, Text, Font, e.ClipRectangle, ForeColor,
                (TextAlign == ContentAlignment.MiddleLeft)

                // - если выравнивание текста слева, то используем флаги для левого выравнивания и вертикального центрирования
                ? TextFormatFlags.Left | TextFormatFlags.VerticalCenter 
                : (TextAlign == ContentAlignment.MiddleRight)

                    // - если выравнивание текста справа, то используем флаги для правого выравнивания и вертикального центрирования
                    ? TextFormatFlags.Right | TextFormatFlags.VerticalCenter 
                    : TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
        }
    }

    // ------------
    #endregion


    // ==============================================================
    #region =======  Linkabel с подчеркиванием выбранного   =========
    /* ------------ */
    public partial class UnderlinedLabel : LinkLabel
    {
        // - свойство для включения или отключения подчеркивания
        public bool Underline { get; set; } = false;

        // ==================================== Конструкторы
        public UnderlinedLabel() { }
        // ------------
        public UnderlinedLabel(IContainer container)
        {
            container.Add(this);
            InitializeComponent();
        }

        // - пустой метод для поддержки конструктора с контейнером
        private void InitializeComponent() { }

        // ==================================== Переопределение цвета OnPaint
        protected override void OnPaint(PaintEventArgs e)
        {
            // - Вызываем базовый метод для обеспечения правильного отображения элементов
            base.OnPaint(e);

            // - Если свойство Underline установлено в true и текст не пустой, рисуем линию под текстом
            if (Underline && this.Text.Length > 0)
            {
                // - Получаем размер текста для определения ширины линии
                SizeF textSize = e.Graphics.MeasureString(this.Text, this.Font);

                // - Расстояние от нижней границы текста до линии по необходимости)
                float gap = 11;

                // - Рисуем линию под текстом, начиная с 4 пикселей от левого края и заканчивая на ширине текста + 2 пикселя
                float underlineY = textSize.Height + gap;
                e.Graphics.DrawLine(Pens.White, 4, underlineY, textSize.Width + 2, underlineY);
            }
        }
    }

    // ------------
    #endregion
}
