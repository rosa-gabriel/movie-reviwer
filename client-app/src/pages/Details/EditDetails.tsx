import { useNavigate, useParams } from "react-router-dom";
import { useContext, useEffect, useState } from "react";
import {
  deleteMovie,
  getCast,
  getMovie,
  getTags,
  updateMovie,
} from "../../functions/MovieRequests";
import LoadingCircle from "../../components/UI/LoadingCircle";
import {
  AllMovieInfoType,
  CastEntryType,
  CastType,
  TagEntriesType,
} from "../../Type/Types";
import Container from "../../components/UI/Container";
import { UserContext } from "../../Context/UserContext";
import ItemInput from "../../components/UI/inputs/ItemInput";
import AccountCheck from "../../components/Account/AccountCheck";
import { ModalContext } from "../../Context/ModalContext";
import { NotificationContext } from "../../Context/NotificationContext";

const EditDetails = () => {
  //States
  const [isLoading, setIsLoading] = useState<boolean>(false);
  const [dataTags, setDataTags] = useState<TagEntriesType[]>([]);
  const [tags, setTags] = useState<TagEntriesType[]>([]);
  const [url, setUrl] = useState<string>("");
  const [name, setName] = useState<string>("");
  const [date, setDate] = useState<Date>(new Date());
  const [role, setRole] = useState<string>("");
  const [dataCast, setDataCast] = useState<CastType[]>([]);
  const [cast, setCast] = useState<CastEntryType[]>([]);

  //Contexts
  const context = useContext(UserContext);
  const modal = useContext(ModalContext);
  const notification = useContext(NotificationContext);

  //Hooks
  const params = useParams();
  const navigate = useNavigate();

  //Input Handler
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
    inputCast: CastEntryType[],
    inputDataCast: CastType[]
  ) => {
    setDataCast([...inputDataCast]);
    setCast([...inputCast]);
    setRole("");
  };
  const tagsChangeHandler = (
    inputTags: TagEntriesType[],
    inputDataTags: TagEntriesType[]
  ) => {
    setDataTags([...inputDataTags]);
    setTags([...inputTags]);
  };

  //Delete
  const deleteClickHandler = () => {
    modal.showModal(
      "DELETE",
      "Are you sure you want to delete this movie?",
      async () => {
        try {
          await deleteMovie(
            String(params.movieId),
            String(context.userInfo?.token)
          );
          navigate("/");
        } catch (ex: any) {
          console.error(ex);
        }
      }
    );
  };

  //Submit
  const editConfirmHandler = async () => {
    const movie: AllMovieInfoType = {
      movie: {
        id: String(params.movieId),
        name: name,
        coverUrl: url,
        releaseDate: date,
      },
      favorites: 0,
      tags: tags,
      castMembers: cast,
    };
    try {
      await updateMovie(movie, String(context.userInfo?.token));
      notification.addNotification({
        code: "UPDATED",
        text: "The movie was successfully updated.",
        error: false,
      });
      navigate(`/details/${String(params.movieId)}`);
    } catch (ex: any) {
      notification.addNotification({
        code: "FAIL",
        text: "Failed to update the movie! Try again later.",
        error: true,
      });
    }
  };

  const cancelHandler = () => {
    navigate(`/details/${String(params.movieId)}`);
  };

  //Effect
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
            return { personId: p.personId, name: p.name, role: p.role };
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

  return (
    <Container>
      <>
        <AccountCheck />
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
          <>
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
                  itemId={"tagId"}
                  dataItemId={"tagId"}
                />

                <p className="info_container_title">CAST</p>

                <ItemInput
                  onChange={castChangeHandler}
                  dataItems={dataCast}
                  items={cast}
                  role={role}
                  placeHolder={"Choose a person"}
                  itemId={"personId"}
                  dataItemId={"id"}
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

            <div className="edit-buttons-container">
              <button className="button" type="button" onClick={cancelHandler}>
                Cancel
              </button>
              <button
                className="button"
                type={"button"}
                onClick={editConfirmHandler}
              >
                Confirm
              </button>
            </div>
          </>
        )}
        {isLoading && <LoadingCircle />}
      </>
    </Container>
  );
};
export default EditDetails;
