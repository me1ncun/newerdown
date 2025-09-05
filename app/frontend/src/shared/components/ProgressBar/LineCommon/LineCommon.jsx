import cx from 'classnames';

import styles from './LineCommon.module.scss';

const LineCommon = ({
  wrapperClassName = '',
  innerClassName = '',
  currentProgressNum = 1,
  totalProgressNum = 10,
}) => {
  const currProgressScaling = (currentProgressNum / totalProgressNum) * 100;

  return (
    <div className={cx(styles.wrapper, wrapperClassName)}>
      <div
        className={cx(styles.inner, innerClassName)}
        style={{ width: `${currProgressScaling}%` }}
      />
    </div>
  );
};

export default LineCommon;
