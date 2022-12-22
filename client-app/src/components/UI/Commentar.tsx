import { useEffect } from "react";

type CommentarProps = {
  comment: any;
};

const Commentar = (props: CommentarProps) => {
  useEffect(() => {}, []);
  return (
    <div className="comment">
      <div className="nav-profile-container">
        <img src={props.comment.creator.profileImageUrl} alt="comment" />
      </div>
      <div className="comment-text">
        <div className="comment-header">
          <h4>{props.comment.creator.name}</h4>
          <p>{props.comment.postDate}</p>
          <i className="fa-solid fa-ellipsis"></i>
        </div>
        <p>{props.comment.message}</p>
      </div>
    </div>
  );
};
export default Commentar;
