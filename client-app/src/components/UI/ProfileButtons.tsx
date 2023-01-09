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
    navigate("/account/friends");
  };

  const otherFavoritesClickHandler = () => {
    navigate(`/favorites/${props.username}`);
  };

  const favoritesClickHandler = () => {
    navigate(`/favorites/`);
  };

  const settingsClickHandler = () => {
    navigate("/account/settings/");
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
