namespace SimpleMinimalAPI.Helper
{
    public class AppSettings
    {
        public static AppSettings? Instance { get; private set; }

        public static void Initialize(AppSettings settings)
        {
            Instance = settings;
        }

        public EmailInfo EmailInfo { get; set; } = new EmailInfo();
    }

    public class EmailInfo
    {
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}
