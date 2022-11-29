import { useContext } from "react";
import { NavLink } from "react-router-dom";
import { UserContext } from "../Context/UserContext";
import { active } from "../CreationForms/CreationOptions";

const NavBar = () => {
  const context = useContext(UserContext);
  return (
    <nav>
      <NavLink to={"/"} className={active}>
        Home
      </NavLink>
      <form className="search-bar">
        <input className="input-dark" type="text" placeholder="Search..." />
        <button type="submit">
          <i className="fa-solid fa-magnifying-glass" />
        </button>
      </form>
      {context.isLogedIn && (
        <NavLink to={"/add/movie"} className={active}>
          Add
        </NavLink>
      )}
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
                <img src={context.userInfo?.profileImageUrl} />
              </div>
            </NavLink>
            <button onClick={context.logOut} className={"nav-link"}>
              Log Out
            </button>
          </>
        )}
      </div>
    </nav>
  );
};

export default NavBar;
