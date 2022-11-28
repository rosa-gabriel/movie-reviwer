import React, { useContext, useState } from "react";
import { useNavigate } from "react-router";
import { addPerson, addTag } from "../../functions/MoviesData";
import { CastType, PersonType } from "../../Type/Types";
import { UserContext } from "../Context/UserContext";
import SubmitButton from "../SubmitButton";
import LoadingCircle from "../UI/LoadingCircle";

const AddPersonForm = () => {
  const [error, setError] = useState<string | null>(null);
  const [isLoading, setIsLoading] = useState<boolean>(false);
  const [name, setName] = useState<string>("");
  const [url, setUrl] = useState<string>("");
  const navigate = useNavigate();
  const context = useContext(UserContext);

  const nameChangeHandler = (event: any) => {
    setName(event.target.value);
  };
  const urlChangeHandler = (event: any) => {
    setUrl(event.target.value);
  };

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
