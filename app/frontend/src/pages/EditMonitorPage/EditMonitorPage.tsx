import { useEffect } from 'react';
import { useForm } from 'react-hook-form';
import { useNavigate, useParams } from 'react-router-dom';
import { useTranslation } from 'react-i18next';
import { useAppSelector, useAppDispatch } from '../../shared/hooks/reduxHooks';
import { updateMonitor } from '../../features/monitorsSlice';
import {
  CreateMonitorRequest,
  MonitorType,
  UpdateMonitorRequest,
} from '../../shared/types/Monitor';
import { ArrowLeft, Save } from 'lucide-react';
import styles from './EditMonitorPage.module.scss';

export const EditMonitorPage = () => {
  const { id } = useParams<{ id: string }>();
  const navigate = useNavigate();
  const dispatch = useAppDispatch();
  const { t } = useTranslation('monitoring');
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
    if (!id) return;

    try {
      const updateData: UpdateMonitorRequest = {
        type: data.type,
        name: data.name,
        port: data.port,
        url: data.target,
        isActive: data.isActive,
      };

      await dispatch(updateMonitor({ id, data: updateData })).unwrap();
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
        <div className={styles.errorMessage}>{t('monitoring.noMonitorsFound')}</div>
      </div>
    );
  }

  return (
    <div className={styles.editMonitorPage}>
      <div className={styles.header}>
        <button className={styles.backButton} onClick={handleCancel}>
          <ArrowLeft size={20} />
          <span>{t('monitoring.backToMonitors')}</span>
        </button>
      </div>

      <div className={styles.formCard}>
        <h1 className={styles.formTitle}>{t('monitoring.editMonitor')}</h1>
        <p className={styles.formSubtitle}>
          {t('monitoring.editFormSubtitle')} {monitor.name}
        </p>

        <form onSubmit={handleSubmit(onSubmit)} className={styles.form}>
          <div className={styles.formGroup}>
            <label htmlFor="name" className={styles.label}>
              {t('monitoring.monitorName')} <span className={styles.required}>*</span>
            </label>
            <input
              id="name"
              type="text"
              className={`${styles.input} ${errors.name ? styles.inputError : ''}`}
              placeholder="e.g., Production API"
              {...register('name', {
                required: t('monitoring.required'),
                minLength: {
                  value: 3,
                  message: t('monitoring.minLength', { count: 3 }),
                },
                maxLength: {
                  value: 100,
                  message: t('monitoring.maxLength', { count: 100 }),
                },
              })}
            />
            {errors.name && <span className={styles.errorMessage}>{errors.name.message}</span>}
          </div>

          <div className={styles.formGroup}>
            <label htmlFor="target" className={styles.label}>
              {t('monitoring.target')} <span className={styles.required}>*</span>
            </label>
            <input
              id="target"
              type="text"
              className={`${styles.input} ${errors.target ? styles.inputError : ''}`}
              placeholder="e.g., https://example.com or 192.168.1.1"
              {...register('target', {
                required: t('monitoring.required'),
                pattern: {
                  value:
                    /^(https?:\/\/)?([\w-]+\.)+[\w-]+(\/[\w-./?%&=]*)?$|^(\d{1,3}\.){3}\d{1,3}$/,
                  message: t('monitoring.validUrl'),
                },
              })}
            />
            {errors.target && <span className={styles.errorMessage}>{errors.target.message}</span>}
          </div>

          <div className={styles.formRow}>
            <div className={styles.formGroup}>
              <label htmlFor="type" className={styles.label}>
                {t('monitoring.monitorType')} <span className={styles.required}>*</span>
              </label>
              <select
                id="type"
                className={`${styles.select} ${errors.type ? styles.inputError : ''}`}
                {...register('type', {
                  required: t('monitoring.required'),
                  valueAsNumber: true,
                })}
              >
                <option value={MonitorType.HTTP}>{t('monitoring.http')}</option>
                <option value={MonitorType.HTTPS}>{t('monitoring.https')}</option>
                <option value={MonitorType.TCP}>{t('monitoring.tcp')}</option>
                <option value={MonitorType.PING}>{t('monitoring.ping')}</option>
              </select>
              {errors.type && <span className={styles.errorMessage}>{errors.type.message}</span>}
            </div>

            <div className={styles.formGroup}>
              <label htmlFor="port" className={styles.label}>
                {t('monitoring.port')} <span className={styles.required}>*</span>
              </label>
              <input
                id="port"
                type="number"
                className={`${styles.input} ${errors.port ? styles.inputError : ''}`}
                placeholder="e.g., 80 or 443"
                {...register('port', {
                  required: t('monitoring.required'),
                  valueAsNumber: true,
                  min: {
                    value: 1,
                    message: t('monitoring.validPort'),
                  },
                  max: {
                    value: 65535,
                    message: t('monitoring.validPort'),
                  },
                })}
              />
              {errors.port && <span className={styles.errorMessage}>{errors.port.message}</span>}
            </div>
          </div>

          <div className={styles.formGroup}>
            <label htmlFor="intervalSeconds" className={styles.label}>
              {t('monitoring.intervalSeconds')} <span className={styles.required}>*</span>
            </label>
            <input
              id="intervalSeconds"
              type="number"
              className={`${styles.input} ${errors.intervalSeconds ? styles.inputError : ''}`}
              placeholder="e.g., 300 (5 minutes)"
              {...register('intervalSeconds', {
                required: t('monitoring.required'),
                valueAsNumber: true,
                min: {
                  value: 60,
                  message: t('monitoring.minInterval'),
                },
                max: {
                  value: 86400,
                  message: t('monitoring.maxInterval'),
                },
              })}
            />
            {errors.intervalSeconds && (
              <span className={styles.errorMessage}>{errors.intervalSeconds.message}</span>
            )}
            <span className={styles.helpText}>{t('monitoring.helpInterval')}</span>
          </div>

          <div className={styles.formGroup}>
            <label className={styles.checkboxLabel}>
              <input type="checkbox" className={styles.checkbox} {...register('isActive')} />
              <span>{t('monitoring.isActive')}</span>
            </label>
            <span className={styles.helpText}>{t('monitoring.helpActive')}</span>
          </div>

          <div className={styles.formActions}>
            <button
              type="button"
              className={styles.buttonSecondary}
              onClick={handleCancel}
              disabled={isSubmitting}
            >
              {t('monitoring.cancel')}
            </button>
            <button type="submit" className={styles.buttonPrimary} disabled={isSubmitting}>
              <Save size={18} />
              <span>{isSubmitting ? t('monitoring.saving') : t('monitoring.saveChanges')}</span>
            </button>
          </div>
        </form>
      </div>
    </div>
  );
};
