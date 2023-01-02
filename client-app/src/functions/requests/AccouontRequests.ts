import { uri } from "../../App";
import { UserSettings, UserLogin, UserRegister } from "../../types/Types";
import {
  deleteRequest,
  getRequest,
  postRequest,
  putRequest,
} from "./CreateRequest";

export const login = async (userInfo: UserLogin) => {
  const response = await postRequest(`${uri}/Account/login`, userInfo);
  return response;
};

export const logOut = () => {
  localStorage.removeItem("token");
};

export const register = async (userInfo: UserRegister) => {
  const response = await postRequest(`${uri}/Account/register`, userInfo);

  localStorage.setItem("token", JSON.stringify(response));
  return response;
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
    const responseException = checkResponseException(ex);
    if (responseException) {
      throw new Error(responseException);
    }
    throw ex;
  }
};

export const getSettings = async (token: string) => {
  const response: UserSettings = await getRequest(
    `${uri}/Account/settings`,
    token
  );
  return response;
};

export const postFriendRequest = async (userId: string, token: string) => {
  const response: UserSettings = await postRequest(
    `${uri}/Account/friend/${userId}`,
    undefined,
    token
  );
  return response;
};

export const deleteFriend = async (friendId: string, token: string) => {
  const response: UserSettings = await deleteRequest(
    `${uri}/Account/remove/friend/${friendId}`,
    token
  );
  return response;
};

export const putFriendRequest = async (friendId: string, token: string) => {
  const response: UserSettings = await putRequest(
    `${uri}/Account/confirm/friend/${friendId}`,
    undefined,
    token
  );
  return response;
};

export const confirmFriendRequest = async (userId: string, token: string) => {
  const response: UserSettings = await postRequest(
    `${uri}/Account/friend/${userId}`,
    undefined,
    token
  );
  return response;
};

export const getUserFriends = async (token: string) => {
  const response = await getRequest(`${uri}/Account/friends`, token);
  return response;
};

export const changeSettings = async (
  newSettings: UserSettings,
  token: string
) => {
  const response = await putRequest(
    `${uri}/Account/settings/update`,
    newSettings,
    token
  );
  return response;
};

const checkResponseException = (ex: Error): string | null => {
  if ("Failed to fetch") {
    return "Connection";
  }
  return null;
};
