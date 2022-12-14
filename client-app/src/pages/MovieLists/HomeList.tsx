import { getMoviesAtPage } from "../../functions/MovieRequests";
import Container from "../../components/UI/Container";
import GenericMovielist from "../../components/UI/GenericMovieList";

const HomeList = () => {
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
