import { useContext, useState } from "react";
import { NavLink, useNavigate } from "react-router-dom";
import { ModalContext } from "../../contexts/ModalContext";
import { UserContext } from "../../contexts/UserContext";
import { active } from "../../pages/creationForms/CreationOptions";
import SideMenu from "./SideMenu";

const NavBar = () => {
  //States
  const [search, setSeach] = useState<string>();
  const [showSideMenu, setShowSideMenu] = useState<boolean>(false);
  const context = useContext(UserContext);

  //Contexts
  const modal = useContext(ModalContext);

  //Hooks
  const navigate = useNavigate();

  //Input Handler
  const searchChangeHandler = (e: any) => {
    setSeach(e.target.value);
  };

  //Side Menu Handler
  const hamburgerClickHandler = (e: any) => {
    setShowSideMenu((prevS: boolean) => !prevS);
  };

  //Submit Handler
  const submitHandler = (e: any) => {
    e.preventDefault();
    navigate(`/search/${search}`);
  };

  //Log out Handler
  const logOutHandler = () => {
    modal.showModal(
      "LOGGING OUT",
      "Are you sure you want to log out?",
      context.logOut
    );
  };

  return (
    <>
      {showSideMenu && (
        <SideMenu onLogOut={logOutHandler} onClick={hamburgerClickHandler} />
      )}
      <nav>
        <NavLink to={"/"} className={"nav-link home-link"}>
          Home
        </NavLink>
        <form className="search-bar" onSubmit={submitHandler}>
          <input
            className="input-dark"
            onChange={searchChangeHandler}
            type="text"
            placeholder="Search..."
          />
          <button type="submit">
            <i className="fa-solid fa-magnifying-glass" />
          </button>
        </form>
        {context.isLogedIn && context.userInfo?.isAdmin && (
          <NavLink to={"/add/movie"} className={active}>
            Add
          </NavLink>
        )}

        <div className="hamburger" onClick={hamburgerClickHandler}>
          <i className="fa-solid fa-bars"></i>
        </div>

        <div className="login-links">
          {!context.isLogedIn && (
            <>
              <NavLink to={"/account/login"} className={active}>
                Sign in
              </NavLink>
              <NavLink to={"/account/register"} className={active}>
                Register
              </NavLink>
            </>
          )}
          {context.isLogedIn && (
            <>
              <NavLink to={"/favorites"} className={active}>
                Favorites
              </NavLink>
              <NavLink to={`/account/profile/${context.userInfo?.id}`}>
                <div className="nav-profile-container">
                  <img
                    src={context.userInfo?.profileImageUrl}
                    alt={"User profile"}
                  />
                </div>
              </NavLink>
              <button onClick={logOutHandler} className={"nav-link"}>
                Log Out
              </button>
            </>
          )}
        </div>
      </nav>
    </>
  );
};

export default NavBar;
