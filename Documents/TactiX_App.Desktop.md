# TactiX_App.Desktop

## 项目概述

TactiX_App.Desktop 是 TactiX_App 的桌面平台支持项目，支持 Windows、macOS 和 Linux 平台，基于 Avalonia.Desktop 实现。

## 目录结构

```
TactiX_App.Desktop/
├── Program.cs              # 桌面程序入口
├── TactiX.ico              # 应用图标
├── app.manifest            # Windows 应用清单
└── TactiX_App.Desktop.csproj # 项目文件
```

## 主要功能

- 提供桌面平台特定的启动和运行支持
- 处理桌面平台特定的生命周期事件
- 管理桌面窗口和菜单
- 提供桌面特定的资源和配置

## 开发指南

### 构建和运行

```bash
# 构建项目
dotnet build TactiX_App.Desktop.csproj

# 运行项目
dotnet run --project TactiX_App.Desktop.csproj
```

### 平台特定配置

- **app.manifest** - Windows 应用清单，定义应用权限和配置
- **TactiX.ico** - 桌面应用图标

### 平台特定代码

如果需要添加桌面平台特定的代码，可以在 `Program.cs` 中进行扩展，或者创建新的桌面特定服务。

### 调试

可以使用 Visual Studio、Rider 或 VS Code 进行调试，支持断点调试、日志查看等功能。

## 注意事项

1. Windows 平台需要 .NET 7.0 或更高版本
2. macOS 平台需要 .NET 7.0 或更高版本，以及适当的签名
3. Linux 平台需要 .NET 7.0 或更高版本，以及适当的依赖库
4. 不同桌面平台可能有不同的 UI 表现，需要进行测试和调整