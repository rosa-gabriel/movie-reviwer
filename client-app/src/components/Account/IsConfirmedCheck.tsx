import { useContext, useEffect } from "react";
import { useNavigate } from "react-router";
import { UserContext } from "../../contexts/UserContext";

const IsConfirmedCheck = () => {
  const context = useContext(UserContext);
  const navigate = useNavigate();

  useEffect(() => {
    if (context.userInfo == null) return;

    if (context.isLogedIn) {
      console.log("chamada");
      if (!context.userInfo?.confirmed) {
        navigate("/account/message/confirm", { replace: true });
      }
    }
  });

  return <></>;
};

export default IsConfirmedCheck;
