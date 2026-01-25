using Avalonia;
using Avalonia.Controls;
using System.Windows.Input;

namespace TactiX_App.Views.Component;

public partial class InfoListItem : UserControl
{
    #region 依赖属性
    
    /// <summary>
    ///     标题
    /// </summary>
    public static readonly StyledProperty<string?> TitleProperty =
        AvaloniaProperty.Register<InfoListItem, string?>(nameof(Title));

    /// <summary>
    ///     标题
    /// </summary>
    public string? Title
    {
        get => GetValue(TitleProperty);
        set => SetValue(TitleProperty, value);
    }
    
    /// <summary>
    ///     链接
    /// </summary>
    public static readonly StyledProperty<string?> UrlProperty =
        AvaloniaProperty.Register<InfoListItem, string?>(nameof(Url));

    /// <summary>
    ///     链接
    /// </summary>
    public string? Url
    {
        get => GetValue(UrlProperty);
        set => SetValue(UrlProperty, value);
    }
    
    /// <summary>
    ///     日期
    /// </summary>
    public static readonly StyledProperty<string?> DateProperty =
        AvaloniaProperty.Register<InfoListItem, string?>(nameof(Date));

    /// <summary>
    ///     日期
    /// </summary>
    public string? Date
    {
        get => GetValue(DateProperty);
        set => SetValue(DateProperty, value);
    }
    
    /// <summary>
    ///     是否为最后一个元素
    /// </summary>
    public static readonly StyledProperty<bool> IsLastItemProperty =
        AvaloniaProperty.Register<InfoListItem, bool>(nameof(IsLastItem));

    /// <summary>
    ///     是否为最后一个元素
    /// </summary>
    public bool IsLastItem
    {
        get => GetValue(IsLastItemProperty);
        set => SetValue(IsLastItemProperty, value);
    }
    
    /// <summary>
    ///     命令
    /// </summary>
    public static readonly StyledProperty<ICommand?> CommandProperty =
        AvaloniaProperty.Register<InfoListItem, ICommand?>(nameof(Command));

    /// <summary>
    ///     命令
    /// </summary>
    public ICommand? Command
    {
        get => GetValue(CommandProperty);
        set => SetValue(CommandProperty, value);
    }
    
    /// <summary>
    ///     命令
    /// </summary>
    public static readonly StyledProperty<string?> CommandParamProperty =
        AvaloniaProperty.Register<InfoListItem, string?>(nameof(CommandParam));

    /// <summary>
    ///     命令
    /// </summary>
    public string? CommandParam
    {
        get => GetValue(CommandParamProperty);
        set => SetValue(CommandParamProperty, value);
    }

    #endregion
    
    public InfoListItem()
    {
        InitializeComponent();
    }
}