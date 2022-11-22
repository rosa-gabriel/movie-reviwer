import { Link } from "react-router-dom";
import Container from "./UI/Container";

const Details = () => {
  return (
    <Container>
      <div style={{ textAlign: "center" }}>
        <h1 style={{ marginBottom: "20px" }}>
          Url not found! Return to the main menu.
        </h1>
        <Link to={"/"}>Home</Link>
      </div>
    </Container>
  );
};
export default Details;
