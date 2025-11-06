import { useEffect, useState, useMemo } from 'react';
import { useAppDispatch, useAppSelector } from '../../shared/hooks/reduxHooks';
import { fetchMonitors, deleteMonitor } from '../../features/monitorsSlice';
import { MonitorsFilters, FilterOptions } from './components/MonitorsFilters';
import { MonitorsList } from './components/MonitorsList';
import { Monitor } from '../../shared/types/Monitor';
import { AlertCircle } from 'lucide-react';
import styles from './MonitoringPage.module.scss';

export const MonitoringPage = () => {
  const dispatch = useAppDispatch();
  const { monitors, loading, error } = useAppSelector((state) => state.monitors);

  const [filters, setFilters] = useState<FilterOptions>({
    search: '',
    status: 'all',
    sortBy: 'createdAt',
    sortOrder: 'desc',
  });

  useEffect(() => {
    dispatch(fetchMonitors());
  }, [dispatch]);

  const filteredAndSortedMonitors = useMemo(() => {
    let result: Monitor[] = [...monitors];

    if (filters.search) {
      const searchLower = filters.search.toLowerCase();
      result = result.filter(
        (monitor) =>
          monitor.name.toLowerCase().includes(searchLower) ||
          monitor.url.toLowerCase().includes(searchLower),
      );
    }

    if (filters.status !== 'all') {
      const isActive = filters.status === 'active';
      result = result.filter((monitor) => monitor.isActive === isActive);
    }

    result.sort((a, b) => {
      let comparison = 0;

      switch (filters.sortBy) {
        case 'name':
          comparison = a.name.localeCompare(b.name);
          break;
        case 'createdAt':
          comparison = new Date(a.createdAt).getTime() - new Date(b.createdAt).getTime();
          break;
        case 'interval':
          comparison = a.checkIntervalSeconds - b.checkIntervalSeconds;
          break;
      }

      return filters.sortOrder === 'asc' ? comparison : -comparison;
    });

    return result;
  }, [monitors, filters]);

  const handleDelete = (id: string) => {
    dispatch(deleteMonitor(id));
  };

  const handleToggleStatus = (id: string, isActive: boolean) => {
    // TODO: Implement pause/resume functionality when API is ready
    console.log(`Toggle status for monitor ${id} to ${isActive ? 'active' : 'paused'}`);
  };

  return (
    <div className={styles.monitorPage}>
      <header className={styles.pageHeader}>
        <div>
          <h1 className={styles.pageTitle}>Monitors</h1>
          <p className={styles.pageSubtitle}>Manage and track your service monitors</p>
        </div>
      </header>

      <MonitorsFilters filters={filters} onFiltersChange={setFilters} />

      {error && (
        <div className={styles.errorBanner}>
          <AlertCircle size={20} />
          <span>{error}</span>
        </div>
      )}

      <MonitorsList
        monitors={filteredAndSortedMonitors}
        loading={loading}
        onDelete={handleDelete}
        onToggleStatus={handleToggleStatus}
      />
    </div>
  );
};
