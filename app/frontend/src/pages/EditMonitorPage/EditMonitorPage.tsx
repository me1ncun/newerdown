import { useEffect } from 'react';
import { useForm } from 'react-hook-form';
import { useNavigate, useParams } from 'react-router-dom';
import { useAppSelector } from '../../shared/hooks/reduxHooks';
import { CreateMonitorRequest, MonitorType } from '../../shared/types/Monitor';
import { ArrowLeft, Save } from 'lucide-react';
import styles from './EditMonitorPage.module.scss';

export const EditMonitorPage = () => {
  const { id } = useParams<{ id: string }>();
  const navigate = useNavigate();
  const { monitors } = useAppSelector((state) => state.monitors);

  const monitor = monitors.find((m) => m.id === id);

  const {
    register,
    handleSubmit,
    formState: { errors, isSubmitting },
    reset,
  } = useForm<CreateMonitorRequest>({
    defaultValues: {
      name: '',
      target: '',
      type: MonitorType.HTTP,
      port: 80,
      intervalSeconds: 300,
      isActive: true,
    },
  });

  useEffect(() => {
    if (monitor) {
      reset({
        name: monitor.name,
        target: monitor.url,
        type: monitor.type,
        port: 80,
        intervalSeconds: monitor.checkIntervalSeconds,
        isActive: monitor.isActive,
      });
    } else {
      navigate('/monitoring');
    }
  }, [monitor, navigate, reset]);

  const onSubmit = async (data: CreateMonitorRequest) => {
    try {
      // TODO: Implement update API call when endpoint is ready
      console.log('Update monitor:', id, data);
      alert('Update functionality will be implemented when PUT endpoint is ready');
      navigate('/monitoring');
    } catch (error) {
      console.error('Failed to update monitor:', error);
    }
  };

  const handleCancel = () => {
    navigate('/monitoring');
  };

  if (!monitor) {
    return (
      <div className={styles.editMonitorPage}>
        <div className={styles.errorMessage}>Monitor not found</div>
      </div>
    );
  }

  return (
    <div className={styles.editMonitorPage}>
      <div className={styles.header}>
        <button className={styles.backButton} onClick={handleCancel}>
          <ArrowLeft size={20} />
          <span>Back to Monitors</span>
        </button>
      </div>

      <div className={styles.formCard}>
        <h1 className={styles.formTitle}>Edit Monitor</h1>
        <p className={styles.formSubtitle}>Update the configuration for {monitor.name}</p>

        <form onSubmit={handleSubmit(onSubmit)} className={styles.form}>
          <div className={styles.formGroup}>
            <label htmlFor="name" className={styles.label}>
              Monitor Name <span className={styles.required}>*</span>
            </label>
            <input
              id="name"
              type="text"
              className={`${styles.input} ${errors.name ? styles.inputError : ''}`}
              placeholder="e.g., Production API"
              {...register('name', {
                required: 'Monitor name is required',
                minLength: {
                  value: 3,
                  message: 'Name must be at least 3 characters',
                },
                maxLength: {
                  value: 100,
                  message: 'Name must not exceed 100 characters',
                },
              })}
            />
            {errors.name && <span className={styles.errorMessage}>{errors.name.message}</span>}
          </div>

          <div className={styles.formGroup}>
            <label htmlFor="target" className={styles.label}>
              Target URL/IP <span className={styles.required}>*</span>
            </label>
            <input
              id="target"
              type="text"
              className={`${styles.input} ${errors.target ? styles.inputError : ''}`}
              placeholder="e.g., https://example.com or 192.168.1.1"
              {...register('target', {
                required: 'Target is required',
                pattern: {
                  value:
                    /^(https?:\/\/)?([\w-]+\.)+[\w-]+(\/[\w-./?%&=]*)?$|^(\d{1,3}\.){3}\d{1,3}$/,
                  message: 'Please enter a valid URL or IP address',
                },
              })}
            />
            {errors.target && <span className={styles.errorMessage}>{errors.target.message}</span>}
          </div>

          <div className={styles.formRow}>
            <div className={styles.formGroup}>
              <label htmlFor="type" className={styles.label}>
                Monitor Type <span className={styles.required}>*</span>
              </label>
              <select
                id="type"
                className={`${styles.select} ${errors.type ? styles.inputError : ''}`}
                {...register('type', {
                  required: 'Monitor type is required',
                  valueAsNumber: true,
                })}
              >
                <option value={MonitorType.HTTP}>HTTP</option>
                <option value={MonitorType.HTTPS}>HTTPS</option>
                <option value={MonitorType.TCP}>TCP</option>
                <option value={MonitorType.PING}>PING</option>
              </select>
              {errors.type && <span className={styles.errorMessage}>{errors.type.message}</span>}
            </div>

            <div className={styles.formGroup}>
              <label htmlFor="port" className={styles.label}>
                Port <span className={styles.required}>*</span>
              </label>
              <input
                id="port"
                type="number"
                className={`${styles.input} ${errors.port ? styles.inputError : ''}`}
                placeholder="e.g., 80 or 443"
                {...register('port', {
                  required: 'Port is required',
                  valueAsNumber: true,
                  min: {
                    value: 1,
                    message: 'Port must be between 1 and 65535',
                  },
                  max: {
                    value: 65535,
                    message: 'Port must be between 1 and 65535',
                  },
                })}
              />
              {errors.port && <span className={styles.errorMessage}>{errors.port.message}</span>}
            </div>
          </div>

          <div className={styles.formGroup}>
            <label htmlFor="intervalSeconds" className={styles.label}>
              Check Interval (seconds) <span className={styles.required}>*</span>
            </label>
            <input
              id="intervalSeconds"
              type="number"
              className={`${styles.input} ${errors.intervalSeconds ? styles.inputError : ''}`}
              placeholder="e.g., 300 (5 minutes)"
              {...register('intervalSeconds', {
                required: 'Check interval is required',
                valueAsNumber: true,
                min: {
                  value: 60,
                  message: 'Interval must be at least 60 seconds',
                },
                max: {
                  value: 86400,
                  message: 'Interval must not exceed 86400 seconds (24 hours)',
                },
              })}
            />
            {errors.intervalSeconds && (
              <span className={styles.errorMessage}>{errors.intervalSeconds.message}</span>
            )}
            <span className={styles.helpText}>
              How often the monitor should check your service (minimum: 60 seconds)
            </span>
          </div>

          <div className={styles.formGroup}>
            <label className={styles.checkboxLabel}>
              <input type="checkbox" className={styles.checkbox} {...register('isActive')} />
              <span>Monitor is active</span>
            </label>
            <span className={styles.helpText}>Uncheck to pause monitoring for this service</span>
          </div>

          <div className={styles.formActions}>
            <button
              type="button"
              className={styles.buttonSecondary}
              onClick={handleCancel}
              disabled={isSubmitting}
            >
              Cancel
            </button>
            <button type="submit" className={styles.buttonPrimary} disabled={isSubmitting}>
              <Save size={18} />
              <span>{isSubmitting ? 'Saving...' : 'Save Changes'}</span>
            </button>
          </div>
        </form>
      </div>
    </div>
  );
};
