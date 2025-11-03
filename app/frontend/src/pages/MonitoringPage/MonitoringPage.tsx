import { useEffect, useState } from 'react';
// import { useParams } from 'react-router-dom';
import styles from './MonitoringPage.module.scss';
import {
  Bell,
  CheckCircle2,
  AlertCircle,
  ExternalLink,
  ChevronDown,
  ArrowUp,
  ArrowDown,
  ThumbsUp,
  Calendar,
  Lock,
} from 'lucide-react';

const mockMonitorData = {
  id: '12345',
  url: 'https://www.lanet.ua/',
  displayName: 'www.lanet.ua/',
  status: 'Up',
  uptime: '0h 17m 18s',
  lastCheck: 'Coming soon',
  checkIntervalMinutes: 5,
  uptimeStats: {
    '24h': 100,
    '7d': 100,
    '30d': 100,
    '365d': null,
  },
  incidents: {
    '24h': 0,
    '7d': 0,
    '30d': 0,
    '365d': null,
  },
  downTime: {
    '24h': '0m',
    '7d': '0m',
    '30d': '0m',
    '365d': null,
  },
  responseTime: {
    average: 439,
    minimum: 439,
    maximum: 439,
  },
  latestIncidents: [],
};

type MonitorData = typeof mockMonitorData;

const getMonitorById = (id: string): Promise<MonitorData> => {
  console.log(`Fetching monitor: ${id}`);
  return new Promise((resolve) => setTimeout(() => resolve(mockMonitorData), 0));
};

const testNotification = (id: string) =>
  console.log(`API: POST /api/monitors/${id}/test-notification`);

