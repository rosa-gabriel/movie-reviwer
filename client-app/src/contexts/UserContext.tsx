import { useEffect, useState } from "react";
import { createContext } from "react";
import { useNavigate } from "react-router";
import { checkUser, logOut } from "../functions/requests/AccouontRequests";
import { UserInfoContext } from "../types/Types";

type UserContextType = {
  isLogedIn: boolean;
  userInfo: null | UserInfoContext;
  logIn(newUser: UserInfoContext): any;
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
  const [userInfo, setUserInfo] = useState<UserInfoContext | null>(() => {
    const localUserInfoString = localStorage.getItem("token");
    if (localUserInfoString) {
      let parsedInfo: UserInfoContext | null = JSON.parse(localUserInfoString);
      return parsedInfo;
    }
    return null;
  });
  const [isLoged, setIsLoged] = useState<boolean>(true);

  const navigate = useNavigate();

  //Effect
  useEffect(() => {
    (async () => {
      const localUserInfoString: string | null = localStorage.getItem("token");
      if (localUserInfoString) {
        let parsedInfo: UserInfoContext | null =
          JSON.parse(localUserInfoString);
        try {
          const response: boolean = await checkUser(String(parsedInfo?.token));
          if (response) {
            setUserInfo(parsedInfo);
            setIsLoged(true);
          } else setIsLoged(false);
        } catch (ex: any) {
          if (ex.message === "Connection") {
            setUserInfo(parsedInfo);
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
        logIn: (newUser: UserInfoContext) => {
          if (!!newUser) {
            localStorage.setItem("token", JSON.stringify(newUser));
            setUserInfo(newUser);
            setIsLoged(true);
          }
        },
        logOut: () => {
          logOut();
          setUserInfo(null);
          setIsLoged(false);
          navigate("/");
        },
      }}
    >
      {props.children}
    </UserContext.Provider>
  );
};
