import { useEffect } from 'react';
import { useAppDispatch, useAppSelector } from '../../shared/hooks/reduxHooks';
import { clearError, singUpUser } from '../../features/authSlice';
import { Link, useNavigate } from 'react-router-dom';
import { useTranslation } from 'react-i18next';
import { useForm } from 'react-hook-form';
import { AlertCircle, Mail, Lock, User, UserPlus } from 'lucide-react';
import styles from './RegisterPage.module.scss';

interface RegisterFormInputs {
  userName: string;
  email: string;
  password: string;
  confirmPassword: string;
}

export const RegisterPage = () => {
  const dispatch = useAppDispatch();
  const navigate = useNavigate();
  const { loading, error } = useAppSelector((state) => state.auth);
  const { t } = useTranslation('common');

  const {
    register,
    handleSubmit,
    formState: { errors, isSubmitting },
    watch,
  } = useForm<RegisterFormInputs>();

  const password = watch('password');

  useEffect(() => {
    dispatch(clearError());
  }, [dispatch]);

  const onSubmit = async (data: RegisterFormInputs) => {
    const { userName, email, password } = data;
    const resultAction = await dispatch(singUpUser({ userName, email, password }));

    if (singUpUser.fulfilled.match(resultAction)) {
      navigate('/login');
    }
  };

  return (
    <div className={styles.registerPage}>
      <div className={styles.registerCard}>
        <div className={styles.header}>
          <div className={styles.iconWrapper}>
            <UserPlus size={32} />
          </div>
          <h1 className={styles.title}>{t('registerPage.register')}</h1>
          <p className={styles.subtitle}>{t('registerPage.subtitle')}</p>
        </div>

        <form onSubmit={handleSubmit(onSubmit)} className={styles.form}>
          <div className={styles.field}>
            <label className={styles.label}>{t('registerPage.name')}</label>
            <div className={styles.inputWrapper}>
              <User className={styles.inputIcon} size={20} />
              <input
                className={`${styles.input} ${errors.userName ? styles.inputError : ''}`}
                type="text"
                placeholder={t('registerPage.namePlaceholder')}
                {...register('userName', {
                  required: t('validation.required'),
                  minLength: {
                    value: 2,
                    message: t('validation.nameMinLength'),
                  },
                })}
                disabled={loading || isSubmitting}
              />
            </div>
            {errors.userName && <span className={styles.errorText}>{errors.userName.message}</span>}
          </div>

          <div className={styles.field}>
            <label className={styles.label}>{t('registerPage.email')}</label>
            <div className={styles.inputWrapper}>
              <Mail className={styles.inputIcon} size={20} />
              <input
                className={`${styles.input} ${errors.email ? styles.inputError : ''}`}
                type="email"
                placeholder={t('registerPage.emailPlaceholder')}
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
            <label className={styles.label}>{t('registerPage.password')}</label>
            <div className={styles.inputWrapper}>
              <Lock className={styles.inputIcon} size={20} />
              <input
                className={`${styles.input} ${errors.password ? styles.inputError : ''}`}
                type="password"
                placeholder={t('registerPage.passwordPlaceholder')}
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

          <div className={styles.field}>
            <label className={styles.label}>{t('registerPage.confirmPassword')}</label>
            <div className={styles.inputWrapper}>
              <Lock className={styles.inputIcon} size={20} />
              <input
                className={`${styles.input} ${errors.confirmPassword ? styles.inputError : ''}`}
                type="password"
                placeholder={t('registerPage.confirmPasswordPlaceholder')}
                {...register('confirmPassword', {
                  required: t('validation.required'),
                  validate: (value) =>
                    value === password || (t('validation.passwordMismatch') as string),
                })}
                disabled={loading || isSubmitting}
              />
            </div>
            {errors.confirmPassword && (
              <span className={styles.errorText}>{errors.confirmPassword.message}</span>
            )}
          </div>

          {error && (
            <div className={styles.errorMessage}>
              <AlertCircle size={18} />
              <span>{error}</span>
            </div>
          )}

          <button className={styles.submitButton} type="submit" disabled={loading || isSubmitting}>
            {loading || isSubmitting
              ? t('registerPage.registering')
              : t('registerPage.registerButton')}
          </button>
        </form>

        <div className={styles.footer}>
          {t('registerPage.have')}{' '}
          <Link to="/login" className={styles.link}>
            {t('registerPage.login')}
          </Link>
        </div>
      </div>
    </div>
  );
};
