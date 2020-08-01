Param(
    [string] $subscriptionId,
    [string] $location,
    [string] $rg,
    [string] $sa,
    [string] $fa,
    [string] $domain
)

#az login

if (-not $subscriptionId)
{
    az account list --output table
    $subscriptionId = Read-Host "Subscription ID"
}

az account set --subscription $subscriptionId

if (-not $location)
{
    az account list-locations --output table
    $location = Read-Host "Location Name"
}

if (-not $rg)
{
    Write-Host "Resource group names only allow alphanumeric characters, periods, underscores, hyphens and parenthesis and cannot end in a period."
    $rg = Read-Host "Resource Group Name"
}

az group create `
    --name $rg `
    --location $location

if (-not $sa)
{
    Write-Host "Storage account names can contain only lowercase letters and numbers. Name must be between 3 and 24 characters."
    $sa = Read-Host "Storage Account Name"
}

az storage account create `
    --name $sa `
    --location $location `
    --resource-group $rg `
    --sku STANDARD_LRS

if (-not $fa)
{
    Write-Host "Function app names can contain only letters, numbers and hyphens."
    $fa = Read-Host "Function App Name"
}

az functionapp create `
    --name $fa `
    --resource-group $rg `
    --storage-account $sa `
    --consumption-plan-location $location `
    --functions-version 3

if (-not $domain)
{
    Write-Host "Space-separated list of CORS allowed origins. '*' to allow all origins."
    $domain = Read-Host "Allowed Origins"
}

az functionapp cors add `
    --name $fa `
    --resource-group $rg `
    --allowed-origins $domain.Split()

func azure functionapp publish $fa