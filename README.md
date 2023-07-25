
<h1>Federated Learning For Everyone (FL4E) </h1>

<img src="/FL/FL4E.png" >

Welcome to FL4E - your comprehensive and customizable framework designed to revolutionize clinical research. FL4E addresses challenges like data fragmentation and regulatory barriers, promoting collaboration between data collection initiatives and data analysis experts. Our framework includes a multitude of services and modules, making it user-friendly and versatile for different stakeholders, and making the process of data sharing transparent and straightforward.

## Modules

FL4E is home to several functional modules:


1. **Study Center**: This module serves as a collaborative hub for coordinating and directing research activities. It includes the Study Catalogue, a repository for shared studies, and the Analysis Center, designed for data scientists to create analysis entries. It encourages collaboration among data scientists, data providers, and downstream users, enabling sharing and enrichment of study data. The Center follows a strict access control and ensures transparency in script sharing and execution.
2. **Repository Center**: This module is a centralized storage facility for scripts and models essential for research and collaboration. It's linked to the Study Center and Model Center, enabling easy access, retrieval, and tracing of files. This helps maintain the integrity and reliability of the data and models being used. 
3. **Model Center**: This module is geared towards the coordination and sharing of trained models. It includes a Model Catalogue for high-level model information and a Model Repository for detailed model data. It offers various features, such as sharing trained models after analysis, which can be used by other data scientists or data providers for testing. The Model Center also includes an interactive web page to enable non-technical users to interact with the trained models.
4. **Data Center**: This module implements a three-step process for data sharing, aimed at ensuring data quality and secure handling. It facilitates the sharing of raw data schemes (data dictionaries), data cleaning and enhancement scripts, and the aggregation of cleaned data. The Data Center helps maintain data integrity while allowing for efficient sharing and aggregation of data.


The FL4E framework has incorporated **two comprehensive analyses**. These analyses utilize the Fed-Heart-Disease and Fed_Tcga_Brca datasets from Flamby, thereby facilitating research on clinical healthcare data. This particular repository serves as a crucial component of our project, "Federated Learning for Everyone". Designed to operate independently, it can be run locally and acts as the primary analytical resource for the use case section within FL4E. The repository also features a WBCD experiment, which provides a blueprint for generalizing scripts intended for use within the framework.
For more details about the analyses, please refer to the [FL4E-Analysis GitHub repository](https://github.com/ashkan-pirmani/FL4E-Analysis).



## FL4E Highlevel Architecture
<img src="/FL/FL4E-architecture-2.png" >



## Code Structure

```

FL4E
└── FL
    ├── FL-Clients
    │   ├── Controllers
    │   ├── Properties
    │   ├── Views
    │   └── wwwroot
    ├── FL-Server
    │   ├── Areas
    │   ├── Properties
    │   ├── Views
    │   └── wwwroot
    ├── FL-Server.DataAccess
    │   ├── Data
    ├── FL-Server.Models
    │   ├── Models
    ├── FL-Server.Utility
    └── FLFEO

```


## Getting Started

FL4E uses Docker to build and run the FL-Server and FL-Clients modules. The instructions to build and run these modules are as follows:

**FL-Server**

```
Docker build -t <ImageName> -f ./FL-Server/Dockerfile .
Docker run -it -p <YourDesiredPort>:80 <ImageName>
```


**FL-Client**

```
Docker build -t <ImageName> -f ./FL-Clients/Dockerfile .
Docker run -it -p <YourDesiredPort>:80 <ImageName>
```
For further development, we recommend using the .NET Core SDK. You can find more information about the .NET Core SDK here.

For further development its better to use the *.Net Core* SDK for the development process. You can refer to this [link](https://dotnet.microsoft.com/en-us/download) for further information.
 .Net Core is Cross-platform. Open source avaible for all the platforms.
 

A docker image is also available on the docker hub: [link for the image]([https://dotnet.microsoft.com/en-us/download](https://hub.docker.com/r/ashkanpirmani/flclients-arm)).
 
 <h3>Using dotnet SDK</h3>
  
 **FL-Server**

 
  To Restore the Server Configuration 
  ```
  dotnet restore "FL-Server/FL-Server.csproj"
  ```
  To Build the Server
  ```
  dotnet build "FL-Server.csproj" -c Release -o /app/build
  ```
  To Publish the Server
  ```
  dotnet publish "FL-Server.csproj" -c Release -o /app/publish
  ```
  
  You have different options to deploy the server, either IIS or Kestrel. You can also finetune the publish as SingleFile or Framework dependent.
 
 
we also recommend running the FL4E server directly from Azure container registry:
     

    docker -it -p 80:80 flserver.azurecr.io/flserver:20220313112131


For FL4E client, you can also pull it directly from the docker hub:


    docker -it -p 80:80 ashkanpirmani/flclients-arm


## Database design
Please note that Data Sharing Layers and DB design notations are yet to be implemented.
<img src="/FL/DB.png" >


## Acknowledgments

FL4E is powered by Flower as its backend engine. For more information about Flower, please refer to [here](https://flower.dev)

```
@article{beutel2020flower,
  title={Flower: A Friendly Federated Learning Research Framework},
  author={Beutel, Daniel J and Topal, Taner and Mathur, Akhil and Qiu, Xinchi and Parcollet, Titouan and Lane, Nicholas D},
  journal={arXiv preprint arXiv:2007.14390},
  year={2020}
}
```
For the visualization, Metronic has been used, for more information regarding the dashboard template, please refer to [here](https://themeforest.net/item/metronic-responsive-admin-dashboard-template)
