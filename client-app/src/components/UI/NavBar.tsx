import { NavLink } from "react-router-dom";
import { active } from "../CreationForms/CreationOptions";

const NavBar = () => {
  return (
    <nav>
      <NavLink to={"/"} className={active}>
        Home
      </NavLink>
      <form className="search-bar">
        <input className="input-dark" type="text" placeholder="Search..."/>
        <button type="submit">
          <i className="fa-solid fa-magnifying-glass"/>
        </button>
      </form>
      <NavLink to={"/add/movie"} className={active}>
        Add
      </NavLink>
      <NavLink to={"/:userId/favorites/"} className={active}>
        Favorites
      </NavLink>
      <div className="login-links">
        <NavLink to={""} className={active}>
          Sign in
        </NavLink>
        <NavLink to={""} className={active}>
          Register
        </NavLink>
      </div>
    </nav>
  );
};

export default NavBar;
