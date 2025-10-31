import { createRoot } from 'react-dom/client';

import 'bulma/css/bulma.css';
import '@fortawesome/fontawesome-free/css/all.css';
import './index.scss';
import './i18n';
import { Root } from './Root';

createRoot(document.getElementById('root') as HTMLElement).render(<Root />);
