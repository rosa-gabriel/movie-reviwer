import { useState, useContext } from "react";
import { useNavigate } from "react-router";
import { login } from "../../functions/TokenData";
import { UserLoginType } from "../../Type/Types";
import { UserContext } from "../Context/UserContext";
import SubmitButton from "../SubmitButton";
import Container from "../UI/Container";

const LoginForm = () => {
  const context = useContext(UserContext);
  const navigate = useNavigate();
  const [error, setError] = useState<string | null>(null);
  const [isLoading, setIsLoading] = useState<boolean>(false);
  const [username, setUsername] = useState<string>("");
  const [password, setPassword] = useState<string>("");

  const usernameChangeHandler = (event: any) => {
    setUsername(event.target.value);
  };

  const passwordChangeHandler = (event: any) => {
    setPassword(event.target.value);
  };

  const submitHandler = async (event: any) => {
    event.preventDefault();

    try {
      setIsLoading(true);
      const userInfo: UserLoginType = {
        email: username,
        password: password,
      };
      const response = await login(userInfo);
      context.logIn(response);
      setIsLoading(false);
      setError(null);
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
      </form>
    </Container>
  );
};

export default LoginForm;
