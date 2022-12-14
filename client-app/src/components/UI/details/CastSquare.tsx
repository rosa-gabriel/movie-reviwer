import { Link } from "react-router-dom";
import { CastEntryType } from "../../../Type/Types";

type CastSquareProps = {
  person: CastEntryType;
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
