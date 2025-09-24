using Avalonia.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using Material.Icons;
using Microsoft.Extensions.DependencyInjection;
using SukiUI.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using TactiX_App.Service;
using TactiX_App.Views;

namespace TactiX_App.ViewModels
{
    public abstract partial class PageViewModelBase : ViewModelBase
    {
        [ObservableProperty] private string _displayName = string.Empty;
        [ObservableProperty] private MaterialIconKind _icon;
        [ObservableProperty] private int _index;
    }
}
