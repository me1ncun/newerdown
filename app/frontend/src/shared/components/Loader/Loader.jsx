import cx from 'classnames';
import styles from './Loader.module.scss';

const Loader = ({ className }) => {
  return <div className={cx(className, styles.loader)}></div>;
};

export default Loader;
