param webAppName string
param serverFarmId string
param appInsightsInstrumentationKey string
param appInsightsPrimaryConnectionString string
param runtimeStack string

resource webAppAdmin 'Microsoft.Web/sites@2023-01-01' = {
  name: webAppName
  location: resourceGroup().location
  kind: 'app'
  properties: {
    serverFarmId: serverFarmId
    siteConfig: {
      windowsFxVersion: runtimeStack 
      appSettings: [
        {
          name: 'APPINSIGHTS_INSTRUMENTATIONKEY'
          value: appInsightsInstrumentationKey
        }
        {
          name: 'APPLICATIONINSIGHTS_CONNECTION_STRING'
          value: appInsightsPrimaryConnectionString
        }
        {
          name: 'WEBSITE_RUN_FROM_PACKAGE'
          value: '1'
        }
      ]
    }
  }
}

output webAppAdminUrl string = 'https://${webAppAdmin.properties.defaultHostName}'