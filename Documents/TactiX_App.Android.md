# TactiX_App.Android

## 项目概述

TactiX_App.Android 是 TactiX_App 的 Android 平台支持项目，基于 Avalonia.Android 实现跨平台支持。

## 目录结构

```
TactiX_App.Android/
├── Properties/              # Android 属性文件
├── Resources/               # Android 资源文件
├── Icon.png                 # 应用图标
├── MainActivity.cs          # Android 主活动
└── TactiX_App.Android.csproj # 项目文件
```

## 主要功能

- 提供 Android 平台特定的启动和运行支持
- 处理 Android 平台特定的生命周期事件
- 管理 Android 权限
- 提供 Android 特定的资源和配置

## 开发指南

### 构建和运行

```bash
# 构建项目
dotnet build TactiX_App.Android.csproj

# 运行项目（需要连接 Android 设备或模拟器）
dotnet run --project TactiX_App.Android.csproj
```

### Android 特定配置

- **AndroidManifest.xml** - 定义应用权限和配置
- **Resources/values/colors.xml** - 定义应用颜色
- **Resources/values/styles.xml** - 定义应用样式
- **Resources/drawable/splash_screen.xml** - 定义启动屏幕

### 平台特定代码

如果需要添加 Android 平台特定的代码，可以在 `MainActivity.cs` 中进行扩展，或者创建新的 Android 特定服务。

### 调试

可以使用 Visual Studio 或 Rider 连接 Android 设备或模拟器进行调试，也可以使用 `logcat` 查看日志。

## 注意事项

1. 确保安装了 Android SDK 和 NDK
2. 确保 Android 设备或模拟器运行 Android 6.0 或更高版本
3. 某些功能可能需要特定的 Android 权限
4. 性能优化可能需要针对 Android 平台进行特定调整