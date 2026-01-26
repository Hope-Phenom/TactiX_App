# TactiX_Logger

## 项目概述

TactiX_Logger 是 TactiX_App 的日志服务项目，提供统一的日志记录功能。

## 目录结构

```
TactiX_Logger/
├── ILoggerContainer.cs       # 日志容器接口
├── LoggerContainer.cs        # 日志容器实现
├── nlog.config               # NLog 配置文件
└── TactiX_Logger.csproj      # 项目文件
```

## 主要功能

- 提供统一的日志记录接口
- 支持多种日志级别（Debug、Info、Warn、Error、Fatal）
- 支持多种日志输出目标（文件、控制台、数据库等）
- 支持日志格式化和过滤

## 日志配置

项目使用 NLog 作为日志框架，配置文件为 `nlog.config`，可以在该文件中配置日志的输出目标、格式、级别等。

## 开发指南

### 使用日志服务

1. 注入 `ILoggerContainer` 接口
2. 使用日志方法记录不同级别的日志

```csharp
// 注入日志服务
private readonly ILoggerContainer _logger;

// 记录不同级别的日志
_logger.Debug("调试信息");
_logger.Info("普通信息");
_logger.Warn("警告信息");
_logger.Error("错误信息");
_logger.Fatal("致命错误");
```

### 记录异常

可以使用 `Error` 方法记录异常信息：

```csharp
try
{
    // 可能抛出异常的代码
}
catch (Exception ex)
{
    _logger.Error(ex, "发生异常");
}
```

### 配置日志

可以通过修改 `nlog.config` 文件来配置日志：

- **targets** - 配置日志输出目标
- **rules** - 配置日志过滤规则
- **layouts** - 配置日志格式

### 日志级别

日志级别从低到高依次为：
- **Debug** - 调试信息，用于开发和调试
- **Info** - 普通信息，用于记录应用运行状态
- **Warn** - 警告信息，用于记录潜在问题
- **Error** - 错误信息，用于记录错误情况
- **Fatal** - 致命错误，用于记录导致应用崩溃的严重错误

## 注意事项

1. 生产环境中建议将日志级别设置为 Info 或更高，避免产生过多日志
2. 敏感信息不应记录到日志中
3. 定期清理旧日志文件，避免占用过多磁盘空间
4. 可以根据需要扩展日志配置，添加更多输出目标