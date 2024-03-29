﻿using L4D2Toolbox.Data;
using L4D2Toolbox.Helper;
using L4D2Toolbox.Steam;
using L4D2Toolbox.Utils;

namespace L4D2Toolbox;

/// <summary>
/// MainWindow.xaml 的交互逻辑
/// </summary>
public partial class MainWindow
{
    /// <summary>
    /// 导航菜单
    /// </summary>
    public List<NavMenu> NavMenus { get; set; } = new();
    /// <summary>
    /// 导航字典
    /// </summary>
    private readonly Dictionary<string, UserControl> NavDictionary = new();

    /// 主窗口关闭委托
    /// </summary>
    public delegate void WindowClosingDelegate();
    /// <summary>
    /// 主窗口关闭事件
    /// </summary>
    public static event WindowClosingDelegate WindowClosingEvent;

    /// <summary>
    /// 向外暴露主窗口实例
    /// </summary>
    public static Window MainWindowInstance { get; private set; }

    ///////////////////////////////////////////////////////

    public MainWindow()
    {
        InitializeComponent();
        this.DataContext = this;

        MainWindowInstance = this;

        CreateNavMenu();
        Navigate(NavDictionary.First().Key);
    }

    private void Window_Main_Loaded(object sender, RoutedEventArgs e)
    {
        Task.Run(async () =>
        {
            var version = await HttpHelper.DownloadString("https://raw.githubusercontent.com/CrazyZhang666/L4D2Toolbox/main/version.txt");
            if (Version.TryParse(version, out Version result))
            {
                if (result != MiscUtil.VersionInfo)
                {
                    this.Dispatcher.Invoke(() =>
                    {
                        Hyperlink_AppUpdate.Inlines.Add($"发现新版本 v{result}");
                        Hyperlink_AppUpdate.NavigateUri = new Uri("https://github.com/CrazyZhang666/L4D2Toolbox/releases");
                        NotifierHelper.Show(NotifierType.Notification, "发现新版本，请前往GitHub下载");
                    });
                }
            }
        });
    }

    private void Window_Main_Closing(object sender, CancelEventArgs e)
    {
        WindowClosingEvent();
        Workshop.ShutDown();
    }

    /// <summary>
    /// 创建导航菜单
    /// </summary>
    private void CreateNavMenu()
    {
        NavMenus.Add(new() { Icon = "\xe61f", Title = "首页", ViewName = "HomeView" });
        NavMenus.Add(new() { Icon = "\xe738", Title = "MOD信息模板", ViewName = "AddonInfoView" });
        NavMenus.Add(new() { Icon = "\xe899", Title = "管理创意工坊", ViewName = "WorkshopView" });
        NavMenus.Add(new() { Icon = "\xe77e", Title = "Steam云存储", ViewName = "StorageView" });
        NavMenus.Add(new() { Icon = "\xe64e", Title = "订阅管理", ViewName = "SubscribeView" });
        NavMenus.Add(new() { Icon = "\xe783", Title = "MOD管理", ViewName = "AddonsView" });
        NavMenus.Add(new() { Icon = "\xe704", Title = "自定中文字体", ViewName = "GameFontView" });
        NavMenus.Add(new() { Icon = "\xe606", Title = "常用工具", ViewName = "ToolkitView" });
        NavMenus.Add(new() { Icon = "\xe612", Title = "MDL重编译", ViewName = "ReCompileView" });
        NavMenus.Add(new() { Icon = "\xe603", Title = "关于", ViewName = "AboutView" });

        NavMenus.ForEach(menu =>
        {
            var type = Type.GetType($"L4D2Toolbox.Views.{menu.ViewName}");
            NavDictionary.Add(menu.ViewName, Activator.CreateInstance(type) as UserControl);
        });
    }

    /// <summary>
    /// 页面导航
    /// </summary>
    /// <param name="menu"></param>
    [RelayCommand]
    private void Navigate(string viewName)
    {
        if (NavDictionary.ContainsKey(viewName))
        {
            ContentControl_NavRegion.Content = NavDictionary[viewName];
        }
    }

    /// <summary>
    /// 超链接请求导航事件
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void Hyperlink_RequestNavigate(object sender, RequestNavigateEventArgs e)
    {
        ProcessUtil.OpenLink(e.Uri.OriginalString);
        e.Handled = true;
    }
}
