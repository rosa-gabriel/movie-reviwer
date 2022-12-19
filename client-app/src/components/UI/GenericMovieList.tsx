import Movie from "./Movie";
import LoadingCircle from "./LoadingCircle";
import { useEffect, useState } from "react";
import { MoviePageType, MovieType } from "../../types/Types";
import ErrorContainer from "./ErrorContainer";

type GenericMovielistProps = {
  filterId?: number | string;
  fetchFunction(...args: any[]): any;
};

const GenericMovielist = (props: GenericMovielistProps) => {
  //States
  const [movies, setMovies] = useState<MovieType[]>([]);
  const [isLoading, setIsLoading] = useState<boolean>(false);
  const [nextPage, setNextPage] = useState<number>(1);
  const [nextPageLoading, setNextPageLoading] = useState<boolean>(true);
  const [maxPage, setMaxPage] = useState<number>(1);
  const [error, setError] = useState<string | null>(null);

  const isAtEnd = nextPage > maxPage;
  //Load next page
  const loadNext = () => {
    (async () => {
      setIsLoading(true);
      try {
        const data: MoviePageType = await props.fetchFunction(
          nextPage,
          props.filterId
        );
        setMovies((prevMovies) => [...prevMovies, ...data.movies]);
        setMaxPage(data.count);
        setNextPage((prevPage) => prevPage + 1);
        setError(null);
      } catch (ex: any) {
        setError(ex.message);
      } finally {
        setIsLoading(false);
        setNextPageLoading(false);
      }
    })();
  };

  //Scroll Handler
  const scrollHandler = (e: any) => {
    if (
      e.target.documentElement.scrollTop + window.innerHeight ===
        e.target.documentElement.scrollHeight &&
      !nextPageLoading
    ) {
      setNextPageLoading(true);
    }
  };

  //Effect
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
    <ErrorContainer error={error}>
      <>
        {movies.length > 0 && (
          <div className="grid-container">
            {movies.map((movie: any) => {
              return <Movie movie={movie} key={movie.id} />;
            })}
          </div>
        )}

        {movies.length > 0 && isAtEnd && (
          <p className="faded centered-message">All Movies Loaded.</p>
        )}

        {!isLoading && movies.length === 0 && (
          <p className="faded centered-message">No movies registered.</p>
        )}

        {(isLoading || nextPageLoading) && !error && <LoadingCircle />}
      </>
    </ErrorContainer>
  );
};
export default GenericMovielist;
