# API para cadastro de clientes e cálculo de um Score de Confiança

Esta API foi desenvolvida em ASP.NET Core com o objetivo de cadastrar, consultar, atualizar e remover clientes de um sistema, e calcular o score baseado nos dados do cliente.

## Endpoints Disponíveis
- POST /api/cliente: Cadastra um novo cliente após validar os dados enviados.
- GET /api/cliente: Retorna a lista de todos os clientes cadastrados.
- GET /api/cliente/{id}: Busca um cliente específico pelo seu ID.
- GET /api/cliente/cpf/{cpf}: Busca um cliente pelo CPF.
- GET /api/cliente/email/{email}: Busca um cliente pelo e-mail.
- GET /api/cliente/estado/{estado}: Retorna todos os clientes de um determinado estado.
- PATCH /api/cliente/{id}: Atualiza dados parciais de um cliente, como e-mail, endereço, cidade, estado, CEP, DDD, telefone e rendimento anual.
- DELETE /api/cliente/{id}: Remove um cliente do sistema com base no ID informado.
- GET /api/cliente/score/{cpf}: Calcula e retorna o score de um cliente com base nos seus dados.
