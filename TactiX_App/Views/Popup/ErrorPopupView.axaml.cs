using System;
using Avalonia.Controls;
using CommunityToolkit.Mvvm.Messaging;
using TactiX_Models.MessageBus;

namespace TactiX_App.Views.Popup;

public partial class ErrorPopupView : Window, IRecipient<MB_WindowClose>
{
    private const string WINDOW_NAME = "ErrorPopupView";
    private readonly IMessenger _messenger;

    public ErrorPopupView(IMessenger messenger)
    {
        InitializeComponent();

        Closed += ErrorPopupView_Closed;

        _messenger = messenger;
        _messenger.RegisterAll(this);
    }

#if DEBUG
#pragma warning disable CS8618 // ���˳����캯��ʱ������Ϊ null ���ֶα�������� null ֵ���뿼������ "required" ���η�������Ϊ��Ϊ null��
    public ErrorPopupView() // �˹��캯�������ڱ�֤�����������
#pragma warning restore CS8618 // ���˳����캯��ʱ������Ϊ null ���ֶα�������� null ֵ���뿼������ "required" ���η�������Ϊ��Ϊ null��
    {
        InitializeComponent();
    }
#endif

    public void Receive(MB_WindowClose message)
    {
        if (message.Name.Equals(WINDOW_NAME)) Close();
    }

    private void ErrorPopupView_Closed(object? sender, EventArgs e)
    {
        _messenger?.UnregisterAll(this);
    }
}