import { Link } from "react-router-dom";

const Details = () => {

  return (
    <div style={{textAlign: "center"}}>
      <h1 style={{marginBottom: "20px"}}>Url not found! Return to the main menu.</h1>
      <Link to={"/"}>Home</Link>
    </div>
  );
};
export default Details;
