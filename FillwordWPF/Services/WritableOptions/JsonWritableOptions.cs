using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;

namespace FillwordWPF.Services.WritableOptions
{
    public class JsonWritableOptions<T> : IWritableOptions<T> where T : class, new()
    {
        private readonly IHostEnvironment environment;
        private readonly IOptionsMonitor<T> options;
        private readonly IConfigurationRoot configuration;
        private readonly string section;
        private readonly string file;

        public JsonWritableOptions(
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

        public event Action OnUpdateEvent;

        public async void Update(Action<T> applyChanges)
        {
            var fileProvider = environment.ContentRootFileProvider;
            var fileInfo = fileProvider.GetFileInfo(file);
            var physicalPath = fileInfo.PhysicalPath;

            var fileText = await File.ReadAllTextAsync(physicalPath);

            var jObject = JsonConvert.DeserializeObject<JObject>(fileText);
            var sectionObject = jObject.TryGetValue(this.section, out JToken section) ?
                JsonConvert.DeserializeObject<T>(section.ToString()) : (Value ?? new T());

            applyChanges(sectionObject);

            jObject[this.section] = JObject.Parse(JsonConvert.SerializeObject(sectionObject));
            await File.WriteAllTextAsync(physicalPath, JsonConvert.SerializeObject(jObject, Formatting.Indented));
            configuration.Reload();
            OnUpdate();
        }

        private void OnUpdate()
        {
            OnUpdateEvent?.Invoke();
        }
    }
}
