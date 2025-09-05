import cx from 'classnames';

import QA_AUTOMATION_ATTRIBUTES_AF from '~system-host/shared/constants/attributesAutomationAf';

import StarRating from '../StarRating';
import Typography from '../Typography';

import styles from './Testimonial.module.scss';

const Testimonial = ({
  userName = 'Maam',
  ratingNum = 5,
  text = '5 stars',
  className = '',
  image,
  isAdaptive = false,
}) => {
  return (
    <div
      className={cx(styles.wrapper, className, {
        [styles.wrapperAdaptive]: isAdaptive,
      })}
      data-e2e={QA_AUTOMATION_ATTRIBUTES_AF.COMMENT_CARD}
    >
      {image && <img className={styles.image} src={image} alt="testimonial" />}
      <div className={styles.row}>
        <div>
          <Typography.Text type="secondary bold" className={styles.note}>
            {userName}
          </Typography.Text>
          <div className={styles.row}>
            <StarRating rating={ratingNum} />
            <div className={styles.rating}>{ratingNum}</div>
          </div>
        </div>
      </div>
      <Typography.Text type="secondary light" className={styles.noteText}>
        {text}
      </Typography.Text>
    </div>
  );
};

export default Testimonial;
