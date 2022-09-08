using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Reflection;

using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.Compatibility;
using Microsoft.Maui.Controls.Xaml;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.JSInterop;
using Microsoft.AspNetCore.Components.WebView.Maui;
using Microsoft.Maui.LifecycleEvents;

using TryCostEngine.MAUI.Data;
using TryCostEngine.MAUI.Services;
using TryCostEngine.MAUI.Models;
using TryCostEngine.Core;
using TryCostEngine;
using TryCostEngine.MAUI.Extensions;
using TryCostEngine.MAUI.Services.Notifications;
using TryCostEngine.MAUI.Services.UserPreferences;
using Blazored.LocalStorage;
using MudBlazor.Services;


#if WINDOWS

using Microsoft.UI;
using Microsoft.UI.Windowing;
using Windows.Graphics;

#elif MACCATALYST

#elif ANDROID

#endif

namespace TryCostEngine.MAUI;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();

        builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
            });

        builder.Services.AddMauiBlazorWebView();

        builder.Services.AddMudServices();

#if DEBUG
        builder.Services.AddBlazorWebViewDeveloperTools();
#endif

#if WINDOWS

        //Unable to cast object of type 'Microsoft.AspNetCore.Components.WebView.Services.WebViewJSRuntime' to type 'Microsoft.JSInterop.IJSInProcessRuntime'.”

        //builder.Services.AddSingleton(serviceProvider => (IJSInProcessRuntime)serviceProvider.GetRequiredService<IJSRuntime>());

        //builder.Services.AddSingleton(serviceProvider => (IJSUnmarshalledRuntime)serviceProvider.GetRequiredService<IJSRuntime>());

        //builder.Services.AddScoped(_ => new HttpClient() });

        //片段处理
        builder.Services.AddScoped<SnippetsService>();
        //编译服务
        builder.Services.AddSingleton(new CompilationService());
        //Codemirror
        //builder.Services.AddTransient<Codemirror>();
        //片段配置
        builder.Services
            .AddOptions<SnippetsOptions>()
            .Configure<IConfiguration>((options, configuration) => configuration.GetSection("Snippets").Bind(options));

        builder.Logging.Services.AddSingleton<ILoggerProvider, HandleCriticalUserComponentExceptionsLoggerProvider>();

        builder.Services.AddMudServices();
        builder.Services.AddBlazoredLocalStorage();
        builder.Services.AddScoped<IUserPreferencesService, UserPreferencesService>();
        builder.Services.AddScoped<LayoutService>();

        // load user-defined services
        ExecuteUserDefinedConfiguration(builder);

        //==================================================

        //注册视图服务
        builder.Services.TryAddDocsViewServices();
        builder.Services.AddSingleton<IRenderQueueService>(new RenderQueueService { Capacity = int.MaxValue });
        builder.Services.AddTransient<IFolderPicker, TryCostEngine.MAUI.Windows.FolderPicker>();

        builder.ConfigureLifecycleEvents(events =>
        {
            events.AddWindows(wndLifeCycleBuilder =>
            {
                wndLifeCycleBuilder.OnWindowCreated(window =>
                {
                    window.ExtendsContentIntoTitleBar = true; 
                    window.SetTitleBar(new CustomTitleBar());
                    IntPtr nativeWindowHandle = WinRT.Interop.WindowNative.GetWindowHandle(window);
                    WindowId win32WindowsId = Win32Interop.GetWindowIdFromWindow(nativeWindowHandle);
                    AppWindow winuiAppWindow = AppWindow.GetFromWindowId(win32WindowsId);    
                    if(winuiAppWindow.Presenter is OverlappedPresenter p)
                    { 
                       //初始最大化
                       p.Maximize();
                       //p.IsAlwaysOnTop=true;
                       //初始最小化
                       //p.Minimize();
                       p.IsResizable=false;
                       p.IsMaximizable = false;
                       p.IsMinimizable=false;
                       //p.SetBorderAndTitleBar(false, false);
                    }                     
                    else
                    {
                        const int width = 1200;
                        const int height = 800;
                        winuiAppWindow.MoveAndResize(new RectInt32(1920 / 2 - width / 2, 1080 / 2 - height / 2, width, height));                      
                    }                        
                });
            });
        });

#elif MACCATALYST
		builder.Services.AddTransient<IFolderPicker, TryCostEngine.MAUI.MacCatalyst.FolderPicker>();
#endif

        var app = builder.Build();

        //System.InvalidOperationException:“Unable to resolve service for type 'Microsoft.JSInterop.IJSUnmarshalledRuntime' while attempting to activate 'TryCostEngine.MAUI.Services.HandleCriticalUserComponentExceptionsLoggerProvider'.”

#if WINDOWS
        var notificationService = app.Services.GetService<INotificationService>();
        if (notificationService is InMemoryNotificationService inmemoryService)
        {
            inmemoryService.Preload();
        }
#endif
        return app;
    }

    private static void ExecuteUserDefinedConfiguration(MauiAppBuilder builder)
    {
        var userComponentsAssembly = typeof(_Imports).Assembly;
        var startupType = userComponentsAssembly.GetType("UserStartup", throwOnError: false, ignoreCase: true)
                          ?? userComponentsAssembly.GetType("TryCostEngine.UserStartup", throwOnError: false, ignoreCase: true);
        if (startupType == null)
            return;
        var configureMethod = startupType.GetMethod("Configure", BindingFlags.Static | BindingFlags.Public);
        if (configureMethod == null)
            return;
        var configureMethodParams = configureMethod.GetParameters();
        if (configureMethodParams.Length != 1 || configureMethodParams[0].ParameterType != typeof(MauiAppBuilder))
            return;
        configureMethod.Invoke(obj: null, new object[] { builder });
    }
}
