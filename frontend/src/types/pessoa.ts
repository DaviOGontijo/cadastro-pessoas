export interface Pessoa {
  id: number;
  nome: string;
  sexo: string;
  email: string;
  dataNascimento: string; // ISO string, ex: "2025-07-23T21:00:18.191Z"
  naturalidade: string;
  nacionalidade: string;
  cpf: string;
  endereco: Endereco;
}

export interface Endereco {
  logradouro: string;
  numero: string;
  complemento?: string;
  bairro: string;
  cidade: string;
  estado: string;
  cep: string;
}
