using Microsoft.Extensions.Configuration;

namespace FillwordWPF.Services
{
    public class AppSettings
    {
        private readonly IConfigurationRoot configuration = Program.Configuration;

        public IConfigurationSection Section => configuration.GetSection(nameof(AppSettings));

        public string SaveDataFileName
        {
            get => Section[nameof(SaveDataFileName)];
            set => Section[nameof(SaveDataFileName)] = value;
        }

        public string Version
        {
            get => Section[nameof(Version)];
            set => Section[nameof(Version)] = value;
        }
    }
}
