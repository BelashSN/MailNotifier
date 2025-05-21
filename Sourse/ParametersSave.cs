using System.Collections.Generic;

// ==============================================================
namespace MailNotifier
{
    // ==============================================================
    #region ==========  Сохраняемые настройки аккаунтов   ===========
    // ------------
    public class SaveAccount
    {
        public int Port { get; set; } = 993;
        public string Host { get; set; } = "";
        public string Url { get; set; } = "";
        public string Color { get; set; } = "#ccc";
        public string Login { get; set; } = "user";
        public string Password { get; set; } = "";
    }
    // ------------
    #endregion


    // ==============================================================
    #region ==========  Сохраняемые настройки программы  ============
    // ------------
    public class SaveSettings
    {
        public int Period { get; set; } = 300000; // Период проверки аккаунта в миллисекундах
        public string Password { get; set; } = ""; // Пароль администратора программы
        public bool AutoCheck { get; set; } = true;  // Проверять почту автоматически
        public bool ClearErrors { get; set; } = true;  // Очищать предыдущее сообщение ошибки при проверке
        public string Browser { get; set; } = "default";  // Имя браузера для перехода в аккаунт
    }
    // ------------
    #endregion


    // ==============================================================
    #region ===========   Общие сохраняемые параметры    ============
    // ------------
    public class ParametersSave
    {
        public SaveSettings FileSetting { get; set; } = new SaveSettings();
        public List<SaveAccount> FileAccount { get; set; } = new List<SaveAccount>();
    }
    // ------------
    #endregion
}
