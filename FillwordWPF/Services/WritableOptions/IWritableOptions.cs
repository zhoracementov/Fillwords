using Microsoft.Extensions.Options;
using System;

namespace FillwordWPF.Services.WritableOptions
{
    public interface IWritableOptions<out T> : IOptions<T> where T : class
    {
        public event Action OnUpdateEvent;
        void Update(Action<T> applyChanges);
    }
}
