using System;
using System.Collections.Generic;

using Avalonia.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Extensions.DependencyInjection;

namespace TactiX_App.Service
{
    public class NavigationService : ObservableObject, INavigationService
    {
        private readonly Stack<UserControl> _backStack = new();
        private readonly IServiceProvider _serviceProvider;

        private UserControl? _currentView;
        public UserControl? CurrentView
        {
            get => _currentView;
            private set => SetProperty(ref _currentView, value);
        }

        public NavigationService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public void NavigateTo<T>() where T : class
        {
            if (CurrentView != null)
                _backStack.Push(CurrentView);

            // 根据命名约定查找视图 (ViewModels.HomeViewModel -> Views.HomeView)
            var viewType = Type.GetType(typeof(T).FullName
                !.Replace("ViewModels", "Views")
                !.Replace("ViewModel", "View"))!;

            CurrentView = (UserControl)ActivatorUtilities.CreateInstance(_serviceProvider, viewType);
            CurrentView.DataContext = _serviceProvider.GetRequiredService<T>();
        }

        public void GoBack()
        {
            if (_backStack.Count > 0)
                CurrentView = _backStack.Pop();
        }
    }
}
