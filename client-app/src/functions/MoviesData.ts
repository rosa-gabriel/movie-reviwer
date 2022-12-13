import { uri } from "../App";
import {
  AllMovieInfoType,
  CastEntryType,
  CastType,
  MoviePageType,
  MovieType,
  PersonType,
  ProfileType,
  TagEntriesType,
  TagType,
} from "../Type/Types";

const connectionFailString: string =
  "Failed to connect to the database! Try again later.";

const getRequest = async (url: string, token?: string) => {
  let requestBody: any = {};

  requestBody.method = "GET";

  if (token) {
    requestBody.headers = {
      Authorization: "Bearer " + token,
      "Content-Type": "application/json",
    };
  }

  let response: any;
  try {
    response = await fetch(url, requestBody);

    if (!response.ok) {
      throw response.status;
    }

    if (response.json) {
      return response.json();
    }
  } catch (ex: any) {
    if (response.status == 404) {
      throw connectionFailString;
    }
    throw ex.message;
  }
};

export const getMoviesAtPage = async (page: number) => {
  try {
    const response: MoviePageType = await getRequest(`${uri}/Movies/${page}`);
    return response;
  } catch (ex) {
    throw ex;
  }
};

export const addMovie = async (movie: AllMovieInfoType, token: string) => {
  try {
    const response: Response = await fetch(`${uri}/Create/movie`, {
      method: "POST",
      headers: {
        Authorization: "Bearer " + token,
        "Content-Type": "application/json",
      },
      body: JSON.stringify(movie),
    });
    switch (response.status) {
      case 500:
        throw new Error("Invalid name or url! Try again.");
    }
  } catch (ex: any) {
    if (ex.message === "Failed to fetch") {
      throw new Error(connectionFailString);
    }
    throw ex;
  }
};

export const updateMovie = async (movie: AllMovieInfoType, token: string) => {
  try {
    const response: Response = await fetch(`${uri}/Update/movie`, {
      method: "PUT",
      headers: {
        Authorization: "Bearer " + token,
        "Content-Type": "application/json",
      },
      body: JSON.stringify(movie),
    });
    switch (response.status) {
      case 500:
        throw "";
        break;
    }
  } catch (ex: any) {
    if (ex.message === "Failed to fetch") {
      throw new Error(connectionFailString);
    }
    throw ex;
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
    throw ex;
  }
};

export const addTag = async (tag: string, token: string) => {
  try {
    const response: Response = await fetch(`${uri}/Create/tag`, {
      headers: {
        Authorization: "Bearer " + token,
        "Content-Type": "application/json",
      },
      method: "POST",
      body: JSON.stringify({ Name: tag }),
    });

    switch (response.status) {
      case 500:
        throw new Error("Invalid name or url! Try again.");
    }

    const data = await response.json();
    return await data;
  } catch (ex: any) {
    if (ex.message === "Failed to fetch") {
      throw new Error(connectionFailString);
    }
    throw ex;
  }
};

export const addPerson = async (person: PersonType, token: string) => {
  try {
    const response: Response = await fetch(`${uri}/Create/person`, {
      headers: {
        Authorization: "Bearer " + token,
        "Content-Type": "application/json",
      },
      method: "POST",
      body: JSON.stringify(person),
    });
    switch (response.status) {
      case 500:
        throw new Error("Invalid name or url! Try again.");
    }

    const data = await response.json();
    return await data;
  } catch (ex: any) {
    if (ex.message === "Failed to fetch") {
      throw new Error(connectionFailString);
    }
    throw ex;
  }
};

export const getMovie = async (id: string) => {
  try {
    const response: AllMovieInfoType = await getRequest(`${uri}/Movie/${id}`);
    return response;
  } catch (ex) {
    throw ex;
  }
};

export const getMoviesFromTagAtPage = async (page: number, id: string) => {
  try {
    const response: MoviePageType = await getRequest(
      `${uri}/Tag/${id}/movies/${page}`
    );
    return response;
  } catch (ex) {
    throw ex;
  }
};

export const getMoviesFromPersonAtPage = async (page: number, id: string) => {
  try {
    const response: MoviePageType = await getRequest(
      `${uri}/Person/${id}/movies/${page}`
    );
    return response;
  } catch (ex) {
    throw connectionFailString;
  }
};

