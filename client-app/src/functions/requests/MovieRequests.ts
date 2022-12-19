import { uri } from "../../App";
import {
  AllMovieInfoType,
  CastType,
  MoviePageType,
  PersonType,
  ProfileType,
  TagEntriesType,
  TagType,
} from "../../types/Types";
import { connectionFailString, getRequest, postRequest } from "./CreateRequest";

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
    const response = await postRequest(`${uri}/Create/movie`, movie, token);

    return response.json();
  } catch (ex: any) {
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
        throw new Error("Error updating movie!");
    }
  } catch (ex: any) {
    if (ex.message === "Failed to fetch") {
      throw new Error(connectionFailString);
    }
    throw ex;
  }
};

export const addTag = async (tag: string, token: string) => {
  try {
    const response = await postRequest(`${uri}/Create/tag`, tag, token);
    return response;
  } catch (ex: any) {
    if (ex.message === "500") throw new Error("Invalid name! Try again.");
    throw ex;
  }
};

export const addPerson = async (person: PersonType, token: string) => {
  try {
    const response: any = await postRequest(
      `${uri}/Create/person`,
      person,
      token
    );
    return response;
  } catch (ex: any) {
    if (ex.message === "500") {
      throw new Error("Invalid name or url! Try again.");
    }
    throw ex;
  }
};

export const getMovie = async (id: string) => {
  try {
    const response: AllMovieInfoType = await getRequest(`${uri}/Movie/${id}`);
    return response;
  } catch (ex) {
    console.error(ex);
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
  try {
    const response: MoviePageType = await getRequest(
      `${uri}/Movies/search/${search}/${page}`
    );
    return response;
  } catch (ex) {
    throw ex;
  }
};

export const getPerson = async (id: string) => {
  try {
    const response: PersonType = await getRequest(`${uri}/Person/${id}`);
    return response;
  } catch (ex: any) {
    throw ex;
  }
};

export const getTag = async (id: string) => {
  try {
    const response: TagType = await getRequest(`${uri}/Tag/${id}`);
    return response;
  } catch (ex) {
    throw ex;
  }
};

export const getTags = async () => {
  try {
    const response: TagEntriesType[] = await getRequest(`${uri}/Tags`);
    return response;
  } catch (ex) {
    throw ex;
  }
};

export const getMissingTags = async (id: string) => {
  try {
    const response: TagEntriesType[] = await getRequest(
      `${uri}/Movies/${id}/missingTags`
    );
    return response;
  } catch (ex) {
    throw ex;
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

    if (!response.ok) throw new Error("");
  } catch (ex) {
    throw new Error(connectionFailString);
  }
};

export const getCast = async () => {
  try {
    const response: CastType[] = await getRequest(`${uri}/Cast`);
    return response;
  } catch (ex) {
    throw ex;
  }
};

export const addFavorite = async (
  movieId: string,
  desiredBool: boolean,
  token: string
) => {
  try {
    const response = await postRequest(
      `${uri}/Account/favorite`,
      { movieId, desiredBool },
      token
    );
    return response;
  } catch (ex) {
    throw ex;
  }
};

export const getIsFavorite = async (movieId: string, token: string) => {
  try {
    const response = await getRequest(
      `${uri}/Account/favorite/${movieId}`,
      token
    );
    return response;
  } catch (ex) {
    throw ex;
  }
};

export const getUserFavorites = async (page: number, token: string) => {
  try {
    const response: Response = await getRequest(
      `${uri}/Account/favorites/${page}`,
      token
    );
    return response;
  } catch (ex) {
    throw ex;
  }
};

export const getProfile = async (userId: string, token?: string) => {
  try {
    let response: ProfileType = await getRequest(
      `${uri}/Account/profile/${userId}`,
      token
    );
    response = { ...response, creationDate: new Date(response.creationDate) };
    return response;
  } catch (ex) {
    throw ex;
  }
};
