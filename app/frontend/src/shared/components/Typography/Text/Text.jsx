import cx from 'classnames';

import styles from './Text.module.scss';

const Text = ({
  type,
  className = '',
  center = false,
  children = 'lorem ipsum',
  dataQa = '',
  dataE2e = '',
  style = {},
}) => {
  const baseStyles = cx(
    styles.text,
    {
      [styles.center]: center,
      [styles.button1Bold]: type === 'button1 bold',
      [styles.button2Bold]: type === 'button2 bold',
      [styles.button1Extrabold]: type === 'button1 extrabold',
      [styles.bodyBold]: type === 'body bold',
      [styles.boydBoldUnredline]: type === 'body bold underline',
      [styles.bodyLight]: type === 'body light',
      [styles.bodyRegular]: type === 'body regular',
      [styles.secondaryBold]: type === 'secondary bold',
      [styles.secondaryLight]: type === 'secondary light',
      [styles.smallBold]: type === 'small bold',
      [styles.smallLight]: type === 'small light',
      [styles.verySmallBold]: type === 'very small bold',
      [styles.verySmallRegular]: type === 'very small regular',
      [styles.verySmallLight]: type === 'very small light',
    },
    className,
  );

  const sanitizeHTML = (html) => {
    if (!html) {
      return;
    }

    return html.replace(/<script.*?>.*?<\/script>/gi, '');
  };

  return (
    <p
      className={baseStyles}
      data-qa={dataQa}
      data-e2e={dataE2e}
      style={style}
      dangerouslySetInnerHTML={{ __html: sanitizeHTML(children) }}
    />
  );
};

export default Text;
