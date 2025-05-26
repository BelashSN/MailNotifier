using System;
using System.Drawing;
using System.Reflection;
using System.Globalization;
using System.ComponentModel;


// ==============================================================
namespace MailNotifier
{
    // ==============================================================
    #region ==========   Сохраняемые настройки периода   ============
    /* ------------*/

    // ==============================================================
    #region =============    Перечисление периода     ===============
    // ------------
    public enum Periods
    {
        [Description("2 минуты")]   m120000,
        [Description("5 минут")]    m300000,
        [Description("10 минут")]   m600000,
        [Description("15 минут")]   m900000,
        [Description("30 минут")]   m1800000
    }
    // ------------
    #endregion


    // ==============================================================
    #region ========   Конвертатор перечисления периода   ===========
    // ------------
    class PeriodConverter : EnumConverter
    {
        private Type type;

        // ====================================  Конструктор
        public PeriodConverter(Type type) : base(type)
        {
            this.type = type;
        }

        // ====================================  Конвертация в Periods
        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destType)
        {
            FieldInfo fi = type.GetField(Enum.GetName(type, value));
            // ------------
            DescriptionAttribute descAttr =
              (DescriptionAttribute)Attribute.GetCustomAttribute(fi, typeof(DescriptionAttribute));
            // ------------
            if (descAttr != null) return descAttr.Description;
            else return value.ToString();
        }

        // ====================================  Конвертация из Periods
        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            foreach (FieldInfo fi in type.GetFields())
            {
                DescriptionAttribute descAttr =
                  (DescriptionAttribute)Attribute.GetCustomAttribute(fi, typeof(DescriptionAttribute));
                // ------------
                if ((descAttr != null) && ((string)value == descAttr.Description)) return Enum.Parse(type, fi.Name);
            }
            // ------------
            return Enum.Parse(type, (string)value);
        }
        // ------------
        #endregion
    }
    // ------------
    #endregion


    // ==============================================================
    #region ==========  Сохраняемые настройки программы  ============
    // ------------
    [DefaultProperty("Password")]
    public class SaveSettings
    {
        private String SavedPassword { get; set; } = "";
        // ------------
        [PasswordPropertyText(true)]
        [DisplayName("Админ. пароль")]
        [Description("Пароль администратора программы для настройки")]
        public string Password
        {
            get { return SavedPassword; }
            set { SavedPassword = value; }
        }
        // ------------ ------------
        private String SavedBrowser { get; set; } = "default";
        // ------------
        [DisplayName("Браузер")]
        [Description("Путь программы браузера для перехода в аккаунт")]
        public string Browser
        {
            get { return SavedBrowser; }
            set { SavedBrowser = value; }
        }
        // ------------ ------------
        private bool SavedClearErrors { get; set; } = true;
        // ------------
        [DisplayName("Очищать ошибки")]
        [Description("Очищать предыдущее сообщение ошибки при проверке почты")]
        public bool ClearErrors
        {
            get { return SavedClearErrors; }
            set { SavedClearErrors = value; }
        }
        // ------------ ------------
        private bool SavedAutoCheck { get; set; }
        // ------------
        [DisplayName("Автопроверка")]
        [Description("Проверять почту автоматически")]
        public bool AutoCheck
        {
            get { return SavedAutoCheck; }
            set { SavedAutoCheck = value; }
        }
        // ------------ ------------
        private Periods SavedPeriod { get; set; } = Periods.m600000;
        // ------------       
        [DisplayName("Период")]
        [Description("Период автоматической проверки почты")]
        [TypeConverter(typeof(PeriodConverter))]
        public Periods Period
        {
            get { return SavedPeriod; }
            set { SavedPeriod = value; }
        }
    }
    // ------------
    #endregion

    
    // ==============================================================
    #region ==========  Сохраняемые настройки аккаунтов   ===========
    // ------------
    [DefaultProperty("Host")]
    public class SaveAccount
    {
        private string SavedHost { get; set; } = "";
        // ------------
        [DisplayName("Хост IMAP")]
        [Description("Адрес хоста IMAP соединения")]
        public string Host
        {
            get { return SavedHost; }
            set { SavedHost = value; }
        }
        // ------------ ------------
        private int SavedPort { get; set; } = 993;
        // ------------       
        [DisplayName("Порт IMAP")]
        [Description("Номер порта IMAP соединения")]
        public int Port
        {
            get { return SavedPort; }
            set { SavedPort = value; }
        }
        // ------------ ------------
        private string SavedColor { get; set; } = "#ccc";
        // ------------
        [DisplayName("Цвет значка")]
        [Description("Отображаемый в меню цвет значка аккаунта")]
        public Color Color
        {
            get { return ColorTranslator.FromHtml(SavedColor); }
            set { SavedColor = ColorTranslator.ToHtml(value); }
        }
        // ------------ ------------
        private string SavedUrl { get; set; } = "";
        // ------------
        // [PasswordPropertyText(true)]
        [DisplayName("Строка соединения")]
        [Description("Адрес строки соединения с почтовым аккаунтом")]
        public string Url
        {
            get { return SavedUrl; }
            set { SavedUrl = value; }
        }
        // ------------ ------------
        private string SavedLogin { get; set; } = "user";
        // ------------
        [DisplayName("Логин")]
        [Description("Логин пользователя почтового аккаунта")]
        public string Login
        {
            get { return SavedLogin; }
            set { SavedLogin = value; }
        }
        // ------------ ------------      
        private string SavedPassword { get; set; } = "";
        // ------------
        [PasswordPropertyText(true)]
        [DisplayName("Пароль")]
        [Description("Пароль пользователя почтового аккаунта")]
        public string Password
        {
            get { return SavedPassword; }
            set { SavedPassword = value; }
        }
    }
    // ------------
    #endregion

}
