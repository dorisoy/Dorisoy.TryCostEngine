using MudBlazor;
using System;
using Microsoft.AspNetCore.Components;
using TryCostEngine.MAUI.Models;
using MudBlazor.Utilities;

namespace TryCostEngine.MAUI.Components;

public partial class TPHeader
{
    [Parameter] public string Title { get; set; }
  

    protected override void OnInitialized()
    {
        base.OnInitialized();
    }

    protected override void OnAfterRender(bool firstRender)
    {
        base.OnAfterRender(firstRender);
     }
}
