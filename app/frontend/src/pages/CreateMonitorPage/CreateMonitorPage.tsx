import { useForm } from 'react-hook-form';
import { useNavigate } from 'react-router-dom';
import { useAppDispatch } from '../../shared/hooks/reduxHooks';
import { createMonitor } from '../../features/monitorsSlice';
import { CreateMonitorRequest, MonitorType } from '../../shared/types/Monitor';
import { ArrowLeft, Save } from 'lucide-react';
import styles from './CreateMonitorPage.module.scss';

export const CreateMonitorPage = () => {
  const navigate = useNavigate();
  const dispatch = useAppDispatch();

  const {
    register,
    handleSubmit,
    formState: { errors, isSubmitting },
  } = useForm<CreateMonitorRequest>({
    defaultValues: {
      name: '',
      target: '',
      type: MonitorType.HTTP,
      port: 80,
      intervalSeconds: 300, // 5 minutes default
      isActive: true,
    },
  });

  const onSubmit = async (data: CreateMonitorRequest) => {
    try {
      await dispatch(createMonitor(data)).unwrap();
      navigate('/monitoring');
    } catch (error) {
      console.error('Failed to create monitor:', error);
    }
  };

  const handleCancel = () => {
    navigate('/monitoring');
  };

  return (
    <div className={styles.createMonitorPage}>
      <div className={styles.header}>
        <button className={styles.backButton} onClick={handleCancel}>
          <ArrowLeft size={20} />
          <span>Back to Monitors</span>
        </button>
      </div>

      <div className={styles.formCard}>
        <h1 className={styles.formTitle}>Create New Monitor</h1>
        <p className={styles.formSubtitle}>
          Configure a new monitor to track the availability and performance of your services
        </p>

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
              <span>Start monitoring immediately</span>
            </label>
            <span className={styles.helpText}>
              If unchecked, the monitor will be created in paused state
            </span>
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
              <span>{isSubmitting ? 'Creating...' : 'Create Monitor'}</span>
            </button>
          </div>
        </form>
      </div>
    </div>
  );
};
