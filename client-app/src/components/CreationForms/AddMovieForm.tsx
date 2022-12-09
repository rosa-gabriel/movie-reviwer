import { useContext, useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";
import { addMovie, getCast, getTags } from "../../functions/MoviesData";
import LoadingCircle from "../UI/LoadingCircle";
import {
  AllMovieInfoType,
  CastEntryType,
  CastType,
  TagEntriesType,
} from "../../Type/Types";
import ItemInput from "./inputs/ItemInput";
import SubmitButton from "../SubmitButton";
import { UserContext } from "../Context/UserContext";

const AddMovieForm = () => {
  const [error, setError] = useState<string | null>(null);
  const [loading, setLoading] = useState<boolean>(false);
  const [name, setName] = useState<string>("");
  const [url, setUrl] = useState<string>("");
  const [role, setRole] = useState<string>("");
  const [date, setDate] = useState<Date>(new Date());

  const [dataTags, setDataTags] = useState<TagEntriesType[]>([]);
  const [tags, setTags] = useState<TagEntriesType[]>([]);

  const [dataCast, setDataCast] = useState<CastType[]>([]);
  const [cast, setCast] = useState<CastEntryType[]>([]);

  const navigate = useNavigate();
  const context = useContext(UserContext);

  useEffect(() => {
    (async () => {
      try {
        const CastResponse = await getCast();
        const tagsResponse = await getTags();
        setDataTags([...tagsResponse]);
        setDataCast([...CastResponse]);
      } catch (ex: any) {
        setError(ex.message);
      }
    })();
  }, []);

  const nameChangeHandler = (event: any) => {
    setName(event.target.value);
  };

  const urlChangeHandler = (event: any) => {
    setUrl(event.target.value);
  };

  const roleChangeHandler = (event: any) => {
    setRole(event.target.value);
  };

  const dateChangeHandler = (event: any) => {
    setDate(event.target.value);
  };

  const tagsChangeHandler = (
    inputTags: TagEntriesType[],
    inputDataTags: TagEntriesType[]
  ) => {
    setDataTags([...inputDataTags]);
    setTags([...inputTags]);
  };

  const castChangeHandler = (
    inputCast: CastEntryType[],
    inputDataCast: CastType[]
  ) => {
    setDataCast([...inputDataCast]);
    setCast([...inputCast]);
    setRole("");
  };

  const submitHandler = async (event: any) => {
    event.preventDefault();
    const newMovie: AllMovieInfoType = {
      movie: {
        name: name,
        coverUrl: url,
        releaseDate: date,
      },
      favorites: 0,
      tags: tags,
      castMembers: cast,
    };
    console.log(cast);
    if (!context.userInfo) return;
    try {
      setLoading(true);
      await addMovie(newMovie, context.userInfo.token);
      setError(null);
      navigate("/");
    } catch (ex: any) {
      setLoading(false);
      setError(ex.message);
    } finally {
    }
  };

  return (
    <form onSubmit={submitHandler}>
      <h1 className="title">Add new Movie</h1>

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
        <ItemInput
          onChange={tagsChangeHandler}
          dataItems={dataTags}
          items={tags}
          placeHolder={"Choose a person"}
          idProp={"tagId"}
        />
      </div>

      <div className="form-pair">
        <label>CAST</label>
        <ItemInput
          onChange={castChangeHandler}
          dataItems={dataCast}
          items={cast}
          role={role}
          placeHolder={"Choose a person"}
          idProp={"id"}
        >
          <input
            type={"text"}
            onChange={roleChangeHandler}
            placeholder={"role"}
            value={role}
          ></input>
        </ItemInput>
      </div>

      <SubmitButton loading={loading} buttonText={"Add"}></SubmitButton>
    </form>
  );
};

export default AddMovieForm;
