# Telerik UI for .NET MAUI Smart Components

1. DataGrid Semantic (AI) Search
2. SpeechToTextButton OpenAI Whisper-1 Integration

### DataGrid Smart Search
The DataGrid search demo rely on similarity search to calculate the vector distance between the search term and the data items using the SmartComponents.LocalEmbeddings package. The solution works great with small to medium amounts of data.

#### How to Build the Solutions

1. Clone the repository on your machine.
1. Install **Telerik UI for .NET MAUI** - you can download it from [here](https://www.telerik.com/maui-ui).
1. Open the corresponding **.sln** file from the cloned repository, for example - [AIDataGridSemanticSearch.sln](/maui/AIDataGridSemanticSearch/AIDataGridSemanticSearch.sln).
1. Restore the NuGet packages. If the NuGet packages are not automatically restored, add one of the following NuGet package sources:
    * **Telerik NuGet Package Source**&mdash;You can follow the steps from [this article](https://docs.telerik.com/devtools/maui/get-started/windows/first-steps-nuget) for the purpose.
    * **Local NuGet Source**&mdash;:
        1. Copy the full path to the "Packages" folder from the Telerik UI for .NET MAUI installation.
            - For Windows - **C:\Program Files (x86)\Progress\Telerik UI for .NET MAUI 7.1.0\Packages**
            - For Mac - **/Users/&lt;Your User Name&gt;/Documents/Progress/Telerik_UI_for_NET_MAUI_7.1.0/Packages**
        1. Add this path to the `packageSources` collection in the [NuGet.Config](/maui/NuGet.Config) file. The added entry should look like this: <br/>
        `<add key="PackageSource" value="C:\Program Files (x86)\Progress\Telerik UI for .NET MAUI 7.1.0\Packages" />`
1. Build the app like any other .NET MAUI solution. You can use [this](https://docs.telerik.com/devtools/maui/demos-and-sample-apps/crypto-app) help article for guidance.

## Support and Feedback

You can find the official Telerik UI for .NET MAUI documentation at https://docs.telerik.com/devtools/maui/introduction.

We would love to hear your feedback, so should you have any questions and/or comments, please share them in our [Telerik UI for .NET MAUI Feedback Portal](https://feedback.telerik.com/maui).

### SpeechToTextButton OpenAI Whisper-1 Integration
The SpeechToTextButton exposes API for plugging in a custom speech-recognizer instance. We utilize that extensibility point to integrate with OpenAI Whisper-1 model for speech-to-text transcription.
Location - [STTB_Whisper.slnx](/maui/SpeechToTextButtonWithOpenAIWhisper/STTB_Whisper.slnx)

#### How to run
To run the demo, provide your Open AI credentials inside `WhisperAudioTranscriber.cs` file (string apiKey = Environment.GetEnvironmentVariable("OPENAI_API_KEY");), then run the project. Press the button to start recording, speak into the microphone, then press the button to stop recording, and the text will appear shortly in the Editor.
