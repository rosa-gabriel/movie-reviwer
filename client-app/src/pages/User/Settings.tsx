import { useContext, useEffect, useState } from "react";
import { useNavigate } from "react-router";
import AccountCheck from "../../components/account/AccountCheck";
import Container from "../../components/UI/Container";
import ErrorContainer from "../../components/UI/ErrorContainer";
import FormPair from "../../components/UI/FormPair";
import LoadingCircle from "../../components/UI/LoadingCircle";
import SendButton from "../../components/UI/SendButton";
import { NotificationContext } from "../../contexts/NotificationContext";
import { UserContext } from "../../contexts/UserContext";
import {
  changeSettings,
  getSettings,
} from "../../functions/requests/AccountRequests";
import {} from "../../functions/requests/MovieRequests";
import { UserSettings } from "../../types/Types";

const Settings = () => {
  const [username, setUsername] = useState<string>("");
  const [email, setEmail] = useState<string>("");
  const [url, setUrl] = useState<string>("");
  const [oldPassword, setOldPassword] = useState<string>("");
  const [newPassword, setNewPassword] = useState<string>("");
  const [confirmPassword, setConfirmPassword] = useState<string>("");
  const [bio, setBio] = useState<string>("");
  const [personError, setPersonError] = useState<string | null>(null);
  const [error, setError] = useState<string | null>(null);
  const [isLoading, setIsLoading] = useState<boolean>(false);
  const [hasChanged, setHasChanged] = useState<boolean>(false);

  const [confirmIsLoading, setConfirmIsLoading] = useState<boolean>(false);

  const navigate = useNavigate();

  const context = useContext(UserContext);
  const notification = useContext(NotificationContext);

  useEffect(() => {
    (async () => {
      setIsLoading(true);
      try {
        const settings = await getSettings(String(context.userInfo?.token));

        setUsername(settings.username);
        setEmail(settings.email);
        setUrl(settings.profileImageUrl);
        setBio(settings.bio == null ? "" : settings.bio);

        setPersonError(null);
      } catch (ex: any) {
        setPersonError(ex.message);
      } finally {
        setIsLoading(false);
      }
    })();
  }, [context.userInfo?.id, context.userInfo?.token]);

  const submitHandler = async (e: any) => {
    e.preventDefault();
    try {
      if (hasChanged) {
        if (username === "") {
          setError("Username cannot be empty!");
          return;
        }
        if (email === "") {
          setError("Email cannot be empty!");
          return;
        }
        if (newPassword !== confirmPassword) {
          setError("The new password needs to match!");
          return;
        }

        setConfirmIsLoading(true);
        const newSettings: UserSettings = {
          username: username,
          email: email,
          profileImageUrl: url,
          bio: bio,
          newPassword: newPassword,
          oldPassword: oldPassword,
        };

        const response = await changeSettings(
          newSettings,
          String(context.userInfo?.token)
        );
        context.logIn(response);
      }
      setError(null);
      notification.addNotification({
        code: "SAVED",
        text: "User Settings have changed",
        error: false,
      });
      navigate("/MovieApp");
    } catch (ex: any) {
      setError(ex.message);
    } finally {
      setConfirmIsLoading(false);
    }
  };

  const cancelHandler = () => {
    navigate(`/MovieApp/account/profile/${context.userInfo?.id}`);
  };

  const usernameChangeHandler = (e: any) => {
    setUsername(e.target.value);
    setHasChanged(true);
  };

  const urlChangeHandler = (e: any) => {
    setUrl(e.target.value);
    setHasChanged(true);
  };

  const oldPasswordChangeHandler = (e: any) => {
    setOldPassword(e.target.value);
    setHasChanged(true);
  };

  const newPasswordChangeHandler = (e: any) => {
    setNewPassword(e.target.value);
    setHasChanged(true);
  };

  const confirmChangeHandler = (e: any) => {
    setConfirmPassword(e.target.value);
    setHasChanged(true);
  };

  const emailChangeHandler = (e: any) => {
    setEmail(e.target.value);
    setHasChanged(true);
  };

  const bioChangeHandler = (e: any) => {
    setBio(e.target.value);
    setHasChanged(true);
  };

  return (
    <Container>
      <>
        <AccountCheck />
        <ErrorContainer error={personError}>
          <>
            {!isLoading && (
              <>
                <form>
                  <h1 className="title">Settings</h1>
                  <p className="error centered-message">{error}</p>
                  <FormPair
                    title={"Username"}
                    value={username}
                    type={"text"}
                    onChange={usernameChangeHandler}
                  ></FormPair>

                  <FormPair
                    title={"Email"}
                    value={email}
                    type={"text"}
                    onChange={emailChangeHandler}
                  ></FormPair>

                  <FormPair
                    title={"Url"}
                    value={url}
                    type={"text"}
                    onChange={urlChangeHandler}
                  ></FormPair>

                  <FormPair
                    title={"Bio"}
                    value={bio}
                    type={"text"}
                    onChange={bioChangeHandler}
                  ></FormPair>

                  <FormPair
                    title={"Old password"}
                    value={oldPassword}
                    type={"password"}
                    onChange={oldPasswordChangeHandler}
                  ></FormPair>

                  <FormPair
                    title={"New password"}
                    value={newPassword}
                    onChange={newPasswordChangeHandler}
                    type={"password"}
                  ></FormPair>

                  <FormPair
                    title={"Confirm new password"}
                    value={confirmPassword}
                    type={"password"}
                    onChange={confirmChangeHandler}
                  ></FormPair>
                </form>

                <div className="edit-buttons-container">
                  <button
                    type="button"
                    onClick={cancelHandler}
                    className="button"
                  >
                    Cancel
                  </button>
                  <SendButton
                    className={""}
                    text={"Save"}
                    isLoading={confirmIsLoading}
                    type={"submit"}
                    onClick={submitHandler}
                  />
                </div>
              </>
            )}
            {isLoading && <LoadingCircle />}
          </>
        </ErrorContainer>
      </>
    </Container>
  );
};

export default Settings;
