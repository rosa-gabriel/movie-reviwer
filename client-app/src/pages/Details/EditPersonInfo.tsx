import { useNavigate, useParams } from "react-router-dom";
import { useContext, useEffect, useState } from "react";
import {
  deletePerson,
  getPerson,
  updatePerson,
} from "../../functions/requests/MovieRequests";
import LoadingCircle from "../../components/UI/LoadingCircle";
import { PersonResponse } from "../../types/Types";
import Container from "../../components/UI/Container";
import { UserContext } from "../../contexts/UserContext";
import AccountCheck from "../../components/account/AccountCheck";
import { ModalContext } from "../../contexts/ModalContext";
import { NotificationContext } from "../../contexts/NotificationContext";
import ErrorContainer from "../../components/UI/ErrorContainer";
import IsAdminCheck from "../../components/account/IsAdminCheck";
import SendButton from "../../components/UI/SendButton";

const EditPersonInfo = () => {
  //States
  const [isLoading, setIsLoading] = useState<boolean>(false);
  const [isConfirmLoading, setIsConfirmLoading] = useState<boolean>(false);
  const [error, setError] = useState<string | null>(null);
  const [url, setUrl] = useState<string>("");
  const [name, setName] = useState<string>("");
  const [date, setDate] = useState<Date>(new Date());
  const [gender, setGender] = useState<number>(0);
  const [bio, setBio] = useState<string>("");

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
  const bioChangeHandler = (event: any) => {
    setBio(event.target.value);
  };
  const genderChangeHandler = (event: any) => {
    setGender(event.target.value);
  };

  //Delete
  const deleteClickHandler = () => {
    modal.showModal(
      "DELETE",
      "Are you sure you want to delete this person?",
      async () => {
        try {
          await deletePerson(
            String(params.personId),
            String(context.userInfo?.token)
          );

          notification.addNotification({
            code: "DELETED",
            text: "The person was deleted successfully.",
            error: false,
          });
          navigate("/MovieApp");
        } catch (ex: any) {
          notification.addNotification({
            code: "FAILED",
            text: "Failed to delete the person! Try again.",
            error: true,
          });
        }
      }
    );
  };

  //Submit
  const editConfirmHandler = async () => {
    setIsConfirmLoading(true);
    const person: PersonResponse = {
      id: String(params.personId),
      name: name,
      profileImageUrl: url,
      birthday: date,
      biography: bio,
      gender: gender,
    };
    try {
      await updatePerson(person, String(context.userInfo?.token));
      notification.addNotification({
        code: "UPDATED",
        text: "The person was successfully updated.",
        error: false,
      });
      navigate(`/MovieApp/person/${String(params.personId)}`);
    } catch (ex: any) {
      notification.addNotification({
        code: "FAIL",
        text: "Failed to update the person! Try again later.",
        error: true,
      });
    } finally {
      setIsConfirmLoading(false);
    }
  };

  const cancelHandler = () => {
    navigate(`/MovieApp/person/${String(params.personId)}`);
  };

  //Effect
  useEffect(() => {
    setIsLoading(true);
    (async () => {
      if (params.personId === undefined) return;

      try {
        const data: PersonResponse = await getPerson(String(params.personId));
        setName(data.name);
        setUrl(data.profileImageUrl);
        setDate(data.birthday);
        setBio(data.biography);
        setGender(data.gender);
        setError(null);
      } catch (ex: any) {
        setError(ex.message);
      } finally {
        setIsLoading(false);
      }
    })();
  }, [params.movieId]);

  return (
    <Container>
      <ErrorContainer error={error}>
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

                  <p className="info_container_title">BIO</p>
                  <textarea
                    value={bio}
                    onChange={bioChangeHandler}
                    className={"input-dark input-add person-bio-edit"}
                  />

                  <p className="info_container_title">GENDER</p>
                  <select
                    onChange={genderChangeHandler}
                    className="input-dark input-add"
                    placeholder="Gender"
                    value={gender}
                  >
                    <option value={0}>-- Choose a Gender --</option>
                    <option value={1}>Male</option>
                    <option value={2}>Female</option>
                    <option value={3}>Other</option>
                  </select>

                  <p className="info_container_title">RELEASE DATE</p>
                  <input
                    type={"date"}
                    value={date.toISOString().split("T")[0]}
                    onChange={dateChangeHandler}
                    className={"input-dark input-add"}
                  />
                </div>
              </div>

              <div className="edit-buttons-container">
                <button
                  className="button"
                  type="button"
                  onClick={cancelHandler}
                >
                  Cancel
                </button>
                <SendButton
                  className={""}
                  text={"Confirm"}
                  isLoading={isConfirmLoading}
                  onClick={editConfirmHandler}
                  type={"button"}
                />
              </div>
            </>
          )}
          {isLoading && <LoadingCircle />}
        </>
      </ErrorContainer>
    </Container>
  );
};
export default EditPersonInfo;
