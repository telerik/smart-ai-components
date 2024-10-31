# Telerik UI for .NET MAUI Smart Components

1. DataGrid Semantic (AI) Search

### Smart Search
The DataGrid search demo rely on similarity search to calculate the vector distance between the search term and the data items using the SmartComponents.LocalEmbeddings package. The solution works great with small to medium amounts of data.

## How to Build the Solutions

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
