# 📋 Cadastro de Pessoas - API .NET 9

Este projeto é uma aplicação fullstack desenvolvida como desafio técnico para cadastro de pessoas físicas, com backend em .NET 9.

---

## 🚀 Tecnologias utilizadas

- ✅ ASP.NET 9 Web API
- ✅ Entity Framework Core + SQLite
- ✅ Swagger (documentação)
- ✅ Padrões SOLID e Clean Code
- ✅ DTOs para entrada e saída
- ✅ Table-per-hierarchy (TPH) com herança
- ✅ Separação por camadas (Controllers, Services, Repositories, Validators)

---

## 🧱 Estrutura de Pastas

```
/Models
  Pessoa.cs
  Endereco.cs
/DTOs
  CadastroDto.cs
/DTOs/V1
  PessoaFisicaCreateDtoV1.cs
  PessoaFisicaReadDtoV1.cs
/DTOs/V2
  PessoaFisicaCreateDtoV2.cs
  PessoaFisicaReadDtoV2.cs
/Data
  PessoaDbContext.cs
/Repositories
  IPessoaRepository.cs
  PessoaRepository.cs
/Services
  IPessoaFisicaServiceV1.cs
  PessoaFisicaServiceV1.cs
  IPessoaFisicaServiceV2.cs
  PessoaFisicaServiceV2.cs
  ITokenService.cs
  TokenService.cs
/Controllers
  AuthController.cs
/Controllers/V1
  PessoaFisicaControllerV1.cs
/Controllers/V2
  PessoaFisicaControllerV2.cs
/Validators
  CpfCnpjValidator.cs
```

---

## 🧠 Funcionalidades

### Autenticação
- Login (`POST /api/auth/login`)
- Registro (`POST /api/auth/register`)
### Pessoa Física
## V1
- - Criar (`POST /api/v1/pessoafisica`)
- Buscar por ID (`GET /api/v1/pessoafisica/{id}`)
- Listar todas (`GET /api/v1/pessoafisica`)
- Atualizar (`PUT /api/v1/pessoafisica/{id}`)
- Deletar (`DELETE /api/v1/pessoafisica/{id}`)

## V2
- - Criar (`POST /api/v2/pessoafisica`)
- Buscar por ID (`GET /api/v2/pessoafisica/{id}`)
- Listar todas (`GET /api/v2/pessoafisica`)
- Atualizar (`PUT /api/v2/pessoafisica/{id}`)
- Deletar (`DELETE /api/v2/pessoafisica/{id}`)

---

## 🧪 Como rodar o projeto localmente

### 1. Clone o repositório
```bash
git clone https://github.com/DaviOGontijo/cadastro-pessoas
cd backend
```

### 2. Restaure os pacotes
```bash
dotnet restore
```

### 3. Crie o banco e aplique a migration
```bash
dotnet ef database update
```

> O banco será criado localmente com o nome `cadastro.db` usando SQLite.

### 4. Rode a aplicação
```bash
dotnet run
```

---

## 📖 Documentação da API

Acesse via Swagger:

```
https://localhost:5001/swagger
```

> ⚠️ O Swagger está habilitado para produção também.

---

## 🧑‍💻 Desenvolvedor

**Davi Gontijo**  
Desenvolvedor Fullstack .NET / React  
[LinkedIn](https://linkedin.com/in/davigontijo) | davi0400@gmail.com

---

## 📝 Licença

Projeto open-source para fins demonstrativos e de avaliação técnica.
