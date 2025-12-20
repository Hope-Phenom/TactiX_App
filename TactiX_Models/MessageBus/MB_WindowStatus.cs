namespace TactiX_Models.MessageBus;

public class MbWindowStatus
{
    public enum MbEnumWindowStatus
    {
        Normal = 0,
        Minimized,
        Maximized
    }

    public MbEnumWindowStatus WindowStatus { get; set; }
}