export const getMoviesFromSearchAtPage = async (
  page: number,
  search: string
) => {
  console.log(search);
  try {
    const response: Response = await fetch(
      `${uri}/Movies/search/${search}/${page}`
    );
    const data: MoviePageType = await response.json();
    return data;
  } catch (ex) {
    throw connectionFailString;
  }
};

export const getPerson = async (id: string) => {
  try {
    const response: Response = await fetch(`${uri}/Person/${id}`);
    const data: PersonType = await response.json();

    if (!response.ok) throw "";

    return data;
  } catch (ex) {
    throw connectionFailString;
  }
};
export const getTag = async (id: string) => {
  try {
    const response: Response = await fetch(`${uri}/Tag/${id}`);
    const data: TagType = await response.json();

    if (!response.ok) throw "";

    return data;
  } catch (ex) {
    throw connectionFailString;
  }
};

export const getTags = async () => {
  try {
    const response: Response = await fetch(`${uri}/Tags`, {
      method: "GET",
    });
    const data: TagEntriesType[] = await response.json();

    if (!response.ok) throw "";

    return data;
  } catch (ex) {
    throw new Error(connectionFailString);
  }
};

export const getMissingTags = async (id: string) => {
  try {
    const response: Response = await fetch(`${uri}/Movies/${id}/missingTags`, {
      method: "GET",
    });
    const data: TagEntriesType[] = await response.json();

    if (!response.ok) throw "";

    return data;
  } catch (ex) {
    throw new Error(connectionFailString);
  }
};

export const deleteMovie = async (movieId: string, token: string) => {
  try {
    const response: Response = await fetch(`${uri}/Movies/${movieId}`, {
      method: "DELETE",
      headers: {
        Authorization: "Bearer " + token,
        "Content-Type": "application/json",
      },
    });

    if (!response.ok) throw "";
  } catch (ex) {
    throw new Error(connectionFailString);
  }
};

export const getCast = async () => {
  try {
    const response: Response = await fetch(`${uri}/Cast`, {
      method: "GET",
    });
    const data: CastType[] = await response.json();

    if (!response.ok) throw "";

    return data;
  } catch (ex) {
    throw new Error(connectionFailString);
  }
};

export const putFavorite = async (
  movieId: string,
  desiredBool: boolean,
  token: string
) => {
  try {
    const response: Response = await fetch(`${uri}/Account/favorite`, {
      headers: {
        Authorization: "Bearer " + token,
        "Content-Type": "application/json",
      },
      method: "POST",
      body: JSON.stringify({ movieId, desiredBool }),
    });

    if (!response.ok) throw "";
  } catch (ex) {
    throw new Error(connectionFailString);
  }
};

export const getIsFavorite = async (movieId: string, token: string) => {
  try {
    const response: Response = await fetch(
      `${uri}/Account/favorite/${movieId}`,
      {
        method: "GET",
        headers: {
          Authorization: "Bearer " + token,
        },
      }
    );
    if (!response.ok) throw "";

    const data = await response.json();
    return data;
  } catch (ex) {
    console.error(ex);
    throw new Error(connectionFailString);
  }
};

export const getUserFavorites = async (page: number, token: string) => {
  try {
    const response: Response = await fetch(`${uri}/Account/favorites`, {
      method: "GET",
      headers: {
        Authorization: "Bearer " + token,
      },
    });
    if (!response.ok) throw "";
    const data = await response.json();
    return { movies: data, page: 1 };
  } catch (ex) {
    console.error(ex);
    throw new Error(connectionFailString);
  }
};

export const getProfile = async (userId: string, token?: string) => {
  try {
    const response: Response = await fetch(`${uri}/Account/profile/${userId}`, {
      method: "GET",
      headers: {
        Authorization: "Bearer " + String(token),
      },
    });

    if (!response.ok) throw "";

    let data: ProfileType = await response.json();
    data = { ...data, creationDate: new Date(data.creationDate) };
    return data;
  } catch (ex) {
    console.error(ex);
    throw new Error(connectionFailString);
  }
};
