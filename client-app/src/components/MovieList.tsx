import Movie from "./UI/Movie";
import LoadingCircle from "./UI/LoadingCircle";
import { useEffect, useState } from "react";
import { getMovies } from "../functions/MoviesData";

const MovieList = (props: any): JSX.Element => {
  const [movies, setMovies] = useState([]);
  const [isLoading, setIsLoading] = useState(false);
  const [error, setError] = useState('');
  const [seed, setSeed] = useState(1);

  const reset = () => {
    setSeed(Math.random());
  };

  const hasError = error.trim().length > 0;

  useEffect(() => {
    (async () => {
      setIsLoading(true);
      try {
        const data = await getMovies();
        setMovies(data);
        setError('');
      } catch (ex: any) {
        setError(ex);
      } finally {
        setIsLoading(false);
      }
    })();
  }, [seed]);

  return (
    <>
      <h1 className="title">Latest Movies</h1>
      {(!isLoading && !hasError) && (
        <div className="grid-container">
          {movies.map((item: any) => {
            return <Movie item={item} key={item.id} />;
          })}
        </div>
      )}
      {(!isLoading && hasError) && <p className="error-message">{error}</p>}
      {(isLoading && !hasError) && <LoadingCircle />}
    </>
  );
};
export default MovieList;
