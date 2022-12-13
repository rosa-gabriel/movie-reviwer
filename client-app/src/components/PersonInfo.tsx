import GenericMovielist from "./UI/GenericMovieList";
import { getMoviesFromPersonAtPage, getPerson } from "../functions/MoviesData";
import { PersonType } from "../Type/Types";
import { useNavigate, useParams } from "react-router";
import { useEffect, useState } from "react";
import LoadingCircle from "./UI/LoadingCircle";
import Container from "./UI/Container";

const TagList = () => {
  const params = useParams();
  const navigate = useNavigate();
  const [person, setPerson] = useState<PersonType | null>(null);
  const [hasError, setHasError] = useState<string | null>(null);

  useEffect(() => {
    (async () => {
      if (params.id == undefined) return;
      try {
        const data: PersonType = await getPerson(String(params.id));
        console.log(data);
        setPerson(data);
        setHasError(null);
      } catch (ex: any) {
        setHasError(ex.message);
        navigate("/error");
      }
    })();
  }, []);

  return (
    <Container>
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
    </Container>
  );
};

export default TagList;
