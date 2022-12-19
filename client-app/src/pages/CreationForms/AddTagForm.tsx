import { useContext, useState } from "react";
import { addTag } from "../../functions/requests/MovieRequests";
import { useNavigate } from "react-router";
import SubmitButton from "../../components/UI/SubmitButton";
import { UserContext } from "../../contexts/UserContext";
import { NotificationContext } from "../../contexts/NotificationContext";

const AddTagForm = () => {
  //States
  const [error, setError] = useState<string | null>(null);
  const [isLoading, setIsLoading] = useState<boolean>(false);
  const [tagName, setTagName] = useState<string>("");

  //Contexts
  const notification = useContext(NotificationContext);

  //Hooks
  const navigate = useNavigate();
  const context = useContext(UserContext);

  //Input Handlers
  const tagNameChangeHandler = (event: any) => {
    setTagName(event.target.value);
  };

  //Submit Handler
  const submitHandler = async (event: any) => {
    event.preventDefault();
    console.log("submit");

    setIsLoading(true);
    try {
      await addTag(tagName, String(context.userInfo?.token));
      setError(null);
      notification.addNotification({
        code: "ADDED",
        text: "Tag was successfully added.",
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
      <h1 className="title">Add new Tag</h1>

      {error !== null && <p className="error centered-message">{error}</p>}

      <div className="form-pair">
        <label>NAME</label>
        <input
          onChange={tagNameChangeHandler}
          className="input-dark input-add"
          type={"text"}
        />
      </div>

      <SubmitButton loading={isLoading} buttonText={"Add"}></SubmitButton>
    </form>
  );
};

export default AddTagForm;
