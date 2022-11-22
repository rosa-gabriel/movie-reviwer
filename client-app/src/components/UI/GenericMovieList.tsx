import Movie from "./Movie";
import LoadingCircle from "./LoadingCircle";
import { useEffect, useState } from "react";
import { getMovies } from "../../functions/MoviesData";
import { MovieType } from "../../Type/Types";
import Container from "./Container";

type GenericMovielistProps = {
  title: string;
  filterId: number | string,
  fetchFunction: Function;
  children?: JSX.Element;
};

const GenericMovielist = (props: GenericMovielistProps) => {
  const [movies, setMovies] = useState<MovieType[]>([]);
  const [isLoading, setIsLoading] = useState<boolean>(false);
  const [error, setError] = useState<string | null>(null);

  const hasError = error !== null;

  useEffect(() => {
    (async () => {
      setIsLoading(true);

      try {
        const data: MovieType[] = await props.fetchFunction(props.filterId);
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
    <>
      <h1 className="title">{props.title}</h1>
      <Container>
        <div style={{marginTop: '50px'}}>
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
        </div>
      </Container>
    </>
  );
};
export default GenericMovielist;
