import React, { useState } from "react";
import { addTag } from "../../functions/MoviesData";
import LoadingCircle from "../UI/LoadingCircle";

const AddTagForm = () => {
  const [error, setError] = useState<string | null>(null);
  const [isLoading, setIsLoading] = useState<boolean>(false);
  const [tagName, setTagName] = useState<string>("");

  const tagNameChangeHandler = (event: any) => {
    setTagName(event.target.value);
  };

  const submitHandler = async (event : any) => {
    event.preventDefault();

    setIsLoading(true);
    try {
      await addTag(tagName);
      setError(null);
    } catch (ex: any) {
      setError(ex.message);
    } finally {
      setIsLoading(false);
    }
  };

  return (
    <div>
      <h1 className="title">Add new Tag</h1>

      {error !== null && <p className="error-message">{error}</p>}

      <div className="form-pair">
        <label>NAME</label>
        <input
          onChange={tagNameChangeHandler}
          className="input-dark input-add"
          type={"text"}
        />
      </div>

      {!isLoading && (
        <button
          onClick={submitHandler}
          type="button"
          className="add-button button"
        >
          Add
        </button>
      )}
      {isLoading && (
        <button style={{ display: "flex" }} className="add-button" disabled>
          <LoadingCircle />
        </button>
      )}
    </div>
  );
};

export default AddTagForm;
