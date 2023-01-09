import { Link } from "react-router-dom";
import { CastInfo } from "../../../types/Types";

type CastSquareProps = {
  person: CastInfo;
};

const CastSquare = (props: CastSquareProps) => {
  return (
    <div className="tag">
      <Link to={`/person/${String(props.person.personId)}`}>
        <span>
          {props.person.name} ({props.person.role})
        </span>
      </Link>
    </div>
  );
};

export default CastSquare;
