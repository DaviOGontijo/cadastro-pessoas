import { useState, useEffect } from 'react';
import { AlertColor } from '@mui/material';
import {
  getPessoa,
  createPessoa,
  updatePessoa,
  deletePessoa,
  getPessoaById,
} from '../services/pessoaFisicaService';
import { Pessoa } from '../types/pessoa';

export type PessoaRow = {
  id: string;
  nome: string;
  cpf: string;
  email: string;
  idPessoa: number;
};

export function usePessoas() {
  const [rows, setRows] = useState<PessoaRow[]>([]);
  const [loading, setLoading] = useState(false);
  const [alertMsg, setAlertMsg] = useState('');
  const [alertType, setAlertType] = useState<AlertColor>('success');

  const showAlert = (msg: string, type: AlertColor = 'success') => {
    setAlertMsg(msg);
    setAlertType(type);
    setTimeout(() => setAlertMsg(''), 3000);
  };

  const load = async () => {
    setLoading(true);
    try {
      const pessoa = await getPessoa();
      const data: Pessoa[] = Array.isArray(pessoa) ? pessoa : [pessoa];

      const mapped = data.map(p => ({
        id: String(p.id),
        nome: p.nome,
        cpf: p.cpf,
        email: p.email,
        idPessoa: p.id,
      }));

      setRows(mapped);
    } catch (err) {
      console.error('Erro ao carregar pessoas', err);
      showAlert('Erro ao carregar pessoas', 'error');
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    load();
  }, []);

  const savePessoa = async (data: Pessoa) => {
    if (!data) return;
    try {
      if (data.id) {
        await updatePessoa(data.id, data);
        showAlert(`Pessoa ${data.nome} editada com sucesso`);
      } else {
        await createPessoa(data);
        showAlert(`Pessoa ${data.nome} cadastrada com sucesso`);
      }
      await load();
    } catch (err:any) {

      if (err.response?.status === 400 && typeof err.response.data === 'string' &&
        err.response.data.toLowerCase().includes('cpf já cadastrado')) {
          showAlert('Erro ao salvar pessoa. CPF já cadastrado!.', 'error');
      throw err
    }

    showAlert('Erro ao salvar pessoa. Contate o administrador.', 'error');
      throw err;
    }
  };

  const fetchPessoaById = async (id: number) => {
    try {
      const pessoaCompleta = await getPessoaById(id);
      return pessoaCompleta;
    } catch (err) {
      console.error('Erro ao buscar pessoa:', err);
      showAlert('Erro ao buscar dados da pessoa.', 'error');
      throw err;
    }
  };

  const deletePessoaById = async (row: PessoaRow | null) => {
    if (!row) return;
    try {
      await deletePessoa(row.idPessoa);
      showAlert(`Pessoa ${row.nome} excluída com sucesso`);
      await load();
    } catch (err) {
      console.error('Erro ao excluir pessoa:', err);
      showAlert(`Erro ao excluir pessoa ${row.nome}`, 'error');
      throw err;
    }
  };

  return {
    rows,
    loading,
    alertMsg,
    alertType,
    showAlert,
    load,
    savePessoa,
    fetchPessoaById,
    deletePessoaById,
  };
}
