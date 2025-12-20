using System;
using System.Collections.Generic;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Media.Imaging;
using AvaloniaEdit.CodeCompletion;
using AvaloniaEdit.Document;
using AvaloniaEdit.Utils;
using CommunityToolkit.Mvvm.Messaging;
using TactiX_App.Model;
using TactiX_Localization;
using TactiX_Models;
using TactiX_Models.MessageBus;
using TactiX_Models.Tactics;
using TactiX_ModSupport;
using TactiX_OS_Tools;

namespace TactiX_App.Views.Page;

public partial class TacticEditorPageView : UserControl
{
    public TacticEditorPageView(ILocalizationService localizationService, IosTools oSTools, IMessenger messenger)
    {
        InitializeComponent();

        _localizationService = localizationService;
        _oses = oSTools.OSes;
        _config = _oses.LoadConfig();
        _messenger = messenger;

        _modItems = new List<LModItem>();

        Editor.SyntaxHighlighting = new TacticHighlightingDefinition();
        Editor.TextArea.TextEntered += TextArea_TextEntered;

        AttachedToVisualTree += TacticEditorPageView_AttachedToVisualTree;
    }

#if DEBUG
#pragma warning disable CS8618
    public TacticEditorPageView()
    {
        InitializeComponent();
    }
#pragma warning restore CS8618
#endif

    private void TacticEditorPageView_AttachedToVisualTree(object? sender, VisualTreeAttachmentEventArgs e)
    {
        LoadMod();
    }

    private void TextArea_TextEntered(object? sender, TextInputEventArgs e)
    {
        if (string.IsNullOrEmpty(e.Text)) return;

        if (char.IsLetter(e.Text[0])) ShowCompletion();
    }

    private void ShowCompletion()
    {
        if (_completionWindow != null)
        {
            _completionWindow.Closed -= OnCompletionWindowClosed; // �Ƴ�֮ǰ���¼�����
            _completionWindow.Close();
            _completionWindow = null;
        }
        
        var segment = GetWordSegmentBeforeCaret();
        var partialWord = Editor.Document.GetText(segment);
        
        _completionWindow = new CompletionWindow(Editor.TextArea);
        
        var completionData = _modItems
            .Where(item => item.Abbr.StartsWith(partialWord, StringComparison.OrdinalIgnoreCase))
            .Select(item => new ModCompletionData(_modResourceCache, segment, item))
            .ToList();

        if (!completionData.Any()) return;

        _completionWindow.CompletionList.CompletionData.AddRange(completionData);
        
        _completionWindow.Closed += OnCompletionWindowClosed;
        
        _completionWindow.Show();
    }

    private void OnCompletionWindowClosed(object? sender, EventArgs e)
    {
        if (_completionWindow == null) return;

        _completionWindow.Closed -= OnCompletionWindowClosed;
        _completionWindow = null;
    }

    private ISegment GetWordSegmentBeforeCaret()
    {
        var caret = Editor.TextArea.Caret;
        var document = Editor.Document;

        var start = caret.Offset - 1;
        while (start > 0 && IsWordCharacter(document.GetCharAt(start - 1))) start--;

        return new SimpleSegment(start, caret.Offset - start);
    }

    private bool IsWordCharacter(char c)
    {
        return char.IsLetterOrDigit(c) || c == '_';
    }

    private void LoadMod()
    {
        _modPackage?.Dispose();
        _modResourceCache = null;
        _modItems.Clear();

        if (string.IsNullOrEmpty(_config.CurrentlyEnabledMod))
            _messenger.Send(new MbToastPureText
            {
                Message = _localizationService.GetString("EditorErrorModNotSet"),
                Title = _localizationService.GetString("ToastTitleError"),
                Type = MbEnumToastType.Error
            });

        try
        {
            _modPackage = new ModPackage(_config.CurrentlyEnabledMod);
            _modResourceCache = new ModResourceCache<Bitmap>(_modPackage, ms => new Bitmap(ms));

            var modDesc = _modPackage.ModDesc;
            if (modDesc == null) return;

            _modItems.AddRange(modDesc.Units);
            _modItems.AddRange(modDesc.Actions);
        }
        catch (Exception ex)
        {
            _messenger.Send(new MbToastPureText
            {
                Message = string.Format(_localizationService.GetString("ModsManageViewSelectedModError"),
                    _config.CurrentlyEnabledMod,
                    ex.Message),
                Title = _localizationService.GetString("ToastTitleError"),
                Type = MbEnumToastType.Error
            });
        }
    }

    #region DI容器注入

    private readonly ILocalizationService _localizationService;
    private readonly IoSes _oses;
    private readonly LConfig _config;
    private readonly IMessenger _messenger;

    #endregion

    #region 变量/常量

    private readonly List<LModItem> _modItems;
    private CompletionWindow? _completionWindow;
    private ModPackage? _modPackage;
    private ModResourceCache<Bitmap>? _modResourceCache;

    #endregion
}