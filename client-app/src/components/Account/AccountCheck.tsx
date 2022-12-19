import { useContext } from "react";
import { Navigate } from "react-router";
import { UserContext } from "../../contexts/UserContext";

const AccountCheck = () => {
  const context = useContext(UserContext);
  return (
    <>
      {!context.isLogedIn ? (
        <Navigate to={"/account/login"} replace={true} />
      ) : (
        <></>
      )}
    </>
  );
};

export default AccountCheck;
