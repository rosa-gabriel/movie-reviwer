import GenericMovielist from "./UI/GenericMovieList";
import { getMoviesFromTag, getTag } from "../functions/MoviesData";
import { TagType } from "../Type/Types";
import { useNavigate, useParams } from "react-router";
import { useEffect, useState } from "react";
import LoadingCircle from "./UI/LoadingCircle";

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
        navigate('/error');
      }
    })();
  }, []);

  return (
    <>
      {tag !== null && (
        <GenericMovielist
          title={"Tag - " + tag.name}
          fetchFunction={getMoviesFromTag}
          filterId={tag.id}
        ></GenericMovielist>
      )}

      {!tag === null && <LoadingCircle />}
    </>
  );
};

export default TagList;
