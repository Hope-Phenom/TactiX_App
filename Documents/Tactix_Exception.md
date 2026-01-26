# Tactix_Exception

## 项目概述

Tactix_Exception 是 TactiX_App 的异常处理项目，提供统一的异常处理机制和错误代码定义。

## 目录结构

```
Tactix_Exception/
├── ITactiXExceptionFactory.cs   # 异常工厂接口
├── TactiXErrorCodes.cs          # 错误代码定义
├── TactiXException.cs           # 自定义异常类
├── TactiXExceptionFactory.cs    # 异常工厂实现
└── Tactix_Exception.csproj      # 项目文件
```

## 主要功能

- 提供统一的异常处理机制
- 定义应用程序的错误代码
- 提供异常工厂，用于创建和管理异常
- 支持异常分类和错误信息本地化

## 核心组件

### TactiXException

自定义异常类，继承自 `Exception`，包含以下扩展属性：
- **ErrorCode** - 错误代码
- **ErrorMessage** - 错误消息
- **ErrorDetail** - 错误详情
- **ErrorType** - 错误类型

### 错误代码 (TactiXErrorCodes)

定义了应用程序的错误代码，用于分类和标识不同类型的错误。错误代码采用枚举类型，便于管理和扩展。

### 异常工厂 (ITactiXExceptionFactory)

用于创建和管理异常，提供以下功能：
- 创建不同类型的异常
- 格式化异常信息
- 支持异常本地化

## 开发指南

### 使用自定义异常

```csharp
// 抛出自定义异常
throw new TactiXException(TactiXErrorCodes.FileNotFound, "文件未找到", "详细错误信息");

// 使用异常工厂创建异常
var exception = _exceptionFactory.CreateException(TactiXErrorCodes.FileNotFound, "文件未找到");
```

### 添加新的错误代码

1. 在 `TactiXErrorCodes.cs` 中添加新的错误代码枚举值
2. 在本地化资源文件中添加对应的错误消息
3. 在异常工厂中注册新的错误代码

### 异常处理

建议使用 try-catch 块捕获和处理异常，并使用日志服务记录异常信息：

```csharp
try
{
    // 可能抛出异常的代码
}
catch (TactiXException ex)
{
    _logger.Error(ex, "发生自定义异常");
    // 处理自定义异常
}
catch (Exception ex)
{
    _logger.Error(ex, "发生未处理的异常");
    // 处理其他异常
}
```

## 注意事项

1. 避免滥用异常，只在真正的错误情况下抛出异常
2. 使用具体的错误代码，便于错误分类和处理
3. 异常信息应清晰、准确，便于调试和用户理解
4. 所有异常都应被适当捕获和处理，避免应用崩溃
5. 异常信息应考虑本地化，便于向用户展示友好的错误消息