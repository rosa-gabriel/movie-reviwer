import { useEffect, useState } from "react";
import { createContext } from "react";
import { logOut } from "../../functions/TokenData";
import { UserInfoType } from "../../Type/Types";
import RegisterForm from "../AccountForms/RegisterForm";
import Modal from "../UI/Modal";

type UserContextType = {
  isLogedIn: boolean;
  userInfo: null | UserInfoType;
  logIn: any;
  logOut: any;
};

export const UserContext = createContext<UserContextType>({
  isLogedIn: false,
  userInfo: null,
  logIn: () => {},
  logOut: () => {},
});

type UserContextProviderProps = {
  children: JSX.Element;
};

export const UserContextProvider = (props: UserContextProviderProps) => {
  const [userInfo, setUserInfo] = useState<UserInfoType | null>(null);
  const [isLoged, setIsLoged] = useState<boolean>(() => {
  const localUserInfoString: string | null = localStorage.getItem("token");
    if (localUserInfoString) {
      setUserInfo(JSON.parse(localUserInfoString));
      return true;
    }
    return false
  });
  const [showModal, setShowModal] = useState<boolean>(false);

  const cancelModalHandler = () => {
    setShowModal(false);
  };

  const confirmModalHandler = () => {
    setShowModal(false);
    logOut();
    setUserInfo(null);
    setIsLoged(false);
  };

  return (
    <UserContext.Provider
      value={{
        isLogedIn: isLoged,
        userInfo: userInfo,
        logIn: (newUser: UserInfoType) => {
          if (!!newUser) {
            setUserInfo(newUser);
            setIsLoged(true);
          }
        },
        logOut: () => {
          setShowModal(true);
        },
      }}
    >
      {showModal && (
        <Modal
          title={"Sining out!"}
          text={"Are you sure you want to leave your account?"}
          onCancel={cancelModalHandler}
          onConfirm={confirmModalHandler}
        />
      )}
      {props.children}
    </UserContext.Provider>
  );
};
