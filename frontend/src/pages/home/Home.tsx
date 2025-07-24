import React, { useState } from 'react';
import {
  Alert,
  Box,
  Collapse,
  Container,
  IconButton,
  LinearProgress,
  Typography,
} from '@mui/material';
import RefreshIcon from '@mui/icons-material/Refresh';

import { DataTable, Column } from '../../components/DataTable';
import { ConfirmDialog } from '../../components/ConfirmDialog';
import { PessoaDialog } from '../../components/PessoaDialog';

import { Pessoa } from '../../types/pessoa';
import { usePessoas, PessoaRow } from '../../hooks/usePessoas';
import { usePessoaForm } from '../../hooks/usePessoaForm';

export default function Pessoas() {
  const {
    rows,
    loading,
    alertMsg,
    alertType,
    showAlert,
    load,
    savePessoa,
    fetchPessoaById,
    deletePessoaById,
  } = usePessoas();

  const {
    form,
    formTouched,
    cpfError,
    emailError,
    handleChange,
    handleEnderecoChange,
    handleSave,
    resetForm,
  } = usePessoaForm();

  const [editRow, setEditRow] = useState<Pessoa | null>(null);
  const [deleteRow, setDeleteRow] = useState<PessoaRow | null>(null);

  const [openDialog, setOpenDialog] = useState(false);
  const [openConfirm, setOpenConfirm] = useState(false);
  const [loadingPessoa, setLoadingPessoa] = useState(false);

  function formatCPF(cpf: string): string {
    const onlyDigits = cpf.replace(/\D/g, '');
    if (onlyDigits.length !== 11) return cpf;
    return onlyDigits.replace(/(\d{3})(\d{3})(\d{3})(\d{2})/, '$1.$2.$3-$4');
  }

  const columns: Column<PessoaRow>[] = [
    { field: 'id', headerName: 'ID' },
    { field: 'nome', headerName: 'Nome' },
    { field: 'cpf', headerName: 'CPF', render: (value) => formatCPF(value as string) },
    { field: 'email', headerName: 'Email' },
  ];
  const onSubmit = async (data: Pessoa) => {
    try {
      await savePessoa(data);
      setOpenDialog(false);
      resetForm();
    } catch {
      //erro tratado no hook
    }
  };

  const handleEdit = async (row: PessoaRow) => {
    setLoadingPessoa(true);
    try {
      const pessoaCompleta = await fetchPessoaById(Number(row.id));
      setEditRow(pessoaCompleta);
      setOpenDialog(true);
    } catch {
      // erro tratado no hook
    } finally {
      setLoadingPessoa(false);
    }
  };

  const handleDeleteConfirm = async () => {
    try {
      await deletePessoaById(deleteRow);
    } finally {
      setDeleteRow(null);
      setOpenConfirm(false);
    }
  };

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
          variant="filled"
          severity={alertType}
          sx={{ mb: 2 }}
          onClose={() => showAlert('')}
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
        onDelete={(row) => {
          setDeleteRow(row);
          setOpenConfirm(true);
        }}
        onAdd={() => {
          setEditRow(null);
          resetForm();
          setOpenDialog(true);
        }}
        loading={loading}
      />

      <PessoaDialog
        open={openDialog}
        initial={editRow || undefined}
        onClose={() => {
          setOpenDialog(false);
          setEditRow(null);
        }}
        onSubmit={onSubmit}
      />

      <ConfirmDialog
        open={openConfirm}
        title="Confirmar Exclusão"
        message={`Deseja realmente excluir "${deleteRow?.nome}" ?`}
        onCancel={() => setOpenConfirm(false)}
        onConfirm={handleDeleteConfirm}
      />
    </Container>
  );
}
