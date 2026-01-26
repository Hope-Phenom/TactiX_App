# TactiX_App 主项目

## 项目概述

TactiX_App 是整个解决方案的主应用程序项目，基于 Avalonia 框架开发，实现了跨平台战术编辑器和管理功能。

## 目录结构

```
TactiX_App/
├── Assets/              # 应用资源文件
├── Converters/          # 值转换器
├── Model/               # 数据模型
├── Resources/           # Avalonia 资源文件
├── Service/             # 应用服务
├── ViewModels/          # 视图模型（MVVM）
├── Views/               # 视图（界面）
├── App.axaml            # 应用入口 XAML
├── App.axaml.cs         # 应用入口代码
└── TactiX_App.csproj    # 项目文件
```

## 主要功能

1. **战术编辑器** - 用于创建和编辑战术
2. **战术大厅** - 浏览和管理战术
3. **回放分析** - 分析游戏回放
4. **模组管理** - 管理应用模组
5. **设置页面** - 应用配置
6. **新闻页面** - 显示最新消息

## MVVM 架构

项目采用 MVVM（Model-View-ViewModel）架构：
- **Model**：数据模型，定义应用程序的数据结构
- **View**：界面，使用 Avalonia XAML 实现
- **ViewModel**：视图模型，连接 Model 和 View，处理业务逻辑

## 导航服务

应用使用 `NavigationService` 实现页面之间的导航，通过 `INavigationService` 接口进行依赖注入。

## 自定义控件

项目包含多个自定义控件，位于 `Views/Component/` 目录下，如：
- `InfoListItem` - 信息列表项
- `KeyMapItem` - 按键映射项
- `TacticInfoDisplay` - 战术信息显示
- `TacticItem` - 战术项

## 主题定制

应用支持主题定制，主要通过 `Resources/CustomTheme.axaml` 文件进行配置。

## 开发指南

### 添加新页面

1. 在 `ViewModels/Page/` 目录下创建新的 ViewModel 类，继承自 `ViewModelBase`
2. 在 `Views/Page/` 目录下创建新的 View 类，使用 Avalonia XAML
3. 在 `App.axaml.cs` 中注册新页面
4. 使用 `NavigationService` 实现导航

### 添加新组件

1. 在 `Views/Component/` 目录下创建新的自定义控件
2. 在需要使用的页面中引用该控件

### 本地化支持

应用支持多语言本地化，通过 `TactiX_Localization` 项目实现。

## 构建和运行

```bash
# 构建项目
dotnet build TactiX_App.csproj

# 运行项目
dotnet run --project TactiX_App.csproj
```