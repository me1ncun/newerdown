import cx from 'classnames';

import StarRating from '../../components/StarRating';
import Typography from '../../components/Typography';

import styles from './TestimonialCompliance.module.scss';

const TestimonialCompliance = ({
  userName = 'Angel',
  ratingNum = 5,
  className = '',
  text = 'Lorem ipsum dolor sit amet, consectetur adipisicing elit. Eligendi, ratione.',
}) => {
  return (
    <div className={cx(styles.wrapper, className)}>
      <div className={styles.header}>
        <Typography.Text type="body bold" className={styles.note}>
          {userName}
        </Typography.Text>

        <StarRating rating={ratingNum} />
      </div>
      <Typography.Text type="small light" className={styles.noteText}>
        {text}
      </Typography.Text>
    </div>
  );
};

export default TestimonialCompliance;
