param signalRName string

resource signalR 'Microsoft.SignalRService/SignalR@2021-10-01' = {
  name: signalRName
  location: resourceGroup().location
  sku: {
    name: 'Free_F1'
    tier: 'Free'
    capacity: 1
  }
  properties: {
    features: [
      {
        flag: 'ServiceMode'
        value: 'Default'
      }
    ]
  }
}