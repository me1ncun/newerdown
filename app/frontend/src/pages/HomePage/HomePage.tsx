import { Link } from 'react-router-dom';

import whiteLogo from './images/white-logo.png';
import blackLogo from './images/black-logo.png';
import sporty4Bike from './images/white-logo.png';
import rideInTownST from './images/white-logo.png';
import agileRide3 from './images/white-logo.png';
import autoUnlockWide from './images/white-logo.png';
import autoUnlockSquare from './images/white-logo.png';
import rangeAndLightsWide from './images/white-logo.png';
import rangeAndLightsSquare from './images/white-logo.png';
import brakesLightweightSquare from './images/white-logo.png';
import brakesLightweightWide from './images/white-logo.png';

import './styles/style.scss';

export const HomePage = () => (
  <div className="page__body">
    <header className="header">
      <div className="container">
        <div className="header__content">
          <div className="top-bar">
            <a href="#" className="top-bar__logo-link">
              <img src={whiteLogo} alt="MyBike logo" className="top-bar__logo" />
            </a>
            <div className="top-bar__icons">
              <span className="top-bar__phone-number">+1 234 5555-55-55</span>
              <a href="tel:+1 234 5555-55-55" className="icon icon--phone"></a>
              <a href="#menu" className="icon icon--menu"></a>
            </div>
          </div>
          <div className="header__bottom">
            <h1 className="header__title">Monitor Your Websites</h1>
          </div>
        </div>
      </div>
    </header>

    <aside className="page_menu menu" id="menu">
      <div className="container">
        <div className="top-bar menu__top">
          <a href="#" className="top-bar__logo-link top-bar__logo-link--black">
            <img src={blackLogo} alt="MyBike logo" className="top-bar__logo" />
          </a>
          <a href="#" className="icon icon--close"></a>
        </div>
        <div className="menu__bottom">
          <nav className="nav menu__nav">
            <ul className="nav__list">
              <li className="nav__item">
                <a className="nav__link" href="#">
                  Home
                </a>
              </li>
              <li className="nav__item">
                <a className="nav__link" href="#about-us">
                  About Us
                </a>
              </li>
              <li className="nav__item">
                <a className="nav__link" href="#compare-bikes">
                  Top Sites
                </a>
              </li>
              <li className="nav__item">
                <a className="nav__link" href="#details">
                  Details
                </a>
              </li>
              <li className="nav__item">
                <a className="nav__link" href="#contacts">
                  Contacts
                </a>
              </li>
            </ul>
          </nav>
          <a href="tel:+1 234 5555-55-55" className="menu__phone-number">
            +1 234 5555-55-55
          </a>
          <a href="tel:+1 234 5555-55-55" className="menu__call-to-order">
            Call to order
          </a>
        </div>
      </div>
    </aside>

    <main className="main">
      <div className="container">
        <div className="main__content">
          <section className="about-us" id="about-us">
            <h2 className="about-us__title section-title section-title--align--left">
              Stay Informed
            </h2>

            <div className="about-us__content">
              <p className="about-us__description">
                NewerBroken monitors your favorite websites in real-time. Get instant alerts when a
                site goes down or experiences issues so you never miss a thing.
              </p>
            </div>
          </section>

          <section className="compare-bikes" id="compare-bikes">
            <h2 className="section-title">Top Monitored Sites</h2>

            <div className="recommended__products">
              <article className="product recommended__product">
                <img src={sporty4Bike} alt="Sporty 4" className="product__photo" />
                <h3 className="product__title">Website A</h3>
                <p className="product__description">
                  Uptime 99.99%, response time 120ms. Reliable performance tracked 24/7.
                </p>
                <p className="product__price">$ 2 590</p>
              </article>

              <article className="product recommended__product">
                <img src={rideInTownST} alt="Ride in town ST" className="product__photo" />
                <h3 className="product__title">Website B</h3>
                <p className="product__description">
                  Uptime 99.5%, response time 300ms. Historical performance data and alerts.
                </p>
                <p className="product__price">$ 2 590</p>
              </article>

              <article className="product recommended__product">
                <img src={agileRide3} alt="Agile ride 3" className="product__photo" />
                <h3 className="product__title">Website C</h3>
                <p className="product__description">
                  Uptime 98.7%, response time 450ms. Track outages and performance metrics easily.
                </p>
                <p className="product__price">$ 2 090</p>
              </article>
            </div>
          </section>

          <section className="details" id="details">
            <h2 className="section-title">Features</h2>

            <div className="details__wrapper">
              <article className="detail">
                <div className="detail__photos">
                  <a href="#" className="detail__link detail__link--wide">
                    <img className="detail__photo" src={autoUnlockWide} alt="auto unlock" />
                  </a>

                  <a href="#" className="detail__link detail__link--square">
                    <img className="detail__photo" src={autoUnlockSquare} alt="auto unlock" />
                  </a>
                </div>

                <h3 className="detail__title">Real-time Monitoring</h3>
                <p className="detail__description">
                  Get instant notifications when a website goes down or experiences slowdowns.
                  Always stay ahead of issues.
                </p>
              </article>

              <article className="detail">
                <div className="detail__photos">
                  <a href="#" className="detail__link detail__link--wide">
                    <img className="detail__photo" src={rangeAndLightsWide} alt="auto unlock" />
                  </a>

                  <a href="#" className="detail__link detail__link--square">
                    <img className="detail__photo" src={rangeAndLightsSquare} alt="auto unlock" />
                  </a>
                </div>

                <h3 className="detail__title">Historical Data</h3>
                <p className="detail__description">
                  Access detailed uptime and performance reports for your websites. Analyze trends
                  and improve reliability.
                </p>
              </article>

              <article className="detail">
                <div className="detail__photos">
                  <a href="#" className="detail__link detail__link--square">
                    <img
                      className="detail__photo"
                      src={brakesLightweightSquare}
                      alt="auto unlock"
                    />
                  </a>

                  <a href="#" className="detail__link detail__link--wide">
                    <img className="detail__photo" src={brakesLightweightWide} alt="auto unlock" />
                  </a>
                </div>

                <h3 className="detail__title">Custom Alerts</h3>
                <p className="detail__description">
                  Configure alerts via email, SMS, or push notifications. Receive updates tailored
                  to your needs.
                </p>
              </article>

              <article className="detail__buttons">
                <Link to="/monitoring" className="detail__button buttons">
                  Get started
                </Link>
              </article>
            </div>
          </section>

          <section className="contacts" id="contacts">
            <h2 className="section-title">Contact us</h2>

            <div className="contacts__wrapper">
              <form
                className="contact"
                onSubmit={(e) => {
                  e.preventDefault();
                  e.currentTarget.reset();
                }}
              >
                <div className="contact__left-side">
                  <div className="contact__inputs">
                    <div className="contact__left-side-input">
                      <input
                        className="contact__input inputs"
                        type="text"
                        placeholder="Name (e.g. John Doe)"
                        minLength={2}
                        maxLength={20}
                        required
                      />
                    </div>
                    <div className="contact__left-side-input">
                      <input
                        className="contact__input inputs"
                        type="email"
                        placeholder="Email (e.g. example@mail.com)"
                        required
                      />
                    </div>
                    <div className="contact__left-side-input">
                      <textarea
                        className="contact__input contact__input--textarea inputs"
                        placeholder="Message (Your message here)"
                        required
                      ></textarea>
                    </div>
                  </div>
                  <article className="contact__buttons">
                    <button type="submit" className="contact__button buttons">
                      Send
                    </button>
                  </article>
                </div>

                <div className="contact__right-side">
                  <div className="contact__info">
                    <div className="contact__left-side-info">
                      <p className="contact__p">Phone</p>
                      <p className="contact__p--down">
                        <a className="contact__link" href="tel:+123455555555">
                          +1 234 5555-55-55
                        </a>
                      </p>
                    </div>

                    <div className="contact__left-side-info">
                      <p className="contact__p">Email</p>
                      <p className="contact__p--down">
                        <a className="contact__link" href="mailto:hello@miami.com">
                          hello@miami.com
                        </a>
                      </p>
                    </div>

                    <div className="contact__left-side-info">
                      <p className="contact__p">Address</p>
                      <p className="contact__p--down">
                        <a
                          className="contact__link"
                          href="https://www.google.com/maps/place/400+First+Ave,+Suite+700,+Minneapolis,+MN+55401"
                          target="_blank"
                        >
                          400 first ave.
                          <br />
                          suite 700
                          <br />
                          Minneapolis, MN 55401
                        </a>
                      </p>
                    </div>
                  </div>
                </div>
              </form>
            </div>
          </section>
        </div>
      </div>
    </main>

    <footer className="footer"></footer>
  </div>
);
