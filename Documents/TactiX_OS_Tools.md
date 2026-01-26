# TactiX_OS_Tools

## 项目概述

TactiX_OS_Tools 是 TactiX_App 的操作系统工具项目，提供跨平台的操作系统相关功能支持。

## 目录结构

```
TactiX_OS_Tools/
├── IOSTools.cs            # iOS 操作系统工具
├── IOSes.cs              # 操作系统枚举
├── OSTools.cs            # 跨平台操作系统工具
├── WindowsImpl.cs        # Windows 平台实现
└── TactiX_OS_Tools.csproj # 项目文件
```

## 主要功能

- 提供跨平台的操作系统功能支持
- 支持多种操作系统（Windows、macOS、Linux、iOS、Android）
- 提供文件系统操作
- 提供进程管理
- 提供系统信息获取
- 提供平台特定的功能实现

## 核心组件

### OSTools

跨平台操作系统工具的主要入口，提供统一的 API 接口，根据运行平台自动调用相应的平台实现。

### 平台特定实现

- **WindowsImpl** - Windows 平台的特定实现
- **IOSTools** - iOS 平台的特定实现

### 操作系统枚举 (IOSes)

定义了支持的操作系统枚举，包括：
- Windows
- macOS
- Linux
- iOS
- Android
- Browser

## 开发指南

### 使用操作系统工具

1. 注入或直接使用 `OSTools` 类
2. 调用相应的方法获取操作系统功能

```csharp
// 获取当前操作系统
var os = OSTools.GetOS();

// 打开文件
OSTools.OpenFile("path/to/file");

// 获取系统信息
var systemInfo = OSTools.GetSystemInfo();
```

### 添加新的平台实现

1. 为新平台创建实现类
2. 在 `OSTools` 类中添加平台检测和调用逻辑
3. 实现相应的平台特定功能

### 添加新的操作系统功能

1. 在 `OSTools` 类中添加统一的 API 接口
2. 在各个平台实现类中添加相应的实现
3. 测试所有支持的平台

## 注意事项

1. 确保所有平台实现提供一致的 API 接口
2. 处理平台特定的异常和限制
3. 考虑跨平台兼容性
4. 避免使用平台特定的依赖
5. 测试所有支持的平台