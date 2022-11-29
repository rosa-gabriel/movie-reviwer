import { useContext, useEffect, useState } from "react";
import { useNavigate, useParams } from "react-router";
import { getProfile } from "../functions/MoviesData";
import { MovieType, ProfileType } from "../Type/Types";
import { UserContext } from "./Context/UserContext";
import Container from "./UI/Container";
import LoadingCircle from "./UI/LoadingCircle";
import Movie from "./UI/Movie";

const Profile = () => {
  const params = useParams();
  const navigate = useNavigate();
  const context = useContext(UserContext);
  const [user, setUser] = useState<ProfileType | null>(null);
  const [hasError, setHasError] = useState<string | null>(null);
  const [isLoading, setIsLoading] = useState<boolean>(false);

  useEffect(() => {
    (async () => {
      if (params.id == undefined) return;
      setIsLoading(true);
      try {
        const userData: ProfileType = await getProfile(
          params.id,
          context.userInfo?.token
        );
        setUser(userData);
      } catch (ex: any) {
        setHasError(ex.message);
        navigate("/error");
      } finally {
        setIsLoading(false);
      }
    })();
  }, [params.id]);

  return (
    <Container>
      <>
        {user !== null && (
          <div style={{ padding: "30px" }}>
            <div>
              <div className="details_container">
                <img
                  src={user.imageUrl}
                  className="details_cover"
                  alt={"Profile picture for" + user.name}
                />
                <div className="details_info">
                  <h1>{user.name}</h1>
                  <p className="profile-info">Bio: {user.bio}</p>
                  <p className="profile-info">Favorite tags: {user.bio}</p>
                  <p className="profile-info">
                    <>Creation date: {user.creationDate.toDateString()}</>
                  </p>
                </div>
              </div>
            </div>

            <h1 className="title">Recent Favorites</h1>

            <div style={{ marginTop: "50px" }}>
              {!isLoading && !hasError && user.recentFavorites.length > 0 && (
                <div className="grid-container">
                  {user.recentFavorites.map((movie: any) => {
                    return <Movie movie={movie} key={movie.id} />;
                  })}
                </div>
              )}

              {!isLoading && !hasError && user.recentFavorites.length === 0 && (
                <p className="faded centered-message">No favorites.</p>
              )}

              {!isLoading && hasError && (
                <p className="error-message">{hasError}</p>
              )}
              {isLoading && !hasError && <LoadingCircle />}
            </div>
          </div>
        )}
        {user === null && <LoadingCircle />}
      </>
    </Container>
  );
};

export default Profile;
