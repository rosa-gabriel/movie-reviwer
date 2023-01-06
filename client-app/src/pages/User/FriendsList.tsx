import { useContext, useEffect, useState } from "react";
import AccountCheck from "../../components/account/AccountCheck";
import Container from "../../components/UI/Container";
import FormPair from "../../components/UI/FormPair";
import FriendContainer from "../../components/UI/FriendContainer";
import SendButton from "../../components/UI/SendButton";
import SubmitButton from "../../components/UI/SubmitButton";
import { NotificationContext } from "../../contexts/NotificationContext";
import { UserContext } from "../../contexts/UserContext";
import {
  getUserFriends,
  postFriendRequest,
} from "../../functions/requests/AccountRequests";
import { Friend } from "../../types/Types";

const FriendsList = () => {
  const [friends, setFriends] = useState<Friend[]>([]);
  const [friendRequest, setFriendRequest] = useState<Friend[]>([]);
  const [seed, setSeed] = useState<number>(1);
  const [id, setId] = useState<string>("");
  const [isLoading, setIsLoading] = useState<boolean>(false);

  const reset = () => {
    setSeed(Math.random);
  };

  const context = useContext(UserContext);
  const notification = useContext(NotificationContext);

  useEffect(() => {
    (async () => {
      try {
        const response = await getUserFriends(String(context.userInfo?.token));
        setFriends(response.friends);
        setFriendRequest(response.friendRequests);
      } catch (ex: any) {}
    })();
  }, [seed, context.userInfo?.token]);

  const addFriendHandler = async () => {
    if (id === "") return;

    setIsLoading(true);
    try {
      await postFriendRequest(id, String(context.userInfo?.token));

      notification.addNotification({
        code: "SENT",
        text: "Friend request sent!",
        error: false,
      });
      reset();
    } catch (ex: any) {
      notification.addNotification({
        code: "FAILED",
        text: ex.message,
        error: true,
      });
    } finally {
      setIsLoading(false);
    }
  };

  const idChangeHandler = (e: any) => {
    setId(e.target.value);
  }

  return (
    <Container>
      <>
        <AccountCheck />
        <h1 className="title">Friends</h1>
        {friends.length === 0 && (
          <p className="faded centered-message">No friends.</p>
        )}
        <div className="friends-container">
          {friends.length > 0 &&
            friends.map((friend) => {
              return (
                <FriendContainer
                  key={friend.id}
                  friendRequest={friend}
                  pending={false}
                  reset={reset}
                />
              );
            })}
        </div>
        <h1 className="title">Pending friend requests</h1>
        {friendRequest.length === 0 && (
          <p className="faded centered-message">No friend requests.</p>
        )}
        <div className="friends-container">
          {friendRequest.length > 0 &&
            friendRequest.map((friend) => {
              return (
                <FriendContainer
                  key={friend.id}
                  friendRequest={friend}
                  pending={true}
                  reset={reset}
                />
              );
            })}
        </div>

        <div className="form-pair">
          <h1 className="title">Add Friend</h1>
          <input
            placeholder="New friend ID"
            className="input-dark input-add"
            onChange={idChangeHandler}
          ></input>
        </div>
        <SendButton
          className={"add-button"}
          text={"SEND"}
          isLoading={isLoading}
          onClick={addFriendHandler}
          type={"button"}
        />
      </>
    </Container>
  );
};

export default FriendsList;
