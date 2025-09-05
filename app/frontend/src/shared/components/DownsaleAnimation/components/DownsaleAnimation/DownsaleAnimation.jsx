import ClientOnly from '~modules-private/pageBuilder/components/ClientOnly';

import { Animation } from '../Animation/Animation.client.jsx';

const DownsaleAnimation = () => {
  return (
    <ClientOnly>
      <Animation />
    </ClientOnly>
  );
};

export default DownsaleAnimation;
