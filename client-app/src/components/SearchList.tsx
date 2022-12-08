import GenericMovielist from "./UI/GenericMovieList";
import {
  getMoviesFromSearchAtPage,
  getMoviesFromTagAtPage,
  getTag,
} from "../functions/MoviesData";
import { TagType } from "../Type/Types";
import { useNavigate, useParams } from "react-router";
import { useEffect, useState } from "react";
import LoadingCircle from "./UI/LoadingCircle";
import Container from "./UI/Container";

const SearchList = () => {
  const params = useParams();

  return (
    <Container>
      <>
        <>
          <h1 className="title">{`Search - ${params.search}`}</h1>
          <GenericMovielist
            fetchFunction={getMoviesFromSearchAtPage}
            filterId={params.search}
          ></GenericMovielist>
        </>
      </>
    </Container>
  );
};

export default SearchList;
