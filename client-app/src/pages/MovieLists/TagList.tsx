import GenericMovielist from "../../components/UI/GenericMovieList";
import { getMoviesFromTagAtPage, getTag } from "../../functions/MovieRequests";
import { TagType } from "../../Type/Types";
import { useParams } from "react-router";
import { useEffect, useState } from "react";
import LoadingCircle from "../../components/UI/LoadingCircle";
import Container from "../../components/UI/Container";
import ErrorContainer from "../../components/UI/ErrorContainer";

const TagList = () => {
  //States
  const [tag, setTag] = useState<TagType | null>(null);
  const [error, setError] = useState<string | null>(null);

  //Hooks
  const params = useParams();

  useEffect(() => {
    (async () => {
      try {
        if (params.id === undefined) return;

        const data: TagType = await getTag(String(params.id));
        setTag(data);
        setError(null);
      } catch (ex: any) {
        setError("Error loading movie!");
      }
    })();
  }, [params.id]);

  return (
    <Container>
      <ErrorContainer error={error}>
        <>
          {tag !== null && (
            <>
              <h1 className="title">{`Tag - ${tag.name}`}</h1>
              <GenericMovielist
                fetchFunction={getMoviesFromTagAtPage}
                filterId={tag.id}
              ></GenericMovielist>
            </>
          )}

          {!tag === null && <LoadingCircle />}
        </>
      </ErrorContainer>
    </Container>
  );
};

export default TagList;
