import { useCustomTranslation } from '~modules-public/translations';

import photo1 from '../assets/testimonial-photo-small-1.webp';
import photo2 from '../assets/testimonial-photo-small-2.webp';
import photo3 from '../assets/testimonial-photo-small-3.webp';

const useTestimonialsConfig = () => {
  const { t } = useCustomTranslation();

  return [
    {
      name: t('testimonials.t1.name'),
      text: t('testimonials.t1.text'),
      textFull: t('testimonials.t1.textFull'),
      rating: t('testimonials.t1.rating'),
      photo: photo1,
    },
    {
      name: t('testimonials.t2.name'),
      text: t('testimonials.t2.text'),
      rating: t('testimonials.t2.rating'),
      photo: photo2,
    },
    {
      name: t('testimonials.t3.name'),
      text: t('testimonials.t3.text'),
      rating: t('testimonials.t3.rating'),
      photo: photo3,
    },
  ];
};

export default useTestimonialsConfig;
