# TactiX_App.iOS

## 项目概述

TactiX_App.iOS 是 TactiX_App 的 iOS 平台支持项目，基于 Avalonia.iOS 实现跨平台支持。

## 目录结构

```
TactiX_App.iOS/
├── Resources/              # iOS 资源文件
├── AppDelegate.cs          # iOS 应用委托
├── Entitlements.plist      # iOS 权限配置
├── Info.plist              # iOS 应用信息
├── Main.cs                 # iOS 程序入口
└── TactiX_App.iOS.csproj   # 项目文件
```

## 主要功能

- 提供 iOS 平台特定的启动和运行支持
- 处理 iOS 平台特定的生命周期事件
- 管理 iOS 权限
- 提供 iOS 特定的资源和配置

## 开发指南

### 构建和运行

```bash
# 构建项目
dotnet build TactiX_App.iOS.csproj

# 运行项目（需要连接 iOS 设备或模拟器）
dotnet run --project TactiX_App.iOS.csproj
```

### iOS 特定配置

- **Info.plist** - iOS 应用信息，定义应用名称、版本、权限等
- **Entitlements.plist** - iOS 权限配置，定义应用需要的特殊权限
- **Resources/LaunchScreen.xib** - iOS 启动屏幕

### 平台特定代码

如果需要添加 iOS 平台特定的代码，可以在 `AppDelegate.cs` 或 `Main.cs` 中进行扩展，或者创建新的 iOS 特定服务。

### 调试

可以使用 Visual Studio 或 Rider 连接 iOS 设备或模拟器进行调试，也可以使用 Xcode 的调试工具查看日志。

## 注意事项

1. 确保安装了 Xcode 和 iOS SDK
2. 确保 iOS 设备或模拟器运行 iOS 14.0 或更高版本
3. 某些功能可能需要特定的 iOS 权限
4. 需要 Apple Developer 账号才能在真实设备上运行和分发
5. 性能优化可能需要针对 iOS 平台进行特定调整