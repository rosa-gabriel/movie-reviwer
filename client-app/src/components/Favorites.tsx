import { useContext, useEffect, useState } from "react";
import { useNavigate } from "react-router";
import { getUserFavorites } from "../functions/MoviesData";
import AccountCheck from "./AccountForms/AccountCheck";
import { UserContext } from "./Context/UserContext";
import GenericMovielist from "./UI/GenericMovieList";

const Details = () => {
  const navigate = useNavigate();
  const [error, setError] = useState<string | null>(null);
  const context = useContext(UserContext);

  useEffect(() => {
    (async () => {
      if (!context.userInfo) return;
      try {
        setError(null);
      } catch (ex: any) {
        setError(ex.message);
        navigate("/error");
      }
    })();
  }, []);

  return (
    <>
      <AccountCheck />
      <GenericMovielist
        title={"Your Favorites"}
        fetchFunction={getUserFavorites}
        filterId={context.userInfo ? context.userInfo.token : ""}
      />
    </>
  );
};
export default Details;
