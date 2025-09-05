import React, { memo } from 'react';
import cx from 'classnames';
import { Autoplay, EffectCoverflow } from 'swiper';
import { Swiper, SwiperSlide } from 'swiper/react';

import styles from './CommonSwiperWithCoverFlow.module.scss';

import 'swiper/css';
import 'swiper/css/pagination';

const CommonSwiperWithCoverFlow = ({
  slidesComponentsList = [],
  slidesChangingDelayMs = 3000,
  spaceBetweenSlides = 24,
  shouldBeCentered = false,
  slidesPerView = `auto`,
  className = '',
}) => {
  return (
    <div className={cx(styles.wrapper, className)}>
      <Swiper
        spaceBetween={spaceBetweenSlides}
        modules={[EffectCoverflow, Autoplay]}
        centeredSlides={shouldBeCentered}
        slidesPerView={slidesPerView}
        effect="coverflow"
        coverflowEffect={{
          scale: 1,
          rotate: 0,
          stretch: -16,
          depth: 100,
          modifier: 1,
          slideShadows: false,
        }}
        threshold={15}
        className={cx(styles.swiper)}
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

export default memo(CommonSwiperWithCoverFlow);
