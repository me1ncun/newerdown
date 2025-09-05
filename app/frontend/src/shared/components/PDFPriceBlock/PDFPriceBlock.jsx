import cx from 'classnames';

import getPriceWithCurrencyTag from '../../lib/currencyHelpers/getPriceWithCurrencyTag';

import styles from './PDFPriceBlock.module.scss';

const PDFPriceBlock = ({
  className = '',
  currency = '$',
  currencyTagType = 'symbol',
  currentPrice,
  type = 'default',
}) => {
  const isBold = type === 'bold';

  const oldDayPrice = getPriceWithCurrencyTag(
    (currentPrice * 2 + 0.01).toFixed(2),
    currency,
    currencyTagType,
  );

  const currentFullPrice = getPriceWithCurrencyTag(currentPrice, currency, currencyTagType);

  return (
    <div className={cx(className, isBold ? [styles.priceBold] : [styles.price])}>
      <div className={cx(styles.wrapper, { [styles.wrapperBold]: isBold })}>
        <span
          className={cx(styles.currentPrice, styles.currentPriceOldPrice, {
            [styles.currentPriceOldPriceBold]: isBold,
          })}
        >
          {oldDayPrice}
        </span>
        {!isBold && <>&nbsp;</>}
        <span
          className={cx(styles.currentPrice, styles.currentPriceNewPrice, {
            [styles.currentPriceNewPriceBold]: isBold,
          })}
        >
          {currentFullPrice}
        </span>
      </div>
    </div>
  );
};

export default PDFPriceBlock;
