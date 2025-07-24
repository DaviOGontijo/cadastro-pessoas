// DataTable.tsx
import React from 'react';
import {
  Table, TableBody, TableCell, TableContainer,
  TableHead, TableRow, Paper,
  IconButton, TextField, Box, TablePagination, Button,
  TableSortLabel, InputAdornment, Skeleton, Tooltip
} from '@mui/material';
import EditIcon from '@mui/icons-material/Edit';
import DeleteIcon from '@mui/icons-material/Delete';
import SearchIcon from '@mui/icons-material/Search';
import AddIcon from '@mui/icons-material/Add';
import { useTableData } from '../hooks/useTableData';

export type Column<T> = {
  field: keyof T;
  headerName: string;
  render?: (value: T[keyof T], row: T) => React.ReactNode;
};

type DataTableProps<T> = {
  columns: Column<T>[];
  rows: T[];
  filterField: (row: T, filter: string) => boolean;
  onEdit?: (row: T) => void;
  onDelete?: (row: T) => void;
  onAdd?: () => void;
  loading?: boolean;
};

export function DataTable<T extends { id: string }>({
  columns, rows, filterField, onEdit, onDelete, onAdd, loading = false,
}: DataTableProps<T>) {
  const [filter, setFilter] = React.useState('');
  const [page, setPage] = React.useState(0);
  const [rowsPerPage, setRowsPerPage] = React.useState(25);
  const [order, setOrder] = React.useState<'asc' | 'desc'>('asc');
  const [orderBy, setOrderBy] = React.useState<keyof T | null>(null);

  const { paged, filteredCount } = useTableData(rows, filter, filterField, orderBy, order, page, rowsPerPage);

  const handleSort = (field: keyof T) => {
    if (loading) return;
    const isAsc = orderBy === field && order === 'asc';
    setOrder(isAsc ? 'desc' : 'asc');
    setOrderBy(field);
  };

  const colSpanCount = columns.length + (onEdit || onDelete ? 1 : 0);

  const renderSkeletonRow = (index: number) => (
    <TableRow key={`skeleton-${index}`}>
      {columns.map((col, i) => (
        <TableCell key={`skeleton-cell-${i}`}>
          <Skeleton variant="text" />
        </TableCell>
      ))}
      {(onEdit || onDelete) && <TableCell><Skeleton variant="rectangular" width={40} height={24} /></TableCell>}
    </TableRow>
  );

  return (
    <>
      <Box sx={{ display: 'flex', mb: 2, gap: 1 }}>
        <TextField
          placeholder="Buscar..."
          variant="outlined"
          size="small"
          fullWidth
          value={filter}
          onChange={e => { setFilter(e.target.value); setPage(0); }}
          disabled={loading}
          InputProps={{
            startAdornment: (
              <InputAdornment position="start">
                <SearchIcon />
              </InputAdornment>
            )
          }}
        />
        {onAdd && (
          <Button
            color="primary"
            variant="contained"
            endIcon={<AddIcon />}
            onClick={onAdd}
            disabled={loading}
          >
            Adicionar
          </Button>
        )}
      </Box>
      <Paper variant='outlined' sx={{ width: '100%', overflow: 'hidden', p: 2 }}>
        <TableContainer sx={{ maxHeight: 500 }}>
          <Table stickyHeader size="small" aria-label="sticky table">
            <TableHead>
              <TableRow>
                {columns.map(col => (
                  <TableCell key={String(col.field)}>
                    <TableSortLabel
                      active={orderBy === col.field}
                      direction={orderBy === col.field ? order : 'asc'}
                      onClick={() => handleSort(col.field)}
                      disabled={loading}
                    >
                      {col.headerName}
                    </TableSortLabel>
                  </TableCell>
                ))}
                {(onEdit || onDelete) && <TableCell align="right">Ações</TableCell>}
              </TableRow>
            </TableHead>
            <TableBody>
              {loading
                ? Array(rowsPerPage).fill(null).map((_, i) => renderSkeletonRow(i))
                : paged.length > 0
                  ? paged.map(row => (
                    <TableRow hover key={row.id}>
                      {columns.map(col => {
                        const cellValue = row[col.field];
                        return (
                          <TableCell key={String(col.field)}>
                            {col.render ? col.render(cellValue, row) : String(cellValue)}
                          </TableCell>
                        );
                      })}
                      {(onEdit || onDelete) && (
                        <TableCell align="right">
                          {onEdit && (
                            <Tooltip title="Editar">
                              <IconButton onClick={() => onEdit(row)} size="small" disabled={loading}>
                                <EditIcon fontSize="small" />
                              </IconButton>
                            </Tooltip>
                          )}
                          {onDelete && (
                            <Tooltip title="Excluir">
                              <IconButton onClick={() => onDelete(row)} size="small" disabled={loading}>
                                <DeleteIcon fontSize="small" />
                              </IconButton>
                            </Tooltip>
                          )}
                        </TableCell>
                      )}
                    </TableRow>
                  ))
                  : (
                    <TableRow>
                      <TableCell colSpan={colSpanCount} align="center">
                        Nenhum registro encontrado.
                      </TableCell>
                    </TableRow>
                  )
              }
            </TableBody>
          </Table>
        </TableContainer>
        <TablePagination
          rowsPerPageOptions={[10, 25, 50]}
          component="div"
          count={filteredCount}
          rowsPerPage={rowsPerPage}
          page={page}
          colSpan={colSpanCount}
          onPageChange={(_, newPage) => setPage(newPage)}
          onRowsPerPageChange={e => {
            setRowsPerPage(parseInt(e.target.value, 10));
            setPage(0);
          }}
          disabled={loading}
          labelRowsPerPage="Linhas por página"
          labelDisplayedRows={({ from, to, count }) =>
            `${from}–${to} de ${count !== -1 ? count : `mais de ${to}`}`
          }
        />
      </Paper>
    </>
  );
}
