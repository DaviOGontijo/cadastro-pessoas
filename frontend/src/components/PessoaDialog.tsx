// src/components/PessoaDialog.tsx
import React, { useEffect, useState } from 'react';
import {
  Dialog, DialogTitle, DialogContent, DialogActions,
  TextField, Button, Grid, IconButton, useMediaQuery, useTheme
} from '@mui/material';
import CloseIcon from '@mui/icons-material/Close';
import { Pessoa } from '../types/pessoa';
import { getPessoaById } from '../services/pessoaFisicaService';

type Props = {
  open: boolean;
  initial?: Partial<Pessoa>;
  onClose: () => void;
  onSubmit: (data: Pessoa) => void;
};

export function PessoaDialog({ open, initial, onClose, onSubmit }: Props) {
  const theme = useTheme();
  const fullScreen = useMediaQuery(theme.breakpoints.down('md'));

  const [form, setForm] = useState<Pessoa>({
    id: 0,
    nome: '',
    sexo: '',
    email: '',
    dataNascimento: '',
    naturalidade: '',
    nacionalidade: '',
    cpf: '',
    endereco: {
      logradouro: '',
      numero: '',
      complemento: '',
      bairro: '',
      cidade: '',
      estado: '',
      cep: '',
    }
  });

  useEffect(() => {
  if (initial) {
    setForm({
      id: initial.id ?? 0,
      nome: initial.nome ?? '',
      sexo: initial.sexo ?? '',
      email: initial.email ?? '',
      dataNascimento: initial.dataNascimento ?? '',
      naturalidade: initial.naturalidade ?? '',
      nacionalidade: initial.nacionalidade ?? '',
      cpf: initial.cpf ?? '',
      endereco: {
        logradouro: initial.endereco?.logradouro ?? '',
        numero: initial.endereco?.numero ?? '',
        complemento: initial.endereco?.complemento ?? '',
        bairro: initial.endereco?.bairro ?? '',
        cidade: initial.endereco?.cidade ?? '',
        estado: initial.endereco?.estado ?? '',
        cep: initial.endereco?.cep ?? '',
      }
    });
  } else {
    setForm({
      id: 0,
      nome: '',
      sexo: '',
      email: '',
      dataNascimento: '',
      naturalidade: '',
      nacionalidade: '',
      cpf: '',
      endereco: {
        logradouro: '',
        numero: '',
        complemento: '',
        bairro: '',
        cidade: '',
        estado: '',
        cep: ''
      }
    });
  }
}, [initial]);

  const handleChange = (field: keyof Pessoa, value: any) => {
    setForm(prev => ({ ...prev, [field]: value }));
  };

  const handleEnderecoChange = (field: keyof Pessoa['endereco'], value: any) => {
    setForm(prev => ({
      ...prev,
      endereco: {
        ...prev.endereco,
        [field]: value
      }
    }));
  };

  const isValid = form.nome.trim() !== '' && form.cpf.trim() !== '';

  const handleSave = () => {
    if (!isValid) return;
    onSubmit(form);
  };

  return (
    <Dialog
      open={open}
      onClose={(e, reason) => {
        if (reason === 'backdropClick' || reason === 'escapeKeyDown') return;
        onClose();
      }}
      transitionDuration={{ enter: 300, exit: 200 }}
      fullScreen={fullScreen}
      maxWidth="md"
      fullWidth
    >
      <DialogTitle sx={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center' }}>
        {initial ? 'Editar Pessoa' : 'Cadastrar Pessoa'}
        <IconButton onClick={onClose}><CloseIcon /></IconButton>
      </DialogTitle>

      <DialogContent dividers>
        <Grid container spacing={2}>
          <Grid size={{ xs:12 , sm: 6 }}>
            <TextField label="Nome" value={form.nome} onChange={e => handleChange('nome', e.target.value)} fullWidth required />
          </Grid>
          <Grid size={{ xs:12 , sm: 3}}>
            <TextField label="Sexo" value={form.sexo} onChange={e => handleChange('sexo', e.target.value)} fullWidth />
          </Grid>
          <Grid size={{ xs:12 , sm: 3}}>
            <TextField type="date" label="Data de Nascimento" value={form.dataNascimento} onChange={e => handleChange('dataNascimento', e.target.value)} fullWidth InputLabelProps={{ shrink: true }} />
          </Grid>
          <Grid size={{ xs:12 , sm: 6 }}>
            <TextField label="Email" value={form.email} onChange={e => handleChange('email', e.target.value)} fullWidth />
          </Grid>
          <Grid size={{ xs:12 , sm: 3}}>
            <TextField label="CPF" value={form.cpf} onChange={e => handleChange('cpf', e.target.value)} fullWidth required />
          </Grid>
          <Grid size={{ xs:12 , sm: 3}}>
            <TextField label="Nacionalidade" value={form.nacionalidade} onChange={e => handleChange('nacionalidade', e.target.value)} fullWidth />
          </Grid>
          <Grid size={{ xs:12 , sm: 3}}>
            <TextField label="Naturalidade" value={form.naturalidade} onChange={e => handleChange('naturalidade', e.target.value)} fullWidth />
          </Grid>

          {/* Endereço */}
          <Grid size={{ xs: 12}}><strong>Endereço</strong></Grid>
          <Grid size={{ xs:12 , sm: 6 }}>
            <TextField label="Logradouro" value={form.endereco.logradouro} onChange={e => handleEnderecoChange('logradouro', e.target.value)} fullWidth />
          </Grid>
          <Grid size={{ xs:12 , sm: 3}}>
            <TextField label="Número" value={form.endereco.numero} onChange={e => handleEnderecoChange('numero', e.target.value)} fullWidth />
          </Grid>
          <Grid size={{ xs:12 , sm: 3}}>
            <TextField label="Complemento" value={form.endereco.complemento} onChange={e => handleEnderecoChange('complemento', e.target.value)} fullWidth />
          </Grid>
          <Grid size={{ xs:12, sm: 4}}>
            <TextField label="Bairro" value={form.endereco.bairro} onChange={e => handleEnderecoChange('bairro', e.target.value)} fullWidth />
          </Grid>
          <Grid size={{ xs:12, sm: 4}}>
            <TextField label="Cidade" value={form.endereco.cidade} onChange={e => handleEnderecoChange('cidade', e.target.value)} fullWidth />
          </Grid>
          <Grid size={{ xs:12, sm: 2}}>
            <TextField label="Estado" value={form.endereco.estado} onChange={e => handleEnderecoChange('estado', e.target.value)} fullWidth />
          </Grid>
          <Grid size={{ xs:12, sm: 2}}>
            <TextField label="CEP" value={form.endereco.cep} onChange={e => handleEnderecoChange('cep', e.target.value)} fullWidth />
          </Grid>
        </Grid>
      </DialogContent>

      <DialogActions sx={{ px: 3, py: 2 }}>
        <Button onClick={onClose}>Cancelar</Button>
        <Button onClick={handleSave} variant="contained" disabled={!isValid}>
          {initial ? 'Salvar alterações' : 'Cadastrar'}
        </Button>
      </DialogActions>
    </Dialog>
  );
}
