import GenericMovielist from "../../components/UI/GenericMovieList";
import {
  getMoviesFromPersonAtPage,
  getPerson,
} from "../../functions/requests/MovieRequests";
import { PersonResponse } from "../../types/Types";
import { useNavigate, useParams } from "react-router";
import { useContext, useEffect, useState } from "react";
import LoadingCircle from "../../components/UI/LoadingCircle";
import Container from "../../components/UI/Container";
import ErrorContainer from "../../components/UI/ErrorContainer";
import { IdToGender } from "../../functions/Conversion/Convertions";
import { UserContext } from "../../contexts/UserContext";

const TagList = () => {
  //States
  const [person, setPerson] = useState<PersonResponse | null>(null);
  const [error, setError] = useState<string | null>(null);

  const context = useContext(UserContext);
  const navigate = useNavigate();

  //Hooks
  const params = useParams();

  useEffect(() => {
    (async () => {
      if (params.id === undefined) return;
      try {
        const data: PersonResponse = await getPerson(String(params.id));
        setPerson(data);
        setError(null);
      } catch (ex: any) {
        setError(ex.message);
      }
    })();
  }, [params.id]);

  const editClickHandler = async () => {
    navigate("edit");
  };

  return (
    <Container>
      <ErrorContainer error={error}>
        <>
          {person !== null && (
            <>
              {context.isLogedIn && context.userInfo?.isAdmin && (
                <button
                  className="button edit-button"
                  type="button"
                  onClick={editClickHandler}
                >
                  <i className="fa-solid fa-pen-to-square"></i>
                </button>
              )}
              <div className="person-info">
                <div className="details_container">
                  <img
                    src={person.profileImageUrl}
                    className="details_cover"
                    alt={"Cover image for " + person.name}
                  />

                  <div className="details_info">
                    <h1>{person.name}</h1>
                    <div className="person-details">
                      <p className="info_container_title">BIRTHDAY</p>
                      <p className="profile-info">
                        {person.birthday && person.birthday.toDateString()}
                        {!person.birthday && "Unknown"}
                      </p>

                      <p className="info_container_title">GENDER</p>
                      <p className="profile-info">
                        {IdToGender(person.gender)}
                      </p>

                      <p className="info_container_title">BIO</p>
                      <p className="profile-info">{person.biography}</p>
                    </div>
                  </div>
                </div>
              </div>

              <h1 className="title">Movie Apearences</h1>
              <GenericMovielist
                fetchFunction={getMoviesFromPersonAtPage}
                filterId={person.id}
              />
            </>
          )}
          {person === null && <LoadingCircle />}
        </>
      </ErrorContainer>
    </Container>
  );
};

export default TagList;
