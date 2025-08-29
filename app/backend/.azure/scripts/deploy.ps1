param(
    [string]$ResourceGroupName = "",
    [string]$WebAppName = "",
    [string]$AppServicePlanSku = "P1v3",
    [string]$RuntimeStack = "DOTNETCORE|9.0"
)

az login

$rgExists = az group exists --name $ResourceGroupName | ConvertFrom-Json
if (-not $rgExists) {
    Write-Host "Creating resource group: $ResourceGroupName in $Location"
    az group create --name $ResourceGroupName --location $Location | Out-Null
}

Write-Host "Starting deployment..."
az deployment group create `
    --resource-group $ResourceGroupName `
    --name 'initialdeployment' `
    --template-file "../windows-webapp/main.bicep" `
    --parameters `
    webAppName=$WebAppName `
    appServicePlanSkuName=$AppServicePlanSku `
    runtimeStack="'$RuntimeStack'"

Write-Host "`n✅ Deployment completed."
