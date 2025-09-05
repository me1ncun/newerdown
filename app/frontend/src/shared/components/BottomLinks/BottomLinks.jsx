import cx from 'classnames';

import QA_AUTOMATION_ATTRIBUTES_AF from '~system-host/shared/constants/attributesAutomationAf';

import '~modules-public/fonts/Nunito.css';
import styles from './BottomLinks.module.scss';

const BottomLinks = ({
  t,
  isSubscription = false,
  handleTermsClick = () => {},
  handlePrivacyClick = () => {},
  handleSubscriptionClick = () => {},
  className = '',
  type,
}) => {
  const currentYear = new Date().getFullYear();

  switch (type) {
    case 'yearBegin':
      return (
        <p
          className={cx(styles.rights, className)}
          data-e2e={QA_AUTOMATION_ATTRIBUTES_AF.COMPLIANCE_TEXT}
        >
          {`${t('common:affemity.bottomLinks.rights', { year: `${currentYear}` })}`}
          <br />
          {`${t('common:affemity.bottomLinks.text')} `}
          <a
            className={styles.link}
            href={`https://affemity.com/terms`}
            target="_blank"
            rel="noreferrer"
            onClick={handleTermsClick}
          >
            {` ${t('common:affemity.bottomLinks.policy.terms')} `}
          </a>
          |
          <a
            className={styles.link}
            href={`https://affemity.com/policy`}
            target="_blank"
            rel="noreferrer"
            onClick={handlePrivacyClick}
          >
            {` ${t('common:affemity.bottomLinks.policy.privacy')} `}
          </a>
          {isSubscription && (
            <>
              |
              <a
                className={styles.link}
                href="https://affemity.com/subscription"
                target="_blank"
                rel="noreferrer"
                onClick={handleSubscriptionClick}
              >
                {` ${t('common:affemity.bottomLinks.policy.subscription')}`}
              </a>
            </>
          )}
        </p>
      );
    case 'andSeparator':
      return (
        <p
          className={cx(styles.rights, className)}
          data-e2e={QA_AUTOMATION_ATTRIBUTES_AF.COMPLIANCE_TEXT}
        >
          By continuing, you agree to our{' '}
          <a
            className={styles.link}
            href="https://affemity.com/terms"
            target="_blank"
            rel="noreferrer"
            onClick={handleTermsClick}
          >
            Terms of Service
          </a>{' '}
          and{' '}
          <a
            className={styles.link}
            href="https://affemity.com/policy"
            target="_blank"
            rel="noreferrer"
            onClick={handlePrivacyClick}
          >
            Privacy Policy
          </a>
          <br />
          {`${currentYear} © All Rights Reserved.`}
        </p>
      );
    default:
      return (
        <p
          className={cx(styles.rights, className)}
          data-e2e={QA_AUTOMATION_ATTRIBUTES_AF.COMPLIANCE_TEXT}
        >
          {`${t('common:affemity.bottomLinks.text')} `}
          <a
            className={styles.link}
            href={`https://affemity.com/terms`}
            target="_blank"
            rel="noreferrer"
            onClick={handleTermsClick}
          >
            {` ${t('common:affemity.bottomLinks.policy.terms')} `}
          </a>
          |
          <a
            className={styles.link}
            href={`https://affemity.com/policy`}
            target="_blank"
            rel="noreferrer"
            onClick={handlePrivacyClick}
          >
            {` ${t('common:affemity.bottomLinks.policy.privacy')} `}
          </a>
          {isSubscription && (
            <>
              |
              <a
                className={styles.link}
                href="https://affemity.com/subscription"
                target="_blank"
                rel="noreferrer"
                onClick={handleSubscriptionClick}
              >
                {` ${t('common:affemity.bottomLinks.policy.subscription')}`}
              </a>
            </>
          )}
          <br />
          {`${t('common:affemity.bottomLinks.rights', { year: `${currentYear}` })}`}
        </p>
      );
  }
};

export default BottomLinks;
