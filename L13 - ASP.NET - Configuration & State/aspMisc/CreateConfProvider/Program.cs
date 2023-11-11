namespace App
{
    public class TextConfigurationProvider : ConfigurationProvider
    {
        public string FilePath { get; set; }

        public TextConfigurationProvider(string path) {
            FilePath = path;
        }

        public override void Load()
        {
            var data = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

            using (StreamReader textReader = new StreamReader(FilePath))
            {
                string? line;
                while ((line = textReader.ReadLine()) != null)
                {
                    string key = line.Trim();
                    string? value = textReader.ReadLine() ?? "";
                    data.Add(key, value);
                }
            }

            Data = data;
        }
    }

    public class TextConfigurationSource : IConfigurationSource
    {
        public string FilePath { get; }

        public TextConfigurationSource(string filename) {
            FilePath = filename;
        }

        public IConfigurationProvider Build(IConfigurationBuilder builder)
        {
            // отримуємо повний шлях для файла
            string filePath =
                   builder.GetFileProvider().GetFileInfo(FilePath).PhysicalPath;
            return new TextConfigurationProvider(filePath);
        }
    }

    public static class TextConfigurationExtensions
    {
        public static IConfigurationBuilder AddTextFile(
            this IConfigurationBuilder builder, string path)
        {
            if (builder == null) {
                throw new ArgumentNullException(nameof(builder));
            }

            if (string.IsNullOrEmpty(path)) {
                throw new ArgumentException("Шлях до файла не вказано");
            }

            var source = new TextConfigurationSource(path);
            builder.Add(source);
            return builder;
        }
    }

    public class Application
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var app = builder.Build();

            builder.Configuration.AddTextFile("config.txt");

            app.Map("/", (IConfiguration appConfig) =>
                $"{appConfig["color"]} - {appConfig["width"]}");

            app.Run();
        }
    }
}
