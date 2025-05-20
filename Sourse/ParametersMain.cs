using System;
using Newtonsoft.Json;
using System.Windows.Forms;
using System.Text.RegularExpressions;


// ==============================================================
namespace MailNotifier
{
    // ========================================================= Настройки приложения
    public class ParametersMain
    {
        // ==========================================================================
        #region ====================    Глобальные переменные    ====================
        // ------------
        private readonly String ExePatch = Application.ExecutablePath;  // Путь исполняемого файла приложения     
        // ------------ 
        public ParametersWork ParamWork { get; } = new ParametersWork();  // Рабочие настройки приложения 
        public ParametersSave ParamSave { get; } = new ParametersSave();  // Рабочие настройки приложения 
        // ------------
        #endregion

        
        // ==========================================================================
        #region ======== Реализацияе методов загрузки и сохранения настроек  ========
        // ------------

        // ==================================== Загрузка сохраняемых параметров из файла
        public bool LoadSettings()
        {
            // --------------- // Чтение сохраненных настроек //
            String JsonText = MailRecoder.Load(ExePatch);
            // ---------------
            switch (JsonText.Substring(0, 2)) {
                case "ᵎ-":                              // Если нет настроек, сохраним пустые 
                    return SaveSettings();
                case "ᴥ-":                              // Зарегистрируем, если ошибка 
                    ParamWork.Settings.IsError = true;
                    ParamWork.Settings.ErrorText = 
                        "Ошибка чтения настроек:" + JsonText.Replace("ᴥ-", "");                   
                    return false;
            }

            // --------------- // Попытка применения сохраняемых настроек программы //
            ParametersSave CurrrentFileSettings;
            // ------------
            try
            {
                CurrrentFileSettings = JsonConvert.DeserializeObject<ParametersSave>(JsonText);
                ParamWork.Settings.SavedSettings = CurrrentFileSettings.FileSetting;              
            }
            // ------------
            catch (Exception e)
            {
                ParamWork.Settings.ErrorText = "Ошибка инициализации настроек:\r\n-----\r\n" + e.Message;
                ParamWork.Settings.IsError = true;
                return false;
            }

            // --------------- // Попытка применения сохраняемых настроек аккаунтов //
            try
            {
                foreach (SaveAccount SavedAccount in CurrrentFileSettings.FileAccount)
                {
                    ParamWork.Accounts.Add(new WorkAccount
                    {
                        Name = Regex.Replace(SavedAccount.Login, @"[^0-9a-zA-Z]+", ""),
                        Account = SavedAccount
                    });
                }
            }
            // ------------
            catch (Exception e)
            {
                ParamWork.Settings.ErrorText = "Ошибка списка аккаутнов:\r\n-----\r\n" + e.Message;
                ParamWork.Settings.IsError = true;
                return false;
            }
            // --------------- /////!!!!! this.SaveSettings(); /////!!!!! - Удалить после разработки!!!!*/
            return true;
        }

        // ==================================== Схранение сохраняемых параметров в файл  
        /* /////!!!!!  Добавить вызов записи при изменении настроек !!!!! */
        public bool SaveSettings()
        {
            // --------------- //  Инициализация сохраняемых настроек программы //
            ParametersSave curFileSettings = new ParametersSave();
            curFileSettings.FileSetting = ParamWork.Settings.SavedSettings;

            // --------------- // Инициализация сохраняемых настроек аккаунтов //
            foreach (var curAccount in this.ParamWork.Accounts)
            {
                curFileSettings.FileAccount.Add(curAccount.Account);
            }

            // --------------- // Попытка сериализацмм строки настроек и ее кодирования //
            String JsonText;
            // ------------
            try
            {
                JsonText = JsonConvert.SerializeObject(curFileSettings);
            }
            // ------------
            catch (Exception e)
            {
                ParamWork.Settings.ErrorText = "Ошибка подготовки настроек:\r\n-----\r\n" + e.Message;
                ParamWork.Settings.IsError = true;
                return false;
            }

            // --------------- // Сохранение сохраненных настроек //
            String ResSave = MailRecoder.Save(ExePatch, JsonText);
            // ---------------
            switch (ResSave.Substring(0, 2))
            {
                case "ᴥ-":                              // Зарегистрируем, если ошибка                  
                    ParamWork.Settings.ErrorText = ResSave.Replace("ᴥ-", "");
                    ParamWork.Settings.IsError = true;
                    return false;
                default:
                    return true;
            }
        }
        // ------------
        #endregion
    }
}
