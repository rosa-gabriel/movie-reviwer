import { useContext, useState } from "react";
import { useNavigate } from "react-router";
import { login, register } from "../../functions/TokenData";
import { UserRegisterType } from "../../Type/Types";
import { UserContext } from "../Context/UserContext";
import SubmitButton from "../SubmitButton";
import Container from "../UI/Container";

const RegisterForm = () => {
  const context = useContext(UserContext);
  const navigate = useNavigate();
  const [error, setError] = useState<string | null>(null);
  const [isLoading, setIsLoading] = useState<boolean>(false);
  const [username, setUsername] = useState<string>("");
  const [email, setEmail] = useState<string>("");
  const [password, setPassword] = useState<string>("");

  const usernameChangeHandler = (event: any) => {
    setUsername(event.target.value);
  };

  const passwordChangeHandler = (event: any) => {
    setPassword(event.target.value);
  };

  const emailChangeHandler = (event: any) => {
    setEmail(event.target.value);
  };

  const submitHandler = async (event: any) => {
    event.preventDefault();

    try {
      setIsLoading(true);
      const userInfo: UserRegisterType = {
        email: email,
        username: username,
        password: password,
      };

      const response = await register(userInfo);
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
        <h1 className="title">Register </h1>
        {error && <p className="error centered-message">{error}</p>}

        <div className="form-pair">
          <label>Username</label>
          <input
            type="text"
            className="input-add input-dark"
            onChange={usernameChangeHandler}
            placeholder="Username"
          />
        </div>

        <div className="form-pair">
          <label>Email</label>
          <input
            type="text"
            className="input-add input-dark"
            onChange={emailChangeHandler}
            placeholder="Email"
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

        <SubmitButton loading={isLoading} buttonText={"Register"} />
      </form>
    </Container>
  );
};

export default RegisterForm;
