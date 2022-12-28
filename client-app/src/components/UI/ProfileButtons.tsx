import { useNavigate } from "react-router";

type ProfileButtonsProps = {
  userId: string;
};
const ProfileButtons = (props: ProfileButtonsProps) => {
  const navigate = useNavigate();

  const friendsClickHandler = () => {
    navigate("/");
  };
  const favoritesClickHandler = () => {
    navigate(`/favorites/${props.userId}`);
  };
  const settingsClickHandler = () => {
    navigate("/account/settings/");
  };
  return (
    <div className="profile-buttons">
      <button onClick={friendsClickHandler} className="button">
        Friends
      </button>
      <button onClick={favoritesClickHandler} className="button">
        Favorites
      </button>
      <button onClick={settingsClickHandler} className="button">
        Settings
      </button>
    </div>
  );
};

export default ProfileButtons;
