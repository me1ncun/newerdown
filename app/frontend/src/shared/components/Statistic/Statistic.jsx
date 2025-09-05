import cx from 'classnames';

import IntersectionCounterItem from '../IntersectionCounterItem';

import styles from './Statistic.module.scss';

const Statistic = ({ statistic, className, isTextingOrGuidesOnly }) => {
  return (
    <ul className={cx(styles.wrapper, className)}>
      {statistic.map((itemConfig) => {
        return (
          <IntersectionCounterItem
            className={styles.item}
            key={itemConfig.id}
            itemConfig={itemConfig}
            changeTitleWidth={isTextingOrGuidesOnly}
          />
        );
      })}
    </ul>
  );
};

export default Statistic;
