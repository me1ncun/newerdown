import { useCustomTranslation } from '~modules-public/translations';

import chatAvatar from '../assets/chatAvatar.webp';
import userAvatar from '../assets/userAvatar.webp';

const useTestimonialsConfig = () => {
  const { t } = useCustomTranslation();
  return [
    {
      id: 'msg1',
      author: 'chat',
      avatar: chatAvatar,
      content: t('commonText.chat.message1'),
      withLike: false,
      delay: 1000,
      typingDelay: 1500,
    },
    {
      id: 'msg2',
      author: 'user',
      avatar: userAvatar,
      content: t('commonText.chat.message2'),
      delay: 5400,
    },
    {
      id: 'msg3',
      author: 'chat',
      avatar: chatAvatar,
      content: t('commonText.chat.message3'),
      withLike: true,
      delay: 1500,
      typingDelay: 3000,
    },
  ];
};

export default useTestimonialsConfig;
