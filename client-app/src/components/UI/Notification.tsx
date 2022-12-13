import { useEffect, useState } from "react";
import { MessageType } from "../../Type/Types";

type NotificationProps = {
  message: MessageType;
  close: boolean;
};

const Notification = ({ message, close }: NotificationProps) => {
  const [hide, setHide] = useState<boolean>(false);

  console.log(hide);

  const closed = hide ? "closed " : "";
  const color = message.error ? "error-message" : "notification";

  useEffect(() => {
    setHide(close);
  }, [close]);

  const closeHandler = () => {
    setHide(true);
  };

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
