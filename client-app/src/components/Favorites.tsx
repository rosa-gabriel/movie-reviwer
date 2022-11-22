import { useParams } from "react-router-dom";
import Container from "./UI/Container";

const Details = () => {
  const params = useParams();

  return (
    <Container>
      <h1>Favorites</h1>
    </Container>
  );
};
export default Details;
