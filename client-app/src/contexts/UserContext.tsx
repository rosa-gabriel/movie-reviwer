import { useEffect, useState } from "react";
import { createContext } from "react";
import { useLocation } from "react-router";
import { useNavigate } from "react-router";
import IsConfirmedCheck from "../components/account/IsConfirmedCheck";
import LoadingUserInfo from "../components/UI/LoadingUserInfo";
import { checkUser, logOut } from "../functions/requests/AccountRequests";
import { UserInfoResponse } from "../types/Types";

type UserContextType = {
  isLogedIn: boolean;
  userInfo: null | UserInfoResponse;
  logIn(newUser: UserInfoResponse): any;
  logOut(): any;
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
  //States
  const [userInfo, setUserInfoResponse] = useState<UserInfoResponse | null>(
    null
  );
  const [isLoged, setIsLoged] = useState<boolean>(() => {
    const localUserInfoResponseString: string | null =
      localStorage.getItem("token");
    return !!localUserInfoResponseString;
  });

  const navigate = useNavigate();
  const location = useLocation();

  const [error, setError] = useState<boolean>(false);

  //Effect
  useEffect(() => {
    (async () => {
      const localUserInfoResponseString: string | null =
        localStorage.getItem("token");
      if (localUserInfoResponseString) {
        setIsLoged(true);
        let parsedInfo: string = JSON.parse(localUserInfoResponseString);
        try {
          const response: UserInfoResponse = await checkUser(
            String(parsedInfo)
          );
          setUserInfoResponse(response);
          setError(false);
        } catch (ex: any) {
          if (ex.message === "Connection") {
            setError(true);
          } else {
            setError(false);
            setIsLoged(false);
            localStorage.removeItem("token");
          }
        }
      } else setIsLoged(false);
    })();
  }, []);

  console.log(userInfo);

  return (
    <UserContext.Provider
      value={{
        isLogedIn: isLoged,
        userInfo: userInfo,
        logIn: (newUser: UserInfoResponse) => {
          if (!!newUser) {
            localStorage.setItem("token", JSON.stringify(newUser.token));
            setUserInfoResponse(newUser);
            setIsLoged(true);
          }
        },
        logOut: () => {
          logOut();
          setUserInfoResponse(null);
          setIsLoged(false);
          navigate("/");
        },
      }}
    >
      <>
        {(!isLoged || (isLoged && userInfo)) && (
          <>
            {location.pathname !== "/account/message/confirm" && (
              <IsConfirmedCheck />
            )}
            {props.children}
          </>
        )}
        {isLoged && !userInfo && <LoadingUserInfo error={error} />}
      </>
    </UserContext.Provider>
  );
};
