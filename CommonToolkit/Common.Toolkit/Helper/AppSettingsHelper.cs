using Microsoft.Extensions.Configuration;

namespace Common.Toolkit.Helper
{
    public class AppSettingsHelper
    {
        private static IConfiguration configuration;
        private static List<string> Files { get; set; } = new List<string>();


        static AppSettingsHelper()
        {
            var env = System.Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production";
            Console.WriteLine($"当前环境:{env}");
            AddFile("appsettings.json");
            AddFile($"appsettings.{env}.json");

            var builder = new ConfigurationBuilder();

            foreach (var item in Files)
            {
                builder.AddJsonFile(item, optional: true, reloadOnChange: true);
            }

            configuration = builder.AddEnvironmentVariables().Build();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fileName"></param>
        public static void AddFile(string fileName)
        {
            if (!File.Exists(AppContext.BaseDirectory + fileName ?? ""))
            {
                Console.WriteLine("can not found " + AppContext.BaseDirectory + (fileName ?? ""));
                return;
            }

            Files.Add(fileName);

            Console.WriteLine($"add file {fileName}");
        }

        public static string GetSetting(params string[] sections)
        {
            if (configuration == null)
            {
                return string.Empty;
            }
            if (sections != null && sections.Length > 0)
            {
                if (sections.Length == 1)
                {
                    return configuration.GetSection(sections[0]).Value;
                }
                else
                {
                    return configuration[string.Join(":", sections)];
                }
            }

            return string.Empty;
        }

        /// <summary>
        /// 获取指定对象字段
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="section"></param>
        /// <returns></returns>
        public static T? GetObject<T>(string section) where T : class, new()
        {
            T result = new();
            IConfigurationSection configSection = configuration.GetSection(section);
            if (configSection == null)
            {
                return null;
            }

            configSection.Bind(result);

            return result;
        }
    }
}
