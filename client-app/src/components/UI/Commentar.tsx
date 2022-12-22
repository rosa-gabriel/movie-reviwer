import { useEffect } from "react";
import { Link } from "react-router-dom";
import { CommentType } from "../../types/Types";

type CommentarProps = {
  comment: CommentType;
};

const Commentar = (props: CommentarProps) => {
  useEffect(() => {}, []);
  return (
    <div className="comment">
      <Link to={`/account/profile/${props.comment.creator.id}`}>
        <div className="nav-profile-container">
          <img src={props.comment.creator.profileImageUrl} alt="comment" />
        </div>
      </Link>
      <div className="comment-text">
        <div className="comment-header">
          <Link to={`/account/profile/${props.comment.creator.id}`}>
            <h4>{props.comment.creator.name}</h4>
          </Link>
          <p>{props.comment.postDate.toDateString()}</p>
          <i className="fa-solid fa-ellipsis"></i>
        </div>
        <p>{props.comment.message}</p>
      </div>
    </div>
  );
};
export default Commentar;
