using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.IO;

namespace FillwordWPF.Services.WriteableOptions
{
    public class WritableOptions<T> : IWritableOptions<T> where T : class, new()
    {
        private readonly IHostEnvironment environment;
        private readonly IOptionsMonitor<T> options;
        private readonly IConfigurationRoot configuration;
        private readonly string section;
        private readonly string file;

        public WritableOptions(
            IHostEnvironment environment,
            IOptionsMonitor<T> options,
            IConfigurationRoot configuration,
            string section,
            string file)
        {
            this.environment = environment;
            this.options = options;
            this.configuration = configuration;
            this.section = section;
            this.file = file;
        }

        public T Value => options.CurrentValue;
        public T Get(string name) => options.Get(name);

        public void Update(Action<T> applyChanges)
        {
            var fileProvider = environment.ContentRootFileProvider;
            var fileInfo = fileProvider.GetFileInfo(file);
            var physicalPath = fileInfo.PhysicalPath;

            var jObject = JsonConvert.DeserializeObject<JObject>(File.ReadAllText(physicalPath));
            var sectionObject = jObject.TryGetValue(this.section, out JToken section) ?
                JsonConvert.DeserializeObject<T>(section.ToString()) : (Value ?? new T());

            applyChanges(sectionObject);

            jObject[this.section] = JObject.Parse(JsonConvert.SerializeObject(sectionObject));
            File.WriteAllText(physicalPath, JsonConvert.SerializeObject(jObject, Formatting.Indented));
            configuration.Reload();
        }
    }
}
