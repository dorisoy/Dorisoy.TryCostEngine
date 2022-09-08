using Microsoft.AspNetCore.Components;
using TryCostEngine.MAUI.Services;
namespace TryCostEngine.MAUI.Shared;

public partial class DocsLayout : LayoutComponentBase
{
    [Inject] private LayoutService LayoutService { get; set; }

    private NavMenu _navMenuRef;
    private bool _drawerOpen = true;
    private bool _topMenuOpen = false;
    protected override void OnInitialized()
    {
    }
    
    protected override void OnAfterRender(bool firstRender)
    {
        _navMenuRef?.Refresh();
    }

    private void ToggleDrawer()
    {
        _drawerOpen = !_drawerOpen;
    }

    private void OpenTopMenu()
    {
        _topMenuOpen = true;
    }

    private void OnDrawerOpenChanged(bool value)
    {
        _topMenuOpen = false;
        _drawerOpen = value;
        StateHasChanged();
    }

}
