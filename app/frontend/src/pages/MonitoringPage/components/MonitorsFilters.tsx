import { Search, Plus, Filter, Upload } from 'lucide-react';
import { useState, useRef } from 'react';
import { useNavigate } from 'react-router-dom';
import { useTranslation } from 'react-i18next';
import styles from './MonitorsFilters.module.scss';

export interface FilterOptions {
  search: string;
  status: 'all' | 'active' | 'paused';
  sortBy: 'name' | 'createdAt' | 'interval';
  sortOrder: 'asc' | 'desc';
}

interface MonitorsFiltersProps {
  filters: FilterOptions;
  onFiltersChange: (filters: FilterOptions) => void;
  onImport?: (file: File) => void;
}

export const MonitorsFilters: React.FC<MonitorsFiltersProps> = ({
  filters,
  onFiltersChange,
  onImport,
}) => {
  const navigate = useNavigate();
  const { t } = useTranslation('monitoring');
  const [showAdvanced, setShowAdvanced] = useState(false);
  const fileInputRef = useRef<HTMLInputElement>(null);

  const handleSearchChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    onFiltersChange({ ...filters, search: e.target.value });
  };

  const handleStatusChange = (e: React.ChangeEvent<HTMLSelectElement>) => {
    onFiltersChange({
      ...filters,
      status: e.target.value as FilterOptions['status'],
    });
  };

  const handleSortByChange = (e: React.ChangeEvent<HTMLSelectElement>) => {
    onFiltersChange({
      ...filters,
      sortBy: e.target.value as FilterOptions['sortBy'],
    });
  };

  const handleSortOrderChange = (e: React.ChangeEvent<HTMLSelectElement>) => {
    onFiltersChange({
      ...filters,
      sortOrder: e.target.value as FilterOptions['sortOrder'],
    });
  };

  const handleCreateClick = () => {
    navigate('/monitoring/create');
  };

  const handleImportClick = () => {
    fileInputRef.current?.click();
  };

  const handleFileChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    const file = e.target.files?.[0];
    if (file && onImport) {
      onImport(file);
      e.target.value = '';
    }
  };

  return (
    <div className={styles.filtersContainer}>
      <div className={styles.mainFilters}>
        <div className={styles.searchWrapper}>
          <Search size={20} className={styles.searchIcon} />
          <input
            type="text"
            placeholder={t('monitoring.searchPlaceholder')}
            value={filters.search}
            onChange={handleSearchChange}
            className={styles.searchInput}
          />
        </div>

        <button className={styles.filterToggle} onClick={() => setShowAdvanced(!showAdvanced)}>
          <Filter size={20} />
          <span>{t('monitoring.filters')}</span>
        </button>

        {onImport && (
          <>
            <input
              ref={fileInputRef}
              type="file"
              accept=".csv"
              onChange={handleFileChange}
              className={styles.fileInput}
              aria-label={t('monitoring.selectFile')}
            />
            <button className={styles.importButton} onClick={handleImportClick}>
              <Upload size={20} />
              <span>{t('monitoring.import')}</span>
            </button>
          </>
        )}

        <button className={styles.createButton} onClick={handleCreateClick}>
          <Plus size={20} />
          <span>{t('monitoring.createMonitor')}</span>
        </button>
      </div>

      {showAdvanced && (
        <div className={styles.advancedFilters}>
          <div className={styles.filterGroup}>
            <label htmlFor="status" className={styles.filterLabel}>
              {t('monitoring.status')}:
            </label>
            <select
              id="status"
              value={filters.status}
              onChange={handleStatusChange}
              className={styles.filterSelect}
            >
              <option value="all">{t('monitoring.statusAll')}</option>
              <option value="active">{t('monitoring.statusActive')}</option>
              <option value="paused">{t('monitoring.statusPaused')}</option>
            </select>
          </div>

          <div className={styles.filterGroup}>
            <label htmlFor="sortBy" className={styles.filterLabel}>
              {t('monitoring.sortBy')}:
            </label>
            <select
              id="sortBy"
              value={filters.sortBy}
              onChange={handleSortByChange}
              className={styles.filterSelect}
            >
              <option value="name">{t('monitoring.sortByName')}</option>
              <option value="createdAt">{t('monitoring.sortByCreatedAt')}</option>
              <option value="interval">{t('monitoring.sortByInterval')}</option>
            </select>
          </div>

          <div className={styles.filterGroup}>
            <label htmlFor="sortOrder" className={styles.filterLabel}>
              {t('monitoring.sortOrder')}:
            </label>
            <select
              id="sortOrder"
              value={filters.sortOrder}
              onChange={handleSortOrderChange}
              className={styles.filterSelect}
            >
              <option value="asc">{t('monitoring.sortOrderAsc')}</option>
              <option value="desc">{t('monitoring.sortOrderDesc')}</option>
            </select>
          </div>
        </div>
      )}
    </div>
  );
};
