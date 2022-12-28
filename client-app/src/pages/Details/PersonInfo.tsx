import GenericMovielist from "../../components/UI/GenericMovieList";
import {
  getMoviesFromPersonAtPage,
  getPerson,
} from "../../functions/requests/MovieRequests";
import { PersonResponse } from "../../types/Types";
import { useParams } from "react-router";
import { useEffect, useState } from "react";
import LoadingCircle from "../../components/UI/LoadingCircle";
import Container from "../../components/UI/Container";
import ErrorContainer from "../../components/UI/ErrorContainer";

const TagList = () => {
  //States
  const [person, setPerson] = useState<PersonResponse | null>(null);
  const [error, setError] = useState<string | null>(null);

  //Hooks
  const params = useParams();

  useEffect(() => {
    (async () => {
      if (params.id === undefined) return;
      try {
        const data: PersonResponse= await getPerson(String(params.id));
        setPerson(data);
        setError(null);
      } catch (ex: any) {
        setError(ex.message);
      }
    })();
  }, [params.id]);

  return (
    <Container>
      <ErrorContainer error={error}>
        <>
          {person !== null && (
            <>
              <div className="person-info">
                <div className="details_container">
                  <img
                    src={person.profileImageUrl}
                    className="details_cover"
                    alt={"Cover image for " + person.name}
                  />

                  <div className="details_info">
                    <h1>{person.name}</h1>
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
