import { useEffect, useState } from 'react';
import { Pessoa } from '../types/pessoa';
import { validateCPF, validateEmail } from '../utils/validators';

const emptyForm: Pessoa = {
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
};

function parseDate(dateStr?: string): string {
  if (!dateStr) return '';
  return dateStr.split('T')[0];
}

export function usePessoaForm(initial?: Partial<Pessoa>) {
  const [form, setForm] = useState<Pessoa>(emptyForm);
  const [cpfError, setCpfError] = useState('');
  const [emailError, setEmailError] = useState('');
  const [formTouched, setFormTouched] = useState(false);
  const [errors, setErrors] = useState<{ [key: string]: string }>({});

  useEffect(() => {
    if (initial) {
      setForm({
        ...emptyForm,
        ...initial,
        dataNascimento: parseDate(initial.dataNascimento),
        endereco: { ...emptyForm.endereco, ...initial.endereco }
      });
    } else {
      resetForm();
    }
  }, [initial]);

  const handleChange = (field: keyof Pessoa, value: any) => {
    setForm(prev => ({ ...prev, [field]: value }));

    // Validação em tempo real
    if (field === 'nome' && !value.trim()) {
      setErrors(prev => ({ ...prev, nome: 'Nome obrigatório' }));
    } else if (field === 'dataNascimento' && !value) {
      setErrors(prev => ({ ...prev, dataNascimento: 'Data obrigatória' }));
    } else {
      setErrors(prev => ({ ...prev, [field]: '' }));
    }

    // Validação específica
    if (field === 'email') {
      const msg = validateEmail(value);
      setErrors(prev => ({ ...prev, email: msg }));

    }

    if (field === 'cpf') {
      const rawCpf = value.replace(/\D/g, '');
      const msg = validateCPF(rawCpf);
       setErrors(prev => ({ ...prev, cpf: msg }));
    }
  };

  const handleEnderecoChange = (field: keyof Pessoa['endereco'], value: any) => {
    setForm(prev => ({
      ...prev,
      endereco: {
        ...prev.endereco,
        [field]: value
      }
    }));

    if (
      ['logradouro', 'numero', 'bairro', 'cidade', 'estado', 'cep'].includes(field) &&
      !value.trim()
    ) {
      setErrors(prev => ({ ...prev, [field]: 'Campo obrigatório' }));
    } else {
      setErrors(prev => ({ ...prev, [field]: '' }));
    }
  };

  const validateForm = (): boolean => {
    const rawCpf = form.cpf.replace(/\D/g, '');
    const requiredFields = [
      form.nome.trim(),
      form.dataNascimento.trim(),
      rawCpf,
      form.endereco.logradouro.trim(),
      form.endereco.numero.trim(),
      form.endereco.bairro.trim(),
      form.endereco.cidade.trim(),
      form.endereco.estado.trim(),
      form.endereco.cep.trim()
    ];
    const isValid = requiredFields.every(Boolean) &&
      validateCPF(rawCpf) === '' &&
      validateEmail(form.email) === '';
    return isValid;
  };

  const resetForm = () => {
    setForm(emptyForm);

  };

  const handleSave = (onSubmit: (data: Pessoa) => void, onClose: () => void) => {
    setFormTouched(true);
    const rawCpf = form.cpf.replace(/\D/g, '');
    const emailMsg = validateEmail(form.email);
    const cpfMsg = validateCPF(rawCpf);

    setEmailError(emailMsg);
    setCpfError(cpfMsg);

    if (emailMsg || cpfMsg || !validateForm()) return;

    const data = { ...form, cpf: rawCpf };

    try {
      onSubmit(data);
      resetForm();
      onClose();
    } catch (err: any) {
      if (err.response?.status === 400 && typeof err.response?.data === 'string') {
        if (err.response.data.includes('CPF')) {
          setCpfError(err.response.data);
        } else {
        }
      } else {
      }
    }
  };

  return {
    form,
    formTouched,
    cpfError,
    emailError,
    errors,
    handleChange,
    handleEnderecoChange,
    validateForm,
    resetForm,
    handleSave,
    setEmailError,
    setCpfError
  };
}
