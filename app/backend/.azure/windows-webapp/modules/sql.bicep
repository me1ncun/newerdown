param sqlServerName string
param sqlDbName string
param administratorLogin string
param administratorPassword string

param location string = 'polandcentral'

resource sqlServer 'Microsoft.Sql/servers@2022-02-01-preview' = {
  name: sqlServerName
  location: location
  properties: {
    administratorLogin: administratorLogin
    administratorLoginPassword: administratorPassword
  }
}

resource sqlDb 'Microsoft.Sql/servers/databases@2022-02-01-preview' = {
  name: '${sqlServer.name}/${sqlDbName}'
  location: location
  properties: {
    collation: 'SQL_Latin1_General_CP1_CI_AS'
    maxSizeBytes: 2147483648
    sampleName: 'AdventureWorksLT'
  }
  sku: {
    name: 'Basic'
    tier: 'Basic'
    capacity: 5
  }
}

output sqlConnectionString string = 'Server=tcp:${sqlServer.name}.database.windows.net,1433;Initial Catalog=${sqlDb.name};User ID=${administratorLogin};Password=${administratorPassword};'