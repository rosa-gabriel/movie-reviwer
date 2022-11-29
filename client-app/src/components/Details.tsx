import { Navigate, useNavigate, useParams } from "react-router-dom";
import { useContext, useEffect, useState } from "react";
import Tag from "./UI/details/Tag";
import { getIsFavorite, getMovie, putFavorite } from "../functions/MoviesData";
import CastSquare from "./UI/details/CastSquare";
import LoadingCircle from "./UI/LoadingCircle";
import { AllMovieInfoType } from "../Type/Types";
import Container from "./UI/Container";
import { UserContext } from "./Context/UserContext";

const Details = (props: any) => {
  const params = useParams();
  const context = useContext(UserContext);
  const [isLoading, setIsLoading] = useState<boolean>(false);
  const [isLiked, setIsLiked] = useState<boolean>(false);
  const [movieInfo, setMovieInfo] = useState<AllMovieInfoType | null>(null);
  const [showAllTags, setShowAllTags] = useState<boolean>(false);
  const [seed, setSeed] = useState<number>(1);

  const reset = () => {
    setSeed(Math.random);
  };

  const favoriteHandler = async () => {
    if (!context.userInfo) return;
    await putFavorite(String(params.movieId), !isLiked, context.userInfo.token);
    reset();
  };

  const showAllHandler = () => {
    setShowAllTags(true);
  };

  useEffect(() => {
    (async (id: any) => {
      try {
        const movieData: AllMovieInfoType = await getMovie(id);

        setMovieInfo({
          ...movieData,
          movie: {
            ...movieData.movie,
            releaseDate: new Date(movieData.movie.releaseDate),
          },
        });

        if (context.isLogedIn) {
          if (!context.userInfo) return;

          const favoriteData: any = await getIsFavorite(
            String(params.movieId),
            context.userInfo.token
          );
          setIsLiked(favoriteData);
        }
        setIsLoading(false);
      } catch (ex: any) {
        console.error(ex);
        setIsLoading(false);
      }
    })(params.movieId);
  }, [params.movieId, seed]);

  return (
    <Container>
      <>
        {!isLoading && movieInfo != null && (
          <div className="details_container">
            <div>
              <img
                src={movieInfo.movie.coverUrl}
                className="details_cover"
                alt={"Cover for " + movieInfo.movie.name}
              />
            </div>

            <div className="details_info">
              <h1>{movieInfo.movie.name}</h1>

              <p className="info_container_title">TAGS</p>
              {movieInfo.tags.length > 0 && (
                <div className="info_container">
                  {movieInfo.tags.map((tag) => (
                    <Tag key={tag.name} tagItem={tag} />
                  ))}
                </div>
              )}
              {movieInfo.tags.length === 0 && <p className="faded">0 Tags.</p>}

              <p className="info_container_title">CAST</p>
              {movieInfo.castMembers.length > 0 && showAllTags && (
                <div className="info_container">
                  {movieInfo.castMembers.map((person) => (
                    <CastSquare key={person.name} person={person} />
                  ))}
                </div>
              )}

              {movieInfo.castMembers.length > 0 && !showAllTags && (
                <div className="info_container">
                  {movieInfo.castMembers
                    .filter((item, i) => {
                      return i < 15;
                    })
                    .map((person) => (
                      <CastSquare key={person.name} person={person} />
                    ))}
                  {movieInfo.castMembers.length > 20 && (
                    <button
                      onClick={showAllHandler}
                      className="show-more"
                      style={{ marginTop: 0 }}
                    >
                      Show all...
                    </button>
                  )}
                </div>
              )}

              {movieInfo.castMembers.length === 0 && (
                <p className="faded">No cast members.</p>
              )}

              <p className="details-releaseDate">
                Release date: {movieInfo.movie.releaseDate.toDateString()}
              </p>

              {context.isLogedIn && (
                <button
                  className={
                    "favorite-button" + (isLiked ? " selected-text" : "")
                  }
                  onClick={favoriteHandler}
                >
                  <i className={!isLiked ? "fa-regular fa-heart" : "fa-solid fa-heart"} />
                  {movieInfo.favorites}
                </button>
              )}

              {!context.isLogedIn && (
                <button className="favorite-button" disabled>
                  <i className="fa-regular fa-heart" />
                  {movieInfo.favorites}
                </button>
              )}
            </div>
          </div>
        )}
        {isLoading && <LoadingCircle />}
      </>
    </Container>
  );
};
export default Details;
