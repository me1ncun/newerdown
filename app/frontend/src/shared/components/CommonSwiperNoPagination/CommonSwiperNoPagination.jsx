import React, { memo } from 'react';
import cx from 'classnames';
import { Autoplay } from 'swiper';
import { Swiper, SwiperSlide } from 'swiper/react';

import styles from './CommonSwiperNoPagination.module.css';

import 'swiper/css';
import 'swiper/css/pagination';

const CommonSwiperNoPagination = ({
  slidesComponentsList = [],
  slidesChangingDelayMs = 3000,
  className = '',
}) => {
  return (
    <div className={cx(styles.wrapper, className)}>
      <Swiper
        spaceBetween={24}
        modules={[Autoplay]}
        threshold={15}
        className={styles.swiper}
        autoplay={{
          delay: slidesChangingDelayMs,
        }}
        loop
      >
        {slidesComponentsList.map(({ slideEl, id }) => {
          return (
            <SwiperSlide className={styles.item} key={id}>
              {React.cloneElement(slideEl)}
            </SwiperSlide>
          );
        })}
      </Swiper>
    </div>
  );
};

export default memo(CommonSwiperNoPagination);
