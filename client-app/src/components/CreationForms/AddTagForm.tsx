import React, { useContext, useState } from "react";
import { addTag } from "../../functions/MoviesData";
import LoadingCircle from "../UI/LoadingCircle";
import { useNavigate } from "react-router";
import SubmitButton from "../SubmitButton";
import { UserContext } from "../Context/UserContext";
import AccountCheck from "../AccountForms/AccountCheck";

const AddTagForm = () => {
  const [error, setError] = useState<string | null>(null);
  const [isLoading, setIsLoading] = useState<boolean>(false);
  const [tagName, setTagName] = useState<string>("");
  const navigate = useNavigate();
  const context = useContext(UserContext);

  const tagNameChangeHandler = (event: any) => {
    setTagName(event.target.value);
  };

  const submitHandler = async (event: any) => {
    event.preventDefault();

    if (!context.userInfo) return;
    setIsLoading(true);
    try {
      await addTag(tagName, context.userInfo.token);
      setError(null);
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
