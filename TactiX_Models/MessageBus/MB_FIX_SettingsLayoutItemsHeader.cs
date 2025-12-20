namespace TactiX_Models.MessageBus;

/// <summary>
///     规避SukiUI的Bug临时消息，后续Bugfix后应该改回数据绑定
/// </summary>
public class MB_FIX_SettingsLayoutItemsHeader
{
    public required string Name { get; set; }
    public required string HeaderText { get; set; }
}