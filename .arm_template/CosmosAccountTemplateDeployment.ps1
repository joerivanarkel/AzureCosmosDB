$resourcegroup = 'fillinresourcegroupname'
$cosmosAccountName = 'fillincosmosaccountname'

New-AzResourceGroup -Name $resourcegroup -Location northeurope -Force

New-AzResourceGroupDeployment `
    -Name 'new-storage' `
    -ResourceGroupName $resourcegroup `
    -TemplateFile 'CosmosAccountTemplate.json' `
    -cosmosAccountName $cosmosAccountName