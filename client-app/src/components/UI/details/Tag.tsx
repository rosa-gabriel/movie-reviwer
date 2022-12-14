import { Link } from "react-router-dom";
import { TagEntriesType } from "../../../Type/Types";

type TagProps = {
  tagItem: TagEntriesType;
};

const Tag = (props: TagProps) => {
  return (
    <div className="tag">
      <Link to={`/tag/${String(props.tagItem.tagId)}`}>
        <span>{props.tagItem.name}</span>
        <span>{props.tagItem.entries}</span>
      </Link>
    </div>
  );
};

export default Tag;
