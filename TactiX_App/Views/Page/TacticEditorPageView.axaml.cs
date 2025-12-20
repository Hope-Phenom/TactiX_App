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
using TactiX_I18N;
using TactiX_Models;
using TactiX_Models.MessageBus;
using TactiX_Models.Tactics;
using TactiX_ModSupport;
using TactiX_OS_Tools;

namespace TactiX_App.Views.Page;

public partial class TacticEditorPageView : UserControl
{
    public TacticEditorPageView(ILang lang, IosTools oSTools, IMessenger messenger)
    {
        InitializeComponent();

        Language = lang.Language;
        _oses = oSTools.OSes;
        _config = _oses.LoadConfig();
        _messenger = messenger;

        _modItems = new List<LModItem>();

        Editor.SyntaxHighlighting = new TacticHighlightingDefinition();
        Editor.TextArea.TextEntered += TextArea_TextEntered;

        AttachedToVisualTree += TacticEditorPageView_AttachedToVisualTree;
    }

#if DEBUG
#pragma warning disable CS8618 // ���˳����캯��ʱ������Ϊ null ���ֶα�������� null ֵ���뿼������ "required" ���η�������Ϊ��Ϊ null��
    public TacticEditorPageView() // �˹��캯�������ڱ�֤��Ԥ��
    {
        InitializeComponent();
    }
#pragma warning restore CS8618 // ���˳����캯��ʱ������Ϊ null ���ֶα�������� null ֵ���뿼������ "required" ���η�������Ϊ��Ϊ null��
#endif

    #region ���ݰ�

    public ILanguage Language { get; }

    #endregion

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
        // �ر����еĲ�ȫ���ڣ�������ڣ�
        if (_completionWindow != null)
        {
            _completionWindow.Closed -= OnCompletionWindowClosed; // �Ƴ�֮ǰ���¼�����
            _completionWindow.Close();
            _completionWindow = null;
        }

        // ��ȡ��ǰ���ǰ�ĵ���Ƭ��
        var segment = GetWordSegmentBeforeCaret();
        var partialWord = Editor.Document.GetText(segment);

        // ������ȫ����
        _completionWindow = new CompletionWindow(Editor.TextArea);

        // ������ȫ���б�
        var completionData = _modItems
            .Where(item => item.Abbr.StartsWith(partialWord, StringComparison.OrdinalIgnoreCase))
            .Select(item => new ModCompletionData(_modResourceCache, segment, item))
            .ToList();

        if (!completionData.Any()) return;

        // ���ò�ȫ����
        _completionWindow.CompletionList.CompletionData.AddRange(completionData);

        // ���ڹر�ʱ��������
        _completionWindow.Closed += OnCompletionWindowClosed;

        // ��ʾ��ȫ����
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

        // ���ҵ�����ʼλ��
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
                Message = Language.EditorErrorModNotSet,
                Title = Language.ToastTitleError,
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
                Message = string.Format(Language.ModsManageViewSelectedModError,
                    _config.CurrentlyEnabledMod,
                    ex.Message),
                Title = Language.ToastTitleError,
                Type = MbEnumToastType.Error
            });
        }
    }

    #region DI����ע��

    private readonly IoSes _oses;
    private readonly LConfig _config;
    private readonly IMessenger _messenger;

    #endregion

    #region ����

    private readonly List<LModItem> _modItems;
    private CompletionWindow? _completionWindow;
    private ModPackage? _modPackage;
    private ModResourceCache<Bitmap>? _modResourceCache;

    #endregion
}