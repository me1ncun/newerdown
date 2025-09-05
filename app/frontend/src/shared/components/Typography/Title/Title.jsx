import cx from 'classnames';

import styles from './Title.module.scss';

const Title = ({ level, center = false, className = '', children, dataE2e = '' }) => {
  const baseStyles = cx({ [styles.center]: center }, styles.header, className);

  const sanitizeHTML = (html) => {
    if (!html) {
      return;
    }

    return html.replace(/<script.*?>.*?<\/script>/gi, '');
  };

  switch (level) {
    case 1:
      return (
        <h1
          className={cx(baseStyles, styles.header1)}
          dangerouslySetInnerHTML={{ __html: sanitizeHTML(children) }}
          data-e2e={dataE2e}
        />
      );
    case 2:
      return (
        <h2
          className={cx(baseStyles, styles.header2)}
          dangerouslySetInnerHTML={{ __html: sanitizeHTML(children) }}
          data-e2e={dataE2e}
        />
      );
    case 3:
      return (
        <h3
          className={cx(baseStyles, styles.header3)}
          dangerouslySetInnerHTML={{ __html: sanitizeHTML(children) }}
          data-e2e={dataE2e}
        />
      );
    case 4:
      return (
        <h3
          className={cx(baseStyles, styles.header4)}
          dangerouslySetInnerHTML={{ __html: sanitizeHTML(children) }}
        />
      );
    default:
      return (
        <h1
          className={cx(baseStyles, styles.header1)}
          dangerouslySetInnerHTML={{ __html: sanitizeHTML(children) }}
          data-e2e={dataE2e}
        />
      );
  }
};

export default Title;
