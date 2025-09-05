import { useEffect, useState } from 'react';
import cx from 'classnames';

import Typography from '../Typography';

import styles from './SummaryProgressBar.module.scss';

const SummaryProgressBar = ({
  isHigh = false,
  title = 'Your Openess to Change',
  className = '',
  isSlow = false,
  text1 = 'Exploration',
  text2 = 'Discovery',
  text3 = 'Growth',
  text4 = 'Wisdom',
}) => {
  const [startAnimation, setStartAnimation] = useState(false);

  useEffect(() => {
    setTimeout(() => {
      setStartAnimation(true);
    }, 0);
  }, []);

  return (
    <div className={cx(className, styles.wrapper)}>
      <Typography.Text type="body bold" center className={styles.title}>
        {title}
      </Typography.Text>
      <div className={styles.lineWrapper}>
        <span
          className={cx(styles.line, {
            [styles.lineSlow]: isSlow,
            [styles.lineAnimation]: startAnimation,
            [styles.lineAnimationHigh]: isHigh,
          })}
        />
      </div>
      <ul className={styles.list}>
        <li className={styles.item}>
          <Typography.Text type="very small regular" className={styles.itemTitle} center>
            {text1}
          </Typography.Text>
        </li>
        <li className={styles.item}>
          <Typography.Text type="very small regular" className={styles.itemTitle} center>
            {text2}
          </Typography.Text>
        </li>
        <li className={styles.item}>
          <Typography.Text type="very small regular" className={styles.itemTitle} center>
            {text3}
          </Typography.Text>
        </li>
        <li className={styles.item}>
          <Typography.Text type="very small regular" className={styles.itemTitle} center>
            {text4}
          </Typography.Text>
        </li>
      </ul>
    </div>
  );
};

export default SummaryProgressBar;
