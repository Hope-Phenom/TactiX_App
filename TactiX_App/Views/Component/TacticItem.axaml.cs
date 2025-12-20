using Avalonia.Controls;
using Avalonia.Media.Imaging;
using CommunityToolkit.Mvvm.Messaging;
using TactiX_Models.MessageBus;

namespace TactiX_App.Views.Component;

public partial class TacticItem : UserControl, IRecipient<MbDisplayStep>
{
    public TacticItem()
    {
        InitializeComponent();

        WeakReferenceMessenger.Default.RegisterAll(this);
    }

    /// <summary>
    ///     ��λ��ţ�0��4�����������ҵ�1��5��
    /// </summary>
    public int SlotNo { get; set; }

    public void Receive(MbDisplayStep message)
    {
        if (message.SlotNo != SlotNo) return;

        if (message.Image != null && message.Image is Bitmap bitmap)
            Image_Icon.Source = bitmap;
        else
            Image_Icon.Source = null;

        Textblock_Desc.Text = message.Desc ?? string.Empty;
        Label_Supply.Content = message.Supply ?? string.Empty;
    }
}