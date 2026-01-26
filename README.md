# TactiX_App

TactiX_App 是一个基于 Avalonia 框架开发的跨平台战术编辑器和管理应用程序，支持桌面、浏览器、Android 和 iOS 平台。

## 项目结构

```
TactiX_App/
├── TactiX_App/              # 主应用程序项目
├── TactiX_App.Android/      # Android 平台支持
├── TactiX_App.Browser/      # 浏览器/WebAssembly 支持
├── TactiX_App.Desktop/      # 桌面平台支持
├── TactiX_App.iOS/          # iOS 平台支持
├── TactiX_Localization/     # 本地化服务
├── TactiX_Logger/           # 日志服务
├── TactiX_ModSupport/       # 模组支持
├── TactiX_Models/           # 数据模型
├── TactiX_OS_Tools/         # 操作系统工具
├── Tactix_Exception/        # 异常处理
└── Tactix_Network/          # 网络服务
```

## 开发环境要求

- .NET 8.0 SDK 或更高版本
- Visual Studio 2022 或 Rider 或 Visual Studio Code
- Avalonia 扩展（推荐）

## 构建步骤

1. 克隆仓库：
   ```bash
   git clone <repository-url>
   cd TactiX_App
   ```

2. 恢复依赖：
   ```bash
   dotnet restore
   ```

3. 构建解决方案：
   ```bash
   dotnet build
   ```

4. 运行应用程序（桌面版）：
   ```bash
   cd TactiX_App.Desktop
   dotnet run
   ```

## 项目文档

详细的项目文档位于 `Documents/` 目录下，为每个项目提供了单独的说明和开发指导。

## 贡献指南

1. Fork 仓库
2. 创建特性分支 (`git checkout -b feature/AmazingFeature`)
3. 提交更改 (`git commit -m 'Add some AmazingFeature'`)
4. 推送到分支 (`git push origin feature/AmazingFeature`)
5. 打开 Pull Request

## 许可证

本项目采用 Apache 2.0 许可证 - 查看 [LICENSE.txt](LICENSE.txt) 文件了解详情。

## 联系方式

如有问题或建议，请通过 GitHub Issues 提交。