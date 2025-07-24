export function validateEmail(email: string): string {
  if (!email) return '';
  const regex = /^[\w.-]+@[\w.-]+\.\w{2,}$/;
  return regex.test(email) ? '' : 'Email inválido';
}

export function validateCPF(cpf: string): string {
  const onlyDigits = cpf.replace(/\D/g, '');

  if (onlyDigits.length !== 11) return 'CPF inválido';

  // Checa sequências inválidas tipo 11111111111
  if (/^(\d)\1{10}$/.test(onlyDigits)) return 'CPF inválido';

  const calcDigit = (digits: string, factor: number) => {
    let total = 0;
    for (let i = 0; i < digits.length; i++) {
      total += Number(digits[i]) * (factor - i);
    }
    const rest = total % 11;
    return rest < 2 ? 0 : 11 - rest;
  };

  const digit1 = calcDigit(onlyDigits.substring(0, 9), 10);
  const digit2 = calcDigit(onlyDigits.substring(0, 10), 11);

  if (digit1 === Number(onlyDigits[9]) && digit2 === Number(onlyDigits[10])) {
    return '';
  }

  return 'CPF inválido';
}
