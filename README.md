
# 📋 Cadastro de Pessoas - Fullstack (.NET 9 + React)

Este repositório contém a aplicação completa de **Cadastro de Pessoas**, com backend em ASP.NET 9 e frontend em React + Vite. O objetivo é demonstrar uma arquitetura separada por camadas e consumo de API RESTful com autenticação JWT.

---

## 📁 Estrutura do Projeto

- `/backend`  → Backend (.NET 9 Web API)
- `/frontend` → Frontend (React + Vite)

---

## 🔗 Repositórios Individuais

- [📦 Backend (ASP.NET 9)](./backend/CadastroPessoasApi/README.md)
- [💻 Frontend (React + Vite)](./frontend/README.md)

---

## 🧪 Como rodar o projeto completo

### 1. Clone o repositório
```bash
git clone https://github.com/DaviOGontijo/cadastro-pessoas
cd cadastro-pessoas
```

### 2. Rode o backend (.NET 9)
```bash
cd backend
dotnet restore
dotnet ef database update
dotnet run
```
> A API estará disponível em: https://localhost:5001

### 3. Rode o frontend (React)
```bash
cd frontend
npm install
npm run dev
```
O frontend estará em: http://localhost:5173

> ⚠️ Certifique-se de que o backend esteja rodando primeiro, pois o frontend consome os dados via API REST.

---
##  🧑‍💻 Desenvolvedor
 

**Davi Gontijo**

Desenvolvedor Fullstack .NET / React
 
[LinkedIn](https://linkedin.com/in/davigontijo) | davi0400@gmail.com

---
##  📝 Licença
Projeto open-source para fins demonstrativos e de avaliação técnica.
