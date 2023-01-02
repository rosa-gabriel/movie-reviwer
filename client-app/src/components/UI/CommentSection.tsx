import { useEffect, useState } from "react";
import { listMovieComments } from "../../functions/requests/MovieRequests";
import Commentar from "./Commentar";
import CommentForm from "./CommentForm";

type CommentSectionProps = {
  movieId: string;
};

const CommentSection = (props: CommentSectionProps) => {
  const [comments, setComments] = useState<any[]>([]);
  const [reloader, setReloader] = useState<number>(1);

  const reloadComments = () => {
    setReloader(Math.random());
  };

  useEffect(() => {
    (async () => {
      try {
        const response = await listMovieComments(props.movieId);
        setComments(response);
      } catch (ex) {}
    })();
  }, [reloader, props.movieId]);

  return (
    <div className="comment-section">
      <h1 className="title">Comments</h1>
      {comments.length === 0 && (
        <p style={{ marginBottom: "10px" }} className="faded centered-message">
          No comments.
        </p>
      )}
      {comments.map((comment: any) => {
        return (
          <Commentar
            key={comment.creator.id + comment.postDate}
            comment={comment}
            reset={reloadComments}
          ></Commentar>
        );
      })}
      <CommentForm reload={reloadComments} movieId={props.movieId} />
    </div>
  );
};
export default CommentSection;
