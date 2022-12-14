import { useEffect, useState } from "react";
import { createContext } from "react";
import { checkUser, logOut } from "../functions/AccouontRequests";
import { UserInfoType } from "../Type/Types";

type UserContextType = {
  isLogedIn: boolean;
  userInfo: null | UserInfoType;
  logIn(newUser: UserInfoType): any;
  logOut(): any;
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
  //States
  const [userInfo, setUserInfo] = useState<UserInfoType | null>(null);
  const [isLoged, setIsLoged] = useState<boolean>(true);

  //Effect
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
          if (ex.message === "Connection") {
            setIsLoged(true);
          } else {
            setIsLoged(false);
            localStorage.removeItem("token");
          }
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
