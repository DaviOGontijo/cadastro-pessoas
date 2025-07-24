import http from '../api/http';
import { Pessoa } from '../types/pessoa';

export const getPessoa = () : Promise<Pessoa> => {
  return http.get<Pessoa>('/PessoaFisica')
    .then(response => response.data);
}

export const getPessoaById = (id: number) : Promise<Pessoa> => {
  return http.get<Pessoa>(`/PessoaFisica/${id}`)
    .then(response => response.data);
}

export const createPessoa = (data: Pessoa) : Promise<Pessoa> => {
  return http.post<Pessoa>('/PessoaFisica', data)
    .then(response => response.data);
}

export const updatePessoa = (id: number, data: Pessoa) : Promise<Pessoa> => {
  return http.put<Pessoa>(`/PessoaFisica/${id}`, data)
    .then(response => response.data);
}

export const deletePessoa = (id: number) : Promise<void> => {
  return http.delete(`/PessoaFisica/${id}`)
    .then(() => {});
}