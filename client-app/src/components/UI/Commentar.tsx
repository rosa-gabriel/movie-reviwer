import { useContext, useEffect, useState } from "react";
import { Link } from "react-router-dom";
import { NotificationContext } from "../../contexts/NotificationContext";
import { UserContext } from "../../contexts/UserContext";
import {
  deleteComment,
  updateComment,
} from "../../functions/requests/MovieRequests";
import { CommentType } from "../../types/Types";
import CommentEditInput from "./CommentEditInput";
import CommentsMenu from "./CommentsMenu";

type CommentarProps = {
  comment: CommentType;
  reset(): any;
};

const Commentar = (props: CommentarProps) => {
  const [showMenu, setShowMenu] = useState<boolean>(false);
  const [isEditing, setIsEditing] = useState<boolean>(false);
  const [newMessage, setNewMessage] = useState<string>("");

  const context = useContext(UserContext);
  const notification = useContext(NotificationContext);

  const ellipsisClickHandler = () => {
    setShowMenu((prevShowMenu) => !prevShowMenu);
  };

  const newMessageChangeHandler = (e: any) => {
    setNewMessage(e.target.value);
  };

  const cancelEditHandler = () => {
    setIsEditing(false);
    setNewMessage(props.comment.message);
  };

  const editHandler = () => {
    setShowMenu(false);
    setIsEditing(true);
  };

  const confirmEditHandler = async () => {
    if (newMessage.trim() == "") {
      notification.addNotification({
        code: "EMPTY",
        text: "Comment text cannot be empty.",
        error: true,
      });
      return;
    }
    try {
      await updateComment(
        { ...props.comment, message: newMessage },
        String(context.userInfo?.token)
      );
      notification.addNotification({
        code: "EDITED",
        text: "The comment was successfuly edited.",
        error: false,
      });
      setIsEditing(false);
      props.reset();
    } catch (ex: any) {
      notification.addNotification({
        code: "FAILED",
        text: "Failed to remove the comment. Try again",
        error: true,
      });
    }
  };

  const deleteHandler = async () => {
    try {
      await deleteComment(props.comment.id, String(context.userInfo?.token));
      notification.addNotification({
        code: "DELETED",
        text: "The comment was deleted.",
        error: false,
      });
      props.reset();
    } catch (ex: any) {
      notification.addNotification({
        code: "FAILED",
        text: "Failed to remove the comment. Try again",
        error: true,
      });
    }
  };
  useEffect(() => {
    setNewMessage(props.comment.message);
  }, [props.comment.message]);

  useEffect(() => {}, []);
  return (
    <div className="comment">
      {showMenu && (
        <CommentsMenu onDelete={deleteHandler} onEdit={editHandler} />
      )}
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
          {props.comment.wasEdited && <p>(edited)</p>}

          {props.comment.creator.id == String(context.userInfo?.id) && (
            <i
              onClick={ellipsisClickHandler}
              className="fa-solid fa-ellipsis"
            />
          )}
        </div>
        {isEditing && (
          <CommentEditInput
            onCancel={cancelEditHandler}
            onConfirmEdit={confirmEditHandler}
            editMessage={newMessageChangeHandler}
            value={newMessage}
          />
        )}
        {!isEditing && <p>{props.comment.message}</p>}
      </div>
    </div>
  );
};
export default Commentar;
