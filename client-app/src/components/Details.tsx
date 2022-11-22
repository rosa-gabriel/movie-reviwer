import { useParams } from "react-router-dom";
import { useEffect, useState } from "react";
import Tag from "./UI/details/Tag";
import { getMovie } from "../functions/MoviesData";
import CastSquare from "./UI/details/CastSquare";
import LoadingCircle from "./UI/LoadingCircle";
import { AllMovieInfoType } from "../Type/Types";
import Container from "./UI/Container";

const Details = (props: any) => {
  const params = useParams();
  const [isLoaded, setIsLoaded] = useState<boolean>(false);
  const [movieInfo, setMovieInfo] = useState<AllMovieInfoType | null>(null);

  useEffect(() => {
    (async (id: any) => {
      const data: AllMovieInfoType = await getMovie(id);
      setMovieInfo({
        ...data,
        movie: {
          ...data.movie,
          releaseDate: new Date(data.movie.releaseDate),
        },
      });
      setIsLoaded(true);
    })(params.movieId);
  }, [params.movieId]);

  return (
    <Container>
    <>
      {isLoaded && movieInfo != null && (
        <div className="details_container">
          <img
            src={movieInfo.movie.coverUrl}
            className="details_cover"
            alt={"Cover for " + movieInfo.movie.name}
          />

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
            {movieInfo.castMembers.length > 0 && (
              <div className="info_container">
                {movieInfo.castMembers.map((person) => (
                  <CastSquare key={person.name} person={person} />
                ))}
              </div>
            )}
            {movieInfo.castMembers.length === 0 && <p className="faded">0 Cast members.</p>}

            <p className="details-releaseDate">Release date: {movieInfo.movie.releaseDate.toISOString()}</p>

            <button className="button">
              <i className="fa-regular fa-heart" /> Favorites{" "}
              {movieInfo.favorites}
            </button>
          </div>
        </div>
      )}
      {!isLoaded && <LoadingCircle />}
    </>
    </Container>
  );
};
export default Details;
