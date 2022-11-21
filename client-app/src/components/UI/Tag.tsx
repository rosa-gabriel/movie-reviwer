import { Link } from "react-router-dom";
import { TagType } from "../../Type/Types";

type TagProps = {
  tagItem: TagType;
};

const Tag = (props: TagProps) => {
  return (
    <div className="tag">
      <Link to={String(props.tagItem.tagId)}>
        <span>{props.tagItem.name}</span>
        <span>{props.tagItem.entries}</span>
      </Link>
    </div>
  );
};

export default Tag;
