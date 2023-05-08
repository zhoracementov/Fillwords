using FillwordWPF.Models;
using FillwordWPF.Services;
using FillwordWPF.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows.Data;

namespace FillwordWPF.Converters
{
    internal class FillwordItemToViewModelConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var g = App.Host.Services.GetRequiredService<GameProcessService>();
            var q = App.Host.Services.GetRequiredService<BrushQueue>();
            var vm = new FillwordItemViewModel(g, q)
            {
                FillwordItem = (FillwordItem)value
            };

            return vm;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return ((FillwordItemViewModel)value).FillwordItem;
        }
    }
}
