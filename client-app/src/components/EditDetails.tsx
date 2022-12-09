import { Navigate, useNavigate, useParams } from "react-router-dom";
import { useContext, useEffect, useState } from "react";
import Tag from "./UI/details/Tag";
import {
  getCast,
  getIsFavorite,
  getMissingTags,
  getMovie,
  getTags,
  putFavorite,
} from "../functions/MoviesData";
import CastSquare from "./UI/details/CastSquare";
import LoadingCircle from "./UI/LoadingCircle";
import {
  AllMovieInfoType,
  CastType,
  PersonType,
  TagEntriesType,
} from "../Type/Types";
import Container from "./UI/Container";
import { UserContext } from "./Context/UserContext";
import AccountCheck from "./AccountForms/AccountCheck";
import ItemInput from "./CreationForms/inputs/ItemInput";
import { idText } from "typescript";

const EditDetails = (props: any) => {
  const params = useParams();
  const context = useContext(UserContext);
  const navigate = useNavigate();
  const [isLoading, setIsLoading] = useState<boolean>(false);
  const [dataTags, setDataTags] = useState<TagEntriesType[]>([]);
  const [tags, setTags] = useState<TagEntriesType[]>([]);
  const [url, setUrl] = useState<string>("");
  const [name, setName] = useState<string>("");
  const [date, setDate] = useState<Date>(new Date());
  const [role, setRole] = useState<string>("");

  const [dataCast, setDataCast] = useState<CastType[]>([]);
  const [cast, setCast] = useState<CastType[]>([]);

  useEffect(() => {
    (async (id: any) => {
      try {
        const movieData: AllMovieInfoType = await getMovie(id);
        const tagsResponse = await getTags();
        const castResponse = await getCast();

        const missingTags: TagEntriesType[] = [];

        tagsResponse.forEach((tag) => {
          let add = true;
          movieData.tags.forEach((usedTag, index) => {
            if (tag.tagId === usedTag.tagId) add = false;
            if (index === movieData.tags.length - 1 && add) {
              missingTags.push(tag);
            }
          });
        });

        const missingCast: CastType[] = [];

        castResponse.forEach((person) => {
          let add = true;
          movieData.castMembers.forEach((usedPerson, index) => {
            if (person.id === usedPerson.personId) add = false;
            if (index === movieData.castMembers.length - 1 && add) {
              missingCast.push({
                id: person.id,
                name: person.name,
                role: person.role,
              });
            }
          });
        });

        setDataTags([...missingTags]);
        setTags(movieData.tags);
        setDataCast(missingCast);
        setCast(
          movieData.castMembers.map((p) => {
            return { id: p.personId, name: p.name, role: p.role };
          })
        );
        setUrl(movieData.movie.coverUrl);
        setDate(new Date(movieData.movie.releaseDate));
        setName(movieData.movie.name);

        setIsLoading(false);
      } catch (ex: any) {
        console.error(ex);
        setIsLoading(false);
      }
    })(params.movieId);
  }, [params.movieId]);

  const deleteClickHandler = () => {};

  const tagsChangeHandler = (
    inputTags: TagEntriesType[],
    inputDataTags: TagEntriesType[]
  ) => {
    setDataTags([...inputDataTags]);
    setTags([...inputTags]);
  };

  const urlChangeHandler = (e: any) => {
    setUrl(e.target.value);
  };

  const nameChangeHandler = (e: any) => {
    setName(e.target.value);
  };

  const dateChangeHandler = (event: any) => {
    setDate(new Date(event.target.value));
  };

  const roleChangeHandler = (event: any) => {
    setRole(event.target.value);
  };

  const castChangeHandler = (
    inputCast: CastType[],
    inputDataCast: CastType[]
  ) => {
    setDataCast([...inputDataCast]);
    setCast([...inputCast]);
    setRole("");
  };

  return (
    <Container>
      <>
        {context.isLogedIn && (
          <button
            className="button edit-button"
            type="button"
            onClick={deleteClickHandler}
          >
            <i className="fa-solid fa-trash"></i>
          </button>
        )}
        {!isLoading && (
          <div className="details_container">
            <div className="edit-image">
              <img
                src={url}
                className="details_cover"
                alt={"Cover for " + name}
              />
              <div>
                <input
                  type={"text"}
                  value={url}
                  onChange={urlChangeHandler}
                  className={"input-dark input-add"}
                ></input>
              </div>
            </div>

            <div className="details_info">
              <input
                type={"text"}
                className={"input-dark input-add"}
                value={name}
                onChange={nameChangeHandler}
              ></input>

              <p className="info_container_title">TAGS</p>

              <ItemInput
                onChange={tagsChangeHandler}
                dataItems={dataTags}
                items={tags}
                placeHolder={"Choose a person"}
                idProp={"tagId"}
              />

              <p className="info_container_title">CAST</p>

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
                ></input>
              </ItemInput>

              <input
                type={"date"}
                value={date.toISOString().split("T")[0]}
                onChange={dateChangeHandler}
                className={"input-dark input-add"}
              ></input>
            </div>
          </div>
        )}
        {isLoading && <LoadingCircle />}
      </>
    </Container>
  );
};
export default EditDetails;
