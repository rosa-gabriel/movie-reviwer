import { useContext, useState } from "react";
import { useNavigate } from "react-router";
import { addPerson } from "../../functions/MovieRequests";
import { NotificationContext } from "../../Context/NotificationContext";
import { UserContext } from "../../Context/UserContext";
import SubmitButton from "../../components/UI/SubmitButton";

const AddPersonForm = () => {
  //States
  const [error, setError] = useState<string | null>(null);
  const [isLoading, setIsLoading] = useState<boolean>(false);
  const [name, setName] = useState<string>("");
  const [url, setUrl] = useState<string>("");

  //Contexts
  const notification = useContext(NotificationContext);

  //Hooks
  const navigate = useNavigate();
  const context = useContext(UserContext);

  //Input Handlers
  const nameChangeHandler = (event: any) => {
    setName(event.target.value);
  };
  const urlChangeHandler = (event: any) => {
    setUrl(event.target.value);
  };

  //Submit Handler
  const submitHandler = async (event: any) => {
    event.preventDefault();
    if (!context.userInfo) return;
    setIsLoading(true);
    try {
      await addPerson(
        {
          name: name,
          profileImageUrl: url,
        },
        context.userInfo.token
      );
      setError(null);
      notification.addNotification({
        code: "ADDED",
        text: "Person was successfully added.",
        error: false,
      });
      navigate("/add/movie");
    } catch (ex: any) {
      setError(ex.message);
    } finally {
      setIsLoading(false);
    }
  };

  return (
    <form onSubmit={submitHandler}>
      <h1 className="title">Add new Person</h1>

      {error !== null && <p className="error centered-message">{error}</p>}

      <div className="form-pair">
        <label>NAME</label>
        <input
          onChange={nameChangeHandler}
          className="input-dark input-add"
          type={"text"}
        />
      </div>

      <div className="form-pair">
        <label>IMAGE URL</label>
        <input
          onChange={urlChangeHandler}
          className="input-dark input-add"
          type={"text"}
        />
      </div>

      <SubmitButton loading={isLoading} buttonText={"Add"}></SubmitButton>
    </form>
  );
};

export default AddPersonForm;
