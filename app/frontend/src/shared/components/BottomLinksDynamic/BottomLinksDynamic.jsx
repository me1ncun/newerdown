import cx from 'classnames';

import QA_AUTOMATION_ATTRIBUTES_AF from '~system-host/shared/constants/attributesAutomationAf';

import useUnitsByDaysNum from '../../hooks/useUnitsByDaysNum';

import getPriceWithCurrencyTag from '../../lib/currencyHelpers/getPriceWithCurrencyTag';

import styles from './BottomLinksDynamic.module.scss';

const BottomLinksDynamic = ({
  className,
  period = 30,
  price,
  currency = 'USD',
  currencyTagType = 'code-alpha',
  isRecurrent = false,
}) => {
  const fullPrice = getPriceWithCurrencyTag(price?.toFixed(2), currency, currencyTagType);

  const { unitsNum, unitsText } = useUnitsByDaysNum({
    totalDaysNum: period,
  });

  const periodName = isRecurrent ? `renewal period` : `${unitsNum}-week introductory offer`;

  const repeatPeriod = unitsNum === 1 ? `${unitsText}` : `${unitsNum} ${unitsText}`;

  const shownPeriod = isRecurrent ? 'week' : repeatPeriod;

  return (
    <p
      className={cx(styles.rights, className)}
      data-e2e={QA_AUTOMATION_ATTRIBUTES_AF.COMPLIANCE_TEXT}
    >
      {`By continuing you agree that if you don’t cancel at least 24 hours prior
      to the end of the ${periodName}, you will automatically be charged the
      full price of `}
      <span className={styles.rightsStrong}>{fullPrice}</span>
      {` every ${shownPeriod} until you cancel through contacting support team. Learn more about cancellation policy in `}
      <a
        className={styles.link}
        href="https://affemity.com/info/subscription"
        target="_blank"
        rel="noreferrer"
      >
        Subscription Terms.
      </a>
      {isRecurrent &&
        ` Payment will be processed automatically based on the billing information
      you provided earlier.`}
    </p>
  );
};

export default BottomLinksDynamic;
