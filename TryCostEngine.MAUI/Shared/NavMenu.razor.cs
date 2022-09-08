using System.Linq;
using TryCostEngine.MAUI.Models;
using Microsoft.AspNetCore.Components;
using TryCostEngine.MAUI.Services;
using TryCostEngine.MAUI.Extensions;


namespace TryCostEngine.MAUI.Shared
{
    public partial class NavMenu
    {
        [Inject] IMenuService MenuService { get; set; } 
        string _section;
        string _componentLink;

        protected override void OnInitialized()
        {
            Refresh();
            base.OnInitialized();
        }

        public void Refresh()
        {
            _section = NavigationManager.GetSection();
            _componentLink = NavigationManager.GetComponentLink();
            StateHasChanged();
        }

        bool IsSubGroupExpanded(MudComponent item)
        {
            #region comment about is subgroup expanded
            //if the route contains any of the links of the subgroup, then the subgroup
            //should be expanded
            //Example:
            //subgroup: form inputs & controls
            //the subgroup "form inputs & controls" should be open if the current page has in the route
            //a component included in the subgroup elements, that in this case are autocomplete, form, field,
            //radio, select...
            //this route `/components/autocomplete` should open the subgroup "form inputs..."
            #endregion
            return item.GroupComponents.Any(i => i.Link == _componentLink);
        }
    }
}
