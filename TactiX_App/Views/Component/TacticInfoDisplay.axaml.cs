using System.ComponentModel;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Data;
using TactiX_Models.Tactics;

namespace TactiX_App.Views.Component;

public partial class TacticInfoDisplay : UserControl
{
    public static readonly StyledProperty<LTactic?> TacticDataProperty =
        AvaloniaProperty.Register<TacticInfoDisplay, LTactic?>(nameof(TacticData));

    public LTactic? TacticData
    {
        get => GetValue(TacticDataProperty);
        set
        {
            SetValue(TacticDataProperty, value);
            UpdateData();
        }
    }

    public TacticInfoDisplay()
    {
        InitializeComponent();
    }

    protected override void OnPropertyChanged(AvaloniaPropertyChangedEventArgs change)
    {
        base.OnPropertyChanged(change);

        if (change.Property == TacticDataProperty)
        {
            UpdateData();
        }
    }

    private void UpdateData()
    {
        if (TacticData is null) return;

        TbxFileName.Text = TacticData.Name;
        TbxAuthor.Text = TacticData.Author;
        TbxDesc.Text = TacticData.Description;
        TbxApplyVer.Text = TacticData.ApplicableVersion;
        TbxType.Text = TacticData.TacticType.ToString();
        TbxTacVer.Text = TacticData.TacVersion.ToString();
        TbxUpdateTime.Text = TacticData.UpdateTime;
        TbxModName.Text = TacticData.ModName;
        TbxModVer.Text = TacticData.ModVersion.ToString();
        TbxActions.Text = TacticData.ActionsListStr;
    }
}