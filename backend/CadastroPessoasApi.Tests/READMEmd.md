
# 📦 Projeto de Testes - NomeDoSeuProjeto.Tests

Este projeto contém os testes automatizados para a aplicação `NomeDoSeuProjeto`, utilizando a biblioteca **xUnit** e ferramenta de cobertura **Coverlet**.

---

## 🚀 Tecnologias e Ferramentas

- [.NET](https://dotnet.microsoft.com/)
- [xUnit](https://xunit.net/)
- [Coverlet](https://github.com/coverlet-coverage/coverlet)
- [Moq](https://github.com/moq/moq4) (mocking)
- [FluentAssertions](https://fluentassertions.com/) (asserts legíveis)

---

## 📂 Estrutura

```
NomeDoSeuProjeto.Tests/
├── Services/
│   └── PessoaFisicaServiceV1Tests.cs
│   └── PessoaFisicaServiceV2Tests.cs
├── Controllers/
│   └── PessoaFisicaControllerV1Tests.cs
│   └── PessoaFisicaControllerV2Tests.cs
│   └── AuthController.cs
├── Repository/
│   └── PessoaRepositoryTests.cs
├── ...
├── CadastroPessoaApi.Tests.csproj
```

---

## ✅ Como rodar os testes

```bash
dotnet test
```


---

## 📝 Observações

Na pasta coveragereport contém o relatório de cobertura, basta abrir o index.html

---

## 👨‍💻 Autor

Davi Gontijo  
[LinkedIn](https://linkedin.com/in/davigontijo) · davi0400@gmail.com