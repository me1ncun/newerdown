import cx from 'classnames';

import styles from './Button.module.scss';

const Button = ({
  className = '',
  onClick = () => {},
  children = '',
  type = '',
  icon = '',
  iconClassName = '',
  ...rest
}) => {
  return (
    <button
      className={cx(styles.button, className, {
        [styles.buttonTransparent]: type === 'transparent',
        [styles.buttonWithIcon]: !!icon,
      })}
      onClick={onClick}
      {...rest}
    >
      {icon && <img src={icon} alt="icon" className={iconClassName} />}
      {children}
    </button>
  );
};

export default Button;
