import GenericMovielist from "./UI/GenericMovieList";
import { getMoviesFromTag, getTag } from "../functions/MoviesData";
import { TagType } from "../Type/Types";
import { useNavigate, useParams } from "react-router";
import { useEffect, useState } from "react";
import LoadingCircle from "./UI/LoadingCircle";
import Container from "./UI/Container";

const TagList = () => {
  const params = useParams();
  const navigate = useNavigate();
  const [tag, setTag] = useState<TagType | null>(null);
  const [error, setError] = useState<string | null>(null);

  useEffect(() => {
    (async () => {
      if (params.id == undefined) return;
      try {
        const data: TagType = await getTag(String(params.id));
        setTag(data);
        setError(null);
      } catch (ex: any) {
        setError(ex.message);
        navigate("/error");
      }
    })();
  }, []);

  return (
    <Container>
      <>
        {tag !== null && (
          <>
            <h1 className="title">{`Tag - ${tag.name}`}</h1>
            <GenericMovielist
              fetchFunction={getMoviesFromTag}
              filterId={tag.id}
            ></GenericMovielist>
          </>
        )}

        {!tag === null && <LoadingCircle />}
      </>
    </Container>
  );
};

export default TagList;
