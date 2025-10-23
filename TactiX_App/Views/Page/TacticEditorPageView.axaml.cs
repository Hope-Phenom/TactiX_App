using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Markup.Xaml;
using Avalonia.Media.Imaging;
using AvaloniaEdit.CodeCompletion;
using AvaloniaEdit.Document;
using AvaloniaEdit.Editing;
using AvaloniaEdit.Utils;
using CommunityToolkit.Mvvm.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
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
    #region DI容器注入
    private ILanguage _language;
    private IOSes _oses;
    private L_Config _config;
    private IMessenger _messenger;
    #endregion

    #region 变量
    private readonly List<L_ModItem> _modItems;
    private CompletionWindow? _completionWindow;
    private ModPackage? _modPackage;
    private ModResourceCache<Bitmap>? _modResourceCache;
    #endregion

    #region 数据绑定
    public ILanguage Language => _language;
    #endregion

    public TacticEditorPageView(ILang lang, IOSTools oSTools, IMessenger messenger)
    {
        InitializeComponent();

        _language = lang.Language;
        _oses = oSTools.OSes;
        _config = _oses.LoadConfig();
        _messenger = messenger;

        _modItems = new();

        Editor.SyntaxHighlighting = new TacticHighlightingDefinition();
        Editor.TextArea.TextEntered += TextArea_TextEntered;

        AttachedToVisualTree += TacticEditorPageView_AttachedToVisualTree;
    }

#if DEBUG
#pragma warning disable CS8618 // 在退出构造函数时，不可为 null 的字段必须包含非 null 值。请考虑添加 "required" 修饰符或声明为可为 null。
    public TacticEditorPageView() // 此构造函数仅用于保证可预览
    {
        InitializeComponent();
    }
#pragma warning restore CS8618 // 在退出构造函数时，不可为 null 的字段必须包含非 null 值。请考虑添加 "required" 修饰符或声明为可为 null。
#endif

    private void TacticEditorPageView_AttachedToVisualTree(object? sender, VisualTreeAttachmentEventArgs e)
    {
        LoadMod();
    }

    private void TextArea_TextEntered(object? sender, TextInputEventArgs e)
    {
        if (string.IsNullOrEmpty(e.Text)) return;

        if (char.IsLetter(e.Text[0]))
        {
            ShowCompletion();
        }
    }

    private void ShowCompletion()
    {
        // 关闭现有的补全窗口（如果存在）
        if (_completionWindow != null)
        {
            _completionWindow.Closed -= OnCompletionWindowClosed; // 移除之前的事件处理
            _completionWindow.Close();
            _completionWindow = null;
        }

        // 获取当前光标前的单词片段
        var segment = GetWordSegmentBeforeCaret();
        var partialWord = Editor.Document.GetText(segment);

        // 创建补全窗口
        _completionWindow = new CompletionWindow(Editor.TextArea);

        // 创建补全项列表
        var completionData = _modItems
            .Where(item => item.Abbr.StartsWith(partialWord, StringComparison.OrdinalIgnoreCase))
            .Select(item => new ModCompletionData(_modResourceCache, segment, item))
            .ToList();

        if (!completionData.Any()) return;

        // 设置补全数据
        _completionWindow.CompletionList.CompletionData.AddRange(completionData);

        // 窗口关闭时清理引用
        _completionWindow.Closed += OnCompletionWindowClosed;

        // 显示补全窗口
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

        // 查找单词起始位置
        int start = caret.Offset - 1;
        while (start > 0 && IsWordCharacter(document.GetCharAt(start - 1)))
        {
            start--;
        }

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

        if (string.IsNullOrEmpty(_config.CurrentlyEnabledMOD))
        {
            _messenger.Send(new MB_ToastPureText()
            {
                Message = Language.EDITOR_ERROR_MOD_NOT_SET,
                Title = Language.TOAST_TITLE_ERROR,
                Type = MB_Enum_ToastType.Error
            });
        }

        try
        {
            _modPackage = new ModPackage(_config.CurrentlyEnabledMOD);
            _modResourceCache = new ModResourceCache<Bitmap>(_modPackage, (ms) => new Bitmap(ms));

            var modDesc = _modPackage.ModDesc;
            if (modDesc == null) return;

            _modItems.AddRange(modDesc.Units);
            _modItems.AddRange(modDesc.Actions);
        }
        catch
        {
            _messenger.Send(new MB_ToastPureText()
            {
                Message = Language.MODS_MANAGE_VIEW_SELECTED_MOD_ERROR,
                Title = Language.TOAST_TITLE_ERROR,
                Type = MB_Enum_ToastType.Error
            });
        }
    }
}