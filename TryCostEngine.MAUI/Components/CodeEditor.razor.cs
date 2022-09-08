namespace TryCostEngine.MAUI.Components
{
    using System;
    using System.Threading.Tasks;
    using TryCostEngine.Core;
    using Microsoft.AspNetCore.Components;
    using Microsoft.JSInterop;
    using System.Text.RegularExpressions;

    public partial class CodeEditor : IDisposable
    {
        private bool hasCodeChanged;

        //[Inject]
        //public IJSInProcessRuntime JsRuntime { get; set; }

        [Parameter]
        public string Code { get; set; }

        [Parameter]
        public string EditorId { get; set; } = "user-code-editor";

        [Parameter]
        public string Height { get; set; } = "100%";

        [Parameter]
        public CodeFileType CodeFileType { get; set; }

        public override Task SetParametersAsync(ParameterView parameters)
        {
            if (parameters.TryGetValue<string>(nameof(this.Code), out var parameterValue))
            {
                this.hasCodeChanged = this.Code != parameterValue;
            }
            return base.SetParametersAsync(parameters);
        }

        public void Dispose() { }// => this.JsRuntime.InvokeVoid("App.CodeEditor.dispose");

        internal void Focus() { }// => this.JsRuntime.InvokeVoid("App.CodeEditor.focus");

        internal string GetCode() { return "";  }// => this.JsRuntime.Invoke<string>("App.CodeEditor.getValue");

        protected override void OnAfterRender(bool firstRender)
        {
            var newCode = this.Code;
            if (newCode != null)
            {
                this.Code = Regex.Replace(newCode, @"@code\s*\r?\n\s*{", "@code {");
            }

            if (firstRender)
            {
                //this.JsRuntime.InvokeVoid(
                //   "App.CodeEditor.init",
                //   EditorId,
                //   this.Code ?? CoreConstants.MainComponentDefaultFileContent);
            }
            else if (this.hasCodeChanged)
            {
                var language = "csharp";
                switch (this.CodeFileType)
                {
                    case CodeFileType.CSharp:
                        language = "csharp";
                        break;
                    case CodeFileType.Razor:
                        language = "razor";
                        break;
                    case CodeFileType.Json:
                        language = "json";
                        break;
                    case CodeFileType.XML:
                        language = "xml";
                        break;
                }

               // this.JsRuntime.InvokeVoid("App.CodeEditor.setValue", this.Code, language);
            }

            base.OnAfterRender(firstRender);
        }
    }
}
