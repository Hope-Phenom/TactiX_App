# TactiX_Localization

## 项目概述

TactiX_Localization 是 TactiX_App 的本地化服务项目，提供多语言支持功能。

## 目录结构

```
TactiX_Localization/
├── Resources/              # 本地化资源文件
├── ILocalizationService.cs # 本地化服务接口
├── LocalizationService.cs  # 本地化服务实现
└── TactiX_Localization.csproj # 项目文件
```

## 主要功能

- 提供多语言支持
- 支持动态语言切换
- 提供本地化服务接口
- 管理本地化资源

## 本地化资源

本地化资源文件位于 `Resources/Locales/` 目录下，采用 Avalonia 资源格式：
- **Strings.en-US.axaml** - 英文（美国）资源
- **Strings.zh-CN.axaml** - 中文（简体）资源

## 开发指南

### 添加新语言

1. 在 `Resources/Locales/` 目录下创建新的语言资源文件，命名格式为 `Strings.{language}-{region}.axaml`
2. 在新的资源文件中添加翻译后的字符串
3. 在 `LocalizationService.cs` 中注册新语言

### 使用本地化服务

1. 注入 `ILocalizationService` 接口
2. 使用 `GetString` 方法获取本地化字符串

```csharp
// 注入本地化服务
private readonly ILocalizationService _localizationService;

// 使用本地化服务
var localizedString = _localizationService.GetString("KeyName");
```

### 动态切换语言

使用 `LocalizationService` 的 `ChangeLanguage` 方法可以动态切换应用语言：

```csharp
_localizationService.ChangeLanguage("zh-CN");
```

### 本地化键命名规范

建议使用以下命名规范来命名本地化键：
- 使用点号分隔的层级结构
- 第一部分表示功能模块
- 第二部分表示控件类型
- 第三部分表示控件名称

例如：`Settings.Button.Save`、`TacticEditor.Label.Title`

## 构建和运行

```bash
# 构建项目
dotnet build TactiX_Localization.csproj
```

## 注意事项

1. 确保所有本地化资源文件中的键名保持一致
2. 建议使用英文作为默认语言
3. 定期更新和维护本地化资源
4. 考虑使用专业的本地化工具进行翻译管理