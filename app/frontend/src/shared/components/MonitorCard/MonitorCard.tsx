import { Edit2, Trash2, PlayCircle, PauseCircle, Globe, Server, Radio } from 'lucide-react';
import { Monitor, MonitorType } from '../../types/Monitor';
import styles from './MonitorCard.module.scss';

interface MonitorCardProps {
  monitor: Monitor;
  onEdit: (id: string) => void;
  onDelete: (id: string) => void;
  onToggleStatus: (id: string, isActive: boolean) => void;
}

const getMonitorTypeIcon = (type: MonitorType) => {
  switch (type) {
    case MonitorType.HTTP:
      return <Globe size={20} />;
    case MonitorType.HTTPS:
      return <Globe size={20} />;
    case MonitorType.TCP:
      return <Server size={20} />;
    case MonitorType.PING:
      return <Radio size={20} />;
    default:
      return <Globe size={20} />;
  }
};

const getMonitorTypeName = (type: MonitorType): string => {
  switch (type) {
    case MonitorType.HTTP:
      return 'HTTP';
    case MonitorType.HTTPS:
      return 'HTTPS';
    case MonitorType.TCP:
      return 'TCP';
    case MonitorType.PING:
      return 'PING';
    default:
      return 'Unknown';
  }
};

const formatDate = (dateString: string): string => {
  const date = new Date(dateString);
  return date.toLocaleDateString('en-US', {
    year: 'numeric',
    month: 'short',
    day: 'numeric',
  });
};

const formatInterval = (seconds: number): string => {
  if (seconds < 60) {
    return `${seconds}s`;
  }
  const minutes = Math.floor(seconds / 60);
  if (minutes < 60) {
    return `${minutes}m`;
  }
  const hours = Math.floor(minutes / 60);
  return `${hours}h`;
};

export const MonitorCard: React.FC<MonitorCardProps> = ({
  monitor,
  onEdit,
  onDelete,
  onToggleStatus,
}) => {
  return (
    <div className={styles.card}>
      <div className={styles.cardHeader}>
        <div className={styles.cardTitle}>
          <div className={styles.typeIcon}>{getMonitorTypeIcon(monitor.type)}</div>
          <h3 className={styles.monitorName}>{monitor.name}</h3>
        </div>
        <div className={styles.statusBadge} data-active={monitor.isActive}>
          {monitor.isActive ? 'Active' : 'Paused'}
        </div>
      </div>

      <div className={styles.cardBody}>
        <div className={styles.infoRow}>
          <span className={styles.label}>URL:</span>
          <span className={styles.value} title={monitor.url}>
            {monitor.url}
          </span>
        </div>

        <div className={styles.infoRow}>
          <span className={styles.label}>Type:</span>
          <span className={styles.value}>{getMonitorTypeName(monitor.type)}</span>
        </div>

        <div className={styles.infoRow}>
          <span className={styles.label}>Check Interval:</span>
          <span className={styles.value}>{formatInterval(monitor.checkIntervalSeconds)}</span>
        </div>

        <div className={styles.infoRow}>
          <span className={styles.label}>Created:</span>
          <span className={styles.value}>{formatDate(monitor.createdAt)}</span>
        </div>
      </div>

      <div className={styles.cardFooter}>
        <button
          className={`${styles.button} ${styles.buttonEdit}`}
          onClick={() => onEdit(monitor.id)}
          aria-label="Edit monitor"
        >
          <Edit2 size={16} />
          <span>Edit</span>
        </button>

        <button
          className={`${styles.button} ${styles.buttonToggle}`}
          onClick={() => onToggleStatus(monitor.id, !monitor.isActive)}
          aria-label={monitor.isActive ? 'Pause monitor' : 'Resume monitor'}
        >
          {monitor.isActive ? <PauseCircle size={16} /> : <PlayCircle size={16} />}
          <span>{monitor.isActive ? 'Pause' : 'Resume'}</span>
        </button>

        <button
          className={`${styles.button} ${styles.buttonDelete}`}
          onClick={() => onDelete(monitor.id)}
          aria-label="Delete monitor"
        >
          <Trash2 size={16} />
          <span>Delete</span>
        </button>
      </div>
    </div>
  );
};
