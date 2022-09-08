namespace TryCostEngine.MAUI.Services
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.IO.Compression;
    using System.Text;
    using TryCostEngine.Core;
    using Microsoft.AspNetCore.WebUtilities;
    using Microsoft.CodeAnalysis.CSharp;

    public static class CodeFilesHelper
    {
        private const int MainComponentFileContentMinLength = 10;

        /// <summary>
        /// 验证扩展名
        /// </summary>
        private static readonly HashSet<string> ValidCodeFileExtensions = new(StringComparer.Ordinal)
        {
            CodeFile.RazorFileExtension,
            CodeFile.CsharpFileExtension,
            CodeFile.JSONFileExtension,
            CodeFile.XMLFileExtension,
        };

        public static string NormalizeCodeFilePath(string path, out string error)
        {
            if (string.IsNullOrWhiteSpace(path))
            {
                error = "路径不能为空.";
                return null;
            }

            var trimmedPath = path.Trim();

            var extension = Path.GetExtension(trimmedPath).ToLowerInvariant();
            if (string.IsNullOrEmpty(extension))
            {
                extension = CodeFile.RazorFileExtension;
            }

            if (!ValidCodeFileExtensions.Contains(extension))
            {
                error = $"不支持扩展名为 '{extension}'的文件. 有效扩展: {string.Join("; ", ValidCodeFileExtensions)}";
                return null;
            }

            var fileName = Path.GetFileNameWithoutExtension(trimmedPath);
            if (!SyntaxFacts.IsValidIdentifier(fileName))
            {
                error = $"'{fileName}' 不是有效的文件名，它必须是有效的C#标识符.";
                return null;
            }

            if (extension == CodeFile.RazorFileExtension && char.IsLower(fileName[0]))
            {
                error = $"'{fileName}' 以小写字符开头，Razor文件名必须以大写字符或 _. 开头";
                return null;
            }

            error = null;
            return fileName + extension;
        }

        public static string ValidateCodeFilesForSnippetCreation(IEnumerable<CodeFile> codeFiles)
        {
            if (codeFiles == null)
            {
                throw new ArgumentNullException(nameof(codeFiles));
            }

            var containsMainComponent = false;
            var processedFilePaths = new HashSet<string>();
            var index = 0;
            foreach (var codeFile in codeFiles)
            {
                if (codeFile == null)
                {
                    return $"文件 #{index} - 不存在.";
                }

                if (string.IsNullOrWhiteSpace(codeFile.Path))
                {
                    return $"文件 #{index} - 没有文件路径.";
                }

                if (processedFilePaths.Contains(codeFile.Path))
                {
                    return $"文件 '{codeFile.Path}' 重复.";
                }

                var extension = Path.GetExtension(codeFile.Path);
                if (!ValidCodeFileExtensions.Contains(extension))
                {
                    return $"文件 '{codeFile.Path}' 是无效扩展: {extension}. 有效扩展: {string.Join("; ", ValidCodeFileExtensions)}";
                }

                var fileName = Path.GetFileNameWithoutExtension(codeFile.Path);
                if (!SyntaxFacts.IsValidIdentifier(fileName))
                {
                    return $"'{fileName}' 不是有效的文件名，它必须是有效的C#标识符..";
                }

                if (extension == CodeFile.RazorFileExtension && char.IsLower(fileName[0]))
                {
                    return $"'{fileName}' 以小写字符开头，Razor文件名必须以大写字符或 _. 开头";
                }

                if (codeFile.Path == CoreConstants.MainComponentFilePath)
                {
                    if (string.IsNullOrWhiteSpace(codeFile.Content) || codeFile.Content.Trim().Length < MainComponentFileContentMinLength)
                    {
                        return $"主组件内容必须包括 {MainComponentFileContentMinLength} 字符长度.";
                    }

                    containsMainComponent = true;
                }

                processedFilePaths.Add(codeFile.Path);
                index++;
            }

            if (!containsMainComponent)
            {
                return "没有提供主要组件文件.";
            }

            return null;
        }

        public static IEnumerable<CodeFile> ToCodeFiles(this string urlEncodedBase64compressedCode)
        {
            // uncompress
            var bytes = WebEncoders.Base64UrlDecode(urlEncodedBase64compressedCode);
            using (var uncompressed = new MemoryStream())
            using (var compressedStream = new MemoryStream(bytes))
            using (var uncompressor = new DeflateStream(compressedStream, CompressionMode.Decompress))
            {
                uncompressor.CopyTo(uncompressed);
                uncompressor.Close();
                var codeString = Encoding.UTF8.GetString(uncompressed.ToArray());
                var codeFiles = new List<CodeFile>();
                var codeElements = codeString.Split((char)31);
                for (int i = 0; i < codeElements.Length; i += 2)
                {
                    var codeFile = new CodeFile() 
                    { 
                        Path = codeElements[i], 
                        Content = codeElements[i + 1]
                    };
                    codeFiles.Add(codeFile);
                }

                return codeFiles;
            }
        }
    }
}
