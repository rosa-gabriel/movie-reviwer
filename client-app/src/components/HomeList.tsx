import Movie from "./UI/Movie";
import LoadingCircle from "./UI/LoadingCircle";
import React, { useEffect, useState } from "react";
import { getMoviesAtPage } from "../functions/MoviesData";
import { MoviePageType, MovieType } from "../Type/Types";
import Container from "./UI/Container";
import GenericMovielist from "./UI/GenericMovieList";

const HomeList = () => {
  return (
    <>
      <Container>
        <>
          <h1 className="title">Latest Movies</h1>
          <GenericMovielist fetchFunction={getMoviesAtPage}/>
        </>
      </Container>
    </>
  );
};
export default HomeList;
