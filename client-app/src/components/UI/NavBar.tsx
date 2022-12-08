import { useContext, useState } from "react";
import { NavLink, useNavigate } from "react-router-dom";
import { UserContext } from "../Context/UserContext";
import { active } from "../CreationForms/CreationOptions";

const NavBar = () => {
  const context = useContext(UserContext);
  const [search, setSeach] = useState();
  const navigate = useNavigate();

  const searchChangeHandler = (e: any) => {
    setSeach(e.target.value);
  };

  const submitHandler = (e: any) => {
    e.preventDefault();
    navigate(`/search/${search}`);
  };

  return (
    <nav>
      <NavLink to={"/"} className={active}>
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
