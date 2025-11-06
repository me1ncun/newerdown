import { useEffect, useRef, useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { MonitorCard } from '../../../shared/components/MonitorCard';
import { ConfirmModal } from '../../../shared/components/ConfirmModal/ConfirmModal';
import { Monitor } from '../../../shared/types/Monitor';
import { Loader2, Inbox } from 'lucide-react';
import styles from './MonitorsList.module.scss';

interface MonitorsListProps {
  monitors: Monitor[];
  loading: boolean;
  onDelete: (id: string) => void;
  onToggleStatus: (id: string, isActive: boolean) => void;
}

const ITEMS_PER_PAGE = 12;

export const MonitorsList: React.FC<MonitorsListProps> = ({
  monitors,
  loading,
  onDelete,
  onToggleStatus,
}) => {
  const navigate = useNavigate();
  const [displayedCount, setDisplayedCount] = useState(ITEMS_PER_PAGE);
  const [deleteModalOpen, setDeleteModalOpen] = useState(false);
  const [monitorToDelete, setMonitorToDelete] = useState<string | null>(null);
  const loaderRef = useRef<HTMLDivElement>(null);

  const displayedMonitors = monitors.slice(0, displayedCount);
  const hasMore = displayedCount < monitors.length;

  useEffect(() => {
    const observer = new IntersectionObserver(
      (entries) => {
        const first = entries[0];
        if (first.isIntersecting && hasMore && !loading) {
          setDisplayedCount((prev) => Math.min(prev + ITEMS_PER_PAGE, monitors.length));
        }
      },
      { threshold: 0.1 },
    );

    const currentLoader = loaderRef.current;
    if (currentLoader) {
      observer.observe(currentLoader);
    }

    return () => {
      if (currentLoader) {
        observer.unobserve(currentLoader);
      }
    };
  }, [hasMore, loading, monitors.length]);

  useEffect(() => {
    setDisplayedCount(ITEMS_PER_PAGE);
  }, [monitors]);

  const handleEdit = (id: string) => {
    navigate(`/monitoring/${id}/edit`);
  };

  const handleDeleteClick = (id: string) => {
    setMonitorToDelete(id);
    setDeleteModalOpen(true);
  };

  const handleConfirmDelete = () => {
    if (monitorToDelete) {
      onDelete(monitorToDelete);
      setDeleteModalOpen(false);
      setMonitorToDelete(null);
    }
  };

  const handleCancelDelete = () => {
    setDeleteModalOpen(false);
    setMonitorToDelete(null);
  };

  if (loading && monitors.length === 0) {
    return (
      <div className={styles.loadingContainer}>
        <Loader2 size={48} className={styles.spinner} />
        <p>Loading monitors...</p>
      </div>
    );
  }

  if (!loading && monitors.length === 0) {
    return (
      <div className={styles.emptyState}>
        <Inbox size={64} className={styles.emptyIcon} />
        <h3>No monitors found</h3>
        <p>Create your first monitor to start tracking your services</p>
      </div>
    );
  }

  return (
    <>
      <div className={styles.grid}>
        {displayedMonitors.map((monitor) => (
          <MonitorCard
            key={monitor.id}
            monitor={monitor}
            onEdit={handleEdit}
            onDelete={handleDeleteClick}
            onToggleStatus={onToggleStatus}
          />
        ))}
      </div>

      {hasMore && (
        <div ref={loaderRef} className={styles.loadMoreContainer}>
          <Loader2 size={32} className={styles.spinner} />
        </div>
      )}

      <ConfirmModal
        isOpen={deleteModalOpen}
        title="Delete Monitor"
        message="Are you sure you want to delete this monitor? This action cannot be undone."
        confirmText="Delete"
        cancelText="Cancel"
        onConfirm={handleConfirmDelete}
        onCancel={handleCancelDelete}
        isDanger={true}
      />
    </>
  );
};
