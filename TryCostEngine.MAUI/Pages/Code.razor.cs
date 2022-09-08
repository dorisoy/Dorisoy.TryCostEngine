namespace TryCostEngine.MAUI.Pages
{
    using System;
    using System.Web;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;
    using TryCostEngine.MAUI.Components;
    using TryCostEngine.MAUI.Services;
    using TryCostEngine.Core;
    using Microsoft.AspNetCore.Components;
    using Microsoft.CodeAnalysis;
    using Microsoft.JSInterop;
    using MudBlazor;
    using Microsoft.Extensions.FileProviders;
    using TryCostEngine.MAUI.Models;

    public partial class Code : IDisposable
    {
        [Inject] private LayoutService LayoutService { get; set; }
        
        private const string MainComponentCodePrefix = "@page \"/__data\"\n";
        private const string MainUserPagePath = "/__data";

        private DotNetObjectReference<Code> dotNetInstance;
        private string errorMessage;
        private CodeFile activeCodeFile;

        //[Inject]
        //public ISnackbar Snackbar { get; set; }

        [Inject]
        public SnippetsService SnippetsService { get; set; }

        [Inject]
        public CompilationService CompilationService { get; set; }

        ///// <summary>
        ///// 表示可以向其分派调用的JavaScript运行时的实例。
        ///// </summary>
        //[Inject]
        //public IJSInProcessRuntime JsRuntime { get; set; }

        [Parameter]
        public bool ShowTools { get; set; } = true;

        [Parameter]
        public string SnippetId { get; set; }

        [Parameter]
        public string SnippetType { get; set; }

        [Parameter]
        public string Height { get; set; } = "100%";

        [Parameter]
        public string LayoutHeight { get; set; } = "100%";

        [Parameter]
        public string Data { get; set; }

        [Parameter]
        public string EditorId { get; set; }


        public CodeEditor CodeEditorComponent { get; set; }


        public IDictionary<string, CodeFile> CodeFiles { get; set; } = new Dictionary<string, CodeFile>();

        private IList<string> CodeFileNames { get; set; } = new List<string>();

        private string CodeEditorContent => this.activeCodeFile?.Content;

        private CodeFileType CodeFileType => this.activeCodeFile?.Type ?? CodeFileType.XML;

        private bool SaveSnippetPopupVisible { get; set; }

        private IReadOnlyCollection<CompilationDiagnostic> Diagnostics { get; set; } = Array.Empty<CompilationDiagnostic>();

        private int ErrorsCount => this.Diagnostics.Count(d => d.Severity == DiagnosticSeverity.Error);

        private int WarningsCount => this.Diagnostics.Count(d => d.Severity == DiagnosticSeverity.Warning);

        private bool AreDiagnosticsShown { get; set; }

        private string LoaderText { get; set; }

        private bool Loading { get; set; }

        private bool ShowDiagnostics { get; set; }

        private void ToggleDiagnostics()
        {
            ShowDiagnostics = !ShowDiagnostics;
            AreDiagnosticsShown = ShowDiagnostics;
        }

        private string Version
        {
            get
            {
                var v = typeof(MudText).Assembly.GetName().Version;
                return $"v1.0.{v.Build}";
            }
        }

        protected override void OnParametersSet()
        {
            //Console.WriteLine($"{this.SnippetType}");
        }


        [JSInvokable]
        public async Task TriggerCompileAsync()
        {
            await this.CompileAsync();

            this.StateHasChanged();
        }

        public void Dispose()
        {
            this.dotNetInstance?.Dispose();
           // this.JsRuntime.InvokeVoid("App.Code.dispose");
        }

        protected override void OnAfterRender(bool firstRender)
        {
            if (firstRender)
            {
                this.dotNetInstance = DotNetObjectReference.Create(this);
       
                //this.JsRuntime.InvokeVoid(
                //    "App.Code.init",
                //    "code-user-code-editor-container",
                //    "user-page-window-container",
                //    "code-user-code-editor",
                //    this.dotNetInstance,
                //    false);
            }

            if (!string.IsNullOrWhiteSpace(this.errorMessage))
            {
                Snackbar.Add(this.errorMessage, Severity.Error);
                this.errorMessage = null;
            }

            base.OnAfterRender(firstRender);
        }

        protected override async Task OnInitializedAsync()
        {
            Snackbar.Clear();

            //根据片段Id获取片段
            if (!string.IsNullOrWhiteSpace(this.SnippetId))
            {
                try
                {
                    this.CodeFiles = (await this.SnippetsService.GetSnippetContentAsync(this.SnippetId)).ToDictionary(f => f.Path, f => f);
                    if (!this.CodeFiles.Any())
                    {
                        this.errorMessage = "No files in snippet.";
                    }
                    else
                    {
                        this.activeCodeFile = this.CodeFiles.First().Value;
                    }
                }
                catch (ArgumentException)
                {
                    this.errorMessage = "Invalid Snippet ID.";
                }
                catch (Exception)
                {
                    this.errorMessage = "Unable to get snippet content. Please try again later.";
                }
            }

            //'/tp/protocolbasefydlfzh'
            var ap = new Uri(NavigationManager.Uri).AbsolutePath;
            var query = new Uri(NavigationManager.Uri).Query;
            this.SnippetType = HttpUtility.ParseQueryString(query).Get("snippettype");

            //获取示例
            if (!this.CodeFiles.Any())
            {
                if (!string.IsNullOrEmpty(this.SnippetType))
                {
                    var content = "";
                   var embeddedFileProvider = new EmbeddedFileProvider(this.GetType().Assembly, this.GetType().Namespace);
                    // 由于内嵌文件资源不存在目录结果，只有使用空字符串和/才能获取到文件，其他情况都将返回NotFoundDirectoryContents
                    var files = embeddedFileProvider.GetDirectoryContents(string.Empty);
                    if (files.Exists)
                    {
                        using var fls = files.GetEnumerator();
                        while (fls.MoveNext())
                        {
                            if (fls.Current.Name.ToLower() == $"models.{this.SnippetType.ToLower()}.CostEngine.Data.json")
                            {
                                var stream = fls.Current.CreateReadStream();
                                using var streamReader = new StreamReader(stream);
                                content = streamReader.ReadToEnd();
                            }
                        }
                    }

                    this.activeCodeFile = new CodeFile
                    {
                        Path = "__Data.json",
                        Content = content,
                    };
                    this.CodeFiles.Add("__Data.json", this.activeCodeFile);
                }
                else
                {
                    this.activeCodeFile = new CodeFile
                    {
                        Path = "__Data.json",
                        Content = CoreConstants.MainComponentDefaultFileContent,
                    };

                    this.CodeFiles.Add("__Data.json", this.activeCodeFile);
                }
            }

            this.CodeFileNames = this.CodeFiles.Keys.ToList();

            await base.OnInitializedAsync();
        }




        private async Task CompileAsync()
        {
            this.Loading = true;
            this.LoaderText = "Processing";

            //确保有时间调用渲染
            await Task.Delay(10);

            CompileToAssemblyResult compilationResult = null;
            CodeFile mainComponent = null;
            string originalMainComponentContent = null;
            try
            {
                this.UpdateActiveCodeFileContent();

                // 添加必要的主组件代码前缀并存储原始内容，以便我们可以在编译后立即恢复。
                if (this.CodeFiles.TryGetValue(CoreConstants.MainComponentFilePath, out mainComponent))
                {
                    originalMainComponentContent = mainComponent.Content;
                    mainComponent.Content = MainComponentCodePrefix + originalMainComponentContent.Replace(MainComponentCodePrefix, "");
                }

                compilationResult = await this.CompilationService.CompileToAssemblyAsync(
                    this.CodeFiles.Values,
                    this.UpdateLoaderTextAsync);

                this.Diagnostics = compilationResult.Diagnostics.OrderByDescending(x => x.Severity).ThenBy(x => x.Code).ToList();
                this.AreDiagnosticsShown = true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Snackbar.Add("Error while compiling the code.", Severity.Error);
            }
            finally
            {
                if (mainComponent != null)
                {
                    mainComponent.Content = originalMainComponentContent;
                }

                this.Loading = false;
            }

            if (compilationResult?.AssemblyBytes?.Length > 0)
            {
    
                // TODO: Add error page in iframe
                //this.JsRuntime.InvokeVoid("App.reloadIFrame", "user-page-window", MainUserPagePath);
            }
        }

        private void ShowSaveSnippetPopup() => this.SaveSnippetPopupVisible = true;


     
        private  void CopyAllCode()
        {
            //await JsApiService.CopyToClipboardAsync(Snippets.GetCode(this.CodeEditorContent));
        }


        private void HandleTabActivate(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return;
            }

            this.UpdateActiveCodeFileContent();

            if (this.CodeFiles.TryGetValue(name, out var codeFile))
            {
                this.activeCodeFile = codeFile;

                this.CodeEditorComponent.Focus();
            }
        }

        private void HandleTabClose(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return;
            }

            this.CodeFiles.Remove(name);
        }

        /// <summary>
        /// 创建脚本片段
        /// </summary>
        /// <param name="name"></param>
        private void HandleTabCreate(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return;
            }

            var nameWithoutExtension = Path.GetFileNameWithoutExtension(name);

            var newCodeFile = new CodeFile { Path = name };

            //newCodeFile.Content = newCodeFile.Type == CodeFileType.CSharp
            //    ? string.Format(CoreConstants.DefaultCSharpFileContentFormat, nameWithoutExtension)
            //    : string.Format(CoreConstants.DefaultRazorFileContentFormat, nameWithoutExtension);

            this.CodeFiles.TryAdd(name, newCodeFile);


            var ft = "csharp";

            switch (this.CodeFileType)
            {
                case CodeFileType.CSharp:
                    ft = "csharp";
                    break;
                case CodeFileType.Razor:
                    ft = "razor";
                    break;
                case CodeFileType.Json:
                    ft = "json";
                    break;
                case CodeFileType.XML:
                    ft = "xml";
                    break;
            }

            //this.JsRuntime.InvokeVoid("App.Code.setCodeEditorContainerHeight", ft);
        }

        private void UpdateActiveCodeFileContent()
        {
            if (this.activeCodeFile == null)
            {
                Snackbar.Add("No active file to update.", Severity.Error);
                return;
            }

            this.activeCodeFile.Content = this.CodeEditorComponent.GetCode();
        }

        private Task UpdateLoaderTextAsync(string loaderText)
        {
            this.LoaderText = loaderText;

            this.StateHasChanged();

            return Task.Delay(10); 
        }
    }
}
