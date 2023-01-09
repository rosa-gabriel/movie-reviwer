import { useContext, useEffect } from "react";
import { UserContext } from "../contexts/UserContext";
import { seedMovies, seedTags } from "./DataBaseSeeding";

const DataBaseSeedingO = () => {
  const context = useContext(UserContext);
  useEffect(() => {
    (async () => {
      const response = await seedTags(String(context.userInfo?.token));
      setTimeout(async () => {
        console.log(response);
        await seedMovies(response, String(context.userInfo?.token));
      }, 10000);
    })();
  }, []);
  return <></>;
};

export default DataBaseSeedingO;
