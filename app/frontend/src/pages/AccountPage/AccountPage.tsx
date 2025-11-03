import { useEffect, useState, useRef } from 'react';
import { useAppDispatch, useAppSelector } from '../../shared/hooks/reduxHooks';
import {
  getInformation,
  deleteUser,
  updateUserInformation,
  updateUserAvatar,
  removeUserAvatar,
} from '../../features/userAccountSlice';
import { useTranslation } from 'react-i18next';
import { changePasswordUser } from '../../features/authSlice';
import { Loader } from '../Loader';
import { ConfirmModal } from '../../shared/components/ConfirmModal';
import { useForm } from 'react-hook-form';
import type { UserInformation } from '../../shared/types/User';
import type { ChangePassword } from '../../shared/types/Auth';
import defailtAvatar from '../../shared/assets/avatar-default.svg';
import styles from './AccountPage.module.scss';
import { Edit2, X } from 'lucide-react';

type FormInputs = UserInformation & ChangePassword;

export const AccountPage = () => {
  const dispatch = useAppDispatch();
  const { user, loading, error } = useAppSelector((state) => state.userAccount);
  const { t } = useTranslation();

  const [showDeleteUserModal, setShowDeleteUserModal] = useState(false);
  const [showRemoveAvatarModal, setShowRemoveAvatarModal] = useState(false);
  const [isEditMode, setIsEditMode] = useState(false);
  const fileInputRef = useRef<HTMLInputElement>(null);

  const {
    register,
    handleSubmit,
    formState: { errors, isSubmitting },
    reset,
    watch,
  } = useForm<FormInputs>();

  const newPasswordValue = watch('newPassword');
  const currentPasswordValue = watch('currentPassword');

  useEffect(() => {
    dispatch(getInformation());
  }, [dispatch]);

  useEffect(() => {
    if (user) {
      reset({
        userName: user.userName || '',
        displayName: user.displayName || '',
        email: user.email || '',
        organizationName: user.organizationName || '',
        phoneNumber: user.phoneNumber || '',
        language: user.language || 'ua',
        currentPassword: '',
        newPassword: '',
      });
    }
  }, [user, reset]);

  const onSubmit = async (data: FormInputs) => {
    const { currentPassword, newPassword, ...profileData } = data;

    try {
      if (currentPassword && newPassword) {
        await dispatch(changePasswordUser({ currentPassword, newPassword })).unwrap();
      }
      await dispatch(updateUserInformation(profileData)).unwrap();
      await dispatch(getInformation());
      setIsEditMode(false);
    } catch (err) {
      console.error('Failed to update profile:', err);
    }
  };

  const handleDeleteUser = () => {
    dispatch(deleteUser());
    setShowDeleteUserModal(false);
  };

  const handleCancelEdit = () => {
    if (user) reset(user);
    setIsEditMode(false);
  };

  const handleFileChange = async (e: React.ChangeEvent<HTMLInputElement>) => {
    const file = e.target.files?.[0];
    if (file) {
      await dispatch(updateUserAvatar(file));
      await dispatch(getInformation());
    }
    if (e.target) e.target.value = '';
  };

  const handleUploadClick = () => fileInputRef.current?.click();

  const handleRemoveAvatarConfirm = async () => {
    await dispatch(removeUserAvatar());
    await dispatch(getInformation());
    setShowRemoveAvatarModal(false);
  };

  if (loading && !user) return <Loader />;

  if (error) {
    return (
      <main className={styles.accountPage}>
        <p className={styles.error}>Something went wrong: {error}</p>
      </main>
    );
  }

  if (!user) return null;

  return (
    <>
      <main className={styles.accountPage}>
        <div className={styles.container}>
          <header className={styles.accountHeader}>
            <div className={styles.headerLeft}>
              <input
                type="file"
                accept="image/*"
                ref={fileInputRef}
                style={{ display: 'none' }}
                onChange={handleFileChange}
              />
              <div className={styles.avatarWrapper}>
                <figure
                  className={styles.avatarFigure}
                  onClick={isEditMode ? handleUploadClick : undefined}
                  title={isEditMode ? t('accountPage.changeAvatar') : ''}
                >
                  <img src={user.filePath || defailtAvatar} alt="User avatar" />
                  {isEditMode && (
                    <div className={styles.avatarOverlay}>
                      <span>{t('accountPage.change')}</span>
                    </div>
                  )}
                </figure>
                {isEditMode && user.filePath && (
                  <button
                    type="button"
                    className={styles.avatarRemoveBtn}
                    onClick={() => setShowRemoveAvatarModal(true)}
                  >
                    {t('accountPage.removeAvatar')}
                  </button>
                )}
              </div>

              <div className={styles.userInfo}>
                <h1>{user.displayName || user.userName}</h1>
                <p>{user.email}</p>
              </div>
            </div>

            <button
              type="button"
              className={`${styles.button} ${isEditMode ? styles.buttonLight : styles.buttonPrimary}`}
              onClick={() => (isEditMode ? handleCancelEdit() : setIsEditMode(true))}
            >
              {isEditMode ? <X size={16} /> : <Edit2 size={16} />}
              {isEditMode ? t('accountPage.cancel') : t('accountPage.edit')}
            </button>
          </header>

          <form className={styles.formCard} onSubmit={handleSubmit(onSubmit)}>
            <fieldset disabled={!isEditMode && !isSubmitting} className={styles.fieldset}>
              <div className={styles.formGrid}>
                {/* <div className={styles.formField}>
                  <label htmlFor="displayName" className={styles.label}>
                    {t('accountPage.fullName')}
                  </label>
                  <input
                    id="displayName"
                    className={styles.input}
                    {...register('displayName')}
                    type="text"
                  />
                </div> */}

                <div className={styles.formField}>
                  <label htmlFor="userName" className={styles.label}>
                    {t('accountPage.userName')}
                  </label>
                  <input
                    id="userName"
                    className={styles.input}
                    {...register('userName')}
                    type="text"
                  />
                </div>

                <div className={styles.formField}>
                  <label htmlFor="email" className={styles.label}>
                    {t('accountPage.email')}
                  </label>
                  <input
                    id="email"
                    className={`${styles.input} ${errors.email ? styles.isError : ''}`}
                    {...register('email', {
                      pattern: {
                        value: /^[^\s@]+@[^\s@]+\.[^\s@]+$/,
                        message: t('validation.email') as string,
                      },
                    })}
                    type="email"
                  />
                  {errors.email && <p className={styles.errorMsg}>{errors.email.message}</p>}
                </div>

                <div className={styles.formField}>
                  <label htmlFor="language" className={styles.label}>
                    {t('accountPage.language')}
                  </label>
                  <div className={styles.selectWrapper}>
                    <select id="language" className={styles.select} {...register('language')}>
                      <option value="ua">{t('language.ua')}</option>
                      <option value="en">{t('language.en')}</option>
                    </select>
                  </div>
                </div>

                <div className={styles.formField}>
                  <label htmlFor="organizationName" className={styles.label}>
                    {t('accountPage.organization')}
                  </label>
                  <input
                    id="organizationName"
                    className={styles.input}
                    {...register('organizationName')}
                    type="text"
                  />
                </div>

                <div className={styles.formField}>
                  <label htmlFor="phoneNumber" className={styles.label}>
                    {t('accountPage.phone')}
                  </label>
                  <input
                    id="phoneNumber"
                    className={`${styles.input} ${errors.phoneNumber ? styles.isError : ''}`}
                    type="tel"
                    {...register('phoneNumber', {
                      pattern: {
                        value: /^\+380[-\s]?\d{2}[-\s]?\d{3}[-\s]?\d{4}$/,
                        message: t('validation.phone') as string,
                      },
                    })}
                  />
                  {errors.phoneNumber && (
                    <p className={styles.errorMsg}>{errors.phoneNumber.message}</p>
                  )}
                </div>

                <h2 className={styles.formSectionTitle}>{t('accountPage.changePassword')}</h2>

                <div className={styles.formField}>
                  <label htmlFor="currentPassword" className={styles.label}>
                    {t('accountPage.currentPassword')}
                  </label>
                  <input
                    id="currentPassword"
                    className={`${styles.input} ${errors.currentPassword ? styles.isError : ''}`}
                    {...register('currentPassword', {
                      required: newPasswordValue
                        ? (t('validation.currentPasswordRequired') as string)
                        : false,
                    })}
                    type="password"
                  />
                  {errors.currentPassword && (
                    <p className={styles.errorMsg}>{errors.currentPassword.message}</p>
                  )}
                </div>

                <div className={styles.formField}>
                  <label htmlFor="newPassword" className={styles.label}>
                    {t('accountPage.newPassword')}
                  </label>
                  <input
                    id="newPassword"
                    className={`${styles.input} ${errors.newPassword ? styles.isError : ''}`}
                    type="password"
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
                  {errors.newPassword && (
                    <p className={styles.errorMsg}>{errors.newPassword.message}</p>
                  )}
                </div>
              </div>
            </fieldset>

            {isEditMode && (
              <div className={styles.formActions}>
                <button
                  type="button"
                  className={`${styles.button} ${styles.buttonDanger}`}
                  onClick={() => setShowDeleteUserModal(true)}
                  disabled={isSubmitting}
                >
                  {t('accountPage.deleteUser')}
                </button>
                <button
                  type="submit"
                  className={`${styles.button} ${styles.buttonPrimary}`}
                  disabled={isSubmitting}
                >
                  {isSubmitting ? t('accountPage.saving') : t('accountPage.saveChanges')}
                </button>
              </div>
            )}
          </form>
        </div>
      </main>

      <ConfirmModal
        isOpen={showDeleteUserModal}
        title={t('accountPage.confirmDeleteTitle')}
        message={t('accountPage.confirmDeleteMessage')}
        onConfirm={handleDeleteUser}
        onCancel={() => setShowDeleteUserModal(false)}
        confirmText={t('accountPage.confirm')}
        cancelText={t('accountPage.cancel')}
      />
      <ConfirmModal
        isOpen={showRemoveAvatarModal}
        title={t('accountPage.confirmRemoveAvatarTitle')}
        message={t('accountPage.confirmRemoveAvatarMessage')}
        onConfirm={handleRemoveAvatarConfirm}
        onCancel={() => setShowRemoveAvatarModal(false)}
        confirmText={t('accountPage.confirm')}
        cancelText={t('accountPage.cancel')}
      />
    </>
  );
};
