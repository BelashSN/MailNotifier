using System;
using System.Windows.Forms;
using System.Collections.Generic;


// ==============================================================
namespace MailNotifier
{
    // ==============================================================
    #region ============  Рабочие настройки аккаунтов  ==============
    // ------------
    public class WorkAccount
    {
        public SaveAccount Account { get; set; } // Сохраняемые настройки аккаунта
        // ------------
        public string Name { get; set; }         // Имя Аккаунта
        public int Count { get; set; } = 0;      // Количество непрочитанных писем
        public DateTime LastCheck { get; set; }  // Время последней проверки аккаунта     
        public bool IsError { get; set; } = false;   // Есть ошибка проверки аккаунта
        public ButtonPanel MenuPanel { get; set; }   // Ссылка меню на аккаунт
        public ButtonPanel LeftButton { get; set; }  // Ссылка бок.панели на аккаунт
        public string ErrorText { get; set; } =      // Сообщение ошибки
            "Неизвестная ошибка MailAccount:" +
            "\nОписание ошибки не инициализировано...";
    }
    // ------------
    #endregion


    // ==============================================================
    #region ============  Рабочие настройки программы  ==============
    // ------------
    public class WorkSettings
    {
        // ------------ // Сохраняемые настройки программы //
        public SaveSettings SavedSettings { get; set; } = new SaveSettings(); 
        // ------------
        public int Count { get; set; } = 0;         // Количество непрочитанных писем общее
        public int CountBoxes { get; set; } = 0;    // Количество аккаунтов с непрочитанными письмами
        public bool IsAdmin { get; set; } = false;   // Включен режим Админимтратора
        public bool IsUpdate { get; set; } = false;  // Идет процесс проверки почты
        public bool IsError { get; set; } = false;   // Есть ошибка проверки общая
        public DateTime LastCheck { get; set; }      // Время последней проверки общее
        public WorkAccount CurrentAccount { get; set; } = null;  // Текущий выбранный аккаунт
        public string ErrorText { get; set; } =                  // Сообщение ошибки общее
            "Неизвестная ошибка Settings:" +
            "\nОписание ошибки не инициализировано...";
    }
    // ------------
    #endregion


    // ==============================================================
    #region ==========   Рабочние параметры приложения   ============
    // ------------
    public class ParametersWork
    {
        // Рабочие настройки программы
        public WorkSettings Settings { get; set; } = new WorkSettings();
        // Рабочий список аккаунтов
        public List<WorkAccount> Accounts { get; set; } = new List<WorkAccount>();  
    }
    // ------------
    #endregion
}
