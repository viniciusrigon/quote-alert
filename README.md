# Quote Alert

## Passos para executar o programa

1) baixe do repositório, executando o comando:

``` console
git clone https://github.com/viniciusrigon/quote-alert
```

2) entre no diretorio quote-alert e execute o comando: 

``` console
dotnet run "TICKER" "sell-price" "buy-price"
```

## Parâmetros de configuração

**intervaloExecução** : intervalo entre as requisições para a API de cotações de ativos.   
**emaildestino**: endereço de e-mail para onde será enviado o aviso de compra ou venda.  
**SmtpConfig.server**: servidor de SMTP  
**SmtpConfig.user**: usuário de autenticação no servidor SMTP  
**SmtpConfig.password**: senha de acesso ao servidor SMTP.  
**SmtpConfig.port**: porta da conexão. geralmente é 587  
**SmtpConfig.from**: nome do rementente  
**ApiConfig.token**: token para autenticar uma requisição para a API de cotação  
**ApiConfig.baseUrl**: url base da API  
**ApiConfig.endpoints.quote**: endpoint usado para buscar a cotação do ativo  


