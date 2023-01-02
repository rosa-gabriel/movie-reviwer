import { useContext, useEffect, useState } from "react";
import { useParams } from "react-router";
import { getProfile } from "../../functions/requests/MovieRequests";
import { ProfileInfo } from "../../types/Types";
import { UserContext } from "../../contexts/UserContext";
import ErrorContainer from "../../components/UI/ErrorContainer";
import Container from "../../components/UI/Container";
import LoadingCircle from "../../components/UI/LoadingCircle";
import Movie from "../../components/UI/Movie";
import ProfileButtons from "../../components/UI/ProfileButtons";
import { NotificationContext } from "../../contexts/NotificationContext";
import {
  deleteFriend,
  postFriendRequest,
} from "../../functions/requests/AccouontRequests";

const Profile = () => {
  //States
  const [user, setUser] = useState<ProfileInfo | null>(null);
  const [error, setError] = useState<string | null>(null);
  const [isLoading, setIsLoading] = useState<boolean>(false);
  const [seed, setSeed] = useState<number>(1);

  const reset = () => {
    setSeed(Math.random());
  };

  //Contexts
  const params = useParams();
  const context = useContext(UserContext);
  const notification = useContext(NotificationContext);

  useEffect(() => {
    (async () => {
      if (params.id === undefined) return;
      setIsLoading(true);
      try {
        const userData: ProfileInfo = await getProfile(
          params.id,
          context.userInfo?.token
        );
        setUser(userData);
      } catch (ex: any) {
        setError(ex.message);
      } finally {
        setIsLoading(false);
      }
    })();
  }, [context.userInfo, params.id, seed]);

  const friendRequestHandler = async () => {
    try {
      await postFriendRequest(
        String(params.id),
        String(context.userInfo?.token)
      );
      notification.addNotification({
        code: "SENT",
        text: "Friend request sent.",
        error: false,
      });
    } catch (ex: any) {
      notification.addNotification({
        code: "FAILED",
        text: "Failed to make friend request",
        error: true,
      });
    } finally {
      reset();
    }
  };

  const removeFriendHandler = async () => {
    try {
      await deleteFriend(String(user?.id), String(context.userInfo?.token));
      notification.addNotification({
        code: "REMOVED",
        text: "Friend removed.",
        error: false,
      });
    } catch (ex: any) {
      notification.addNotification({
        code: "FAILED",
        text: "Failed to remove friend friend",
        error: true,
      });
    } finally {
      reset();
    }
  };

  return (
    <Container>
      <ErrorContainer error={error}>
        <>
          {context.isLogedIn && (
            <>
              {user !== null &&
                !user?.hasRequested &&
                user?.id !== context.userInfo?.id && (
                  <button
                    className="button edit-button"
                    type="button"
                    onClick={friendRequestHandler}
                  >
                    <i className="fa-solid fa-user-plus"></i>
                  </button>
                )}
              {user?.hasRequested && (
                <button
                  className="button edit-button"
                  type="button"
                  onClick={removeFriendHandler}
                >
                  <i className="fa-solid fa-user-minus"></i>
                </button>
              )}
            </>
          )}
          {user !== null && (
            <div style={{ padding: "30px" }}>
              <div>
                <div className="details_container">
                  <div className="details-img-container">
                    <img
                      src={user.imageUrl}
                      className="details_cover"
                      alt={"Profile picture for" + user.name}
                    />
                  </div>
                  <div className="details_info">
                    <h1>{user.name}</h1>
                    <p className="profile-info">Bio: {user.bio}</p>
                    <p className="profile-info">Favorite tags: {user.bio}</p>
                    <p className="profile-info">
                      <>Creation date: {user.creationDate.toDateString()}</>
                    </p>
                    <ProfileButtons isFriend={user.isFriend} userId={user.id} username={user.name} />
                  </div>
                </div>
              </div>

              <h1 className="title">Recent Favorites</h1>

              <div style={{ marginTop: "50px" }}>
                {!isLoading && user.recentFavorites.length > 0 && (
                  <div className="grid-container">
                    {user.recentFavorites.map((movie: any) => {
                      return <Movie movie={movie} key={movie.id} />;
                    })}
                  </div>
                )}

                {!isLoading && user.recentFavorites.length === 0 && (
                  <p className="faded centered-message">No favorites.</p>
                )}

                {isLoading && <LoadingCircle />}
              </div>
            </div>
          )}
          {user === null && <LoadingCircle />}
        </>
      </ErrorContainer>
    </Container>
  );
};

export default Profile;
