import GenericMovielist from "./UI/GenericMovieList";
import { getMoviesFromTag, getPerson, getTag } from "../functions/MoviesData";
import { CastType, MovieType, PersonType, TagType } from "../Type/Types";
import { useNavigate, useParams } from "react-router";
import { useEffect, useState } from "react";
import LoadingCircle from "./UI/LoadingCircle";
import Container from "./UI/Container";
import Movie from "./UI/Movie";

const TagList = () => {
  const params = useParams();
  const navigate = useNavigate();
  const [person, setPerson] = useState<PersonType | null>(null);
  const [hasError, setHasError] = useState<string | null>(null);
  const [movies, setMovies] = useState<MovieType[]>([]);
  const [isLoading, setIsLoading] = useState<boolean>(false);

  useEffect(() => {
    (async () => {
      if (params.id == undefined) return;
      try {
        const data: PersonType = await getPerson(String(params.id));
        setPerson(data);
        setHasError(null);
      } catch (ex: any) {
        setHasError(ex.message);
        navigate("/error");
      }
    })();
  }, []);

  console.log(person);

  return (
    <Container>
      <>
        {person !== null && (
          <>
            <div className="person-info">
              <div className="details_container">
                <img
                  src={person.profileImageUrl}
                  className="details_cover"
                  alt={"Cover for " + person.name}
                />

                <div className="details_info">
                  <h1>{person.name}</h1>
                </div>
              </div>
            </div>
            <div style={{ marginTop: "50px" }}>
              {!isLoading && !hasError && movies.length > 0 && (
                <div className="grid-container">
                  {movies.map((movie: any) => {
                    return <Movie movie={movie} key={movie.id} />;
                  })}
                </div>
              )}

              {!isLoading && !hasError && movies.length === 0 && (
                <p>0 movies registered.</p>
              )}

              {!isLoading && hasError && (
                <p className="error-message">{hasError}</p>
              )}
              {isLoading && !hasError && <LoadingCircle />}
            </div>
          </>
        )}
        {!person === null && <LoadingCircle />}
      </>
    </Container>
  );
};

export default TagList;
