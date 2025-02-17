﻿// Copyright (c) MudBlazor 2021
// MudBlazor licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using TryCostEngine.MAUI.Extensions;
using TryCostEngine.MAUI.Models;
using TryCostEngine.MAUI.Services;
using TryCostEngine.MAUI.Services.Notifications;

namespace TryCostEngine.MAUI.Shared;

public partial class AppbarButtons
{
    [Inject] private INotificationService NotificationService { get; set; }
    [Inject] private LayoutService LayoutService { get; set; }
    private IDictionary<NotificationMessage, bool> _messages = null;
    private bool _newNotificationsAvailable = false;

    private async Task MarkNotificationAsRead()
    {
        await NotificationService.MarkNotificationsAsRead();
        _newNotificationsAvailable = false;
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender == true)
        {
            _newNotificationsAvailable = await NotificationService.AreNewNotificationsAvailable();
            _messages = await NotificationService.GetNotifications();
            StateHasChanged();
        }

        await base.OnAfterRenderAsync(firstRender);
    }

}