export const MonitoringPage = () => {
  const id = '12345';

  const [monitor, setMonitor] = useState<MonitorData | null>(null);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);

  useEffect(() => {
    if (!id) return;

    const fetchData = async () => {
      try {
        setLoading(true);
        setError(null);
        const data = await getMonitorById(id);
        setMonitor(data);
      } catch (err) {
        setError('Failed to fetch monitor data.');
        console.error('Error fetching monitor data:', err);
      } finally {
        setLoading(false);
      }
    };

    fetchData();
  }, [id]);

  if (loading) {
    return <div className={styles.loading}>Завантаження...</div>;
  }

  if (error || !monitor) {
    return <div className={styles.error}>{error || 'Монітор не знайдено'}</div>;
  }

  const isUp = monitor.status === 'Up';

  return (
    <div className={styles.monitorPage}>
      <header className={styles.monitorHeader}>
        <div className={styles.titleWrapper}>
          {isUp ? (
            <CheckCircle2 size={32} className={styles.statusIconUp} />
          ) : (
            <AlertCircle size={32} className={styles.statusIconDown} />
          )}
          <div>
            <h1 className={styles.monitorTitle}>
              {monitor.displayName}
              <a href={monitor.url} target="_blank" rel="noopener noreferrer">
                <ExternalLink size={16} />
              </a>
            </h1>
            <p className={styles.monitorSubtitle}>HTTP/S monitor for {monitor.url}</p>
          </div>
        </div>
        <button className={styles.testButton} onClick={() => testNotification(monitor.id)}>
          <Bell size={14} />
          Test Notification
        </button>
      </header>

      <div className={styles.statsGrid}>
        <StatCard title="Current status" className={styles.gridCol4}>
          <div className={`${styles.cardValue} ${isUp ? styles.textGreen : styles.textRed}`}>
            {monitor.status}
          </div>
          <div className={styles.cardDetail}>
            Currently {isUp ? 'up' : 'down'} for {monitor.uptime}
          </div>
        </StatCard>

        <StatCard title="Last check" className={styles.gridCol4}>
          <div className={styles.cardValue}>{monitor.lastCheck}</div>
          <div className={styles.cardDetail}>
            Checked every {monitor.checkIntervalMinutes} minutes
          </div>
        </StatCard>

        <StatCard title="Last 24 hours" className={styles.gridCol4}>
          <div className={styles.cardValueRow}>
            <span>{monitor.uptimeStats['24h']}%</span>
            <UptimeBar percentage={monitor.uptimeStats['24h']} />
          </div>
          <div className={styles.cardDetail}>
            {monitor.incidents['24h']} incidents, {monitor.downTime['24h']} down
          </div>
        </StatCard>

        <StatCard title="Last 7 days" className={styles.gridCol3}>
          <div className={styles.cardValue}>{monitor.uptimeStats['7d']}%</div>
          <div className={styles.cardDetail}>
            {monitor.incidents['7d']} incidents, {monitor.downTime['7d']} down
          </div>
        </StatCard>

        <StatCard title="Last 30 days" className={styles.gridCol3}>
          <div className={styles.cardValue}>{monitor.uptimeStats['30d']}%</div>
          <div className={styles.cardDetail}>
            {monitor.incidents['30d']} incidents, {monitor.downTime['30d']} down
          </div>
        </StatCard>

        <StatCard title="Last 365 days" className={styles.gridCol3}>
          {monitor.uptimeStats['365d'] === null ? (
            <div className={styles.lockedFeature}>
              <Lock size={16} />
              <span className={styles.cardValue}>--.--%</span>
              <a href="/pricing" className={styles.cardDetailLink}>
                Unlock with paid plans
              </a>
            </div>
          ) : (
            <>
              <div className={styles.cardValue}>{monitor.uptimeStats['365d']}%</div>
              <div className={styles.cardDetail}>
                {monitor.incidents['365d']} incidents, {monitor.downTime['365d']} down
              </div>
            </>
          )}
        </StatCard>

        <StatCard title="Pick a date range" className={styles.gridCol3} hasDropdown>
          <div className={styles.cardValue}>--.--%</div>
          <div className={styles.cardDetail}>-- incidents, -- down</div>
        </StatCard>
      </div>

      <div className={styles.chartCard}>
        <div className={styles.cardHeader}>
          <h3 className={styles.cardTitle}>Response time</h3>
          <div className={styles.cardControls}>
            <button className={styles.alertButton}>Setup alerts</button>
            <button className={styles.dropdownButton}>
              Last hour <ChevronDown size={16} />
            </button>
          </div>
        </div>
        <div className={styles.chartPlaceholder}>
          <div className={styles.chartDot} style={{ left: '70%', top: '30%' }} />
        </div>
        <div className={styles.chartStats}>
          <div className={styles.chartStatItem}>
            <span className={styles.statIcon}>~</span> Average:{' '}
            <strong>{monitor.responseTime.average} ms</strong>
          </div>
          <div className={styles.chartStatItem}>
            <ArrowUp size={14} className={styles.textGreen} /> Minimum:{' '}
            <strong>{monitor.responseTime.minimum} ms</strong>
          </div>
          <div className={styles.chartStatItem}>
            <ArrowDown size={14} className={styles.textRed} /> Maximum:{' '}
            <strong>{monitor.responseTime.maximum} ms</strong>
          </div>
        </div>
      </div>

      <div className={styles.incidentsCard}>
        <h3 className={styles.cardTitle}>Latest incidents</h3>
        {monitor.latestIncidents.length === 0 ? (
          <div className={styles.noIncidents}>
            <ThumbsUp size={24} />
            <strong>Good job, no incidents.</strong>
            <p>No incidents so far. Keep it up!</p>
          </div>
        ) : (
          <div>{/* список інцидентів */}</div>
        )}
      </div>
    </div>
  );
};

interface StatCardProps {
  title: string;
  children: React.ReactNode;
  className?: string;
  hasDropdown?: boolean;
}

const StatCard = ({ title, children, className, hasDropdown = false }: StatCardProps) => (
  <div className={`${styles.statCard} ${className || ''}`}>
    <div className={styles.cardHeader}>
      <h3 className={styles.cardTitle}>{title}</h3>
      {hasDropdown && <Calendar size={14} />}
    </div>
    <div className={styles.cardBody}>{children}</div>
  </div>
);

const UptimeBar = ({ percentage }: { percentage: number }) => {
  const bars = Array.from({ length: 30 }, (_, i) => {
    const isActive = (percentage / 100) * 30 > i;
    return <div key={i} className={`${styles.bar} ${isActive ? styles.barUp : styles.barDown}`} />;
  });

  return <div className={styles.uptimeBar}>{bars}</div>;
};
