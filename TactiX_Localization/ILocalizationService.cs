using System;

namespace TactiX_Localization;

/// <summary>
///     本地化服务，基于Avalonia资源字典实现
/// </summary>
public interface ILocalizationService
{
    /// <summary>
    ///     改变语言
    /// </summary>
    /// <param name="cultureCode">国家或地区编码</param>
    public void ChangeLanguage(string cultureCode);

    /// <summary>
    ///     依据资源Key获取对应的翻译文本
    /// </summary>
    /// <param name="key">资源键</param>
    /// <returns>文本内容</returns>
    public string GetString(string key);

    /// <summary>
    ///     注册语言变化响应事件
    /// </summary>
    /// <param name="eventHandler">事件处理函数</param>
    public void RegisterLanguageChangedEvent(EventHandler eventHandler);

    /// <summary>
    ///     反注册语言变化响应事件
    /// </summary>
    /// <param name="eventHandler">事件处理函数</param>
    public void UnregisterLanguageChangedEvent(EventHandler eventHandler);
}