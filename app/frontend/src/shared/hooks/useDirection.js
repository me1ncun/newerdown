import { useContext } from 'react';

import { DirectionContext } from '../providers/DirectionProvider.jsx';

const useDirection = () => {
  return useContext(DirectionContext);
};

export default useDirection;
