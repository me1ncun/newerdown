import cx from 'classnames';

import QA_AUTOMATION_ATTRIBUTES_AF from '~system-host/shared/constants/attributesAutomationAf';

import getPriceWithCurrencyTag from '../../lib/currencyHelpers/getPriceWithCurrencyTag';

import styles from './BottomLinksDynamicLatam.module.scss';

const BottomLinksDynamicLatam = ({
  className,
  price,
  currency = 'USD',
  currencyTagType = 'code-alpha',
  isRecurrent = false,
}) => {
  const fullPrice = getPriceWithCurrencyTag(price?.toFixed(2), currency, currencyTagType);

  const periodName = isRecurrent ? `período de renovación` : `oferta introductoria de semana`;

  return (
    <p
      className={cx(styles.rights, className)}
      data-e2e={QA_AUTOMATION_ATTRIBUTES_AF.COMPLIANCE_TEXT}
    >
      {`Al continuar acepta que si no cancela al menos 24 horas antes del final del ${periodName}, se le cobrará automáticamente el precio total de `}
      <span className={styles.rightsStrong}>{fullPrice}</span>
      {` cada semana hasta que cancele contactando con el equipo de soporte. Más información sobre la política de cancelación en `}
      <a
        className={styles.link}
        href="https://affemity.com/info/subscription"
        target="_blank"
        rel="noreferrer"
      >
        Términos de Suscripción.
      </a>
      {isRecurrent &&
        ` Condiciones de suscripciónEl pago se procesará automáticamente en función de la información de facturación que nos haya facilitado previamente.`}
    </p>
  );
};

export default BottomLinksDynamicLatam;
