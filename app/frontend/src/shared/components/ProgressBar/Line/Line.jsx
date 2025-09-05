import { Fragment } from 'react';
import cx from 'classnames';

import QA_AUTOMATION_ATTRIBUTES_AF from '~system-host/shared/constants/attributesAutomationAf';

import NumberBlock from '../NumberBlock';

import styles from './Line.module.scss';

const Subline = ({ scalePercent }) => (
  <div className={styles.wrapper}>
    <div className={styles.inner} style={{ transform: `scaleX(${scalePercent})` }} />
  </div>
);

const Line = ({ className = '', currentProgressNum = 1, progressBlocks = [11, 11, 11] }) => {
  const getCurrentProgress = (blockNum) => {
    const blockStart = progressBlocks.slice(0, blockNum).reduce((acc, steps) => acc + steps, 0);
    const blockEnd = blockStart + progressBlocks[blockNum];

    if (currentProgressNum >= blockEnd) return 1;
    if (currentProgressNum <= blockStart) return 0;

    return (currentProgressNum - blockStart) / progressBlocks[blockNum];
  };

  return (
    <div
      className={cx(styles.progress, className)}
      data-e2e={QA_AUTOMATION_ATTRIBUTES_AF.PROGRESS_BAR}
    >
      <NumberBlock isCompleted />

      {progressBlocks.map((_, i) => (
        <Fragment key={i}>
          <Subline scalePercent={getCurrentProgress(i)} />
          <NumberBlock
            number={i + 2}
            isActive={
              currentProgressNum > progressBlocks.slice(0, i).reduce((acc, steps) => acc + steps, 0)
            }
            isCompleted={
              currentProgressNum >
              progressBlocks.slice(0, i + 1).reduce((acc, steps) => acc + steps, 0)
            }
          />
        </Fragment>
      ))}
    </div>
  );
};

export default Line;
