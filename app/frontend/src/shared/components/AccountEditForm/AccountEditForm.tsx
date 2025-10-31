import { useForm } from 'react-hook-form';
import type { UserInformation } from '../../types/User';
import type { ChangePassword } from '../../types/Auth';
import { useTranslation } from 'react-i18next';
import './AccountEditForm.scss';

interface Props {
  user: UserInformation;
  onUpdate: (data: UserInformation) => Promise<void>;
  onChangePassword: (data: ChangePassword) => Promise<void>;
  onCancel: () => void;
}

export const AccountEditForm = ({ user, onUpdate, onChangePassword, onCancel }: Props) => {
  const { t } = useTranslation();

  const {
    register,
    handleSubmit,
    formState: { errors, isSubmitting },
    reset,
    watch,
  } = useForm<UserInformation & ChangePassword>({
    defaultValues: {
      userName: user.userName || '',
      displayName: user.displayName || '',
      email: user.email || '',
      organizationName: user.organizationName || '',
      phoneNumber: user.phoneNumber || '',
      language: user.language || '',
      currentPassword: '',
      newPassword: '',
    },
  });

  const newPasswordValue = watch('newPassword');
  const currentPasswordValue = watch('currentPassword');

  const onSubmit = async (data: UserInformation & ChangePassword) => {
    const { currentPassword, newPassword, ...profileData } = data;
    console.log('Submitting data:', data);

    try {
      if (currentPassword && newPassword) {
        await onChangePassword({ currentPassword, newPassword });
      }

      await onUpdate(profileData);

      reset({
        ...profileData,
        currentPassword: '',
        newPassword: '',
      });
    } catch (error) {
      console.error('Failed to update profile:', error);
    }
  };

  return (
    <form className="account-form box mt-5" onSubmit={handleSubmit(onSubmit)}>
      <h2 className="title is-5 mb-4">{t('accountPage.editProfile')}</h2>

      <div className="field">
        <label className="label">{t('accountPage.userName')}</label>
        <div className="control">
          <input
            className={`input ${errors.userName ? 'is-danger' : ''}`}
            {...register('userName')}
            type="text"
          />
        </div>
        {errors.userName && <p className="help is-danger">{errors.userName.message}</p>}
      </div>

      <div className="field">
        <label className="label">{t('accountPage.displayName')}</label>
        <div className="control">
          <input
            className={`input ${errors.displayName ? 'is-danger' : ''}`}
            {...register('displayName')}
            type="text"
          />
        </div>
        {errors.displayName && <p className="help is-danger">{errors.displayName.message}</p>}
      </div>

      <div className="field">
        <label className="label">{t('accountPage.email')}</label>
        <div className="control">
          <input
            className={`input ${errors.email ? 'is-danger' : ''}`}
            {...register('email', {
              pattern: {
                value: /^[^\s@]+@[^\s@]+\.[^\s@]+$/,
                message: t('validation.email') as string,
              },
            })}
            type="email"
          />
        </div>
        {errors.email && <p className="help is-danger">{errors.email.message}</p>}
      </div>

      <div className="field">
        <label className="label">{t('accountPage.currentPassword')}</label>
        <div className="control">
          <input
            className={`input ${errors.currentPassword ? 'is-danger' : ''}`}
            {...register('currentPassword', {
              required: newPasswordValue
                ? (t('validation.currentPasswordRequired') as string)
                : false,
            })}
            type="password"
            placeholder={t('accountPage.currentPassword') || ''}
          />
        </div>
        {errors.currentPassword && (
          <p className="help is-danger">{errors.currentPassword.message}</p>
        )}
      </div>

      <div className="field">
        <label className="label">{t('accountPage.newPassword')}</label>
        <div className="control">
          <input
            className={`input ${errors.newPassword ? 'is-danger' : ''}`}
            type="password"
            placeholder={t('accountPage.newPassword') || ''}
            {...register('newPassword', {
              required: currentPasswordValue
                ? (t('validation.newPasswordRequired') as string)
                : false,
              pattern: currentPasswordValue
                ? {
                    value: /^(?=.*[A-Z])(?=.*\d)[A-Za-z\d!@#$%^&*()_+]{8,}$/,
                    message: t('validation.passwordPattern') as string,
                  }
                : undefined,
            })}
          />
        </div>
        {errors.newPassword && <p className="help is-danger">{errors.newPassword.message}</p>}
      </div>

      <div className="field">
        <label className="label">{t('accountPage.organization')}</label>
        <div className="control">
          <input className="input" {...register('organizationName')} type="text" />
        </div>
      </div>

      <div className="field">
        <label className="label">{t('accountPage.phone')}</label>
        <div className="control">
          <input
            className={`input ${errors.phoneNumber ? 'is-danger' : ''}`}
            type="tel"
            {...register('phoneNumber', {
              pattern: {
                value: /^\+380[-\s]?\d{2}[-\s]?\d{3}[-\s]?\d{4}$/,
                message: t('validation.phone') as string,
              },
            })}
          />
        </div>
        {errors.phoneNumber && <p className="help is-danger">{errors.phoneNumber.message}</p>}
      </div>

      <div className="field">
        <label className="label">{t('accountPage.language')}</label>
        <div className="control">
          <div className="select">
            <select {...register('language')}>
              <option value="ua">{t('language.ua')}</option>
              <option value="en">{t('language.en')}</option>
            </select>
          </div>
        </div>
      </div>

      <div className="field mt-4 is-flex is-justify-content-space-between">
        <button type="button" className="button is-light" onClick={onCancel}>
          {t('accountPage.cancel')}
        </button>
        <button type="submit" className="button is-link" disabled={isSubmitting}>
          {isSubmitting ? t('accountPage.saving') : t('accountPage.saveChanges')}
        </button>
      </div>
    </form>
  );
};
