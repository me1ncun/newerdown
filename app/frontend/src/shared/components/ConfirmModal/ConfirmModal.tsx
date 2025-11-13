import styles from './ConfirmModal.module.scss';
import { X, AlertTriangle } from 'lucide-react';

interface ConfirmModalProps {
  isOpen: boolean;
  title: string;
  message: string;
  onConfirm: () => void;
  onCancel: () => void;
  confirmText?: string;
  cancelText?: string;
  isDanger?: boolean;
}

export const ConfirmModal: React.FC<ConfirmModalProps> = ({
  isOpen,
  title,
  message,
  onConfirm,
  onCancel,
  confirmText = 'Confirm',
  cancelText = 'Cancel',
  isDanger = true,
}) => {
  if (!isOpen) return null;

  const confirmButtonClass = isDanger ? styles.buttonDanger : styles.buttonPrimary;

  return (
    <div className={styles.modalOverlay} onClick={onCancel}>
      <div className={styles.modalCard} onClick={(e) => e.stopPropagation()}>
        <header className={styles.modalHeader}>
          <div className={styles.modalTitleWrapper}>
            {isDanger && <AlertTriangle size={20} className={styles.dangerIcon} />}
            <p className={styles.modalTitle}>{title}</p>
          </div>
          <button className={styles.closeButton} aria-label="close" onClick={onCancel}>
            <X size={20} />
          </button>
        </header>

        <section className={styles.modalBody}>{message}</section>

        <footer className={styles.modalFooter}>
          <button className={`${styles.button} ${styles.buttonLight}`} onClick={onCancel}>
            {cancelText}
          </button>
          <button className={`${styles.button} ${confirmButtonClass}`} onClick={onConfirm}>
            {confirmText}
          </button>
        </footer>
      </div>
    </div>
  );
};
