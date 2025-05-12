# Orientações do Projeto

## Descrição Geral

Este projeto ASP.NET WebForms foi desenvolvido com foco em **processamento assíncrono de cálculo de salários**, persistência com **Oracle Database**, e exibição de dados via **Crystal Reports**. Também foi estruturado um CRUD funcional e autenticação de usuários.

---

## Funcionalidades Implementadas

✔️ **Processamento Assíncrono de Salários**  
- O cálculo de salários é realizado de forma assíncrona para melhorar a performance e escalabilidade da aplicação.

✔️ **Separação da Lógica de Cálculo com Banco de Dados Oracle**  
- A lógica de negócio foi organizada em três camadas no banco:
  - **View:** consulta inicial com os dados necessários para cálculo.
  - **Function:** implementa a regra de cálculo dos salários.
  - **Procedure:** executa a operação final de merge na tabela de salários.

✔️ **CRUD de Pessoas**  
- Permite adicionar, editar, visualizar e excluir registros de pessoas.  
- Interface criada com ASP.NET WebForms.
- 
✔️ **Calculo do salário**  
- Permite calcular o salário individual de cada pessoa.
- Permite calcular os salários de todas as pessoas cadastradas.

✔️ **Relatório de Salários com Crystal Reports**  
- Exibição dos salários calculados em formato de relatório.  
- Relatório gerado dinamicamente com base nos dados atuais da base.

✔️ **Sistema de Login com Entidade de Usuário**  
- Implementação de entidade `Usuario` para login na aplicação.

---

## Instruções para Execução Local

### 1. Requisitos

- Visual Studio 2019 ou superior
- Oracle Database e client ODP.NET instalado
- Crystal Reports Runtime instalado (compatível com a versão do Visual Studio)
- .NET Framework 4.6

### 2. Configurar String de Conexão

Edite o `Web.config` com a string de conexão ao seu Oracle Database:

```xml
	<oracle.manageddataaccess.client>
		<version number="*">
			<dataSources>
				<dataSource alias="SampleDataSource" descriptor="(DESCRIPTION=(ADDRESS=(PROTOCOL=tcp)(HOST=localhost)(PORT=1521))(CONNECT_DATA=(SERVICE_NAME=xe))) " />
			</dataSources>
		</version>
	</oracle.manageddataaccess.client>
	<connectionStrings>
		<add name="OracleConn"
			 connectionString="User Id={user};Password={senha};Data Source=localhost:1521/{service_name};"
			 providerName="Oracle.ManagedDataAccess.Client" />
	</connectionStrings>
```
Substitua `{user}`, `{senha}` e `{service_name}` pelos valores correspondentes ao seu ambiente.

### 3. Observações

- Script para criação do banco de dados e objetos necessários está disponível [aqui](https://github.com/kennedyAlvess/RHControl/blob/master/ExportDataBase/exportar.sql).

Certifique-se de que os objetos do banco (view, function e procedure) estejam devidamente criados antes da execução do cálculo de salários.
O relatório .rpt está localizado na pasta Reports e pode ser customizado conforme necessário.
O login inicial pode ser feito com um usuário cadastrado manualmente; (O script disponibilizado para criação do banco de dados possui um usuário pré-cadastrado com o nome de `kennedy` e senha `123mudar`).