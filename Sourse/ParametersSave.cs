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
    /* ------------ */

    // ==============================================================
    #region =============    Перечисление периода     ===============
    /* ------------ */
    public enum Periods
    {
        [Description("2 минуты")]   m120000, // - 120000 мс = 2 мин
        [Description("5 минут")]    m300000, // - 300000 мс = 5 мин
        [Description("10 минут")]   m600000, // - 600000 мс = 10 мин
        [Description("15 минут")]   m900000, // - 900000 мс = 15 мин
        [Description("30 минут")]   m1800000
    }

    // ------------
    #endregion


    // ==============================================================
    #region ========   Конвертатор перечисления периода   ===========
    /* ------------ */
    class PeriodConverter : EnumConverter
    {
        // - переменная для хранения типа перечисления
        private Type type;

        // ====================================  Конструктор
        public PeriodConverter(Type type) : base(type)
        {
            this.type = type;
        }

        // ====================================  Конвертация в Periods
        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destType)
        {
            // - получаем информацию о поле перечисления по его имени
            FieldInfo fi = type.GetField(Enum.GetName(type, value));

            // - получаем атрибут Description для этого поля
            DescriptionAttribute descAttr =
              (DescriptionAttribute)Attribute.GetCustomAttribute(fi, typeof(DescriptionAttribute));

            // - если Description найден, возвращаем описание, иначе возвращаем строковое представление значения
            if (descAttr != null) return descAttr.Description;
            else return value.ToString();
        }

        // ====================================  Конвертация из Periods
        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            // - перебираем все поля перечисления и ищем совпадение по Description
            foreach (FieldInfo fi in type.GetFields())
            {
                DescriptionAttribute descAttr =
                  (DescriptionAttribute)Attribute.GetCustomAttribute(fi, typeof(DescriptionAttribute));

                // - если Description найден, возвращаем соответствующее значение перечисления
                if ((descAttr != null) && ((string)value == descAttr.Description)) return Enum.Parse(type, fi.Name);
            }

            // - если совпадений не найдено, пробуем конвертировать из строки в перечисление по имени
            return Enum.Parse(type, (string)value);
        }
    }

    // ------------
    #endregion


    // ==============================================================
    #region ==========  Сохраняемые настройки программы  ============
    /* ------------ */
    // - значение по умолчанию для отображения в PropertyGrid при открытии настроек
    [DefaultProperty("Password")]

    // - класс для сохранения настроек программы
    public class SaveSettings
    {
        // - сохранённые значения "Пароль администратора"
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

        // - сохранённые значения "Браузер"
        private String SavedBrowser { get; set; } = "default";
        // ------------
        [DisplayName("Браузер")]
        [Description("Путь программы браузера для перехода в аккаунт")]
        public string Browser
        {
            get { return SavedBrowser; }
            set { SavedBrowser = value; }
        }

        // - сохранённые значения "Очищать ошибки"
        private bool SavedClearErrors { get; set; } = true;
        // ------------
        [DisplayName("Очищать ошибки")]
        [Description("Очищать предыдущее сообщение ошибки при проверке почты")]
        public bool ClearErrors
        {
            get { return SavedClearErrors; }
            set { SavedClearErrors = value; }
        }

        // - сохранённые значения "Автопроверка"
        private bool SavedAutoCheck { get; set; }
        // ------------
        [DisplayName("Автопроверка")]
        [Description("Проверять почту автоматически")]
        public bool AutoCheck
        {
            get { return SavedAutoCheck; }
            set { SavedAutoCheck = value; }
        }

        // - сохранённые значения "Период"
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
    /* ------------ */
    // - значение по умолчанию для отображения в PropertyGrid при открытии настроек
    [DefaultProperty("user")]

    // - класс для сохранения настроек аккаунта
    public class SaveAccount
    {
        // - сохранённые значения "Логин"
        private string SavedLogin { get; set; } = "user";
        // ------------
        [DisplayName("Логин")]
        [Description("Логин пользователя почтового аккаунта")]
        public string Login
        {
            get { return SavedLogin; }
            set { SavedLogin = value; }
        }
        
        // - сохранённые значения "Пароль"
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
        
        // - сохранённые значения "Строка соединения"
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
        
        // - сохранённые значения "Хост IMAP"
        private string SavedHost { get; set; } = "";
        // ------------
        [DisplayName("Хост IMAP")]
        [Description("Адрес хоста IMAP соединения")]
        public string Host
        {
            get { return SavedHost; }
            set { SavedHost = value; }
        }
        
        // - сохранённые значения "Порт IMAP"
        private int SavedPort { get; set; } = 993;
        // ------------       
        [DisplayName("Порт IMAP")]
        [Description("Номер порта IMAP соединения")]
        public int Port
        {
            get { return SavedPort; }
            set { SavedPort = value; }
        }
        
        // - сохранённые значения "Цвет значка"
        private string SavedColor { get; set; } = "#ccc";
        // ------------
        [DisplayName("Цвет значка")]
        [Description("Отображаемый в меню цвет значка аккаунта")]
        public Color Color
        {
            get { return ColorTranslator.FromHtml(SavedColor); }
            set { SavedColor = ColorTranslator.ToHtml(value); }
        }
    }
    // ------------
    #endregion

    // ------------
    #endregion
}
