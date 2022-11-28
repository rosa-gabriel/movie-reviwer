import { UserContext } from "../Context/UserContext";
import { useContext } from "react";
import { useNavigate } from "react-router-dom";

const LogOut = () => {
  const context = useContext(UserContext);
  const navigate = useNavigate();
  navigate("/");
  return <></>;
};

export default LogOut;
