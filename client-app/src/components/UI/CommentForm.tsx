import { useContext, useState } from "react";
import { Link } from "react-router-dom";
import { NotificationContext } from "../../contexts/NotificationContext";
import { UserContext } from "../../contexts/UserContext";
import { addComment } from "../../functions/requests/MovieRequests";
import SendButton from "./SendButton";

type CommentFormProps = {
  movieId: string;
  reload(): void;
};

const CommentForm = (props: CommentFormProps) => {
  const [messge, setMessage] = useState<string>("");

  const [isLoading, setIsLoading] = useState<boolean>(false);

  const context = useContext(UserContext);
  const notification = useContext(NotificationContext);

  const commentChangeHandler = (e: any) => {
    setMessage(e.target.value);
  };

  const submitHandler = async (e: any) => {
    try {
      setIsLoading(true);
      await addComment(messge, props.movieId, String(context.userInfo?.token));
      setMessage("");
      notification.addNotification({
        code: "POSTED",
        text: "Comment was successfully posted.",
        error: false,
      });
      props.reload();
    } catch (ex: any) {
      notification.addNotification({
        code: "ERROR",
        text: "Comment could not be added! Try again.",
        error: true,
      });
    }
    finally{
      setIsLoading(false);
    }
  };

  return (
    <>
      {context.isLogedIn && (
        <div className="comment-create-container">
          <input
            className="input-dark"
            type={"text"}
            value={messge}
            onChange={commentChangeHandler}
            placeholder={"Enter the comment text here."}
          ></input>
          <SendButton onClick={submitHandler} className="button" text={"Comment"} isLoading={isLoading} type={"button"} />
        </div>
      )}
      {!context.isLogedIn && (
        <p className="centered-message">
          <Link to={"/account/login"} className="link">
            Log in{" "}
          </Link>{" "}
          or{" "}
          <Link className="link" to={"/account/register"}>
            Register
          </Link>{" "}
          to make a comment.
        </p>
      )}
    </>
  );
};

export default CommentForm;
