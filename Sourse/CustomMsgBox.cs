using System;
using System.Drawing;
using System.Windows.Forms;

// ==============================================================
namespace MailNotifier
{
    public partial class CustomMsgBox : Form
    {
        #region ========================    Конструктор      ========================
        public CustomMsgBox()
        {
            InitializeComponent();
            // ------------
            StartPosition = FormStartPosition.Manual;
            Point thisLocation = FormMain.Form.Location;
            thisLocation.X += FormMain.Form.Width / 2 - 180;
            thisLocation.Y += FormMain.Form.Height / 2 - 55;
            Location = thisLocation;
            // ------------
            btnNo.FlatAppearance.BorderSize = 0;
            btnNo.FlatAppearance.MouseDownBackColor = ColorTranslator.FromHtml("#006");
            btnNo.FlatAppearance.MouseOverBackColor = ColorTranslator.FromHtml("#007");
            // ------------
            btnCansel.FlatAppearance.BorderSize = 0;
            btnCansel.FlatAppearance.MouseDownBackColor = ColorTranslator.FromHtml("#006");
            btnCansel.FlatAppearance.MouseOverBackColor = ColorTranslator.FromHtml("#007");
        }
        // ------------
        #endregion

        // ==========================================================================
        #region ===============  Основные параметры и события формы   ===============
        /* ------------*/
        // ==================================== Возвращаемый результат
        private static DialogResult result = DialogResult.No;

        // ==================================== Вызов окна диалога
        public static DialogResult ShowQuestion()
        {
            CustomMsgBox msBox = new CustomMsgBox();           
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
