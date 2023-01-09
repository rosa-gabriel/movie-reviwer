import { useContext } from "react";
import { getUserFavorites } from "../../functions/requests/MovieRequests";
import AccountCheck from "../../components/account/AccountCheck";
import { UserContext } from "../../contexts/UserContext";
import ErrorContainer from "../../components/UI/ErrorContainer";
import Container from "../../components/UI/Container";
import GenericMovielist from "../../components/UI/GenericMovieList";
import { useParams } from "react-router";

const Favorites = () => {
  //Contexts
  const context = useContext(UserContext);
  const params = useParams();

  const error = context.isLogedIn ? null : "Not Logged In";

  const isSelf = params.Username === undefined;

  const name = isSelf ? context.userInfo?.username : params.Username;

  return (
    <Container>
      <>
        <AccountCheck />
        <ErrorContainer error={error}>
          <>
            <h1 className="title">
              {(isSelf ? "Your" : name + "'s") + " Favorites"}
            </h1>
            <GenericMovielist
              fetchFunction={getUserFavorites}
              token={String(context.userInfo?.token)}
              filterId={name}
            />
          </>
        </ErrorContainer>
      </>
    </Container>
  );
};
export default Favorites;
