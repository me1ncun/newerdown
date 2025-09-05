import { memo } from 'react';
import cx from 'classnames';

import useCounter from '../../hooks/useCounter';

import IntersectionTopLine from '../IntersectionTopLine';
import Typography from '../Typography';

import styles from './IntersectionCounterItem.module.css';

const IntersectionCounterItem = ({ className = '', itemConfig = {}, changeTitleWidth = false }) => {
  const { title, symbol, counterOptions } = itemConfig;
  const { counter, startCounting } = useCounter(counterOptions);

  return (
    <IntersectionTopLine onIntersect={startCounting}>
      <li className={cx(styles.counterItem, className)}>
        <div className={styles.headline}>
          <div className={styles.counter}>
            <span className={styles.fakeNumber}>{`${counter}${symbol}`}</span>
            <span className={styles.countedNumber}>{`${counter}${symbol}`}</span>
          </div>
        </div>
        <Typography.Text
          type="body regular"
          className={cx(styles.title, {
            [styles.changeTitleWidth]: changeTitleWidth,
          })}
        >
          {title}
        </Typography.Text>
      </li>
    </IntersectionTopLine>
  );
};

export default memo(IntersectionCounterItem);
