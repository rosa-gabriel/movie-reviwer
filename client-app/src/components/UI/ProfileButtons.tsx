import { useContext } from "react";
import { useNavigate } from "react-router";
import { UserContext } from "../../contexts/UserContext";

type ProfileButtonsProps = {
  userId: string;
  isFriend: boolean;
};
const ProfileButtons = (props: ProfileButtonsProps) => {
  const navigate = useNavigate();
  const context = useContext(UserContext);

  const friendsClickHandler = () => {
    navigate("/account/friends");
  };
  const favoritesClickHandler = () => {
    navigate(`/favorites/${props.userId}`);
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
      {(props.userId === context.userInfo?.id || props.isFriend) && (
        <button onClick={favoritesClickHandler} className="button">
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
