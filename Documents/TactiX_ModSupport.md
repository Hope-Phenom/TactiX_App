# TactiX_ModSupport

## 项目概述

TactiX_ModSupport 是 TactiX_App 的模组支持项目，提供模组加载、回放解码和战术源编码等功能。

## 目录结构

```
TactiX_ModSupport/
├── DictionaryExtensions.cs      # 字典扩展方法
├── IReplayDecoder.cs            # 回放解码器接口
├── ITactiXSourceEncoder.cs      # 战术源编码器接口
├── ModPackage.cs                # 模组包类
├── ModResourceCache.cs          # 模组资源缓存
├── SC2ProductionDuration.json   # SC2 生产持续时间数据
├── SC2ReplayDecoder.cs          # SC2 回放解码器实现
├── TactiXSourceEncoder.cs       # 战术源编码器实现
└── TactiX_ModSupport.csproj     # 项目文件
```

## 主要功能

- 提供模组加载和管理功能
- 支持游戏回放解码（特别是 SC2 回放）
- 支持战术源文件编码和解码
- 提供模组资源缓存
- 提供字典扩展方法

## 核心组件

### 模组包 (ModPackage)

用于表示和管理模组包，包含模组的元数据、资源和功能。

### 回放解码器 (IReplayDecoder)

用于解码游戏回放文件，提取游戏数据和操作。目前实现了 SC2 回放解码器。

### 战术源编码器 (ITactiXSourceEncoder)

用于编码和解码战术源文件，支持 `.tactixSource` 格式。

### 模组资源缓存 (ModResourceCache)

用于缓存模组资源，提高模组加载性能。

## 开发指南

### 创建新的回放解码器

1. 实现 `IReplayDecoder` 接口
2. 在解码器中实现游戏回放的解码逻辑
3. 注册解码器到模组系统

### 创建新的战术源编码器

1. 实现 `ITactiXSourceEncoder` 接口
2. 在编码器中实现战术源文件的编码和解码逻辑
3. 注册编码器到模组系统

### 开发模组

1. 创建模组项目，引用 `TactiX_ModSupport`
2. 实现模组功能
3. 打包模组为模组包
4. 在 TactiX_App 中加载和测试模组

## 注意事项

1. 模组开发需要遵循 TactiX_App 的模组开发规范
2. 回放解码器需要处理不同版本的游戏回放
3. 战术源编码器需要保持格式兼容性
4. 模组资源需要合理管理，避免内存泄漏