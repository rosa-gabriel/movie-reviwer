import { Link } from "react-router-dom";

const Tag = (props: any) => {
  return (
    <div className="tag">
      <Link to={props.TagItem.id}>
        <span>{props.TagItem.name}</span>
        <span>{props.TagItem.entries}</span>
      </Link>
    </div>
  );
};

export default Tag;
