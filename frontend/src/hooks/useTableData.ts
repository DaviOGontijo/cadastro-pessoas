import React from 'react';

export function useTableData<T>(
  rows: T[],
  filter: string,
  filterField: (row: T, filter: string) => boolean,
  orderBy: keyof T | null,
  order: 'asc' | 'desc',
  page: number,
  rowsPerPage: number
) {
  const filtered = React.useMemo(() => rows.filter(row => filterField(row, filter)), [rows, filter, filterField]);

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

  const paged = React.useMemo(() => {
    return sorted.slice(page * rowsPerPage, (page + 1) * rowsPerPage);
  }, [sorted, page, rowsPerPage]);

  return { paged, filteredCount: filtered.length };
}
