# Sitecore audit trail demo application
## A showcase on how to extend Sitecore using Serverless architectures

_Please note that this is code used for a demo and for research. This project and its instructions are not a step by step guide on how to implement serverless architectures or Azure Functions, nor are they production ready._

This project gives you an insight in how we've built our demo project to test out the extensibility of Sitecore using serverless architectures. It is our sandbox environment to do research on how to extend a PaaS hosted application with FaaS implemented features. Because this repository is a copy for Open Source purposes, it lacks git history and correct references to its contributors.

The project and architecture is designed and built by Mathijs van der Sangen and Rob Habraken. Please read this blog post for more information on the ideas and purpose of this project:  https://www.robhabraken.nl/index.php/3297/extending-sitecore-using-serverless-architectures/ 

### Installation instructions

_These instructions are for demo purposes, setting up the minimum required services. In real life, such would be done by rolling out an ARM template containing said resources._

**Audit Trail Azure services deploy** (for demo):

- Create a Function App in Azure, defaults are fine.
- Create a Signal R instance in Azure, again the defaults are fine.
- Create a CosmosDB account with shared provisioning. Database name "audit-trail", with a collection called "audit-records". Recommended partition key is "/Event".
- From Visual Studio, deploy the AuditTrail.Feature.AuditTrail.AzureFunctions project and set up a publish profile to the Function App. Set "deploy from zip" to true.
- Once the publish profile is made, open "Manage Application Settings..."
- Set variables "COSMOS_CONNECTION_STRING" and "AzureSignalRConnectionString" to their respective connection strings, found on Azure.

**Audit Trail Sitecore deployment** (for demo, this part should integrate with your Sitecore implementation):

- Open the project in Visual Studio
- Open the go to properties > resources in the AuditTrail.Feature.AuditTrail project.
- Set the following resources:
  - "AZURE_API_DOMAIN": Base domain of the Azure Function App.
  - "AZURE_API_STORE_KEY": Function key of the "StoreEventData" Function.
  - "AZURE_API_GET_ITEM_KEY" Function key of the "GetByItem" Function.
- Lastly, install the "Sitecore Audit Trail - Accompanying Items.zip" package.

**Audit Trail Demo UI** (Vue.js app):

- Set API domain and API key in the .env file in project root (retrieved from Azure after Function deployment).
- npm install, npm run build, then upload dist folder to preferred hosting (tested with Azure Blob Storage > Static Hosting).
