# DoRuan.Report 示例：采购订单预览

基于 [DoRuan.Report](https://www.nuget.org/packages/DoRuan.Report.Designer.Avalonia) 的最小可运行示例：加载报表模板 JSON，绑定演示数据，在 Avalonia 窗口内预览、导出 PDF，并可直接打开可视化设计器修改格式。

## 功能

- **预览**：`ReportPreview` 按页渲染报表，支持缩放与滚动
- **导出 PDF**：一键输出到系统“文档”目录
- **设计器**：以窗口形式打开 `ReportTemplate`，拖拽调整带区与组件（拖动与缩放带吸附对齐线）
- **分组小计**：模板按“分类”分组，组头/组尾自动生成分类小计
- **子报表测试数据**：另附“付款记录”（按订单编号）与“入库批次”（按物料编码）两张表，可直接用于主从子报表
- **模板即 JSON**：`Templates/purchase-order.json` 可直接文本编辑，便于版本管理

## 环境要求

- .NET 10 SDK
- Windows / Linux（UOS、银河麒麟）/ macOS

## 运行

```bash
dotnet restore
dotnet run
```

国内网络可改用镜像源还原：

```bash
dotnet restore -s https://nuget.azure.cn/v3/index.json
dotnet run --no-restore
```

## 代码导读

| 文件 | 作用 |
| --- | --- |
| `MainWindow.cs` | 主窗口：加载模板、构建文档、预览、导出 PDF、打开设计器 |
| `DemoData.cs` | 用 `DataTable` 构造单据头、按分类分组的明细，以及子报表用的付款记录、入库批次 |
| `Templates/purchase-order.json` | 采购订单模板 |

核心流程只有三步：

```csharp
var template = ReportTemplateSerializer.Load(templatePath);
var data = DemoData.CreatePurchaseOrder();

using var renderer = new SkiaReportRenderer();
ReportDocument document = renderer.CreateEngine().Build(template, data);
```

## 授权说明

DoRuan.Report 为非商业用途免费使用（输出带“逗软卓越报表”水印），商业用途需购买授权去除水印，授权按开发机数量计价、费用很低（几杯奶茶钱）。详见 [doruan.net](https://doruan.net) 或组件文档。

---

## English

A minimal Avalonia sample for [DoRuan.Report](https://www.nuget.org/packages/DoRuan.Report.Designer.Avalonia): load a JSON report template, bind demo data (including grouped details and subreport tables), preview it, export PDF and open the built-in visual designer.

Requires the .NET 10 SDK. Run with `dotnet restore && dotnet run`. The component is free for non-commercial use with a watermark; a low-cost commercial license (priced per developer machine) removes it — see [doruan.net](https://doruan.net).
