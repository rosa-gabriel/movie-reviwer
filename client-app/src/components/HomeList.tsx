import Movie from "./UI/Movie";
import LoadingCircle from "./UI/LoadingCircle";
import React, { useEffect, useState } from "react";
import { getMoviesAtPage } from "../functions/MoviesData";
import { MoviePageType, MovieType } from "../Type/Types";
import Container from "./UI/Container";

const HomeList = () => {
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
        const data: MoviePageType = await getMoviesAtPage(nextPage);
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
      <Container>
        <>
          <h1 className="title">Latest Movies</h1>
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

          {!isLoading && error && (
            <p className="error centered-message">{error}</p>
          )}
          {(isLoading || nextPageLoading) && !error && <LoadingCircle />}
        </>
      </Container>
    </>
  );
};
export default HomeList;
