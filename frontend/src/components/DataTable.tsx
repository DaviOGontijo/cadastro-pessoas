import React from 'react';
import {
  Table, TableBody, TableCell, TableContainer,
  TableHead, TableRow, Paper,
  IconButton, TextField, Box, TablePagination, Button,
  TableSortLabel, InputAdornment, Skeleton
} from '@mui/material';
import EditIcon from '@mui/icons-material/Edit';
import DeleteIcon from '@mui/icons-material/Delete';
import SearchIcon from '@mui/icons-material/Search';
import AddIcon from '@mui/icons-material/Add';

export type Column<T> = {
  field: keyof T;
  headerName: string;
  render?: (value: any, row: T) => React.ReactNode;
};

type DataTableProps<T> = {
  columns: Column<T>[];
  rows: T[];
  filterField: (row: T, filter: string) => boolean;
  onEdit?: (row: T) => void;
  onDelete?: (row: T) => void;
  onAdd?: () => void;
  loading?: boolean; // nova prop
};

export function DataTable<T extends { id: string }>({
  columns, rows, filterField, onEdit, onDelete, onAdd, loading = false,
}: DataTableProps<T>) {
  const [filter, setFilter] = React.useState('');
  const [page, setPage] = React.useState(0);
  const [rowsPerPage, setRowsPerPage] = React.useState(25);
  const [order, setOrder] = React.useState<'asc' | 'desc'>('asc');
  const [orderBy, setOrderBy] = React.useState<keyof T | null>(null);

  const filtered = rows.filter(row => filterField(row, filter));

  const sorted = React.useMemo(() => {
    if (!orderBy) return filtered;
    return [...filtered].sort((a, b) => {
      const aVal = a[orderBy];
      const bVal = b[orderBy];
      const cmp = typeof aVal === 'number' && typeof bVal === 'number'
        ? aVal - bVal
        : String(aVal).localeCompare(String(bVal), undefined, { numeric: true });
      return order === 'asc' ? cmp : -cmp;
    });
  }, [filtered, orderBy, order]);
  const paged = sorted.slice(page * rowsPerPage, (page + 1) * rowsPerPage);

  const handleSort = (field: keyof T) => {
    if (loading) return; // bloqueia sort no loading
    const isAsc = orderBy === field && order === 'asc';
    setOrder(isAsc ? 'desc' : 'asc');
    setOrderBy(field);
  };

  const colSpanCount = columns.length + (onEdit || onDelete ? 1 : 0);

  // Linha skeleton para o loading
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
      <Box  sx={{ display: 'flex', mb: 2, gap: 1 }}>
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
                      {columns.map(col => (
                        <TableCell key={String(col.field)}>
                          {col.render ? col.render(row[col.field], row) : (row[col.field] as any)}
                        </TableCell>
                      ))}
                      {(onEdit || onDelete) && (
                        <TableCell align="right">
                          {onEdit && (
                            <IconButton onClick={() => onEdit(row)} size="small">
                              <EditIcon fontSize="small" />
                            </IconButton>
                          )}
                          {onDelete && (
                            <IconButton onClick={() => onDelete(row)} size="small">
                              <DeleteIcon fontSize="small" />
                            </IconButton>
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
          count={filtered.length}
          rowsPerPage={rowsPerPage}
          page={page}
          colSpan={colSpanCount}
          onPageChange={(_, newPage) => setPage(newPage)}
          onRowsPerPageChange={e => {
            setRowsPerPage(parseInt(e.target.value, 10));
            setPage(0);
          }}
          disabled={loading}
        />
      </Paper>
    </>
  );
}
