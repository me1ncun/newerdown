import { Link } from 'react-router-dom';
import { useTranslation } from 'react-i18next';
import { Home, AlertCircle } from 'lucide-react';
import styles from './NotFoundPage.module.scss';

export const NotFoundPage = () => {
  const { t } = useTranslation('common');

  return (
    <div className={styles.notFoundPage}>
      <div className={styles.content}>
        <div className={styles.illustration}>
          <AlertCircle size={120} />
        </div>
        <h1 className={styles.errorCode}>404</h1>
        <h2 className={styles.title}>{t('notFoundPage.oops')}</h2>
        <p className={styles.description}>{t('notFoundPage.doesNotExist')}</p>
        <Link to="/home" className={styles.homeButton}>
          <Home size={24} />
          <span>{t('notFoundPage.goHome')}</span>
        </Link>
      </div>
    </div>
  );
};
