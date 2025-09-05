import React, { memo, useRef } from 'react';
import cx from 'classnames';
import { Autoplay, Navigation } from 'swiper';
import { Swiper, SwiperSlide } from 'swiper/react';

import arrowIcon from '../../assets/arrowBold.svg';

import styles from './CommonSwiperWithNavigation.module.scss';

import 'swiper/css';
import 'swiper/css/pagination';

const CommonSwiperWithNavigation = ({
  slidesComponentsList = [],
  slidesChangingDelayMs = 3000,
  spaceBetweenSlides = 24,
  shouldBeCentered = false,
  slidesPerView = `auto`,
  className = '',
  disableAutoplayAndNav = false,
  useFadeMask = false,
}) => {
  const swiperRef = useRef(null);
  const modules = disableAutoplayAndNav ? [] : [Autoplay, Navigation];
  return (
    <div className={cx(styles.wrapper, className, { [styles.fadeMask]: useFadeMask })}>
      {!disableAutoplayAndNav && (
        <>
          <div
            className={cx(styles.swiperButton, styles.swiperButtonNext)}
            onClick={() => swiperRef.current?.slideNext()}
          >
            <img src={arrowIcon} alt="Arrow next" className={styles.swiperButtonNextArrow} />
          </div>
          <div
            className={cx(styles.swiperButton, styles.swiperButtonPrev)}
            onClick={() => swiperRef.current?.slidePrev()}
          >
            <img src={arrowIcon} alt="Arrow next" className={styles.swiperButtonPrevArrow} />
          </div>
        </>
      )}
      <Swiper
        onSwiper={(swiper) => (swiperRef.current = swiper)}
        spaceBetween={spaceBetweenSlides}
        modules={modules}
        threshold={15}
        centeredSlides={shouldBeCentered}
        slidesPerView={slidesPerView}
        allowTouchMove={false}
        navigation={
          disableAutoplayAndNav
            ? undefined
            : {
                nextEl: styles.swiperButtonNext,
                prevEl: styles.swiperButtonPrev,
              }
        }
        className={cx(styles.swiper)}
        autoplay={
          disableAutoplayAndNav
            ? false
            : {
                delay: slidesChangingDelayMs,
              }
        }
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

export default memo(CommonSwiperWithNavigation);
