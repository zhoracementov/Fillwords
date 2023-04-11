using Microsoft.Extensions.Options;
using System;

namespace FillwordWPF.Services.WriteableOptions
{
    public interface IWritableOptions<out T> : IOptions<T> where T : class
    {
        void Update(Action<T> applyChanges);
    }
}
