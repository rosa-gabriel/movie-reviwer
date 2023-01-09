import { Link } from "react-router-dom";
import { TagInfo } from "../../../types/Types";

type TagProps = {
  tagItem: TagInfo;
};

const Tag = (props: TagProps) => {
  return (
    <div className="tag">
      <Link to={`/MovieApp/tag/${String(props.tagItem.tagId)}`}>
        <span>{props.tagItem.name}</span>
        <span>{props.tagItem.entries}</span>
      </Link>
    </div>
  );
};

export default Tag;
