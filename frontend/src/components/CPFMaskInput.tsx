import React from 'react';
import { IMaskInput } from 'react-imask';

interface CustomProps {
  onChange: (event: { target: { name: string; value: string } }) => void;
  name: string;
  value?: string; // valor controlado opcional
}

/**
 * Componente de input para CPF com máscara usando react-imask.
 * Propaga valor formatado no padrão esperado pelo MUI (evento com target.name e target.value).
 */
const CPFMaskInput = React.forwardRef<HTMLInputElement, CustomProps>(
  function CPFMaskInput(props, ref) {
    const { onChange, name, ...other } = props;

    return (
      <IMaskInput
        {...other}
        mask="000.000.000-00"
        inputRef={ref}
        onAccept={(value: string) => onChange({ target: { name, value } })}
        overwrite
      />
    );
  }
);

export default CPFMaskInput;
