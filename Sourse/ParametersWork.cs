using System;
using System.Collections.Generic;


// ==============================================================
namespace MailNotifier
{
    // ==============================================================
    #region ============  Рабочие настройки аккаунтов  ==============
    /* ------------ */
    public class WorkAccount
    {
        // - сохраняемые настройки аккаунта
        public SaveAccount Account { get; set; }

        // - рабочие параметры аккаунта
        public string Name { get; set; }         // - имя Аккаунта
        public int Count { get; set; } = 0;      // - количество непрочитанных писем
        public DateTime LastCheck { get; set; }  // - время последней проверки аккаунта     
        public bool IsError { get; set; } = false;   // - есть ошибка проверки аккаунта
        public ButtonPanel MenuPanel { get; set; }   // - ссылка меню на аккаунт
        public ButtonPanel LeftButton { get; set; }  // - ссылка бок.панели на аккаунт
        public string ErrorText { get; set; } =      // - сообщение ошибки
            "Неизвестная ошибка MailAccount:" +
            "\nОписание ошибки не инициализировано...";
    }

    // ------------
    #endregion


    // ==============================================================
    #region ============  Рабочие настройки программы  ==============
    /* ------------ */
    public class WorkSettings
    {
        // - сохраняемые настройки программы 
        public SaveSettings SavedSettings { get; set; } = new SaveSettings();

        // - рабочие параметры программы
        public int Count { get; set; } = 0;         // - количество непрочитанных писем общее
        public int CountBoxes { get; set; } = 0;    // - количество аккаунтов с непрочитанными письмами
        public bool IsAdmin { get; set; } = false;   // - включен режим Админимтратора
        public bool IsUpdate { get; set; } = false;  // - идет процесс проверки почты
        public bool IsError { get; set; } = false;   // - есть ошибка проверки общая
        public DateTime LastCheck { get; set; }      // - время последней проверки общее
        public WorkAccount CurrentAccount { get; set; } = null;  // - текущий выбранный аккаунт
        public string ErrorText { get; set; } =                  // - сообщение ошибки общее
            "Неизвестная ошибка Settings:" +
            "\nОписание ошибки не инициализировано...";
    }

    // ------------
    #endregion


    // ==============================================================
    #region ==========   Рабочние параметры приложения   ============
    /* ------------*/
    public class ParametersWork
    {
        // - критическая ошибка, не позволяющая работу программы
        public bool IsCriticalError { get; set; } = false;

        // - рабочие настройки программы
        public WorkSettings Settings { get; set; } = new WorkSettings();

        // - рабочий список аккаунтов
        public List<WorkAccount> Accounts { get; set; } = new List<WorkAccount>();  
    }
    
    // ------------
    #endregion
}
