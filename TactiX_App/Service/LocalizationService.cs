using System;
using Avalonia.Controls;

namespace TactiX_App.Service;

using Avalonia;
using Avalonia.Markup.Xaml;
using System.Collections.Generic;
using System.Linq;

public class LocalizationService : ILocalizationService
{
    public event EventHandler? LanguageChanged;

    private ResourceDictionary? _currentLanguageResources;

    private readonly Dictionary<string, ResourceDictionary> _loadedResources = new();

    public void ChangeLanguage(string cultureCode)
    {
        // 尝试加载对应的语言资源
        var resourcePath = $"avares://YourAssembly/Resources/Languages/Strings.{cultureCode}.axaml";

        try
        {
            if (!_loadedResources.TryGetValue(cultureCode, out var resources))
            {
                resources = (ResourceDictionary)AvaloniaXamlLoader.Load(new Uri(resourcePath));
                _loadedResources[cultureCode] = resources;
            }

            _currentLanguageResources = resources;

            // 更新应用程序资源
            if (Application.Current != null)
            {
                // 移除旧的语言资源
                var existingResources = Application.Current.Resources.MergedDictionaries
                    .FirstOrDefault(d => d is ResourceDictionary rd &&
                                         rd.ContainsKey("IsLocalizationDictionary"));

                if (existingResources != null)
                {
                    Application.Current.Resources.MergedDictionaries.Remove(existingResources);
                }

                // 标记这是本地化资源字典
                resources["IsLocalizationDictionary"] = true;

                // 添加新资源
                Application.Current.Resources.MergedDictionaries.Add(resources);
            }

            LanguageChanged?.Invoke(this, EventArgs.Empty);
        }
        catch
        {
            // 回退到默认语言（en-US）
            if (cultureCode != "zh-CN")
            {
                ChangeLanguage("zh-CN");
            }
        }
    }

    public string GetString(string key)
    {
        if (_currentLanguageResources?.TryGetResource(key, null, out var value) == true &&
            value is string str)
        {
            return str;
        }

        return $"[{key}]";
    }

    public void RegisterLanguageChangedEvent(EventHandler eventHandler)
    {
        LanguageChanged += eventHandler;
    }

    public void UnregisterLanguageChangedEvent(EventHandler eventHandler)
    {
        LanguageChanged -= eventHandler;
    }
}