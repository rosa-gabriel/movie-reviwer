import { useEffect, useState } from "react";
import { createContext } from "react";
import { checkUser } from "../../functions/MoviesData";
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
  isLogedIn: true,
  userInfo: null,
  logIn: () => {},
  logOut: () => {},
});

type UserContextProviderProps = {
  children: JSX.Element;
};

export const UserContextProvider = (props: UserContextProviderProps) => {
  const [userInfo, setUserInfo] = useState<UserInfoType | null>(null);
  const [isLoged, setIsLoged] = useState<boolean>(true);

  useEffect(() => {
    (async () => {
      const localUserInfoString: string | null = localStorage.getItem("token");

      if (localUserInfoString) {
        try {
          const parsedInfo = JSON.parse(localUserInfoString);

          const response: boolean = await checkUser(String(parsedInfo.token));

          if (response) {
            setUserInfo(parsedInfo);
            setIsLoged(true);
          } else setIsLoged(false);
        } catch (ex: any) {
          setIsLoged(false);
          localStorage.removeItem("token");
        }
      } else setIsLoged(false);
    })();
  }, []);

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
          logOut();
          setUserInfo(null);
          setIsLoged(false);
        },
      }}
    >
      {props.children}
    </UserContext.Provider>
  );
};
