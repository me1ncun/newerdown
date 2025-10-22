//PARAMETERS
@description('Name of the Web App')
param webAppName string

@description('App Service Plan SKU (e.g. P1v3)')
param appServicePlanSkuName string

@description('Runtime stack (e.g. "DOTNET|8.0")')
param runtimeStack string

//VARIABLES
var webAppName = 'app-${webAppName}'
var webAppAdminName = 'app-${webAppName}-admin'
var appServicePlanName = 'asp-${webAppName}'
var appInsightsName = 'appi-${webAppName}'
var functionAppName = 'func-${webAppName}-servicingfunctions'
var keyVaultName = 'kv-${webAppName}'
var serviceBusNamespaceName = 'sb-${webAppName}'
var signalRName = 'sigr-${webAppName}'
var storageAccountName = 'st${webAppName}'
var sqlServerName = 'sql-${webAppName}'
var sqlDbName = 'sqldb-${webAppName}'

// AZURE APP INSIGHTS
module appInsights './modules/appInsights.bicep' = {
  name: appInsightsName
  params: {
    appInsightsName: appInsightsName
    // appInsightsWorkspaceResourceId: logAnalyticsWorkspaceId
  }
}

// APP SERVICE PLAN (Windows)
module appServicePlan './modules/appServicePlan.bicep' = {
  name: appServicePlanName
  params: {
    appServicePlanName: appServicePlanName
    appServicePlanSkuName: appServicePlanSkuName
  }
}

// WEBAPP
module webApp './modules/appService.bicep' = {
  name: webAppName
  params: {
    webAppName: webAppName
    serverFarmId: appServicePlan.outputs.serverFarmId
    appInsightsInstrumentationKey: appInsights.outputs.appInsightsInstrumentationKey
    appInsightsPrimaryConnectionString: appInsights.outputs.appInsightsPrimaryConnectionString
    runtimeStack: runtimeStack
  }
}

// WEBAPP ADMIN
module webAppAdmin './modules/appServiceAdmin.bicep' = {
  name: webAppAdminName
  params: {
    webAppName: webAppAdminName
    serverFarmId: appServicePlan.outputs.serverFarmId
    appInsightsInstrumentationKey: appInsights.outputs.appInsightsInstrumentationKey
    appInsightsPrimaryConnectionString: appInsights.outputs.appInsightsPrimaryConnectionString
    runtimeStack: runtimeStack
  }
}

// FUNCTION APP
module functionApp './modules/functionApp.bicep' = {
  name: 'functionApp'
  params: {
    functionAppName: functionAppName
    storageAccountName: storageAccountName
    serverFarmId: appServicePlan.outputs.serverFarmId
  }
}

// KEY VAULT
module keyVault './modules/keyVault.bicep' = {
  name: 'keyVault'
  params: {
    keyVaultName: keyVaultName
  }
}

// SERVICE BUS
module serviceBus './modules/serviceBus.bicep' = {
  name: 'serviceBus'
  params: {
    namespaceName: serviceBusNamespaceName
  }
}

// SIGNALR
module signalR './modules/signalR.bicep' = {
  name: 'signalR'
  params: {
    signalRName: signalRName
  }
}

// STORAGE ACCOUNT
module storage './modules/storageAccount.bicep' = {
  name: 'storageAccount'
  params: {
    storageAccountName: storageAccountName
  }
}

// SQL SERVER AND DATABASE
module sql './modules/sql.bicep' = {
  name: 'sql'
  params: {
    sqlServerName: sqlServerName
    sqlDbName: sqlDbName
    administratorLogin: 'sqladmin'
    administratorPassword: '' 
  }
}

output webAppUrl string = webApp.outputs.webAppUrl
output functionAppUrl string = functionApp.outputs.functionAppUrl
output sqlDbConnection string = sql.outputs.sqlConnectionString