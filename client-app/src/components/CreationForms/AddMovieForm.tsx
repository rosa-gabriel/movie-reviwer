import { useState } from "react";
import { useNavigate } from "react-router-dom";
import { addMovie } from "../../functions/MoviesData";
import LoadingCircle from "../UI/LoadingCircle";
import { nMovie } from "../../Type/Types";
import TagInput from "../UI/TagInput";

const AddMovieForm = (props: any) => {
  const [error, setError] = useState("");
  const [loading, setLoading] = useState(false);
  const [name, setName] = useState("");
  const [url, setUrl] = useState("");
  const [date, setDate] = useState(new Date());
  const navigate = useNavigate();

  const nameChangeHandler = (event: any) => {
    setName(event.target.value);
  };

  const urlChangeHandler = (event: any) => {
    setUrl(event.target.value);
  };

  const dateChangeHandler = (event: any) => {
    setDate(event.target.value);
  };

  const tagsChangeHandler = (event: any) => {
    setDate(event.target.value);
  };

  const submitHandler = async (event: any) => {
    console.log('sus');
    const newMovie: nMovie = {
      name: name,
      coverUrl: url,
      ReleaseDate: date,
    };

    try {
      setLoading(true);
      await addMovie(newMovie);
      setError("");
      navigate("/");
    } catch (ex: any) {
      setLoading(false);
      setError(ex.message);
    } finally {
    }
  };

  return (
    <div>
      <h1 className="title">Add new Movie</h1>

      {error.trim().length > 0 && <p className="error-message">{error}</p>}

      <div className="form-pair">
        <label>NAME</label>
        <input
          onChange={nameChangeHandler}
          className="input-dark input-add"
          type={"text"}
        />
      </div>

      <div className="form-pair">
        <label>COVER URL</label>
        <input
          onChange={urlChangeHandler}
          className="input-dark input-add"
          type={"text"}
        />
      </div>

      <div className="form-pair">
        <label>RELEASE DATE</label>
        <input
          onChange={dateChangeHandler}
          className="input-dark input-add"
          type={"date"}
        />
      </div>

      <div className="form-pair">
        <label>TAGS</label>
        <TagInput />
      </div>

      {!loading && <button onClick={submitHandler} type="button" className="add-button button">Add</button>}
      {loading && (
        <button
          style={{ display: "flex" }}
          className="add-button"
          disabled
        >
          <LoadingCircle />
        </button>
      )}
    </div>
  );
};

export default AddMovieForm;
