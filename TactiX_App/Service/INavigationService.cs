using Avalonia.Controls;
using System;

namespace TactiX_App.Service
{
    /// <summary>
    /// 导航服务接口
    /// </summary>
    public interface INavigationService
    {
        /// <summary>
        /// 导航至指定页面
        /// </summary>
        /// <typeparam name="T">指定的页面</typeparam>
        void NavigateTo<T>() where T : class;
        /// <summary>
        /// 导航至指定页面
        /// </summary>
        /// <param name="viewModeType"></param>
        void NavigateTo(Type viewModeType);
        /// <summary>
        /// 返回上一级
        /// </summary>
        void GoBack();
        /// <summary>
        /// 当前的视图
        /// </summary>
        public UserControl? CurrentView { get; }
    }
}
