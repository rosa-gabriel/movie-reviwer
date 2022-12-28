import { useContext, useEffect, useState } from "react";
import AccountCheck from "../../components/account/AccountCheck";
import Container from "../../components/UI/Container";
import FriendContainer from "../../components/UI/FriendContainer";
import { UserContext } from "../../contexts/UserContext";
import { getUserFriends } from "../../functions/requests/AccouontRequests";
import { Friend } from "../../types/Types";

const FriendsList = () => {
  const [friends, setFriends] = useState<Friend[]>([]);
  const [friendRequest, setFriendRequest] = useState<Friend[]>([]);
  const [seed, setSeed] = useState<number>(1);

  const reset = () => {
    setSeed(Math.random);
  };

  const context = useContext(UserContext);

  useEffect(() => {
    (async () => {
      try {
        const response = await getUserFriends(String(context.userInfo?.token));
        setFriends(response.friends);
        setFriendRequest(response.friendRequests);
      } catch (ex: any) {}
    })();
  }, [seed, context.userInfo?.token]);

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
      </>
    </Container>
  );
};

export default FriendsList;
