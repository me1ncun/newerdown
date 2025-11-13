import { useEffect, useState, useMemo } from 'react';
import { useTranslation } from 'react-i18next';
import { useAppDispatch, useAppSelector } from '../../shared/hooks/reduxHooks';
import {
  fetchMonitors,
  deleteMonitor,
  pauseMonitor,
  resumeMonitor,
  importMonitors,
} from '../../features/monitorsSlice';
import { exportMonitor } from '../../api/monitoring';
import { MonitorsFilters, FilterOptions } from './components/MonitorsFilters';
import { MonitorsList } from './components/MonitorsList';
import { Monitor } from '../../shared/types/Monitor';
import { AlertCircle } from 'lucide-react';
import styles from './MonitoringPage.module.scss';

export const MonitoringPage = () => {
  const dispatch = useAppDispatch();
  const { t } = useTranslation('monitoring');
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

  const handleToggleStatus = async (id: string, isActive: boolean) => {
    if (isActive) {
      await dispatch(resumeMonitor(id));
    } else {
      await dispatch(pauseMonitor(id));
    }
  };

  const handleExport = async (id: string) => {
    try {
      await exportMonitor(id);
    } catch (error) {
      console.error('Failed to export monitor:', error);
    }
  };

  const handleImport = async (file: File) => {
    try {
      await dispatch(importMonitors(file)).unwrap();
    } catch (error) {
      console.error('Failed to import monitors:', error);
    }
  };

  return (
    <div className={styles.monitorPage}>
      <header className={styles.pageHeader}>
        <div>
          <h1 className={styles.pageTitle}>{t('monitoring.monitoringPageTitle')}</h1>
          <p className={styles.pageSubtitle}>{t('monitoring.pageSubtitle')}</p>
        </div>
      </header>

      <MonitorsFilters filters={filters} onFiltersChange={setFilters} onImport={handleImport} />

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
        onExport={handleExport}
      />
    </div>
  );
};
