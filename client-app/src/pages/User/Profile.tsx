import { useContext, useEffect, useState } from "react";
import { useParams } from "react-router";
import { getProfile } from "../../functions/MovieRequests";
import { ProfileType } from "../../Type/Types";
import { UserContext } from "../../Context/UserContext";
import ErrorContainer from "../../components/UI/ErrorContainer";
import Container from "../../components/UI/Container";
import LoadingCircle from "../../components/UI/LoadingCircle";
import Movie from "../../components/UI/Movie";

const Profile = () => {
  //States
  const [user, setUser] = useState<ProfileType | null>(null);
  const [error, setError] = useState<string | null>(null);
  const [isLoading, setIsLoading] = useState<boolean>(false);

  //Contexts
  const params = useParams();
  const context = useContext(UserContext);

  useEffect(() => {
    (async () => {
      if (params.id === undefined) return;
      setIsLoading(true);
      try {
        const userData: ProfileType = await getProfile(
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
  }, [context.userInfo, params.id]);

  return (
    <Container>
      <ErrorContainer error={error}>
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
