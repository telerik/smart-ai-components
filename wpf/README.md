## Explore the Telerik UI for WPF Smart (AI) component demos. 

The project showcases 2 demos. 

1. RichTextBox Smart (AI) Content Generation
2. Spreadsheet Smart (AI) Function

### RichTextBox
RichTextbox demo uses on OpenAI service to return a suggestion according to the the text in the current paragraph and the provided context. 

#### How to run
To run the RichTextBox demo, replace your Azure/Open AI credentials inside `MainWindow.xaml.cs` file, then run the project and type something then wait for a response (it will appear as a grey text).

If you want to use different AI Service, you will need to rewrite the `CallOpenAIApi()` method and call API to your preferred model. 

#### Consideration before using in production
Keep in mind, that for the purpose of the demo, 
- We send the whole document to provide context for the service.

### Spreadsheet
The Spreadsheet example creates a custom functions that contains specific questions. We use these questions to ask the the Open AI service for answers based on some context that is provided. The result may no be exactly the same when running the example several times.  
 
#### How to run
To run the Spreadsheet demo, replace your Azure/Open AI credentials inside `MainWindow.xaml.cs` file, then run the project and copy the AI formulas in the subsequent cells and wait for the service to provide the angers. 

If you want to use different AI Service, you will need to rewrite the `CallOpenAIApi()` method and call API to your preferred model. 