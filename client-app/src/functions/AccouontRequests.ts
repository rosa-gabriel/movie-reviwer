import { uri } from "../App";
import { UserInfoType, UserLoginType, UserRegisterType } from "../Type/Types";

export const login = async (userInfo: UserLoginType): Promise<UserInfoType> => {
  try {
    const response: Response = await fetch(`${uri}/Account/login`, {
      method: "POST",
      headers: {
        "Content-Type": "application/json",
      },
      body: JSON.stringify(userInfo),
    });

    if (response.ok) {
      const data = await response.json();
      localStorage.setItem("token", JSON.stringify(data));
      return data;
    }

    throw new Error("");
  } catch (ex: any) {
    throw new Error("Failed to login");
  }
};

export const logOut = () => {
  localStorage.removeItem("token");
};

export const register = async (
  userInfo: UserRegisterType
): Promise<UserInfoType> => {
  try {
    const response: Response = await fetch(`${uri}/Account/register`, {
      method: "POST",
      headers: {
        "Content-Type": "application/json",
      },
      body: JSON.stringify(userInfo),
    });

    if (response.ok) {
      const data = await response.json();
      localStorage.setItem("token", JSON.stringify(data));
      return data;
    }

    throw new Error("");
  } catch (ex: any) {
    throw new Error("Failed to register");
  }
};

export const checkUser = async (token: string): Promise<boolean> => {
  try {
    const response = await fetch(`${uri}/Account/check`, {
      method: "GET",
      headers: {
        Authorization: "Bearer " + token,
        "Content-Type": "application/json",
      },
    });
    return response.ok;
  } catch (ex: any) {
    const res = checkResponseException(ex);
    if (res) {
      throw new Error(res);
    }
    throw ex;
  }
};

const checkResponseException = (ex: Error): string | null => {
  if ("Failed to fetch") {
    return "Connection";
  }
  return null;
};
