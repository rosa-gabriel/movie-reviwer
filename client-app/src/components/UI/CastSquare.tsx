import { Link } from "react-router-dom";

const CastSquare = (props: any) => {
  return (
    <div className="tag">
      <Link to={props.person.id}>
        <span>{props.person.name} ({props.role})</span>
      </Link>
    </div>
  );
};

export default CastSquare;
