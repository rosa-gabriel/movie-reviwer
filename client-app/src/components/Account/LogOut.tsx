import { UserContext } from "../../contexts/UserContext";
import { useContext } from "react";
import { useNavigate } from "react-router-dom";

const LogOut = () => {
  const context = useContext(UserContext);
  const navigate = useNavigate();
  navigate("/MovieApp");
  return <></>;
};

export default LogOut;
