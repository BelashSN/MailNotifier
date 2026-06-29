using System;
using System.Drawing;
using System.Reflection.Emit;
using System.Windows.Forms;

// ==============================================================
namespace MailNotifier
{
    public partial class CustomMsgBox : Form
    {
        #region ========================    Конструктор      ========================
        /* ------------*/
        public CustomMsgBox()
        {
            InitializeComponent();

            // - позиционирование окна по центру относительно главного окна приложения
            StartPosition = FormStartPosition.Manual; // - получение позиции окна
            Point thisLocation = FormMain.Form.Location; // - получение текущей позиции главного окна
            thisLocation.X += FormMain.Form.Width / 2 - 180; // - центрирование по горизонтали
            thisLocation.Y += FormMain.Form.Height / 2 - 55; // - центрирование по вертикали
            Location = thisLocation; // - установка новой позиции для диалогового окна

            // - настройка внешнего вида кнопок
            btnNo.FlatAppearance.BorderSize = 0;
            btnNo.FlatAppearance.MouseDownBackColor = ColorTranslator.FromHtml("#006");
            btnNo.FlatAppearance.MouseOverBackColor = ColorTranslator.FromHtml("#007");

            // - настройка внешнего вида кнопки Отмена
            btnCansel.FlatAppearance.BorderSize = 0;
            btnCansel.FlatAppearance.MouseDownBackColor = ColorTranslator.FromHtml("#006");
            btnCansel.FlatAppearance.MouseOverBackColor = ColorTranslator.FromHtml("#007");
        }

        // ------------
        #endregion

        // ==========================================================================
        #region ===============  Основные параметры и события формы   ===============
        /* ------------*/
        // - статическая переменная для хранения результата выбора пользователя
        private static DialogResult result = DialogResult.No;

        // ==================================== Вызов окна диалога - Предупреждение
        public static void ShowAllert(string Lbl1text = null)
        {
            // - создаём экземпляр диалогового окна
            CustomMsgBox msBox = new CustomMsgBox();

            // - если передан текст для первой метки — установить его
            if (Lbl1text != null) msBox.colorLabel1.Text = Lbl1text;
            else msBox.colorLabel1.Text = "Это тестовое сообщение системы...";

            // - скрыть лишние кнопки и картинку и тексты
            msBox.btnNo.Visible = false;
            msBox.btnCansel.Visible = false;
            msBox.colorLabel1.Visible = false;
            msBox.colorLabel2.Visible = false;
            msBox.pictureBoxQuest.Visible = false;
            

            // - настроить оставшуюся кнопку
            msBox.btnOk.Text = "OK";
            msBox.btnOk.Location = new Point(142, 76);

            // - включить нужную картинку и текст
            msBox.labelAllert.Visible = true;
            msBox.labelAllert.Text = Lbl1text;
            msBox.pictureBoxAllert.Visible = true;

            // - показать диалог модально и вернуть выбранный результат
            msBox.ShowDialog();
        }

        // ==================================== Вызов окна диалога - Вопрос
        public static DialogResult ShowQuestion(string Lbl1text = null, string Lbl2text = null, bool showCancel = true)
        {
            // - создаём экземпляр диалогового окна
            CustomMsgBox msBox = new CustomMsgBox();

            // - если переданы тексты — установим их
            if (Lbl1text != null) msBox.colorLabel1.Text = Lbl1text;
            if (Lbl2text != null) msBox.colorLabel2.Text = Lbl2text;

            // - если не нужно показывать кнопку Отмена — скрыть её и сдвинуть оставшиеся кнопки
            if (!showCancel)
            {
                msBox.btnCansel.Visible = false;
                msBox.btnOk.Location = new Point(100, 76);
                msBox.btnNo.Location = new Point(200, 76);
            }

            // - показать диалог модально и вернуть выбранный результат
            msBox.ShowDialog();
            return result;
        }

        // ==================================== Событие - Нажатие кнопки ОК
        private void btnOk_Click(object sender, EventArgs e)
        {
            result = DialogResult.Yes;
            this.Close();
        }

        // ==================================== Событие - Нажатие кнопки Отмена
        private void btnCansel_Click(object sender, EventArgs e)
        {
            result = DialogResult.Cancel;
            this.Close();
        }

        // ====================================  Событие - Нажатие кнопки Нет
        private void btnNo_Click(object sender, EventArgs e)
        {
            result = DialogResult.No;
            this.Close();
        }
    }

    // ------------
    #endregion

}
