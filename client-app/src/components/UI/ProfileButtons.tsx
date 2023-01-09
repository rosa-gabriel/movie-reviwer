import { useContext } from "react";
import { useNavigate } from "react-router";
import { UserContext } from "../../contexts/UserContext";

type ProfileButtonsProps = {
  userId: string;
  isFriend: boolean;
  username: string;
};
const ProfileButtons = (props: ProfileButtonsProps) => {
  const navigate = useNavigate();
  const context = useContext(UserContext);

  const friendsClickHandler = () => {
    navigate("/MovieApp/account/friends");
  };

  const otherFavoritesClickHandler = () => {
    navigate(`/MovieApp/favorites/${props.username}`);
  };

  const favoritesClickHandler = () => {
    navigate(`/MovieApp/favorites/`);
  };

  const settingsClickHandler = () => {
    navigate("/MovieApp/account/settings/");
  };
  return (
    <div className="profile-buttons">
      {props.userId === context.userInfo?.id && (
        <button onClick={friendsClickHandler} className="button">
          Friends
        </button>
      )}

      {props.userId === context.userInfo?.id && (
        <button onClick={favoritesClickHandler} className="button">
          Favorites
        </button>
      )}

      {props.userId !== context.userInfo?.id && props.isFriend && (
        <button onClick={otherFavoritesClickHandler} className="button">
          Favorites
        </button>
      )}

      {props.userId === context.userInfo?.id && (
        <button onClick={settingsClickHandler} className="button">
          Settings
        </button>
      )}
    </div>
  );
};

export default ProfileButtons;
