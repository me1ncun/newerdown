import {
  Edit2,
  Trash2,
  PlayCircle,
  PauseCircle,
  Globe,
  Server,
  Radio,
  Download,
} from 'lucide-react';
import { useTranslation } from 'react-i18next';
import { Monitor, MonitorType } from '../../types/Monitor';
import styles from './MonitorCard.module.scss';

interface MonitorCardProps {
  monitor: Monitor;
  onEdit: (id: string) => void;
  onDelete: (id: string) => void;
  onToggleStatus: (id: string, isActive: boolean) => void;
  onExport: (id: string) => void;
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

export const MonitorCard: React.FC<MonitorCardProps> = ({
  monitor,
  onEdit,
  onDelete,
  onToggleStatus,
  onExport,
}) => {
  const { t, i18n } = useTranslation('monitoring');

  const getMonitorTypeName = (type: MonitorType): string => {
    switch (type) {
      case MonitorType.HTTP:
        return t('monitoring.http');
      case MonitorType.HTTPS:
        return t('monitoring.https');
      case MonitorType.TCP:
        return t('monitoring.tcp');
      case MonitorType.PING:
        return t('monitoring.ping');
      default:
        return t('monitoring.unknown');
    }
  };

  const formatDate = (dateString: string): string => {
    const date = new Date(dateString);
    return date.toLocaleDateString(i18n.language, {
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

  return (
    <div className={styles.card}>
      <div className={styles.cardHeader}>
        <div className={styles.cardTitle}>
          <div className={styles.typeIcon}>{getMonitorTypeIcon(monitor.type)}</div>
          <h3 className={styles.monitorName}>{monitor.name}</h3>
        </div>
        <div className={styles.statusBadge} data-active={monitor.isActive}>
          {monitor.isActive ? t('monitoring.statusActive') : t('monitoring.statusPaused')}
        </div>
      </div>

      <div className={styles.cardBody}>
        <div className={styles.infoRow}>
          <span className={styles.label}>{t('monitoring.url')}:</span>
          <span className={styles.value} title={monitor.url}>
            {monitor.url}
          </span>
        </div>

        <div className={styles.infoRow}>
          <span className={styles.label}>{t('monitoring.type')}:</span>
          <span className={styles.value}>{getMonitorTypeName(monitor.type)}</span>
        </div>

        <div className={styles.infoRow}>
          <span className={styles.label}>{t('monitoring.checkInterval')}:</span>
          <span className={styles.value}>{formatInterval(monitor.checkIntervalSeconds)}</span>
        </div>

        <div className={styles.infoRow}>
          <span className={styles.label}>{t('monitoring.created')}:</span>
          <span className={styles.value}>{formatDate(monitor.createdAt)}</span>
        </div>
      </div>

      <div className={styles.cardFooter}>
        <button
          className={`${styles.button} ${styles.buttonEdit}`}
          onClick={() => onEdit(monitor.id)}
          aria-label={t('monitoring.edit')}
        >
          <Edit2 size={16} />
          <span>{t('monitoring.edit')}</span>
        </button>

        <button
          className={`${styles.button} ${styles.buttonToggle}`}
          onClick={() => onToggleStatus(monitor.id, !monitor.isActive)}
          aria-label={monitor.isActive ? t('monitoring.pause') : t('monitoring.resume')}
        >
          {monitor.isActive ? <PauseCircle size={16} /> : <PlayCircle size={16} />}
          <span>{monitor.isActive ? t('monitoring.pause') : t('monitoring.resume')}</span>
        </button>

        <button
          className={`${styles.button} ${styles.buttonExport}`}
          onClick={() => onExport(monitor.id)}
          aria-label={t('monitoring.export')}
        >
          <Download size={16} />
          <span>{t('monitoring.export')}</span>
        </button>

        <button
          className={`${styles.button} ${styles.buttonDelete}`}
          onClick={() => onDelete(monitor.id)}
          aria-label={t('monitoring.delete')}
        >
          <Trash2 size={16} />
          <span>{t('monitoring.delete')}</span>
        </button>
      </div>
    </div>
  );
};
