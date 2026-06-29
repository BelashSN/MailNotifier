using System;
using Newtonsoft.Json;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Text.RegularExpressions;


// ==============================================================
namespace MailNotifier
{
    // ==============================================================
    #region ===========   Рабочие настройки приложения   ============
    /* ------------ */
    public class ParametersMain
    {
        // ==========================================================================
        #region ====================    Глобальные переменные    ====================
        /* ------------ */
        // - путь исполняемого файла приложения  
        private readonly String ExePatch = Application.ExecutablePath;

        // - рабочие настройки приложения 
        public ParametersWork ParamWork { get; } = new ParametersWork();  
                                                                          
        // ------------
        #endregion


        // ==========================================================================
        #region ======== Реализацияе методов загрузки и сохранения настроек  ========
        /* ------------ */
        // ==================================== Загрузка сохраняемых параметров из файла
        public bool LoadSettings()
        {
            // --------------- // - чтение сохраненных настроек //
            String JsonText = MailRecoder.Load(ExePatch);

            // - порверим строку на наличие ошибок
            switch (JsonText.Substring(0, 2)) {
                
                // - если нет настроек, сохраним пустые 
                case "ᵎ-":                              
                    return SaveSettings();

                // - зарегистрируем, если ошибка 
                case "ᴥ-":                              
                    ParamWork.Settings.IsError = true;
                    ParamWork.Settings.ErrorText = 
                        "Ошибка чтения настроек:" + JsonText.Replace("ᴥ-", "");

                    // - возврвт критической ошибки 
                    return false;
            }

            // --------------- // - попытка применения сохраняемых настроек программы //
            ParametersSave CurrrentFileSettings;

            // - попытка десериализации строки настроек и ее декодирования 
            try
            {
                CurrrentFileSettings = JsonConvert.DeserializeObject<ParametersSave>(JsonText); 
                ParamWork.Settings.SavedSettings = CurrrentFileSettings.FileSetting;              
            }

            // - при ошмбке зарегистрируем ошибку для вывода пользователю
            catch (Exception e)
            {
                ParamWork.Settings.ErrorText = "Ошибка инициализации настроек:\r\n-----\r\n" + e.Message;
                ParamWork.Settings.IsError = true;
                ParamWork.IsCriticalError = true;

                // - возврвт критической ошибки
                return false;
            }

            // --------------- // попытка применения сохраняемых настроек аккаунтов //
            try
            {
                // - создание рабочих аккаунтов на основе сохраняемых настроек
                foreach (SaveAccount SavedAccount in CurrrentFileSettings.FileAccount)
                    ParamWork.Accounts.Add(new WorkAccount
                    {
                        // - создание имени аккаунтаиз логина, очищенного от всех символов, кроме букв и цифр
                        Name = Regex.Replace(SavedAccount.Login, @"[^0-9a-zA-Z]+", ""),
                        Account = SavedAccount
                    });
            }

            // - при ошмбке зарегистрируем ошибку для вывода пользователю
            catch (Exception e)
            {
                ParamWork.Settings.ErrorText = "Ошибка списка аккаутнов:\r\n-----\r\n" + e.Message;
                ParamWork.Settings.IsError = true;
                ParamWork.IsCriticalError = true;

                // - возврвт критической ошибки
                return false;
            }

            // --------------- ---------------  
            // - возврвт успешного завершения загрузки настроек
            return true;
        }

        // ==================================== Схранение сохраняемых параметров в файл  
        public bool SaveSettings()
        {
            // --------------- // - инициализация сохраняемых настроек программы //
            ParametersSave curFileSettings = new ParametersSave
            {
                FileSetting = ParamWork.Settings.SavedSettings
            };

            // --------------- // - инициализация сохраняемых настроек аккаунтов //
            foreach (var curAccount in this.ParamWork.Accounts)
                curFileSettings.FileAccount.Add(curAccount.Account);

            // --------------- // - попытка сериализацмм строки настроек и ее кодирования //
            String JsonText;
            try
            {
                JsonText = JsonConvert.SerializeObject(curFileSettings);
            }

            // - при ошмбке зарегистрируем ошибку для вывода пользователю
            catch (Exception e)
            {
                ParamWork.Settings.ErrorText = "Ошибка подготовки настроек:\r\n-----\r\n" + e.Message;
                ParamWork.Settings.IsError = true;

                // - возврвт критической ошибки
                return false;
            }

            // --------------- // сохранение сохраненных настроек //
            String ResSave = MailRecoder.Save(ExePatch, JsonText);

            // - порверим строку на наличие ошибок
            switch (ResSave.Substring(0, 2))
            {
                // - при ошмбке зарегистрируем ошибку для вывода пользователю
                case "ᴥ-":                                               
                    ParamWork.Settings.ErrorText = ResSave.Replace("ᴥ-", "");
                    ParamWork.Settings.IsError = true;

                    // - возврвт критической ошибки
                    return false;

                // - возврвт успешного завершения сохранения настроек
                default:
                    return true;
            }
        }
        
        // ------------
        #endregion
    }

    // ------------
    #endregion


    // ==============================================================
    #region ===========   Общие сохраняемые параметры    ============
    /* ------------ */
    public class ParametersSave
    {
        // - сохраняемые настройки программы
        public SaveSettings FileSetting { get; set; } = new SaveSettings();
        // - сохраняемые настройки аккаунтов
        public List<SaveAccount> FileAccount { get; set; } = new List<SaveAccount>();
    }
    // ------------
    #endregion
}
