import GenericMovielist from "./UI/GenericMovieList";
import {
  getMoviesFromPerson,
  getMoviesFromTag,
  getPerson,
  getTag,
} from "../functions/MoviesData";
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
        const MovieData: MovieType[] = await getMoviesFromPerson(
          String(data.id)
        );
        setMovies(MovieData);
      } catch (ex: any) {
        setHasError(ex.message);
        navigate("/error");
      }
    })();
  }, []);

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
                  alt={"Cover image for " + person.name}
                />

                <div className="details_info">
                  <h1>{person.name}</h1>
                </div>
              </div>
            </div>

            <h1 className="title">Movie Apearences</h1>

            <div style={{ marginTop: "50px" }}>
              {!isLoading && !hasError && movies.length > 0 && (
                <div className="grid-container">
                  {movies.map((movie: any) => {
                    return <Movie movie={movie} key={movie.id} />;
                  })}
                </div>
              )}

              {!isLoading && !hasError && movies.length === 0 && (
                <p className="faded centered-message">
                  None movies apearences.
                </p>
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
