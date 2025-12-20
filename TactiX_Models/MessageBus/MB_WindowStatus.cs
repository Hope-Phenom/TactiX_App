namespace TactiX_Models.MessageBus;

public class MB_WindowStatus
{
    public enum MB_ENUM_WINDOW_STATUS
    {
        Normal = 0,
        Minimized,
        Maximized
    }

    public MB_ENUM_WINDOW_STATUS WindowStatus { get; set; }
}