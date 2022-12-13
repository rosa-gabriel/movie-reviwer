import { useContext, useEffect } from "react";
import { getMoviesAtPage } from "../functions/MoviesData";
import Container from "./UI/Container";
import GenericMovielist from "./UI/GenericMovieList";
import { NotificationContext } from "./Context/NotificationContext";

const HomeList = () => {
  const notification = useContext(NotificationContext);

  return (
    <>
      <Container>
        <>
          <h1 className="title">Latest Movies</h1>
          <GenericMovielist fetchFunction={getMoviesAtPage} />
        </>
      </Container>
    </>
  );
};
export default HomeList;
