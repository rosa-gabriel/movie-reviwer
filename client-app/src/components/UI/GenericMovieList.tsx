import Movie from "./Movie";
import LoadingCircle from "./LoadingCircle";
import { useEffect, useState } from "react";
import { MoviePageType, MovieType } from "../../Type/Types";

type GenericMovielistProps = {
  filterId?: number | string;
  fetchFunction: Function;
};

const GenericMovielist = (props: GenericMovielistProps) => {
  const [movies, setMovies] = useState<MovieType[]>([]);
  const [isLoading, setIsLoading] = useState<boolean>(false);
  const [error, setError] = useState<string | null>(null);
  const [nextPage, setNextPage] = useState<number>(1);
  const [nextPageLoading, setNextPageLoading] = useState<boolean>(true);
  const [maxPage, setMaxPage] = useState<number>(1);

  const isAtEnd = nextPage > maxPage;

  const loadNext = () => {
    (async () => {
      setIsLoading(true);
      try {
        const data: MoviePageType = await props.fetchFunction(nextPage, props.filterId);
        console.log(data);
        setMovies((prevMovies) => [...prevMovies, ...data.movies]);
        setMaxPage(data.count);
        setNextPage((prevPage) => prevPage + 1);
        setError(null);
      } catch (ex: any) {
        setError(ex);
      } finally {
        setIsLoading(false);
        setNextPageLoading(false);
      }
    })();
  };

  const scrollHandler = (e: any) => {
    if (
      e.target.documentElement.scrollTop + window.innerHeight ==
        e.target.documentElement.scrollHeight &&
      !nextPageLoading
    ) {
      setNextPageLoading(true);
    }
  };

  useEffect(() => {
    window.addEventListener("scroll", scrollHandler);
    if (isAtEnd) {
      setNextPageLoading(false);
      return;
    }
    if (nextPageLoading) {
      loadNext();
    }
  }, [nextPageLoading]);

  return (
    <>
      {!error && movies.length > 0 && (
        <div className="grid-container">
          {movies.map((movie: any) => {
            return <Movie movie={movie} key={movie.id} />;
          })}
        </div>
      )}

      {!error && movies.length > 0 && isAtEnd && (
        <p className="faded centered-message">All Movies Loaded.</p>
      )}

      {!isLoading && !error && movies.length === 0 && (
        <p className="faded centered-message">No movies registered.</p>
      )}

      {!isLoading && error && <p className="error centered-message">{error}</p>}
      {(isLoading || nextPageLoading) && !error && <LoadingCircle />}
    </>
  );
};
export default GenericMovielist;
