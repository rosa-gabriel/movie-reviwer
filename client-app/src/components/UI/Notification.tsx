import { useEffect, useState } from "react";
import { MessageType } from "../../types/Types";

type NotificationProps = {
  message: MessageType;
  close: boolean;
};

const Notification = ({ message, close }: NotificationProps) => {
  //States
  const [hide, setHide] = useState<boolean>(false);

  //Class variables
  const closed = hide ? "closed " : "";
  const color = message.error ? "error-message" : "notification";

  //Button Handler
  const closeHandler = () => {
    setHide(true);
  };

  //Effect
  useEffect(() => {
    setHide(close);
  }, [close]);

  return (
    <>
      <div
        onClick={closeHandler}
        className={"notification-container " + closed + color}
      >
        <h1>{message.code}</h1>
        <p>{message.text}</p>
      </div>
    </>
  );
};

export default Notification;
