export const connectionFailString: string =
  "Failed to connect to the database! Try again later.";

const createRequest = async (
  url: string,
  method: string,
  body?: any,
  token?: string
) => {
  let requestBody: any = {};

  requestBody.method = method;

  if (token) {
    requestBody.headers = {
      Authorization: "Bearer " + token,
      "Content-Type": "application/json",
    };
  }

  switch (method) {
    case "GET":
      break;
    case "POST":
      requestBody.body = JSON.stringify(body);
      requestBody.headers = {
        ...requestBody.headers,
        Accept: "application/json, text/plain",
        "Content-Type": "application/json;charset=UTF-8",
      };
      break;
    case "PUT":
      requestBody.body = JSON.stringify(body);
      break;
    case "DELETE":
      requestBody.body = JSON.stringify(body);
      break;
  }

  try {
    const response = await fetch(url, requestBody);
    if (!response.ok) {
      switch (response.status) {
        case 401:
          throw new Error(
            "Current user doesn't have authorization to access this page."
          );
        case 400:
          let errorText = "Bad request";

          try {
            errorText = await response.text();
          } catch (ex: any) {}

          try {
            const json = await response.json();
            errorText = json.error[0];
          } catch (ex: any) {}

          throw new Error(errorText);
        case 500:
          throw new Error("Internal server error!");
        default:
          throw new Error(String(response.status));
      }
    }

    try {
      return await response.json();
    } catch (ex) {}
  } catch (ex: any) {
    if (ex.message === "Failed to fetch") throw new Error(connectionFailString);
    throw ex;
  }
};

export const getRequest = async (url: string, token?: string) => {
  return await createRequest(url, "GET", undefined, token);
};

export const postRequest = async (url: string, body: any, token?: string) => {
  return createRequest(url, "POST", body, token);
};

export const putRequest = async (url: string, body: any, token?: string) => {
  return createRequest(url, "PUT", body, token);
};

export const deleteRequest = async (url: string, body: any, token?: string) => {
  return createRequest(url, "DELETE", body, token);
};