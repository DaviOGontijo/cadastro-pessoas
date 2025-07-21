
# 📋 Cadastro de Pessoas - API .NET 6 + React (v1)

Este projeto é uma aplicação fullstack desenvolvida como desafio técnico para cadastro de pessoas físicas e jurídicas, com backend em .NET 6 e frontend em React (a ser implementado na v2).

---

## 🚀 Tecnologias utilizadas

- ✅ ASP.NET 6 Web API
- ✅ Entity Framework Core + SQLite
- ✅ Swagger (documentação)
- ✅ Padrões SOLID e Clean Code
- ✅ DTOs para entrada e saída
- ✅ Table-per-hierarchy (TPH) com herança
- ✅ Estrutura separada para Pessoa Física e Jurídica

---

## 🧱 Estrutura de Pastas

```
/Models
  PessoaBase.cs
  PessoaFisica.cs
  PessoaJuridica.cs
/Models/Enums
  TipoPessoa.cs
/DTOs
  PessoaFisicaCreateDto.cs
  PessoaFisicaReadDto.cs
  PessoaJuridicaCreateDto.cs
  PessoaJuridicaReadDto.cs
/Data
  PessoaDbContext.cs
/Repositories
  IPessoaRepository.cs
  PessoaRepository.cs
/Controllers
  PessoaFisicaController.cs
  PessoaJuridicaController.cs
/Validators
  CpfCnpjValidator.cs
```

---

## 🧠 Funcionalidades

### Pessoa Física
- Criar (`POST /api/pessoafisica`)
- Buscar por ID (`GET /api/pessoafisica/{id}`)
- Listar todas (`GET /api/pessoafisica`)
- Atualizar (`PUT /api/pessoafisica/{id}`)
- Deletar (`DELETE /api/pessoafisica/{id}`)

### Pessoa Jurídica
- Criar (`POST /api/pessoajuridica`)
- Buscar por ID (`GET /api/pessoajuridica/{id}`)
- Listar todas (`GET /api/pessoajuridica`)
- Atualizar (`PUT /api/pessoajuridica/{id}`)
- Deletar (`DELETE /api/pessoajuridica/{id}`)

---

## 🧪 Como rodar o projeto localmente

### 1. Clone o repositório
```bash
git clone https://github.com/seu-usuario/seu-repositorio.git
cd seu-repositorio
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