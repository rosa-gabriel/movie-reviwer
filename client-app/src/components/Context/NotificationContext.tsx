import { useState } from "react";
import { createContext } from "react";
import { MessageType } from "../../Type/Types";
import Modal from "../UI/Modal";
import Notification from "../UI/Notification";

type NotificationContextType = {
  addNotification: any;
  removeNotification: any;
};

export const NotificationContext = createContext<NotificationContextType>({
  addNotification: (message: MessageType) => {},
  removeNotification: () => {},
});

type NotificationContextProviderProps = {
  children: JSX.Element;
};

export const NotificationContextProvider = (
  props: NotificationContextProviderProps
) => {
  const [notification, setNotification] = useState<MessageType>();
  const [hide, setHide] = useState<boolean>(false);
  const [showNotification, setShowNotification] = useState<boolean>(false);

  const remove = () => {
    setHide(true);
    setTimeout(() => {
      setShowNotification(false);
      setNotification(undefined);
      setHide(false);
    }, 1000);
  };

  return (
    <NotificationContext.Provider
      value={{
        addNotification: (message: MessageType) => {
          setNotification(message);
          setShowNotification(true);
          setTimeout(remove, 2000);
        },
        removeNotification: () => {
          remove();
        },
      }}
    >
      {showNotification && notification && (
        <Notification message={notification} close={hide} />
      )}

      {props.children}
    </NotificationContext.Provider>
  );
};
