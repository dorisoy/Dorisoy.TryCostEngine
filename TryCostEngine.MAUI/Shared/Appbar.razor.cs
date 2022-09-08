using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Routing;
using Microsoft.AspNetCore.Components.Web;
using TryCostEngine.MAUI.Extensions;
using TryCostEngine.MAUI.Models;
using TryCostEngine.MAUI.Services;
using MudBlazor;
namespace TryCostEngine.MAUI.Shared;

public partial class Appbar
{
    [Parameter] public EventCallback<MouseEventArgs> DrawerToggleCallback { get; set; }
    [Parameter] public bool DisplaySearchBar { get; set; }
    [Inject] private IApiLinkService ApiLinkService { get; set; }
    [Inject] private LayoutService LayoutService { get; set; }

    MudAutocomplete<ApiLinkServiceEntry> _searchAutocomplete;

    private bool _drawerOpen = true;
    private bool _searchDialogOpen;

    private void OpenSearchDialog() => _searchDialogOpen = true;
    private DialogOptions _dialogOptions = new()
    {
        FullWidth = true,
        Position = DialogPosition.TopCenter, 
        NoHeader = true
    };
    
    private async void OnSearchResult(ApiLinkServiceEntry entry)
    {
        NavigationManager.NavigateTo(entry.Link);
        await Task.Delay(1000);
        await _searchAutocomplete.Clear();
    }

    private void ToggleDrawer()
    {
        _drawerOpen = !_drawerOpen;
    }

    private Task<IEnumerable<ApiLinkServiceEntry>> Search(string text)
    {
        if (string.IsNullOrWhiteSpace(text))
        {
            return Task.FromResult<IEnumerable<ApiLinkServiceEntry>>(null);
        }
        return ApiLinkService.Search(text);
    }
}
