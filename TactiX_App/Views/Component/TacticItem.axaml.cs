using Avalonia.Controls;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using CommunityToolkit.Mvvm.Messaging;
using TactiX_Models.MessageBus;

namespace TactiX_App.Views.Component;

public partial class TacticItem : UserControl, IRecipient<MB_DisplayStep>
{
    /// <summary>
    /// 槽位序号，0至4，代表从左到右第1至5个
    /// </summary>
    public int SlotNo { get; set; }

    public TacticItem()
    {
        InitializeComponent();

        WeakReferenceMessenger.Default.RegisterAll(this);
    }

    public void Receive(MB_DisplayStep message)
    {
        if (message.SlotNo != SlotNo) return;

        if (message.Image != null && message.Image is Bitmap bitmap)
        {
            Image_Icon.Source = bitmap;
        }
        else
        {
            Image_Icon.Source = null;
        }

        Label_Desc.Content = message.Desc ?? string.Empty;
    }
}