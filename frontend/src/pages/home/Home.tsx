import React, { useEffect, useState } from 'react';
import {
  Alert,
  AlertColor,
  Box,
  Collapse,
  Container,
  IconButton,
  LinearProgress,
  Typography,
  Chip
} from '@mui/material';
import RefreshIcon from '@mui/icons-material/Refresh';

import { DataTable, Column } from '../../components/DataTable';
import { ConfirmDialog } from '../../components/ConfirmDialog';
import { PessoaDialog } from '../../components/PessoaDialog';

import {
  getPessoa,
  createPessoa,
  updatePessoa,
  deletePessoa,
  getPessoaById
} from '../../services/pessoaFisicaService';

import { Pessoa } from '../../types/pessoa';

type PessoaRow = {
  id: string;
  nome: string;
  cpf: string;
  email: string;
  idPessoa: number;
};

export default function Pessoas() {
  const [rows, setRows] = useState<PessoaRow[]>([]);
  const [editRow, setEditRow] = useState<Pessoa | null>(null);
  const [deleteRow, setDeleteRow] = useState<PessoaRow | null>(null);
  const [loadingPessoa, setLoadingPessoa] = useState(false);

  const [openDialog, setOpenDialog] = useState(false);
  const [openConfirm, setOpenConfirm] = useState(false);

  const [alertMsg, setAlertMsg] = useState('');
  const [alertType, setAlertType] = useState<AlertColor>('success');
  const [loading, setLoading] = useState(false);

  const showAlert = (msg: string, type: AlertColor = 'success') => {
    setAlertMsg(msg);
    setAlertType(type);
    setTimeout(() => setAlertMsg(''), 3000);
  };

  const load = async () => {
    setLoading(true);
    try {
      const pessoa = await getPessoa(); // supondo que retorna uma pessoa ou lista (verifique)
      const data: Pessoa[] = Array.isArray(pessoa) ? pessoa : [pessoa];

      const mapped = data.map(p => ({
        id: String(p.id),
        nome: p.nome,
        cpf: p.cpf,
        email: p.email,
        idPessoa: p.id
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

  const handleSubmit = async (data: Pessoa) => {
    try {
      if (data.id) {
        await updatePessoa(data.id, data);
        showAlert(`Pessoa ${data.nome} editada com sucesso`);
      } else {
        await createPessoa(data);
        showAlert(`Pessoa ${data.nome} cadastrada com sucesso`);
      }
      setOpenDialog(false);
      load();
    } catch (err) {
      console.error('Erro ao salvar pessoa:', err);
      showAlert('Erro ao salvar pessoa. Contate o administrador.', 'error');
    }
  };
  const handleEdit = async (row: { id: string }) => {
    try {
      setLoadingPessoa(true);
      const pessoaCompleta = await getPessoaById(Number(row.id));
      setEditRow(pessoaCompleta);
      setOpenDialog(true);
    } catch (err) {
      console.error('Erro ao buscar pessoa:', err);
      showAlert('Erro ao buscar dados da pessoa.', 'error');
    } finally {
      setLoadingPessoa(false);
    }
  };
  const handleDelete = async () => {
    if (deleteRow) {
      try {
        await deletePessoa(deleteRow.idPessoa);
        showAlert(`Pessoa ${deleteRow.nome} excluída com sucesso`);
      } catch (err) {
        console.error('Erro ao excluir pessoa:', err);
        showAlert(`Erro ao excluir pessoa ${deleteRow.nome}`, 'error');
      } finally {
        setDeleteRow(null);
        setOpenConfirm(false);
        load();
      }
    }
  };

  const columns: Column<PessoaRow>[] = [
    { field: 'id', headerName: 'ID' },
    { field: 'nome', headerName: 'Nome' },
    { field: 'cpf', headerName: 'CPF' },
    { field: 'email', headerName: 'Email' },
  ];

  return (
    <Container>
      <Box display="flex" alignItems="center" justifyContent="space-between" mb={1}>
        <Box>
          <Typography variant="h4" gutterBottom>Pessoas</Typography>
          <Typography variant="subtitle1" gutterBottom>
            Gerencie o cadastro de pessoas físicas.
          </Typography>
        </Box>
        <IconButton onClick={load} aria-label="Recarregar" color="primary" disabled={loading}>
          <RefreshIcon />
        </IconButton>
      </Box>

      <Collapse in={!!alertMsg}>
        <Alert
          variant='filled'
          severity={alertType}
          sx={{ mb: 2 }}
          onClose={() => setAlertMsg('')}
        >
          {alertMsg}
        </Alert>
      </Collapse>

      {loading && <LinearProgress sx={{ mb: 1 }} />}

      <DataTable
        columns={columns}
        rows={rows}
        filterField={(row, f) =>
          Object.values(row).some(v =>
            String(v).toLowerCase().includes(f.toLowerCase())
          )
        }
        onEdit={handleEdit}
        onDelete={row => {
          setDeleteRow(row);
          setOpenConfirm(true);
        }}
        onAdd={() => {
          setEditRow(null);
          setOpenDialog(true);
        }}
        loading={loading}
      />

      <PessoaDialog
        open={openDialog}
        initial={editRow || undefined}
        onClose={() => { setOpenDialog(false); setEditRow(null); }}
        onSubmit={handleSubmit}
      />

      <ConfirmDialog
        open={openConfirm}
        title="Confirmar Exclusão"
        message={`Deseja realmente excluir "${deleteRow?.nome}" ?`}
        onCancel={() => setOpenConfirm(false)}
        onConfirm={handleDelete}
      />
    </Container>
  );
}
