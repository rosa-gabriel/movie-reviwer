import Movie from "./UI/Movie";
import LoadingCircle from "./UI/LoadingCircle";
import { useEffect, useState } from "react";
import { getMovies } from "../functions/MoviesData";
import { MovieType } from "../Type/Types";
import Container from "./UI/Container";

const HomeList = () => {
  const [movies, setMovies] = useState<MovieType[]>([]);
  const [isLoading, setIsLoading] = useState<boolean>(false);
  const [error, setError] = useState<string | null>(null);

  const hasError = error !== null;

  useEffect(() => {
    (async () => {
      setIsLoading(true);
      try {
        const data: MovieType[] = await getMovies();
        setMovies([...data]);
        setError(null);
      } catch (ex: any) {
        setError(ex);
      } finally {
        setIsLoading(false);
      }
    })();
  }, []);

  return (
    <Container>
      <>
        <h1 className="title">Latest Movies</h1>
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

        {!isLoading && hasError && <p className="error-message">{error}</p>}
        {isLoading && !hasError && <LoadingCircle />}
      </>
    </Container>
  );
};
export default HomeList;
