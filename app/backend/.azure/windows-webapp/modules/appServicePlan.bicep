param appServicePlanName string
param appServicePlanSkuName string

resource appServicePlan 'Microsoft.Web/serverfarms@2022-03-01' = {
  name: appServicePlanName
  location: resourceGroup().location
  kind: 'windows'
  sku: {
    name: appServicePlanSkuName
    tier: 'PremiumV3'
  }
  properties: {
    reserved: false 
  }
}

output serverFarmId string = appServicePlan.id