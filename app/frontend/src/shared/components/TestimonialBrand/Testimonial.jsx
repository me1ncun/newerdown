import cx from 'classnames';

import Typography from '../../../../set-aff_adopt/shared/components/Typography';

import checkIcon from '../../../../set-aff_adopt/shared/assets/checkIcon copy.svg';
import ratingHalf from '../../../steps/step-tariffs-affiliate/assets/trustpilotRating4.svg';
import ratingFull from '../../../steps/step-tariffs-affiliate/assets/trustpilotRating5.svg';

import styles from './Testimonial.module.scss';

const Testimonial = ({
  userName = 'customer',
  ratingNum = '',
  verified = 'Verified',
  text = 'Lorem ipsum dolor sit amet, consectetur adipisicing elit. Eligendi, ratione.',
  date = '',
  className = '',
}) => {
  return (
    <div className={cx(styles.wrapper, className)}>
      <div className={styles.header}>
        <Typography.Text type="secondary bold" className={styles.name}>
          {userName}
        </Typography.Text>
        <div className={styles.rating}>
          <img
            src={ratingNum > 4.0 ? ratingFull : ratingHalf}
            alt="rating"
            className={styles.ratingImage}
          />
        </div>
        <div className={styles.verifiedBlock}>
          <img src={checkIcon} alt="checkIcon" className={styles.verifiedIcon} />
          <p className={styles.verifiedText}>{verified}</p>
        </div>
      </div>
      <div className={styles.textBlock}>
        <Typography.Text type="secondary light" className={styles.text}>
          {text}
        </Typography.Text>
      </div>
      <div className={styles.dateBlock}>
        <Typography.Text type="small light" className={styles.date}>
          {date}
        </Typography.Text>
      </div>
    </div>
  );
};

export default Testimonial;
