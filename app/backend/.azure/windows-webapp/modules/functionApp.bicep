param functionAppName string
param serverFarmId string
param storageAccountName string

resource storage 'Microsoft.Storage/storageAccounts@2022-09-01' existing = {
  name: storageAccountName
}

resource functionApp 'Microsoft.Web/sites@2023-01-01' = {
  name: functionAppName
  location: resourceGroup().location
  kind: 'functionapp'
  properties: {
    serverFarmId: serverFarmId
    siteConfig: {
      appSettings: [
        { name: 'AzureWebJobsStorage', value: storage.properties.primaryEndpoints.blob }
        { name: 'FUNCTIONS_EXTENSION_VERSION', value: '~4' }
        { name: 'FUNCTIONS_WORKER_RUNTIME', value: 'dotnet' }
      ]
    }
  }
}

output functionAppUrl string = 'https://${functionApp.properties.defaultHostName}'