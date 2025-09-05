import cx from 'classnames';

import styles from './Arrow.module.scss';

const Arrow = ({ className }) => {
  return <div className={cx(styles.arrows, className)}></div>;
};

export default Arrow;
