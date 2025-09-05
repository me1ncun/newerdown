import React from 'react';
import cx from 'classnames';
import { Autoplay, Pagination } from 'swiper';
import { Swiper, SwiperSlide } from 'swiper/react';

import styles from './CommonSwiper.module.scss';

import 'swiper/css';
import 'swiper/css/pagination';

const CommonSwiper = ({
  slidesComponentsList = [],
  slidesChangingDelayMs = 3000,
  spaceBetween = 40,
  slidesPerView = `auto`,
  shouldBeCentered = false,
  className = '',
  colorScheme,
  autoplay = true,
}) => {
  const bulletClass =
    colorScheme === 'bullet-color-primary-bg'
      ? cx(styles.swiperBullet, styles.swiperBulletBg)
      : styles.swiperBullet;
  const bulletActiveClass =
    colorScheme === 'bullet-color-primary-bg'
      ? cx(styles.swiperBulletActive, styles.swiperBulletPrimary3)
      : styles.swiperBulletActive;

  const autoplayProp = autoplay && slidesChangingDelayMs ? { delay: slidesChangingDelayMs } : false;

  return (
    <div className={cx(styles.wrapper, className)}>
      <Swiper
        spaceBetween={spaceBetween}
        slidesPerView={slidesPerView}
        centeredSlides={shouldBeCentered}
        pagination={{
          clickable: true,
          bulletClass,
          bulletActiveClass,
        }}
        modules={[Pagination, Autoplay]}
        threshold={15}
        className={styles.swiper}
        style={{ paddingBottom: '4em', zIndex: 1 }}
        autoplay={autoplayProp}
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

export default CommonSwiper;
