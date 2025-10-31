import { Link } from 'react-router-dom';
import { useTranslation } from 'react-i18next';

export const NotFoundPage = () => {
  const { t } = useTranslation();

  return (
    <section className="section has-text-centered">
      <div className="container">
        <h1 className="title is-1 has-text-danger">404</h1>
        <p className="subtitle is-4">{t('notFoundPage.oops')}</p>
        <p className="mb-5">{t('notFoundPage.doesNotExist')}</p>
        <Link to="/home" className="button is-info is-large">
          <span className="icon">
            <i className="fas fa-home"></i>
          </span>
          <span>{t('notFoundPage.goHome')}</span>
        </Link>
      </div>
    </section>
  );
};
