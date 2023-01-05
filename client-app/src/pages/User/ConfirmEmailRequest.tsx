import { useContext, useState } from "react";
import { useNavigate } from "react-router";
import Container from "../../components/UI/Container";
import ErrorContainer from "../../components/UI/ErrorContainer";
import { UserContext } from "../../contexts/UserContext";
import { newConfirmRequest } from "../../functions/requests/AccountRequests";

const ConfirmEmailRequest = () => {
  const [error, setError] = useState<string | null>(null);
  const [sent, setSent] = useState<boolean>(false);

  const context = useContext(UserContext);
  const navigate = useNavigate();

  if (!context.isLogedIn || context.userInfo?.confirmed) {
    navigate("/");
  }

  const resendConfirm = async () => {
    try {
      const response = await newConfirmRequest(String(context.userInfo?.token));
      setSent(true);
    } catch (ex: any) {
      setError(ex.message);
    }
  };

  return (
    <Container>
      <>
        <h1 className="title">Please confirm your email</h1>
        {!sent && (
          <p className="centered-message faded">
            If you have not received the email.{" "}
            <a href="#" onClick={resendConfirm}>
              {" "}
              confirm
            </a>
          </p>
        )}
        <ErrorContainer error={error}>
          <>
            {sent && (
              <p className="centered-message faded">New email was sent.</p>
            )}
          </>
        </ErrorContainer>
      </>
    </Container>
  );
};

export default ConfirmEmailRequest;
