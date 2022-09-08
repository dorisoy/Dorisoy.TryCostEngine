﻿namespace TryCostEngine.MAUI.Services
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.IO.Compression;
    using System.Net.Http;
    using System.Text;
    using System.Threading.Tasks;
    using TryCostEngine.MAUI.Models;
    using TryCostEngine.Core;
    using Microsoft.AspNetCore.Components;
    using Microsoft.Extensions.Options;

    public class SnippetsService
    {
        private const int SnippetIdLength = 16;

        private readonly HttpClient httpClient;
        private readonly string snippetsService;

        public SnippetsService(IOptions<SnippetsOptions> snippetsOptions, HttpClient httpClient, NavigationManager navigationManager)
        {
            this.httpClient = httpClient;
            this.snippetsService = $"{navigationManager.BaseUri}{snippetsOptions.Value.SnippetsService}";
        }

        public async Task<string> SaveSnippetAsync(IEnumerable<CodeFile> codeFiles)
        {
            var snippetId = string.Empty;
            if (codeFiles == null)
            {
                throw new ArgumentNullException(nameof(codeFiles));
            }

            var codeFilesValidationError = CodeFilesHelper.ValidateCodeFilesForSnippetCreation(codeFiles);
            if (!string.IsNullOrWhiteSpace(codeFilesValidationError))
            {
                throw new InvalidOperationException(codeFilesValidationError);
            }

            using (var memoryStream = new MemoryStream())
            {
                using (var archive = new ZipArchive(memoryStream, ZipArchiveMode.Create, true))
                {
                    foreach (var codeFile in codeFiles)
                    {
                        var byteArray = Encoding.UTF8.GetBytes(codeFile.Content);
                        var codeEntry = archive.CreateEntry(codeFile.Path);
                        using var entryStream = codeEntry.Open();
                        entryStream.Write(byteArray);
                    }
                }

                memoryStream.Position = 0;

                var inputData = new StreamContent(memoryStream);

                var response = await this.httpClient.PostAsync(this.snippetsService, inputData);
                snippetId = await response.Content.ReadAsStringAsync();
            }

            return snippetId;
        }


        /// <summary>
        /// 获取代码片段内容
        /// </summary>
        /// <param name="snippetId"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public async Task<IEnumerable<CodeFile>> GetSnippetContentAsync(string snippetId)
        {
            if (string.IsNullOrWhiteSpace(snippetId))
            {
                throw new ArgumentException("Invalid snippet ID.", nameof(snippetId));
            }

            IEnumerable<CodeFile> snippetFiles;
            if (snippetId.Length != SnippetIdLength)
            {
                try
                {
                    snippetFiles = snippetId.ToCodeFiles();
                    var codeFilesValidationError = CodeFilesHelper.ValidateCodeFilesForSnippetCreation(snippetFiles);
                    if (!string.IsNullOrWhiteSpace(codeFilesValidationError))
                    {
                        throw new InvalidOperationException(codeFilesValidationError);
                    }
                }
                catch
                {
                    throw new ArgumentException("Invalid snippet ID.", nameof(snippetId));
                }
            }
            else
            {
                var reponse = await this.httpClient.GetAsync($"{this.snippetsService}/{snippetId}");
                var zipStream = await reponse.Content.ReadAsStreamAsync();
                zipStream.Position = 0;
                snippetFiles = await ExtractSnippetFilesFromZip(zipStream);
            }
            return snippetFiles;
        }

        private static async Task<IEnumerable<CodeFile>> ExtractSnippetFilesFromZip(Stream zipStream)
        {
            var result = new List<CodeFile>();

            using var zipArchive = new ZipArchive(zipStream, ZipArchiveMode.Read);
            foreach (var entry in zipArchive.Entries)
            {
                using var streamReader = new StreamReader(entry.Open());
                result.Add(new CodeFile { Path = entry.FullName, Content = await streamReader.ReadToEndAsync() });
            }

            return result;
        }
    }
}
