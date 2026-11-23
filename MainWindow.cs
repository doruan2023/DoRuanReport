using Avalonia;
using Avalonia.Controls;
using Avalonia.Layout;
using Avalonia.Media;
using Avalonia.Platform;
using DoRuan.Report.Avalonia;
using DoRuan.Report.Designer.Avalonia;
using DoRuan.Report.Model;
using DoRuan.Report.Rendering.Skia;
using DoRuan.Report.Serialization;

namespace DoRuan.Report.Demo;

public sealed class MainWindow : Window
{
    private readonly ReportPreview _preview = new()
    {
        HorizontalAlignment = HorizontalAlignment.Stretch,
        VerticalAlignment = VerticalAlignment.Stretch
    };

    private readonly TextBlock _status = new()
    {
        Text = "准备就绪",
        VerticalAlignment = VerticalAlignment.Center,
        Margin = new Thickness(20, 0, 0, 0),
        Foreground = new SolidColorBrush(Color.Parse("#4B5563")),
        TextTrimming = TextTrimming.CharacterEllipsis
    };

    private readonly TextBlock _zoomText = new()
    {
        Text = "100%",
        Width = 52,
        TextAlignment = TextAlignment.Center,
        VerticalAlignment = VerticalAlignment.Center
    };

    public MainWindow()
    {
        Title = "DoRuan 报表引擎 - 采购订单演示";
        try
        {
            using var icon = AssetLoader.Open(new Uri("avares://DoRuan.Report.Demo/Assets/app-icon.png"));
            Icon = new WindowIcon(icon);
        }
        catch (Exception ex)
        {
            System.Diagnostics.Trace.WriteLine("[DoRuan.Report.Demo] 图标加载失败：" + ex.Message);
        }
        Width = 1120;
        Height = 920;
        WindowStartupLocation = WindowStartupLocation.CenterScreen;

        var toolbar = BuildToolbar();
        DockPanel.SetDock(toolbar, Dock.Top);

        var root = new DockPanel();
        root.Children.Add(toolbar);
        root.Children.Add(_preview);
        Content = root;

        LoadReport();
    }

    private Control BuildToolbar()
    {
        var panel = new StackPanel
        {
            Orientation = Orientation.Horizontal,
            Spacing = 6,
            VerticalAlignment = VerticalAlignment.Center
        };

        panel.Children.Add(new TextBlock
        {
            Text = "DoRuan Report",
            FontSize = 16,
            FontWeight = FontWeight.SemiBold,
            VerticalAlignment = VerticalAlignment.Center,
            Margin = new Thickness(0, 0, 18, 0)
        });

        panel.Children.Add(CreateButton("刷新", LoadReport));
        panel.Children.Add(CreateButton("导出 PDF", ExportPdf));
        panel.Children.Add(CreateButton("设计器", OpenDesigner));
        panel.Children.Add(CreateButton("-", () => ChangeZoom(1 / 1.2)));
        panel.Children.Add(_zoomText);
        panel.Children.Add(CreateButton("+", () => ChangeZoom(1.2)));
        panel.Children.Add(CreateButton("100%", ResetZoom));
        panel.Children.Add(_status);

        return new Border
        {
            Background = new SolidColorBrush(Color.Parse("#F3F4F6")),
            BorderBrush = new SolidColorBrush(Color.Parse("#D1D5DB")),
            BorderThickness = new Thickness(0, 0, 0, 1),
            Padding = new Thickness(12, 8),
            Child = panel
        };
    }

    private static Button CreateButton(string text, Action onClick)
    {
        var button = new Button
        {
            Content = text,
            Padding = new Thickness(14, 6),
            MinWidth = 44,
            HorizontalContentAlignment = HorizontalAlignment.Center
        };
        button.Click += (_, _) => onClick();
        return button;
    }

    private void LoadReport()
    {
        try
        {
            var templatePath = Path.Combine(AppContext.BaseDirectory, "Templates", "purchase-order.json");
            if (!File.Exists(templatePath))
                throw new FileNotFoundException($"找不到模板文件：{templatePath}");

            var template = ReportTemplateSerializer.Load(templatePath);
            var data = DemoData.CreatePurchaseOrder();

            using var renderer = new SkiaReportRenderer();
            ReportDocument document = renderer.CreateEngine().Build(template, data);

            _preview.Document = document;
            _status.Text = $"已生成 {document.Pages.Count} 页，数据 {data.RowCount("明细数据")} 行。";
            _status.Foreground = new SolidColorBrush(Color.Parse("#4B5563"));
        }
        catch (Exception ex)
        {
            _preview.Document = null;
            _status.Text = "生成失败：" + ex.Message;
            _status.Foreground = Brushes.OrangeRed;
        }
    }

    private void OpenDesigner()
    {
        try
        {
            var templatePath = Path.Combine(AppContext.BaseDirectory, "Templates", "purchase-order.json");
            var template = File.Exists(templatePath)
                ? ReportTemplateSerializer.Load(templatePath)
                : DesignerTemplateFactory.CreateBlank("明细数据");

            var window = new ReportDesignerWindow(template, DemoData.CreatePurchaseOrder());
            window.Show(this);
            _status.Text = "已打开报表设计器。";
            _status.Foreground = new SolidColorBrush(Color.Parse("#4B5563"));
        }
        catch (Exception ex)
        {
            _status.Text = "打开设计器失败：" + ex.Message;
            _status.Foreground = Brushes.OrangeRed;
        }
    }

    private void ExportPdf()
    {
        try
        {
            if (_preview.Document is null)
            {
                _status.Text = "没有可导出的报表。";
                return;
            }

            var folder = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            if (string.IsNullOrWhiteSpace(folder) || !Directory.Exists(folder))
                folder = AppContext.BaseDirectory;

            var path = Path.Combine(folder, $"采购订单_{DateTime.Now:yyyyMMdd_HHmmss}.pdf");
            _preview.SavePdf(path);
            var sizeMb = new FileInfo(path).Length / 1024d / 1024d;
            _status.Text = $"已导出：{path}（{sizeMb:0.0} MB）";
            _status.Foreground = new SolidColorBrush(Color.Parse("#047857"));
        }
        catch (Exception ex)
        {
            _status.Text = "导出失败：" + ex.Message;
            _status.Foreground = Brushes.OrangeRed;
        }
    }

    private void ChangeZoom(double factor)
    {
        var zoom = Math.Clamp(_preview.Zoom * factor, 0.25, 4.0);
        _preview.Zoom = zoom;
        _zoomText.Text = $"{zoom * 100:0}%";
    }

    private void ResetZoom()
    {
        _preview.Zoom = 1.0;
        _zoomText.Text = "100%";
    }
}
