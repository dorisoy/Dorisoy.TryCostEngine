﻿using Blazor.Analytics;
using Blazored.LocalStorage;
using CostEngine.Data;
using Microsoft.Extensions.DependencyInjection;
using MudBlazor;
using MudBlazor.Services;
using TryCostEngine.MAUI.Services;
using TryCostEngine.MAUI.Services.Notifications;
using TryCostEngine.MAUI.Services.UserPreferences;


namespace TryCostEngine.MAUI.Extensions
{
    public static class DocsViewExtension
    {
        public static void TryAddDocsViewServices(this IServiceCollection services)
        {
            services.AddMudServices(config =>
            {
                config.SnackbarConfiguration.PositionClass = Defaults.Classes.Position.BottomLeft;
                config.SnackbarConfiguration.PreventDuplicates = false;
                config.SnackbarConfiguration.NewestOnTop = false;
                config.SnackbarConfiguration.ShowCloseIcon = true;
                config.SnackbarConfiguration.VisibleStateDuration = 10000;
                config.SnackbarConfiguration.HideTransitionDuration = 500;
                config.SnackbarConfiguration.ShowTransitionDuration = 500;
                config.SnackbarConfiguration.SnackbarVariant = Variant.Filled;
            });

            services.AddScoped<GitHubApiClient>();
            services.AddSingleton<IApiLinkService, ApiLinkService>();
            services.AddSingleton<IMenuService, MenuService>();
            services.AddScoped<IDocsNavigationService, DocsNavigationService>();
            services.AddBlazoredLocalStorage();
            services.AddScoped<IUserPreferencesService, UserPreferencesService>();
            services.AddScoped<INotificationService, InMemoryNotificationService>();
            services.AddScoped<IPeriodicTableService, PeriodicTableService>();
            services.AddSingleton<IRenderQueueService, RenderQueueService>();

            //Service
            services.AddScoped<ITPCalcService, TPCalcService>();
            services.AddScoped<ISPCalcService, SPCalcService>();

            services.AddScoped<LayoutService>();
            services.AddGoogleAnalytics("G-PRYNCB61NV");

        }
    }
}
