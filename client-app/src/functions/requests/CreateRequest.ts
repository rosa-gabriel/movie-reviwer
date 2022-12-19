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
      break;
    case "PUT":
      break;
    case "DELETE":
      break;
  }

  try {
    const response = await fetch(url, requestBody);
    if (!response.ok) {
      if (response.status === 401)
        throw new Error(
          "Current user doesn't have authorization to access this page."
        );
      throw new Error(String(response.status));
    }
    try {
      return await response.json();
    } catch (ex) {}
  } catch (ex: any) {
    console.error(ex);
    if (ex.message === "Failed to fetch") throw new Error(connectionFailString);
    throw ex;
  }
};

export const getRequest = async (url: string, token?: string) => {
  try {
    return await createRequest(url, "GET", undefined, token);
  } catch (ex: any) {
    throw ex;
  }
};

export const postRequest = async (url: string, body: any, token?: string) => {
  try {
    return createRequest(url, "POST", body, token);
  } catch (ex: any) {
    throw ex;
  }
};
