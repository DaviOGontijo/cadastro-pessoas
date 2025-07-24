import React from 'react';
import {
  Dialog, DialogTitle, DialogContent, DialogActions,
  TextField, Button, Grid, IconButton, useMediaQuery, useTheme, FormControl, InputLabel, MenuItem, Select
} from '@mui/material';
import CloseIcon from '@mui/icons-material/Close';
import { Pessoa } from '../types/pessoa';
import CPFMaskInput from './CPFMaskInput';
import { usePessoaForm } from "../hooks/usePessoaForm";

type Props = {
  open: boolean;
  initial?: Partial<Pessoa>;
  onClose: () => void;
  onSubmit: (data: Pessoa) => void;
};

export function PessoaDialog({ open, initial, onClose, onSubmit }: Props) {
  const theme = useTheme();
  const fullScreen = useMediaQuery(theme.breakpoints.down('md'));

  const {
    form, formTouched, cpfError, emailError,
    handleChange, handleEnderecoChange,
    validateForm, handleSave, setCpfError, setEmailError
  } = usePessoaForm(initial);

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
          {/* Nome */}
          <Grid size ={{xs: 12, sm: 6}}>
            <TextField
              label="Nome"
              value={form.nome}
              onChange={e => handleChange('nome', e.target.value)}
              fullWidth required
              error={formTouched && !form.nome}
              helperText={formTouched && !form.nome && 'Nome obrigatório'}
            />
          </Grid>

          {/* Sexo */}
          <Grid size ={{xs: 12, sm: 3}}>
            <FormControl fullWidth>
              <InputLabel id="sexo-label">Sexo</InputLabel>
              <Select
                labelId="sexo-label"
                value={form.sexo || ''}
                label="Sexo"
                onChange={e => handleChange('sexo', e.target.value)}
              >
                <MenuItem value="">Nenhum</MenuItem>
                <MenuItem value="Masculino">Masculino</MenuItem>
                <MenuItem value="Feminino">Feminino</MenuItem>
                <MenuItem value="Outro">Outro</MenuItem>
              </Select>
            </FormControl>
          </Grid>

          {/* Data de Nascimento */}
          <Grid size ={{xs: 12, sm: 3}}>
            <TextField
              type="date"
              label="Data de Nascimento"
              value={form.dataNascimento}
              onChange={e => handleChange('dataNascimento', e.target.value)}
              fullWidth
              InputLabelProps={{ shrink: true }}
              required
              error={formTouched && !form.dataNascimento}
              helperText={formTouched && !form.dataNascimento && 'Data obrigatória'}
            />
          </Grid>

          {/* Email */}
          <Grid size ={{xs: 12, sm: 3}}>
            <TextField
              label="Email"
              value={form.email}
              onChange={e => {
                handleChange('email', e.target.value);
                setEmailError('');
              }}
              fullWidth
              error={!!emailError}
              helperText={emailError}
            />
          </Grid>

          {/* CPF */}
          <Grid size ={{xs: 12, sm: 3}}>
            <TextField
              label="CPF"
              value={form.cpf}
              onChange={e => {
                handleChange('cpf', e.target.value);
                setCpfError('');
              }}
              fullWidth required
              error={!!cpfError}
              helperText={cpfError}
              InputProps={{
                inputComponent: CPFMaskInput as any
              }}
            />
          </Grid>

          {/* Nacionalidade e Naturalidade */}
          <Grid size ={{xs: 12, sm: 3}}>
            <TextField label="Nacionalidade" value={form.nacionalidade} onChange={e => handleChange('nacionalidade', e.target.value)} fullWidth />
          </Grid>
          <Grid size ={{xs: 12, sm: 3}}>
            <TextField label="Naturalidade" value={form.naturalidade} onChange={e => handleChange('naturalidade', e.target.value)} fullWidth />
          </Grid>

          <Grid size ={{xs: 12}}><strong>Endereço</strong></Grid>

          {/* Endereço */}
          <Grid size ={{xs: 12, sm: 6}}>
            <TextField label="Logradouro" value={form.endereco.logradouro} onChange={e => handleEnderecoChange('logradouro', e.target.value)} fullWidth required />
          </Grid>
          <Grid size ={{xs: 12, sm: 3}}>
            <TextField label="Número" value={form.endereco.numero} onChange={e => handleEnderecoChange('numero', e.target.value)} fullWidth required />
          </Grid>
          <Grid size ={{xs: 12, sm: 3}}>
            <TextField label="Complemento" value={form.endereco.complemento} onChange={e => handleEnderecoChange('complemento', e.target.value)} fullWidth />
          </Grid>
          <Grid size ={{xs: 12, sm: 4}}>
            <TextField label="Bairro" value={form.endereco.bairro} onChange={e => handleEnderecoChange('bairro', e.target.value)} fullWidth required />
          </Grid>
          <Grid size ={{xs: 12, sm: 4}}>
            <TextField label="Cidade" value={form.endereco.cidade} onChange={e => handleEnderecoChange('cidade', e.target.value)} fullWidth required />
          </Grid>
          <Grid size ={{xs: 12, sm: 2}}>
            <TextField label="Estado" value={form.endereco.estado} onChange={e => handleEnderecoChange('estado', e.target.value)} fullWidth required />
          </Grid>
          <Grid size ={{xs: 12, sm: 2}}>
            <TextField label="CEP" value={form.endereco.cep} onChange={e => handleEnderecoChange('cep', e.target.value)} fullWidth required />
          </Grid>
        </Grid>
      </DialogContent>

      <DialogActions sx={{ px: 3, py: 2 }}>
        <Button onClick={onClose}>Cancelar</Button>
        <Button onClick={() => handleSave(onSubmit, onClose)} variant="contained" disabled={!validateForm()}>
          {initial ? 'Salvar alterações' : 'Cadastrar'}
        </Button>
      </DialogActions>
    </Dialog>
  );
}
