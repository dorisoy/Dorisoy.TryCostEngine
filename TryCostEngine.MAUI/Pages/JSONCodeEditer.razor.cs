namespace TryCostEngine.MAUI.Pages
{
    using System;
    using System.Threading.Tasks;
    using TryCostEngine.MAUI.Services;
    using TryCostEngine.Core;
    using Microsoft.AspNetCore.Components;
    using Microsoft.JSInterop;

    public partial class JSONCodeEditer : IDisposable
    {
        private DotNetObjectReference<JSONCodeEditer> dotNetInstance;

        //[Inject]
        //public IJSInProcessRuntime JsRuntime { get; set; }


        [Parameter]
        public bool ShowTools { get; set; } = true;

        [Parameter]
        public string Height { get; set; } = "100%";

        [Parameter]
        public string LayoutHeight { get; set; } = "100%";


        [Parameter]
        public string Language { get; set; }

        [Parameter]
        public string EditorId { get; set; } = "codemirror-editor-json";

        public ElementReference CodeEditorComponent { get; set; }

        [Parameter]
        public ParameterModel ParameterModel { get; set; }

        public void Dispose()
        {
            this.dotNetInstance?.Dispose();
            //this.JsRuntime.InvokeVoid("App.JsonCodemirror.dispose");
        }

        protected override Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                this.dotNetInstance = DotNetObjectReference.Create(this);

                //Console.WriteLine("OnAfterRender");
                //Console.WriteLine(this.ParameterModel.JsonData);

                //this.JsRuntime.InvokeVoid(
                //    "App.JsonCodemirror.init",
                //    this.EditorId,
                //    this.ParameterModel == null ? "null" : this.ParameterModel.JsonData,
                //    "javascript");

                StateHasChanged();
            }

            return base.OnAfterRenderAsync(firstRender);
        }

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            //Console.WriteLine("OnAfterRender");
            //Console.WriteLine(this.ParameterModel.JsonData);
        }
    }
}
