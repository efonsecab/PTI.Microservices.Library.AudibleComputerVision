using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PTI.Microservices.Library.Services.Specialized
{
    /// <summary>
    /// Service in charge of allowing to generate audio of computer vision results
    /// </summary>
    public sealed class AudibleComputerVisionService
    {
        private ILogger<AudibleComputerVisionService> Logger { get; }
        private AzureComputerVisionService AzureComputerVisionService { get; }
        private AzureSpeechService AzureSpeechService { get; }

        /// <summary>
        /// Creates a new instane of <see cref="AudibleComputerVisionService"/>
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="azureComputerVisionService"></param>
        /// <param name="azureSpeechService"></param>
        public AudibleComputerVisionService(ILogger<AudibleComputerVisionService> logger,
            AzureComputerVisionService azureComputerVisionService, AzureSpeechService azureSpeechService)
        {
            this.Logger = logger;
            this.AzureComputerVisionService = azureComputerVisionService;
            this.AzureSpeechService = azureSpeechService;
        }

        /// <summary>
        /// Analyzes an image and send the audible description to the specified output stream
        /// </summary>
        /// <param name="imageStream"></param>
        /// <param name="outputStream"></param>
        /// <param name="fileName"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task ReadToStreamAsync(Stream imageStream, Stream outputStream, string fileName, CancellationToken cancellationToken = default)
        {
            try
            {
                var imageAnalysisResult = await this.AzureComputerVisionService.ReadAsync(imageStream, fileName, cancellationToken);
                if (imageAnalysisResult.readResults.Count() > 0)
                {
                    foreach (var singleReadResult in imageAnalysisResult.readResults)
                    {
                        if (singleReadResult.lines.Count() > 0)
                        {
                            foreach (var singleLine in singleReadResult.lines)
                            {
                                await this.AzureSpeechService.TalkToStreamAsync($"{singleLine.text}", outputStream);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                this.Logger?.LogError(ex.Message, ex);
                throw;
            }
        }

        /// <summary>
        /// Analyzes an image and send the audible description to the specified output stream
        /// </summary>
        /// <param name="imageStream"></param>
        /// <param name="outputStream"></param>
        /// <param name="fileName"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task DescribeImageToStreamAsync(Stream imageStream, Stream outputStream, string fileName, CancellationToken cancellationToken = default)
        {
            try
            {
                StringBuilder stringBuilder = new StringBuilder();
                var imageAnalysisResult = await this.AzureComputerVisionService.DescribeImageAsync(imageStream, fileName, cancellationToken);
                if (imageAnalysisResult.description.captions.Count() > 0)
                {
                    stringBuilder.AppendLine($"{imageAnalysisResult.description.captions.Count()} " +
                        $"descriptions found");
                    foreach (var singleCaption in imageAnalysisResult.description.captions)
                    {
                        stringBuilder.AppendLine(singleCaption.text);
                    }
                }
                if (imageAnalysisResult.description.tags.Count() > 0)
                {
                    stringBuilder.AppendLine($"{imageAnalysisResult.description.tags.Count()} tags found");
                    foreach (var singleTag in imageAnalysisResult.description.tags)
                    {
                        stringBuilder.AppendLine(singleTag);
                    }
                }
                await this.AzureSpeechService.TalkToStreamAsync(stringBuilder.ToString(), outputStream);
            }
            catch (Exception ex)
            {
                this.Logger?.LogError(ex.Message, ex);
                throw;
            }
        }

        /// <summary>
        /// Analyzes an image and send the description to the default speakers
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="fileName"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task DescribeImageToDefaultSpeakersAsync(Stream stream, string fileName, CancellationToken cancellationToken = default)
        {
            try
            {
                var imageAnalysisResult = await this.AzureComputerVisionService.DescribeImageAsync(stream, fileName, cancellationToken);
                if (imageAnalysisResult.description.captions.Count() > 0)
                {
                    await this.AzureSpeechService.TalkToDefaultSpeakersAsync($"{imageAnalysisResult.description.captions.Count()} descriptions found");
                    foreach (var singleCaption in imageAnalysisResult.description.captions)
                    {
                        await this.AzureSpeechService.TalkToDefaultSpeakersAsync(singleCaption.text);
                    }
                }
                if (imageAnalysisResult.description.tags.Count() > 0)
                {
                    await this.AzureSpeechService.TalkToDefaultSpeakersAsync($"{imageAnalysisResult.description.tags.Count()} tags found");
                    foreach (var singleTag in imageAnalysisResult.description.tags)
                    {
                        await this.AzureSpeechService.TalkToDefaultSpeakersAsync(singleTag);
                    }
                }

            }
            catch (Exception ex)
            {
                this.Logger?.LogError(ex.Message, ex);
                throw;
            }
        }
    }
}
