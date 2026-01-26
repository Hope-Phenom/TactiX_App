# TactiX_App.Browser

## 项目概述

TactiX_App.Browser 是 TactiX_App 的浏览器/WebAssembly 平台支持项目，基于 Avalonia.Browser 实现 Web 平台支持。

## 目录结构

```
TactiX_App.Browser/
├── Properties/              # 浏览器属性文件
├── wwwroot/                # Web 根目录
├── Program.cs              # 浏览器程序入口
└── TactiX_App.Browser.csproj # 项目文件
```

## 主要功能

- 提供浏览器/WebAssembly 平台特定的启动和运行支持
- 处理 WebAssembly 平台特定的生命周期事件
- 管理浏览器资源和配置
- 提供 Web 特定的资源和样式

## 开发指南

### 构建和运行

```bash
# 构建项目
dotnet build TactiX_App.Browser.csproj

# 运行项目（启动本地 Web 服务器）
dotnet run --project TactiX_App.Browser.csproj
```

### Web 特定配置

- **wwwroot/index.html** - Web 应用入口 HTML
- **wwwroot/app.css** - Web 应用样式
- **wwwroot/main.js** - Web 应用 JavaScript 入口
- **wwwroot/favicon.ico** - Web 应用图标

### 平台特定代码

如果需要添加浏览器平台特定的代码，可以在 `Program.cs` 中进行扩展，或者创建新的 Web 特定服务。

### 调试

可以使用浏览器的开发者工具进行调试，查看控制台日志和网络请求。

## 注意事项

1. WebAssembly 平台有一些限制，某些功能可能无法使用
2. 性能可能不如原生平台
3. 首次加载可能需要较长时间
4. 某些浏览器可能有兼容性问题
5. 需要考虑 Web 安全性，如 CORS 限制