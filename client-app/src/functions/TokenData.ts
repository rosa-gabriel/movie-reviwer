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

    throw "";
  } catch (ex: any) {
    throw new Error("Failed to login");
  }
};

export const logOut = () => {
  localStorage.removeItem("token");
};

export const register= async (userInfo: UserRegisterType): Promise<UserInfoType> => {
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

    throw "";
  } catch (ex: any) {
    throw new Error("Failed to register");
  }
};