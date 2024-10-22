# Dorisoy.TryCostEngine
.netcore 6 下 使用 Blazor + MAUI 构建桌面版的销售费用计算模拟器实例


## Blazor在MAUI下几个问题：

1. 新手入门实战 ：https://www.codemag.com/Article/2111092/Blazor-Hybrid-Web-Apps-with-.NET-MAUI

2. 如何浏览器调试：https://docs.microsoft.com/en-us/aspnet/core/blazor/hybrid/developer-tools?view=aspnetcore-6.0&pivots=windows

3. 如何执行JS， 抱歉：（IJSInProcessRuntime,IJSUnmarshalledRuntime）它们不被支持：https://github.com/dotnet/maui/issues/5726
https://github.com/dotnet/maui/issues/5804

4. 如何读取本地静态资源：https://docs.microsoft.com/en-us/aspnet/core/blazor/hybrid/static-files?view=aspnetcore-6.0

5.如如何访问本地存储文件：https://blog.verslu.is/maui/folder-picker-with-dotnet-maui/

6. 如何在应用程序中集成身份验证:https://docs.microsoft.com/en-us/aspnet/core/blazor/hybrid/security/?view=aspnetcore-6.0&pivots=maui

7.关于HttpClient 的最佳实践是——不要在 MAUI 中使用 builder.Services.AddHttpClient，请使用AddSingleton 创建实例。

## 屏幕

<img src="https://github.com/dorisoy/Dorisoy.TryCostEngine/blob/main/Scn%20(5).png?raw=true"/>

<img src="https://github.com/dorisoy/Dorisoy.TryCostEngine/blob/main/Scn%20(1).png?raw=true"/>

<img src="https://github.com/dorisoy/Dorisoy.TryCostEngine/blob/main/Scn%20(2).png?raw=true"/>

<img src="https://github.com/dorisoy/Dorisoy.TryCostEngine/blob/main/Scn%20(3).png?raw=true"/>

<img src="https://github.com/dorisoy/Dorisoy.TryCostEngine/blob/main/Scn%20(4).png?raw=true"/>


