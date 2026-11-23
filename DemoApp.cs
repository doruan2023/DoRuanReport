using System.Diagnostics;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Styling;
using Avalonia.Themes.Fluent;
using Avalonia.Threading;

namespace DoRuan.Report.Demo;

public sealed class DemoApp : Application
{
    public override void Initialize()
    {
        // 界面为浅色硬编码配色，不跟随系统深色模式，否则文字与边框会看不清。
        RequestedThemeVariant = ThemeVariant.Light;
        Styles.Add(new FluentTheme());
    }

    public override void OnFrameworkInitializationCompleted()
    {
        // 宿主级最后防线：把未处理异常记入 Trace，避免界面上偶发错误直接终止进程。
        // OOM/StackOverflowException 等不可恢复错误仍会退出，这是 .NET 的限制。
        Dispatcher.UIThread.UnhandledException += (_, e) =>
        {
            Trace.WriteLine("[DoRuan.Report.Demo] 未处理异常：" + e.Exception);
            e.Handled = true;
        };
        TaskScheduler.UnobservedTaskException += (_, e) =>
        {
            e.SetObserved();
            Trace.WriteLine("[DoRuan.Report.Demo] 未观察的任务异常：" + e.Exception);
        };
        AppDomain.CurrentDomain.UnhandledException += (_, e) =>
            Trace.WriteLine("[DoRuan.Report.Demo] 非 UI 线程未处理异常：" + e.ExceptionObject);

        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            desktop.MainWindow = new MainWindow();

        base.OnFrameworkInitializationCompleted();
    }
}
