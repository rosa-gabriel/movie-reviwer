import { useContext, useEffect, useState } from "react";
import { useParams } from "react-router";
import { Link } from "react-router-dom";
import Container from "../../components/UI/Container";
import LoadingCircle from "../../components/UI/LoadingCircle";
import { UserContext } from "../../contexts/UserContext";
import { requestEmailConfirmation } from "../../functions/requests/AccountRequests";

const ConfirmEmail = () => {
  const [error, setError] = useState<string | null>(null);
  const [isLoading, setIsLoading] = useState<boolean>(false);
  const params = useParams();
  const context = useContext(UserContext);

  useEffect(() => {
    (async () => {
      setIsLoading(true);
      try {
        await requestEmailConfirmation(String(params.token));
        setError(null);
      } catch (ex: any) {
        setError(ex.message);
      } finally {
        setIsLoading(false);
      }
    })();
  }, []);
  return (
    <Container>
      <>
        <h1 className="title">Confirming Email</h1>
        {error && !isLoading && (
          <p className="centered-message error">{error}</p>
        )}
        {isLoading && <LoadingCircle />}

        {!isLoading && !error && !context.isLogedIn && (
          <p className="centered-message">
            Email Confirmed, proceed to{" "}
            <Link className="link" to={"/MovieApp/account/login"}>
              login
            </Link>
          </p>
        )}

        {!isLoading && !error && context.isLogedIn && (
          <p className="centered-message faded">Email Confirmed!</p>
        )}
      </>
    </Container>
  );
};

export default ConfirmEmail;
