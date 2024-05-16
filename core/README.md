## Explore the Telerik Core Smart (AI) component demos. 

The project showcases 3 demos. 

1. Grid Smart (AI) Search
1. ComboBox Smart (AI) Search
1. PDFViewer Smart (AI) Assistant. 

### Smart Search
ComboBox and Grid search demos are relying on similarity search calculate the vector distance between search query and the grid data items using the SmartComponents.LocalEmbeddings package. The solution works great with small to medium amaound of data.

#### How to run
To run search demos, start the project and navigate to the Grid Smart Search AI or ComboBox Search AI pages.

### PdfViewer Assistant
The PDFViewer Assistant splits the text pages into chunks and embeds each chunk when the document is loaded. The entire document is then processed to dynamically generate relevant questions, which are used as PromptSuggestions. Users can ask any of these predefined questions or type their own in the AIPrompt input. Once a question is asked, the prompt is matched with similar pages from the document, and the AI Service is used to answer the question, augmenting it with relevant documents. This technique, known as RAG (Retrieval-Augmented Generation), allows you to chat with your documents and summarize, explain, or answer questions based on the context of your own data.

#### Consideration before using in production
Keep in mind, that for the purpuse of the demo,
- we split the document and create chunks exacyly one page long. 
- We send the whole document document to extract the relevant questions. 

You will need to experiment and see what is the optimal chunk size for your document and usecase. Additionally, sending whole document on each load may be suboptimal (or it may break) for documents that are large due to the cost of the AI API and context length. In these cases, you may need to use different model (with larger context size) to summarize the document or do it in batches. 

#### How to run
To run the PDFViewer Assistant demo, replace your Azure/Open AI credentials inside `PDFViewer_AI_AssistantController.cs` file, then run the project and navigate to the PDFViewer Smart AI Assistant page. 

If you want to use different AI Service, you will need to rewrite the `CallOpenAIApi()` method and call API to your prefered model. 