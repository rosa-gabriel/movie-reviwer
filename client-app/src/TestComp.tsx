import { useEffect, useState } from "react";
import { getMoviesAtPage } from "./functions/MoviesData";
import { MoviePageType, MovieType } from "./Type/Types";

const TestComp = () => {
  const [movies, setMovies] = useState<MovieType[]>([]);
  const [isLoading, setIsLoading] = useState<boolean>(false);
  const [error, setError] = useState<string | null>(null);
  const [page, setPage] = useState<number>(1);

  console.log("Fora: " + page);
        console.log(movies);

  const loadNext = () => {
    (async () => {
      setIsLoading(true);
      try {
        console.log("Dentro: " + page);
        const data: MoviePageType = await getMoviesAtPage(page);
        setMovies((prevMovies) => [...prevMovies, ...data.movies]);
        setPage(2);
        setError(null);
      } catch (ex: any) {
        setError(ex);
      } finally {
        setIsLoading(false);
      }
    })();
  };

  useEffect(() => {
    loadNext();
  }, []);

  return (
    <>
      <h1>Teste</h1>
      <p>State: {page}</p>
      <button onClick={loadNext}>Add</button>
    </>
  );
};

export default TestComp;
