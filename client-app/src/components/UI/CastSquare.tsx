import { Link } from "react-router-dom";
import { CastType } from "../../Type/Types";

type CastSquareProps = {
  person: CastType;
};

const CastSquare = (props: CastSquareProps) => {
  return (
    <div className="tag">
      <Link to={String(props.person.castId)}>
        <span>
          {props.person.name} ({props.person.role})
        </span>
      </Link>
    </div>
  );
};

export default CastSquare;
