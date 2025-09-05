import { useCallback, useEffect, useRef } from 'react';
import cx from 'classnames';

import useErrorMessage from '../../hooks/useErrorMessage';

import Typography from '../Typography';

import styles from './ErrorBlock.module.scss';

const ErrorBlock = ({ errorCode, isShown = true, hideError }) => {
  const error = useErrorMessage({ errorCode });

  const { title, subtitle } = error;

  const ref = useRef(null);

  const handleHideError = useCallback(() => {
    hideError();
    if (ref.current !== null) {
      clearTimeout(ref.current);
    }
  }, [hideError]);

  useEffect(() => {
    if (isShown) {
      ref.current = setTimeout(handleHideError, 3000);
    }
    return () => {
      if (ref.current !== null) {
        clearTimeout(ref.current);
      }
    };
  }, [handleHideError, isShown]);

  return (
    <div className={cx(styles.wrapper, { [styles.show]: isShown })}>
      <Typography.Title classNames={styles.title}>{title}</Typography.Title>
      <Typography.Text classNames={styles.subtitle}>{subtitle}</Typography.Text>
    </div>
  );
};

export default ErrorBlock;
