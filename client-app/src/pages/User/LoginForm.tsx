import { useState, useContext } from "react";
import { useNavigate } from "react-router";
import { login } from "../../functions/requests/AccountRequests";
import { UserLogin } from "../../types/Types";
import { UserContext } from "../../contexts/UserContext";
import SubmitButton from "../../components/UI/SubmitButton";
import Container from "../../components/UI/Container";
import { NotificationContext } from "../../contexts/NotificationContext";
import { Link } from "react-router-dom";

const LoginForm = () => {
  //States
  const [error, setError] = useState<string | null>(null);
  const [isLoading, setIsLoading] = useState<boolean>(false);
  const [username, setUsername] = useState<string>("");
  const [password, setPassword] = useState<string>("");

  //Contexts
  const context = useContext(UserContext);
  const notification = useContext(NotificationContext);

  //Hooks
  const navigate = useNavigate();

  //Input Handlers
  const usernameChangeHandler = (event: any) => {
    setUsername(event.target.value);
  };
  const passwordChangeHandler = (event: any) => {
    setPassword(event.target.value);
  };

  //Submir Handler
  const submitHandler = async (event: any) => {
    event.preventDefault();

    if (username.trim() === "") {
      setError("Username or email field is required.");
      return;
    }

    if (password.trim() === "") {
      setError("Password field is required.");
      return;
    }

    try {
      setIsLoading(true);
      const userInfo: UserLogin = {
        login: username,
        password: password,
      };
      const response = await login(userInfo);
      context.logIn(response);
      setIsLoading(false);
      setError(null);
      notification.addNotification({
        code: "LOGGED",
        text: "You are now successfully logged in!",
        error: false,
      });
      navigate("/");

    } catch (ex: any) {
      setError(ex.message);
      setIsLoading(false);
    }
  };

  return (
    <Container>
      <form onSubmit={submitHandler}>
        <h1 className="title">Login</h1>
        {error && <p className="error centered-message">{error}</p>}
        <div className="form-pair">
          <label>Username or email</label>
          <input
            type="text"
            className="input-add input-dark"
            onChange={usernameChangeHandler}
            placeholder="Username"
          />
        </div>

        <div className="form-pair">
          <label>Password</label>
          <input
            type="password"
            className="input-add input-dark"
            onChange={passwordChangeHandler}
            placeholder="Password"
          />
        </div>
        <SubmitButton loading={isLoading} buttonText={"Login"}></SubmitButton>
        <p className="account-form-option">
          Doesn't have an account?{" "}
          <Link className="link" to={"/Account/register"}>
            sign up
          </Link>
          .
        </p>
      </form>
    </Container>
  );
};

export default LoginForm;
