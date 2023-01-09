import { useContext, useEffect } from "react";
import { useNavigate } from "react-router";
import { UserContext } from "../../contexts/UserContext";

const AccountCheck = () => {
  const context = useContext(UserContext);
  const navigate = useNavigate();

  useEffect(() => {
    if (!context.isLogedIn) {
      navigate("/MovieApp/account/login", { replace: true });
    }
  });

  return <></>;
};

export default AccountCheck;
