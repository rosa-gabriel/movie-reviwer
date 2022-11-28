import { useParams } from "react-router-dom";
import AccountCheck from "./AccountForms/AccountCheck";
import Container from "./UI/Container";

const Details = () => {
  const params = useParams();

  return (
    <Container>
      <>
        <AccountCheck />
        <h1 className="title">Favorites</h1>
      </>
    </Container>
  );
};
export default Details;
