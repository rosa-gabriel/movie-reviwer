import { useParams } from "react-router-dom";
import { useEffect, useState } from "react";
import Tag from "./UI/Tag";
import { getMovie } from "../functions/MoviesData";
import CastSquare from "./UI/CastSquare";
import LoadingCircle from "./UI/LoadingCircle";

const Details = (props: any) => {
  const params = useParams();
  const [isLoaded, setIsLoaded] = useState(false);
  const [movieInfo, setMovieInfo] = useState({
    movie: { id: -1, name: "", coverUrl: "", releaseDate: new Date() },
    castMembers: [{ id: -1, name: "", role: "" }],
    favorites: -1,
    tags: [{ name: "", entries: -1 }],
  });

  useEffect(() => {
    (async (id: any) => {
      const data = await getMovie(id);
      console.log(data);
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
    <>
      {isLoaded && (
        <div className="details_container">

          <img
            src={movieInfo.movie.coverUrl}
            className="details_cover"
            alt={"Cover for " + movieInfo.movie.name}
          />

          <div className="details_info">
            <h1>{movieInfo.movie.name}</h1>

            <p className="info_container_title">TAGS</p>
            <div className="info_container">
              {movieInfo.tags.map((tag) => (
                <Tag key={tag.name} TagItem={tag} />
              ))}
            </div>

            <p className="info_container_title">CAST</p>
            <div className="info_container">
              {movieInfo.castMembers.map((person) => (
                <CastSquare key={person.name} person={person.name} />
              ))}
            </div>

            <p>Release date: {movieInfo.movie.releaseDate.toISOString()}</p>

            <button className="button">
              <i className="fa-regular fa-heart" /> Favorites{" "}
              {movieInfo.favorites}
            </button>

          </div>
        </div>
      )}
      {!isLoaded && <LoadingCircle />}
    </>
  );
};
export default Details;
