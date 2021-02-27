import React, { useEffect } from "react";
import M from "materialize-css/dist/js/materialize.min.js";

const Header = (props) => {
  useEffect(() => {
    M.AutoInit();
  });
  return (
    <>
      <nav className="nav_wrapper">
        <div className="container">
          <a href="/" class="brand-logo center">
            MaybeKinectGame
          </a>
          <ul id="nav-mobile" class="right">
            <li>
              <a class="dropdown-trigger" href="#!" data-target="dropdown1">
                {props.name}
                <i
                  className="material-icons right"
                  style={{ paddingTop: "4%" }}
                >
                  account_circle
                </i>
              </a>
            </li>
          </ul>
        </div>
      </nav>
      <ul id="dropdown1" class="dropdown-content">
        <li>
          <a href="/">Sign Out</a>
        </li>
      </ul>
    </>
  );
};

export default Header;
