import { useLottie } from 'lottie-react';

import saleAnimation from '../../assets/saleAnimation.json';

export const Animation = () => {
  const style = {
    width: '45%',
    height: '45%',
  };

  const options = {
    animationData: saleAnimation,
    loop: true,
    autoplay: true,
  };

  const { View } = useLottie(options, style);

  return <>{View}</>;
};
