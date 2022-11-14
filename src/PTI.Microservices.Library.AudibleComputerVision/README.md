# PTI.Microservices.Library.AudibleComputerVision

This is part of PTI.Microservices.Library set of packages

The purpose of this package is to facilitate generating audio for computer vision results

**Examples:**

## Read To Stream

    AzureSpeechService azureSpeechService =
        new AzureSpeechService(null, this.AzureSpeechConfiguration,
        new CustomHttpClient(new CustomHttpClientHandler(null)));
    AzureComputerVisionService azureComputerVisionService =
        new AzureComputerVisionService(null, this.AzureComputerVisionConfiguration,
        new CustomHttpClient(new CustomHttpClientHandler(null)));
    AudibleComputerVisionService audibleComputerVisionService =
        new AudibleComputerVisionService(null,
        azureComputerVisionService, azureSpeechService);
    Stream imageStream = await (new HttpClient().GetStreamAsync(this.TestComputerVisionImageUri));
    Stream outputStream = File.Create(@"C:\Temp\AudibleImageDescription.wav");
    await audibleComputerVisionService.ReadToStreamAsync(imageStream, outputStream, Path.GetFileName(this.TestComputerVisionImageUri));
    outputStream.Close();

## Describe Image To Stream
    AzureSpeechService azureSpeechService =
        new AzureSpeechService(null, this.AzureSpeechConfiguration,
        new CustomHttpClient(new CustomHttpClientHandler(null)));
    AzureComputerVisionService azureComputerVisionService =
        new AzureComputerVisionService(null, this.AzureComputerVisionConfiguration,
        new CustomHttpClient(new CustomHttpClientHandler(null)));
    AudibleComputerVisionService audibleComputerVisionService =
        new AudibleComputerVisionService(null,
        azureComputerVisionService, azureSpeechService);
    Stream imageStream = await (new HttpClient().GetStreamAsync(new Uri(this.TestComputerVisionImageUri)));
    Stream outputStream = File.Create(@"C:\Temp\AudibleImageDescription.wav");
    await audibleComputerVisionService.DescribeImageToStreamAsync(imageStream, outputStream,
        Path.GetFileName(this.TestComputerVisionImageUri));
    outputStream.Close();

## Describe Image To Speakers

    AzureSpeechService azureSpeechService =
        new AzureSpeechService(null, this.AzureSpeechConfiguration,
        new CustomHttpClient(new CustomHttpClientHandler(null)));
    AzureComputerVisionService azureComputerVisionService =
        new AzureComputerVisionService(null, this.AzureComputerVisionConfiguration,
        new CustomHttpClient(new CustomHttpClientHandler(null)));
    AudibleComputerVisionService audibleComputerVisionService =
        new AudibleComputerVisionService(null,
        azureComputerVisionService, azureSpeechService);
    Stream imageStream = await (new HttpClient().GetStreamAsync(this.TestFaceImageUrl));
    await audibleComputerVisionService.DescribeImageToDefaultSpeakersAsync(imageStream,
        Path.GetFileName(this.TestComputerVisionImageUri));