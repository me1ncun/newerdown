import { useEffect, useMemo } from 'react';
import { useAppDispatch, useAppSelector } from '../../shared/hooks/reduxHooks';
import { clearError, loginUser } from '../../features/authSlice';
import { Link, useNavigate, useLocation } from 'react-router-dom';
import { useTranslation } from 'react-i18next';
import { useForm } from 'react-hook-form';
import { AlertCircle, Mail, Lock, LogIn } from 'lucide-react';
import styles from './LoginPage.module.scss';

interface LoginFormInputs {
  email: string;
  password: string;
}

export const LoginPage = () => {
  const navigate = useNavigate();
  const { state } = useLocation();
  const dispatch = useAppDispatch();
  const { loading, error, token } = useAppSelector((state) => state.auth);
  const { t } = useTranslation('common');

  const {
    register,
    handleSubmit,
    formState: { errors, isSubmitting },
  } = useForm<LoginFormInputs>();

  useEffect(() => {
    dispatch(clearError());
  }, [dispatch]);

  const onSubmit = async (data: LoginFormInputs) => {
    await dispatch(loginUser(data));
  };

  const redirectPath = useMemo(() => state?.pathname || '/account', [state?.pathname]);

  useEffect(() => {
    if (token) {
      navigate(redirectPath, { replace: true });
    }
  }, [token, navigate, redirectPath]);

  return (
    <div className={styles.loginPage}>
      <div className={styles.loginCard}>
        <div className={styles.header}>
          <div className={styles.iconWrapper}>
            <LogIn size={32} />
          </div>
          <h1 className={styles.title}>{t('loginPage.login')}</h1>
          <p className={styles.subtitle}>{t('loginPage.subtitle')}</p>
        </div>

        <form onSubmit={handleSubmit(onSubmit)} className={styles.form}>
          <div className={styles.field}>
            <label className={styles.label}>{t('loginPage.email')}</label>
            <div className={styles.inputWrapper}>
              <Mail className={styles.inputIcon} size={20} />
              <input
                className={`${styles.input} ${errors.email ? styles.inputError : ''}`}
                type="email"
                placeholder={t('loginPage.emailPlaceholder')}
                {...register('email', {
                  required: t('validation.required'),
                  pattern: {
                    value: /^[^\s@]+@[^\s@]+\.[^\s@]+$/,
                    message: t('validation.email'),
                  },
                })}
                disabled={loading || isSubmitting}
              />
            </div>
            {errors.email && <span className={styles.errorText}>{errors.email.message}</span>}
          </div>

          <div className={styles.field}>
            <label className={styles.label}>{t('loginPage.password')}</label>
            <div className={styles.inputWrapper}>
              <Lock className={styles.inputIcon} size={20} />
              <input
                className={`${styles.input} ${errors.password ? styles.inputError : ''}`}
                type="password"
                placeholder={t('loginPage.passwordPlaceholder')}
                {...register('password', {
                  required: t('validation.required'),
                  minLength: {
                    value: 8,
                    message: t('validation.minLength'),
                  },
                })}
                disabled={loading || isSubmitting}
              />
            </div>
            {errors.password && <span className={styles.errorText}>{errors.password.message}</span>}
          </div>

          {error && (
            <div className={styles.errorMessage}>
              <AlertCircle size={18} />
              <span>{error}</span>
            </div>
          )}

          <button
            className={styles.submitButton}
            type="submit"
            disabled={loading || isSubmitting}
          >
            {loading || isSubmitting ? t('loginPage.loggingIn') : t('loginPage.loginButton')}
          </button>
        </form>

        <div className={styles.footer}>
          {t('loginPage.notAcc')}{' '}
          <Link to="/register" className={styles.link}>
            {t('loginPage.Register')}
          </Link>
        </div>
      </div>
    </div>
  );
};
