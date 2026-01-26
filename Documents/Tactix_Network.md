# Tactix_Network

## 项目概述

Tactix_Network 是 TactiX_App 的网络服务项目，提供统一的网络通信功能支持。

## 目录结构

```
Tactix_Network/
├── INetwork.cs           # 网络服务接口
├── INetworkApi.cs        # 网络 API 接口
├── Network.cs            # 网络服务实现
└── Tactix_Network.csproj # 项目文件
```

## 主要功能

- 提供统一的网络通信接口
- 支持 HTTP/HTTPS 请求
- 支持 WebSocket 通信
- 提供网络 API 封装
- 支持网络请求重试和超时处理
- 支持网络异常处理

## 核心组件

### INetwork

网络服务接口，定义了以下功能：
- 发送 HTTP 请求（GET、POST、PUT、DELETE 等）
- 建立 WebSocket 连接
- 网络状态检测
- 网络配置管理

### INetworkApi

网络 API 接口，定义了应用程序的网络 API 方法，包括：
- 版本检查
- 新闻获取
- 论坛主题获取
- 视频信息获取
- 异常报告

### Network

网络服务实现类，实现了 `INetwork` 接口，提供具体的网络通信功能。

## 开发指南

### 使用网络服务

1. 注入 `INetwork` 接口
2. 使用网络服务发送请求

```csharp
// 注入网络服务
private readonly INetwork _network;

// 发送 GET 请求
var response = await _network.GetAsync<ResponseModel>("https://api.example.com/endpoint");

// 发送 POST 请求
var request = new RequestModel { Data = "test" };
var response = await _network.PostAsync<ResponseModel>("https://api.example.com/endpoint", request);
```

### 使用网络 API

1. 注入 `INetworkApi` 接口
2. 使用 API 方法获取数据

```csharp
// 注入网络 API
private readonly INetworkApi _networkApi;

// 检查版本
var versionResponse = await _networkApi.CheckVersionAsync();

// 获取新闻
var newsList = await _networkApi.GetNewsAsync();
```

### 添加新的网络 API 方法

1. 在 `INetworkApi.cs` 中添加新的 API 方法定义
2. 在 `Network.cs` 中实现新的 API 方法
3. 在调用处使用新的 API 方法

## 注意事项

1. 网络请求应使用异步方法，避免阻塞 UI 线程
2. 应处理网络异常，提供友好的错误提示
3. 应设置合理的请求超时时间
4. 敏感数据应使用 HTTPS 传输
5. 应考虑网络状态变化，提供离线支持
6. 应实现网络请求重试机制，提高请求成功率