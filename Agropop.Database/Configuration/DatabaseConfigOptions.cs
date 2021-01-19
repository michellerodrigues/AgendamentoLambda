
    public class DatabaseSettings
{
        public const string DatabaseConfig = "DatabaseConfig";
        public DatabaseConfigOptions DatabaseOptions { get; set; }
    }

    public class DatabaseConfigOptions
    {
        public const string DatabaseOptions = "DatabaseOptions";
        public string ConnectionString { get; set; }
    }

