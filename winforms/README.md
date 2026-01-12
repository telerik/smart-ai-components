## Explore the Telerik UI for WinForms Smart (AI) component demos. 

The project showcases 2 demos. 

1. RichTextEditor Smart (AI) Content Generation
2. Spreadsheet Smart (AI) Function
3. SpeechToTextButton OpenAI Whisper-1 Integration

### RichTextBox
The RichTextEditor demo uses on OpenAI service to return a suggestion according to the the text in the current paragraph and the provided context. 

#### How to run
To run the RichTextBox demo, replace your Azure/Open AI credentials inside `MainForm.cs` file, then run the project and type something then wait for a response (it will appear as a grey text).

If you want to use different AI Service, you will need to rewrite the `CallOpenAIApi()` method and call API to your preferred model. 

#### Consideration before using in production
Keep in mind, that for the purpose of the demo, 
- We send the whole document to provide context for the service.

### Spreadsheet
The Spreadsheet example creates a custom function that contains specific questions. We use these questions to ask the the Open AI service for answers based on some context that is provided. The result may no be exactly the same when running the example several times.  
 
#### How to run
To run the Spreadsheet demo, replace your Azure/Open AI credentials inside `MainForm.cs` file, then run the project and copy the AI formulas in the subsequent cells and wait for the service to provide the angers. 

If you want to use different AI Service, you will need to rewrite the `CallOpenAIApi()` method and call API to your preferred model. 

### SpeechToTextButton OpenAI Whisper-1 Integration
The SpeechToTextButton exposes API for plugging in a custom speech-recognizer instance. We utilize that extensibility point to integrate with OpenAI Whisper-1 model for speech-to-text transcription.
Location - [SpeechToTextButtonWithOpenAIWhisper.slnx](/winforms/AISpeechToTextButtonWithOpenAIWhisper/SpeechToTextButtonWithOpenAIWhisper.slnx)

#### How to run
To run the demo, provide your Open AI credentials inside `WhisperAudioTranscriber.cs` file (string apiKey = Environment.GetEnvironmentVariable("OPENAI_API_KEY");), then run the project. Press the button to start recording, speak into the microphone, then press the button to stop recording, and the text will appear shortly in the TextBox.
