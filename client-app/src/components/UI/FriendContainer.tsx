import { useContext } from "react";
import { Link } from "react-router-dom";
import { NotificationContext } from "../../contexts/NotificationContext";
import { UserContext } from "../../contexts/UserContext";
import {
  deleteFriend,
  putFriendRequest,
} from "../../functions/requests/AccountRequests";
import { Friend } from "../../types/Types";

type FriendContainerProps = {
  friendRequest: Friend;
  pending: boolean;
  reset(): any;
};

const FriendContainer = (props: FriendContainerProps) => {
  const context = useContext(UserContext);
  const notification = useContext(NotificationContext);

  const confirmFriendHandler = async () => {
    try {
      await putFriendRequest(
        String(props.friendRequest.id),
        String(context.userInfo?.token)
      );
      notification.addNotification({
        code: "ACCEPTED",
        text: "Friend request acepted.",
        error: false,
      });
    } catch (ex: any) {
      notification.addNotification({
        code: "FAILED",
        text: "Failed to accept friend request",
        error: true,
      });
    } finally {
      props.reset();
    }
  };

  const removeFriendHandler = async () => {
    try {
      await deleteFriend(
        String(props.friendRequest.friend.id),
        String(context.userInfo?.token)
      );
      notification.addNotification({
        code: "DENIED",
        text: "Friend request denied.",
        error: false,
      });
    } catch (ex: any) {
      notification.addNotification({
        code: "FAILED",
        text: "Failed to deny friend friend",
        error: true,
      });
    } finally {
      props.reset();
    }
  };
  return (
    <div className="friend">
      <Link to={`/MovieApp/account/profile/${props.friendRequest.friend.id}`}>
        <div className="nav-profile-container">
          <img src={props.friendRequest.friend.profileImageUrl} alt="comment" />
        </div>
      </Link>
      <div className="friend-name">
        <Link to={`/MovieApp/account/profile/${props.friendRequest.friend.id}`}>
          <h4>{props.friendRequest.friend.name}</h4>
        </Link>
      </div>
      {props.pending && (
        <div>
          {props.friendRequest.sent && (
            <p className="faded">Pending response.</p>
          )}
          {!props.friendRequest.sent && (
            <>
              <button
                type="button"
                onClick={removeFriendHandler}
                className="button"
              >
                Deny
              </button>
              <button
                type="button"
                onClick={confirmFriendHandler}
                className="button"
              >
                Accept
              </button>
            </>
          )}
        </div>
      )}
    </div>
  );
};
export default FriendContainer;
