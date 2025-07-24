import React from 'react';
import {
  Dialog,
  DialogTitle,
  DialogContent,
  DialogActions,
  Button,
  Typography,
  Box
} from '@mui/material';
import WarningAmberOutlinedIcon from '@mui/icons-material/WarningAmberOutlined';

type Props = {
  open: boolean;
  title: string;
  message: string;
  onCancel: () => void;
  onConfirm: () => void;
};

export function ConfirmDialog({
  open,
  title,
  message,
  onCancel,
  onConfirm,
}: Props) {
  const confirmLabel = title.startsWith('Excluir') ? 'Excluir' : 'Confirmar';

  return (
    <Dialog
      open={open}
      onClose={onCancel}
      aria-labelledby="confirm-dialog-title"
      aria-describedby="confirm-dialog-description"
      transitionDuration={{ enter: 300, exit: 200 }}
      role="alertdialog"
    >
      <DialogTitle id="confirm-dialog-title">
        <Box display="flex" alignItems="center" gap={1}>
          <WarningAmberOutlinedIcon color="warning" />
          <Typography variant="h6">
            {title.endsWith('?') ? title : `${title}?`}
          </Typography>
        </Box>
      </DialogTitle>

      <DialogContent dividers>
        <Typography
          id="confirm-dialog-description"
          variant="body1"
          align="center"
        >
          {message}
        </Typography>
      </DialogContent>

      <DialogActions sx={{ justifyContent: 'center', gap: 2, p: 2 }}>
        <Button onClick={onCancel} variant="outlined">
          Cancelar
        </Button>
        <Button
          onClick={onConfirm}
          variant="contained"
          color="error"
          autoFocus
        >
          {confirmLabel}
        </Button>
      </DialogActions>
    </Dialog>
  );
}
