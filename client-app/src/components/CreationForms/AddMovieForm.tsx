import { useState } from "react";
import { useNavigate } from "react-router-dom";
import { addMovie } from "../../functions/MoviesData";
import LoadingCircle from "../UI/LoadingCircle";
import { AllMovieInfoType, MovieType, TagType } from "../../Type/Types";
import TagInput from "../UI/TagInput";

const AddMovieForm = () => {
  const [error, setError] = useState<string | null>(null);
  const [loading, setLoading] = useState<boolean>(false);
  const [name, setName] = useState<string>("");
  const [url, setUrl] = useState<string>("");
  const [date, setDate] = useState<Date>(new Date());
  const [tags, setTags] = useState<TagType[]>([]);

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

  const tagsChangeHandler = (tags: TagType[]) => {
    setTags([...tags]);
  };

  const submitHandler = async (event: any) => {
    const newMovie: AllMovieInfoType = {
      movie: {
        name: name,
        coverUrl: url,
        releaseDate: date,
      },
      favorites: 0,
      tags: tags,
      castMembers: []
    };

    try {
      setLoading(true);
      await addMovie(newMovie);
      setError(null);
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

      {error !== null && <p className="error-message">{error}</p>}

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
        <TagInput onChange={tagsChangeHandler}/>
      </div>

      {!loading && (
        <button
          onClick={submitHandler}
          type="button"
          className="add-button button"
        >
          Add
        </button>
      )}
      {loading && (
        <button style={{ display: "flex" }} className="add-button" disabled>
          <LoadingCircle />
        </button>
      )}
    </div>
  );
};

export default AddMovieForm;
