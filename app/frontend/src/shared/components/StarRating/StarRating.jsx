import React, { useCallback } from 'react';
import cx from 'classnames';

import emptyStar from '../../assets/Star-empty.svg';
import fullStar from '../../assets/Star-full.svg';
import halfStar from '../../assets/Star-half.svg';

import styles from './StarRating.module.scss';

export default function StarRating({ rating = 5, big = false, containerClassName = '' }) {
  const pickLastIcons = useCallback(() => {
    if (rating > 4.2 && rating < 4.9) {
      return (
        <>
          <img
            src={fullStar}
            alt="full star"
            className={cx(styles.icon, {
              [styles.iconBig]: big,
            })}
          />
          <img
            src={fullStar}
            alt="full star"
            className={cx(styles.icon, {
              [styles.iconBig]: big,
            })}
          />
          <img
            src={halfStar}
            alt="half star"
            className={cx(styles.icon, {
              [styles.iconBig]: big,
            })}
          />
        </>
      );
    }
    if (rating > 3.7 && rating < 4.3) {
      return (
        <>
          <img
            src={fullStar}
            alt="full star"
            className={cx(styles.icon, {
              [styles.iconBig]: big,
            })}
          />
          <img
            src={fullStar}
            alt="full star"
            className={cx(styles.icon, {
              [styles.iconBig]: big,
            })}
          />
          <img
            src={emptyStar}
            alt="empty star"
            className={cx(styles.icon, {
              [styles.iconBig]: big,
            })}
          />
        </>
      );
    }
    if (rating > 3.2 && rating < 3.8) {
      return (
        <>
          <img
            src={fullStar}
            alt="full star"
            className={cx(styles.icon, {
              [styles.iconBig]: big,
            })}
          />
          <img
            src={halfStar}
            alt="half star"
            className={cx(styles.icon, {
              [styles.iconBig]: big,
            })}
          />
          <img
            src={emptyStar}
            alt="empty star"
            className={cx(styles.icon, {
              [styles.iconBig]: big,
            })}
          />
        </>
      );
    }
    if (rating > 2.7 && rating < 3.3) {
      return (
        <>
          <img
            src={fullStar}
            alt="full star"
            className={cx(styles.icon, {
              [styles.iconBig]: big,
            })}
          />
          <img
            src={emptyStar}
            alt="empty star"
            className={cx(styles.icon, {
              [styles.iconBig]: big,
            })}
          />
          <img
            src={emptyStar}
            alt="empty star"
            className={cx(styles.icon, {
              [styles.iconBig]: big,
            })}
          />
        </>
      );
    }
    if (rating < 2.8) {
      return (
        <>
          <img
            src={halfStar}
            alt="half star"
            className={cx(styles.icon, {
              [styles.iconBig]: big,
            })}
          />
          <img
            src={emptyStar}
            alt="empty star"
            className={cx(styles.icon, {
              [styles.iconBig]: big,
            })}
          />
          <img
            src={emptyStar}
            alt="empty star"
            className={cx(styles.icon, {
              [styles.iconBig]: big,
            })}
          />
        </>
      );
    }

    return (
      <>
        <img
          src={fullStar}
          alt="full star"
          className={cx(styles.icon, {
            [styles.iconBig]: big,
          })}
        />
        <img
          src={fullStar}
          alt="full star"
          className={cx(styles.icon, {
            [styles.iconBig]: big,
          })}
        />
        <img
          src={fullStar}
          alt="full star"
          className={cx(styles.icon, {
            [styles.iconBig]: big,
          })}
        />
      </>
    );
  }, [big, rating]);

  return (
    <div className={cx(styles.container, containerClassName)}>
      <img
        src={fullStar}
        alt="full star"
        className={cx(styles.icon, {
          [styles.iconBig]: big,
        })}
      />
      <img
        src={fullStar}
        alt="full star"
        className={cx(styles.icon, {
          [styles.iconBig]: big,
        })}
      />
      {pickLastIcons()}
    </div>
  );
}
