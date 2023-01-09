import { useNavigate, useParams } from "react-router-dom";
import { useContext, useEffect, useState } from "react";
import {
  deleteMovie,
  getCast,
  getMovie,
  getTags,
  updateMovie,
} from "../../functions/requests/MovieRequests";
import LoadingCircle from "../../components/UI/LoadingCircle";
import {
  AllMovieInfoType,
  CastInfo,
  Cast,
  NewMovieInfo,
  Tag,
} from "../../types/Types";
import Container from "../../components/UI/Container";
import { UserContext } from "../../contexts/UserContext";
import ItemInput from "../../components/UI/inputs/ItemInput";
import AccountCheck from "../../components/account/AccountCheck";
import { ModalContext } from "../../contexts/ModalContext";
import { NotificationContext } from "../../contexts/NotificationContext";
import { TagInfoToTag } from "../../functions/Conversion/Convertions";
import IsAdminCheck from "../../components/account/IsAdminCheck";
import SendButton from "../../components/UI/SendButton";

const EditDetails = () => {
  //States
  const [isLoading, setIsLoading] = useState<boolean>(false);
  const [isConfirmLoading, setIsConfirmLoading] = useState<boolean>(false);
  const [dataTags, setDataTags] = useState<Tag[]>([]);
  const [tags, setTags] = useState<Tag[]>([]);
  const [url, setUrl] = useState<string>("");
  const [name, setName] = useState<string>("");
  const [date, setDate] = useState<Date>(new Date());
  const [role, setRole] = useState<string>("");
  const [dataCast, setDataCast] = useState<Cast[]>([]);
  const [cast, setCast] = useState<CastInfo[]>([]);

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
  const castChangeHandler = (inputCast: CastInfo[], inputDataCast: Cast[]) => {
    setDataCast([...inputDataCast]);
    setCast([...inputCast]);
    setRole("");
  };
  const tagsChangeHandler = (inputTags: Tag[], inputDataTags: Tag[]) => {
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
          notification.addNotification({
            code: "DELETED",
            text: "The movie was deleted successfully.",
            error: false,
          });
          navigate("/MovieApp");
        } catch (ex: any) {
          notification.addNotification({
            code: "FAILED",
            text: "Failed to delete the movie! Try again.",
            error: true,
          });
        }
      }
    );
  };

  //Submit
  const editConfirmHandler = async () => {
    setIsConfirmLoading(true);
    const movie: NewMovieInfo = {
      id: String(params.movieId),
      name: name,
      coverUrl: url,
      releaseDate: date,
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
      navigate(`/MovieApp/details/${String(params.movieId)}`);
    } catch (ex: any) {
      notification.addNotification({
        code: "FAIL",
        text: "Failed to update the movie! Try again later.",
        error: true,
      });
    } finally {
      setIsConfirmLoading(false);
    }
  };

  const cancelHandler = () => {
    navigate(`/MovieApp/details/${String(params.movieId)}`);
  };

  //Effect
  useEffect(() => {
    setIsLoading(true);
    (async (id: any) => {
      try {
        const movieData: AllMovieInfoType = await getMovie(id);
        const tagsResponse = await getTags();
        const castResponse = await getCast();

        const missingTags: Tag[] = [];

        const usedTags: Tag[] = movieData.tags.map((uTag) =>
          TagInfoToTag(uTag)
        );

        tagsResponse.forEach((tag) => {
          let add = true;
          movieData.tags.forEach((usedTag, index) => {
            if (tag.tagId === usedTag.tagId) add = false;
          });
          if (add) missingTags.push(TagInfoToTag(tag));
        });

        const missingCast: Cast[] = [];

        castResponse.forEach((person) => {
          let add = true;
          movieData.castMembers.forEach((usedPerson, index) => {
            if (person.id === usedPerson.personId) add = false;
          });
          if (add) {
            missingCast.push({
              id: person.id,
              name: person.name,
              role: person.role,
            });
          }
        });

        setDataTags([...missingTags]);
        setTags(usedTags);
        setDataCast(missingCast);
        setCast(
          movieData.castMembers.map((p) => {
            return { personId: p.personId, name: p.name, role: p.role };
          })
        );
        setUrl(movieData.coverUrl);
        setDate(new Date(movieData.releaseDate));
        setName(movieData.name);

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
        <IsAdminCheck />
        <button
          className="button edit-button"
          type="button"
          onClick={deleteClickHandler}
        >
          <i className="fa-solid fa-trash"></i>
        </button>
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
                  itemId={"id"}
                  dataItemId={"id"}
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
              <SendButton
                className={""}
                text={"Confirm"}
                onClick={editConfirmHandler}
                isLoading={isConfirmLoading}
                type={"button"}
              />
            </div>
          </>
        )}
        {isLoading && <LoadingCircle />}
      </>
    </Container>
  );
};
export default EditDetails;
