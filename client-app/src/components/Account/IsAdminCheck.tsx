import { useContext, useEffect } from "react";
import { useNavigate } from "react-router";
import { UserContext } from "../../contexts/UserContext";

const IsAdminCheck = () => {
  const context = useContext(UserContext);
  const navigate = useNavigate();

  useEffect(() => {
    if (!context.userInfo?.isAdmin) {
      navigate("/", { replace: true });
    }
  });

  return <></>;
};

export default IsAdminCheck;
