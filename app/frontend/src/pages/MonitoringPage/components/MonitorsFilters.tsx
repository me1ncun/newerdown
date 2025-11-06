import { Search, Plus, Filter } from 'lucide-react';
import { useState } from 'react';
import { useNavigate } from 'react-router-dom';
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
}

export const MonitorsFilters: React.FC<MonitorsFiltersProps> = ({ filters, onFiltersChange }) => {
  const navigate = useNavigate();
  const [showAdvanced, setShowAdvanced] = useState(false);

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

  return (
    <div className={styles.filtersContainer}>
      <div className={styles.mainFilters}>
        <div className={styles.searchWrapper}>
          <Search size={20} className={styles.searchIcon} />
          <input
            type="text"
            placeholder="Search monitors..."
            value={filters.search}
            onChange={handleSearchChange}
            className={styles.searchInput}
          />
        </div>

        <button className={styles.filterToggle} onClick={() => setShowAdvanced(!showAdvanced)}>
          <Filter size={20} />
          <span>Filters</span>
        </button>

        <button className={styles.createButton} onClick={handleCreateClick}>
          <Plus size={20} />
          <span>Create Monitor</span>
        </button>
      </div>

      {showAdvanced && (
        <div className={styles.advancedFilters}>
          <div className={styles.filterGroup}>
            <label htmlFor="status" className={styles.filterLabel}>
              Status:
            </label>
            <select
              id="status"
              value={filters.status}
              onChange={handleStatusChange}
              className={styles.filterSelect}
            >
              <option value="all">All</option>
              <option value="active">Active</option>
              <option value="paused">Paused</option>
            </select>
          </div>

          <div className={styles.filterGroup}>
            <label htmlFor="sortBy" className={styles.filterLabel}>
              Sort by:
            </label>
            <select
              id="sortBy"
              value={filters.sortBy}
              onChange={handleSortByChange}
              className={styles.filterSelect}
            >
              <option value="name">Name</option>
              <option value="createdAt">Created Date</option>
              <option value="interval">Check Interval</option>
            </select>
          </div>

          <div className={styles.filterGroup}>
            <label htmlFor="sortOrder" className={styles.filterLabel}>
              Order:
            </label>
            <select
              id="sortOrder"
              value={filters.sortOrder}
              onChange={handleSortOrderChange}
              className={styles.filterSelect}
            >
              <option value="asc">Ascending</option>
              <option value="desc">Descending</option>
            </select>
          </div>
        </div>
      )}
    </div>
  );
};
