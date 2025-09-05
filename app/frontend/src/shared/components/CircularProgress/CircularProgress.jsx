import { memo } from 'react';

import styles from './CircularProgress.module.scss';

const CircularProgress = ({
  strokeWidth = 10,
  sqSize = 200,
  percentage = 0,
  className = '',
  durationMs = 0,
  circleColorDefault = '#E8E6EF',
  circleColorActive = '#31728D',
}) => {
  const radius = (sqSize - strokeWidth) / 2;
  const viewBox = `0 0 ${sqSize} ${sqSize}`;
  const dashArray = radius * Math.PI * 2;
  const dashOffset = dashArray - (dashArray * percentage) / 100;

  return (
    <svg width={sqSize} height={sqSize} viewBox={viewBox} className={className}>
      <circle
        className={styles.circleBackground}
        stroke={circleColorDefault}
        cx={sqSize / 2}
        cy={sqSize / 2}
        r={radius}
        strokeWidth={`${strokeWidth}px`}
      />
      <circle
        className={styles.circleProgress}
        stroke={circleColorActive}
        cx={sqSize / 2}
        cy={sqSize / 2}
        r={radius}
        strokeWidth={`${strokeWidth}px`}
        transform={`rotate(-90 ${sqSize / 2} ${sqSize / 2})`}
        style={{
          transition: `${durationMs}ms`,
          strokeDasharray: dashArray,
          strokeDashoffset: dashOffset,
        }}
      />
    </svg>
  );
};

export default memo(CircularProgress);
