
  

#  Cadastro de Pessoas - Frontend

  

  

Este projeto é um frontend em React para o sistema de cadastro e gestão de pessoas físicas, utilizando Vite para desenvolvimento rápido e Material UI para a interface.

  

  

##  Funcionalidades

  

  

- Autenticação de usuário (cadastro e login simples).

  

- Listagem, cadastro, edição e exclusão de pessoas físicas

  
- Busca e filtragem de registros

  

- Interface responsiva e moderna com Material UI

  

- Consumo de API RESTful protegida por JWT

  

  

##  Estrutura do Projeto

  

  

-  `src/pages/`: Páginas principais do sistema (home, login, register)

  

-  `src/components/`: Componentes reutilizáveis (tabela, diálogos, appbar, usermenu)

  

-  `src/services/`: Serviços para comunicação com a API

- `src/hooks/`: Hooks React personalizados que encapsulam lógica de estado, efeitos colaterais e interação com  componentes.
-  `src/types/`: Tipos TypeScript para entidades do domínio

-  `http.ts`: Configuração do Axios para requisições HTTP

  

  

##  Como rodar o projeto

  

  

1. Instale as dependências:

  

  

```bash

  

npm  install

  

```

  

  

2. Inicie o servidor de desenvolvimento:

  

  

```bash

  

npm  run  dev

  

```

  

  

3. Acesse [http://localhost:5173](http://localhost:5173) no navegador.

  

  

##  Scripts disponíveis

  

  

-  `npm run dev` — inicia o ambiente de desenvolvimento

  

-  `npm run build` — gera a build de produção

  

-  `npm run preview` — executa a build localmente

  

-  `npm run lint` — executa o ESLint

  

  

##  Observações

  

  

- O login é simulado: use usuário `admin` e senha `123456`.

  

- A API deve estar rodando em `https://localhost:5001/api/v2`.

  

- O projeto utiliza TypeScript para maior segurança e produtividade.

  

  

---

  

  

Desenvolvido com React, Vite e Material UI.