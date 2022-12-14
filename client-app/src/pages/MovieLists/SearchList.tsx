import GenericMovielist from "../../components/UI/GenericMovieList";
import { getMoviesFromSearchAtPage } from "../../functions/MovieRequests";
import { useParams } from "react-router";
import Container from "../../components/UI/Container";

const SearchList = () => {
  //Hooks
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
