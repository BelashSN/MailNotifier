using System;
using System.Windows.Forms;
using System.Collections.Generic;


// ==============================================================
namespace MailNotifier
{
    // ==============================================================
    #region ======  Параметры элемента отображения настроек   =======
    // ------------
    public class ParamElement
    {
        public string Name { get; set; } = "";  //  Название параметра
        public Label LabellName { get; set; }   //  Надпись - поле имени параметра
        public Label LabellValue { get; set; }  //  Надпись - поле значения параметра
        public string ControlName { get; set; } = "default";   //  /////!!!!! ?????
        public string ParamString { get; set; } = "";  //  Строка с настройками параметра

    }
    // ------------
    #endregion


    // ==============================================================
    #region =====  Параметры контейнера элементов отображения  ======
    // ------------
    public class ParamContainers
    {
        public SplitContainer Container { get; set; }  //  Контейнер элементов отображения настроек
        public Control EditControl { get; set; }       //  Текущий элемент редактировани настройки
        public bool EditEnable { get; set; } = false;  //  Включен режим редактирования настройки
        public List<ParamElement> Elements { get; set; }  //  Список всех элементов отображения контейнера

        // ==================================== Конструктор с установочными параметарми
        public ParamContainers(SplitContainer CurrentContainer)
        {
            Elements = new List<ParamElement>();      //  Список элементов
            Container = CurrentContainer;             //  текущий контейнер
        }
    }
    // ------------
    #endregion

    
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
        public int Count { get; set; } = 0;   // Количество непрочитанных писем общее
        public bool IsAdmin { get; set; } = false;   // Включен режим Админимтратора
        public bool IsUpdate { get; set; } = false;  // Идет процесс проверки почты
        public bool IsError { get; set; } = false;   // Есть ошибка проверки общая
        public bool CanClose { get; set; } = false;  // Разрешено закрытиые программы
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
        public ParamContainers AccountEditor { get; set; }   // Параметры текущего элемента редактирования аккаунта
        public ParamContainers ProgrammEditor { get; set; }  // Параметры текущего элемента редактирования настройки      
        // ------------
        public string AccountParams { get; } = GetAccountParams();   // Список названий параметров аккаунта
        public string ProgrammParams { get; } = GetProgrammParams(); // Список названий параметров программы
        // ------------
        public WorkSettings Settings { get; set; } = new WorkSettings();        // Рабочие настройки программы
        public List<WorkAccount> Accounts { get; set; } = new List<WorkAccount>();  // Рабочий список аккаунтов

        // ==================================== Значение AccountParams по умолчанию
        private static string GetAccountParams()
        {
            return
            "Password" + (char)9678 + "Пароль" + (char)9678 + "Password" + (char)9678 + "String" + (char)9679 +
            "Login" + (char)9678 + "Логин" + (char)9678 + "String" + (char)9678 + "String" + (char)9679 +
            "Url" + (char)9678 + "Адрес" + (char)9678 + "String" + (char)9678 + "String" + (char)9679 +
            "Color" + (char)9678 + "Цвет" + (char)9678 + "String" + (char)9678 + "Color" + (char)9679 +
            "Port" + (char)9678 + "Порт" + (char)9678 + "int" + (char)9678 + "String" + (char)9679 +
            "Host" + (char)9678 + "Хост" + (char)9678 + "String" + (char)9678 + "String";
        }

        // ==================================== Значение ProgrammParams по умолчанию
        private static string GetProgrammParams()
        {
            return
            "Period" + (char)9678 + "Период" + (char)9678 + "Period" + (char)9678 + "Menu" + (char)9679 +
            "AutoCheck" + (char)9678 + "Автопроверка" + (char)9678 + "Bool" + (char)9678 + "Menu" + (char)9679 +
            "ClearErrors" + (char)9678 + "Очищать ошибки" + (char)9678 + "Bool" + (char)9678 + "Menu" + (char)9679 +
            "Browser" + (char)9678 + "Браузер" + (char)9678 + "String" + (char)9678 + "String" + (char)9679 +
            "Password" + (char)9678 + "Админ. пароль" + (char)9678 + "Password" + (char)9678 + "String";
        }
    }
    // ------------
    #endregion
}
