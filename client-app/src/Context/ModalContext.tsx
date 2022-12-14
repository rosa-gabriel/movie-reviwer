import { useState } from "react";
import { createContext } from "react";
import Modal from "../components/UI/Modal";

type ModalContextType = {
  showModal: any;
};

export const ModalContext = createContext<ModalContextType>({
  showModal: () => {},
});

type ModalContextProviderProps = {
  children: JSX.Element;
};

export const ModalContextProvider = (props: ModalContextProviderProps) => {
  //States
  const [showModal, setShowModal] = useState<boolean>(false);
  const [message, setMessage] = useState<string>("");
  const [title, setTitle] = useState<string>("");
  const [confirmModal, setConfirmModal] = useState<any>();

  const confirmModalHandler = async () => {
    setShowModal(false);
    await confirmModal();
  };

  const cancelModalHandler = () => {
    setMessage("");
    setTitle("");
    setConfirmModal(() => {});
    setShowModal(false);
  };

  return (
    <ModalContext.Provider
      value={{
        showModal: (titleB: string, messageB: string, confirmB: any) => {
          console.log("modalSet");
          setMessage(messageB);
          setTitle(titleB);
          setConfirmModal(() => confirmB);
          setShowModal(true);
        },
      }}
    >
      {showModal && (
        <Modal
          title={title}
          text={message}
          onCancel={cancelModalHandler}
          onConfirm={confirmModalHandler}
        />
      )}
      {props.children}
    </ModalContext.Provider>
  );
};
