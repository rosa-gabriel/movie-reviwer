import { useContext } from "react";
import { NavLink } from "react-router-dom";
import { UserContext } from "../../contexts/UserContext";

type SideMenuProps = {
  onClick(...args: any[]): any;
  onLogOut(...args: any[]): any;
};

const SideMenu = (props: SideMenuProps) => {
  const context = useContext(UserContext);

  return (
    <div className="modal-background" onClick={props.onClick}>
      <div className="side-menu">
        {context.isLogedIn && (
          <h1>
            Welcome{" "}
            <NavLink to={"/favorites"}>{context.userInfo?.username}</NavLink>.
          </h1>
        )}
        {!context.isLogedIn && (
          <h1>
            <NavLink to={"/Account/login"}>Sign in</NavLink>, for more options.
          </h1>
        )}
        <ul>
          {context.isLogedIn && (
            <NavLink to={"/favorites"}>
              <li>Add</li>
            </NavLink>
          )}
          {context.isLogedIn && (
            <NavLink to={"/favorites"}>
              <li>Favorites</li>
            </NavLink>
          )}
        </ul>

        {context.isLogedIn && (
          <div className="account-side">
            <NavLink to={`/account/profile/${context.userInfo?.id}`}>
              <div className="nav-profile-container">
                <img
                  src={context.userInfo?.profileImageUrl}
                  alt={"User profile"}
                />
              </div>
            </NavLink>

            <i
              onClick={props.onLogOut}
              className="fa-solid fa-right-from-bracket"
            ></i>
          </div>
        )}
      </div>
    </div>
  );
};

export default SideMenu;
