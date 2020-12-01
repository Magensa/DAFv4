# Introduction 

The Repository Contains Demo Application For  Decrypt and Forward web service (DAFv4) operations includes the below operations

    1. ProcessCardSwipe 
    2. ProcessData
    3. ProcessToken
    
# Clone the repository
 1. Navigate to the main page of the  **repository**. 
 2. Under the  **repository**  name, click  **Clone** .
 3. Use any Git Client(eg.: GitBash, Git Hub for windows, Source tree ) to  **clone**  the  **repository**  using HTTPS.

*** Note ***  : reference for  [Cloning a Github Repository](https://help.github.com/en/articles/cloning-a-repository)


# Getting Started

1.  Install .net core 3.1 LTS

    - Demo app requires dotnet core 3.1 LTS is installed

2.  Software dependencies( The Following nuget packages are automatically installed when we open and run the project), please recheck and add the references from nuget
 
     Microsoft.Extensions.DependencyInjection

     Microsoft.Extensions.Configuration

     Microsoft.Extensions.Configuration.EnvironmentVariables

     Microsoft.Extensions.Configuration.Json
     
     Microsoft.Extensions.Configuration.Binder
     
3.  Latest releases

    - Initial release with all commits and changes as on Apr 3rd 2020

# Build and Test

 Steps to Build and run DafV4.DemoApp project ( .net core 3.1)

 From the cmd,  Navigate to the cloned folder and go to TokenizationDemoApps
    
 Run the following commands
    
 ```dotnet clean DafV4.DemoApps.sln```

 ```dotnet build DafV4.DemoApps.sln```

 Navigate from command prompt to DafV4.DemoApp folder containing DafV4.DemoApp.csproj and run below command

 ```dotnet run --project DafV4.DemoApp.csproj```

 This should open the application running in console.

