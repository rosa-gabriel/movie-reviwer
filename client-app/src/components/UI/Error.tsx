import { Link } from "react-router-dom";
import Container from "./Container";

const Details = () => {
  return (
    <Container>
      <>
        <h1 className="title">Url not found! Return to the main menu.</h1>
        <Link className="error centered-message" to={"/MovieApp"}>
          Home
        </Link>
      </>
    </Container>
  );
};
export default Details;
