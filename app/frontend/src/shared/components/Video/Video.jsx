import { useRef, useState } from 'react';
import cx from 'classnames';

import useIsDesktop from '~system-host/shared/hooks/useIsDesktop';

import playIcon from '../../assets/playIcon.svg';

import styles from './Video.module.scss';

const Video = ({ videoFile, posterImage = null, onPlay = () => {}, className = '' }) => {
  const [isTriggered, setIsTriggered] = useState(false);
  const [isPlaying, setIsPlaying] = useState(false);
  const isDesktop = useIsDesktop();

  const videoRef = useRef(null);

  function playVid() {
    videoRef.current?.play();
    setIsPlaying(true);
  }

  const handlePlay = () => {
    if (!isTriggered) {
      onPlay();
      setIsTriggered(true);
    }
  };

  return (
    <div className={cx(className, styles.wrapper)}>
      <video
        ref={videoRef}
        id="video"
        poster={posterImage}
        className={styles.videoTag}
        controls={isDesktop ? isPlaying : true}
        onPause={() => setIsPlaying(false)}
        onPlay={() => {
          handlePlay();
          setIsPlaying(true);
        }}
        preload="none"
      >
        <source src={videoFile} type="video/mp4" />
      </video>
      {isDesktop && !isPlaying && (
        <span className={styles.play} onClick={() => playVid()}>
          <img src={playIcon} alt="Play icon" className={styles.playIcon} />
        </span>
      )}
    </div>
  );
};

export default Video;
