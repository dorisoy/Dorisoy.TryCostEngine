namespace TryCostEngine.MAUI.Pages
{
    using System;
    using System.Threading.Tasks;
    using TryCostEngine.MAUI.Services;
    using TryCostEngine.Core;
    using Microsoft.AspNetCore.Components;
    using Microsoft.JSInterop;

    public partial class TPMCodeEditer : IDisposable
    {
        private DotNetObjectReference<TPMCodeEditer> dotNetInstance;


        //[Inject]
        //public IJSInProcessRuntime JsRuntime { get; set; }

        [Parameter]
        public bool ShowTools { get; set; } = true;

        [Parameter]
        public string Height { get; set; } = "100%";

        [Parameter]
        public string LayoutHeight { get; set; } = "100%";

        [Parameter]
        public string Code { get; set; }

        [Parameter]
        public string Language { get; set; }

        [Parameter]
        public string EditorId { get; set; } = "codemirror-editor-tpm";

        [CascadingParameter(Name = "ParameterModel")]
        public ParameterModel ParameterModel { get; set; }

        public ElementReference CodeEditorComponent { get; set; }


        public void Dispose()
        {
            //Snackbar.Add("App.Codemirror.dispose", MudBlazor.Severity.Info);
            this.dotNetInstance?.Dispose();
            //this.JsRuntime.InvokeVoid("App.Codemirror.dispose");
        }

        protected override void OnAfterRender(bool firstRender)
        {
            if (firstRender)
            {
                this.dotNetInstance = DotNetObjectReference.Create(this);

                //this.JsRuntime.InvokeVoid(
                //    "App.Codemirror.init",
                //    this.EditorId,
                //    this.ParameterModel.TpmLangData,
                //    this.Language);

                //if (Language == "tpm")
                //{
                //    this.JsRuntime.InvokeVoid("App.Codemirror.setValue", this.ParameterModel.TpmLangData);
                //}
                //else if (Language == "json")
                //{
                //    this.JsRuntime.InvokeVoid("App.Codemirror.setValue", this.ParameterModel.JsonData);
                //}

                //Console.WriteLine($"App.Codemirror.init:{Language} - {this.EditorId}");
                //Snackbar.Add(Language, MudBlazor.Severity.Info);
                StateHasChanged();
            }

            base.OnAfterRender(firstRender);
        }

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
        }
    }
}
