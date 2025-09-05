import { useEffect, useRef, useState } from 'react';
import cx from 'classnames';

import QA_AUTOMATION_ATTRIBUTES_AF from '~system-host/shared/constants/attributesAutomationAf';

import usePrintingText from '../../hooks/usePrintingText';

import Typography from '../Typography';

import styles from './SummaryFieldAnimated.module.scss';

const SummaryFieldAnimated = ({ icon = '', text = '', delay = 500, className = '', ...rest }) => {
  const [isAnimationStarted, setIsAnimationStarted] = useState(false);
  const timerPrintingRef = useRef(null);
  const { printedText, startPrinting } = usePrintingText({
    text: text,
  });
  const printingDelay = delay + 500;

  useEffect(() => {
    setIsAnimationStarted(true);

    if (isAnimationStarted) {
      timerPrintingRef.current = setTimeout(() => {
        startPrinting();
      }, printingDelay);
    }

    return () => {
      clearTimeout(timerPrintingRef.current);
    };
  }, [printingDelay, isAnimationStarted, startPrinting]);

  return (
    <div
      className={cx(styles.infoDescription, className)}
      data-e2e={QA_AUTOMATION_ATTRIBUTES_AF.DYNAMIC_TEXT}
      {...rest}
    >
      <img className={styles.infoIcon} src={icon} alt="icon" />
      <Typography.Text type="secondary light" className={styles.infoText}>
        {printedText}
      </Typography.Text>
    </div>
  );
};

export default SummaryFieldAnimated;
