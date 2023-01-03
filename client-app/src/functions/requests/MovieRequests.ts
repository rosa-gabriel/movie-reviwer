import { uri } from "../../App";
import {
  AllMovieInfoType,
  Cast,
  CommentType,
  MoviePage,
  NewMovieInfo,
  NewPerson,
  PersonResponse,
  ProfileInfo,
  Tag,
  TagInfo,
} from "../../types/Types";
import {
  deleteRequest,
  getRequest,
  postRequest,
  putRequest,
} from "./CreateRequest";

export const getMoviesAtPage = async (page: number) => {
  const response: MoviePage = await getRequest(`${uri}/Movies/page/${page}`);
  return response;
};

export const addMovie = async (movie: NewMovieInfo, token: string) => {
  const response = await postRequest(`${uri}/Movies/create`, movie, token);
  return response;
};

export const listMovieComments = async (movieId: string) => {
  const response: CommentType[] = await getRequest(
    `${uri}/Movies/${movieId}/comments`
  );
  const data = response.map((ct) => {
    return { ...ct, postDate: new Date(ct.postDate) };
  });
  return data;
};

export const addComment = async (
  message: string,
  movieId: string,
  token: string
) => {
  const response = await postRequest(
    `${uri}/Account/comment/${movieId}`,
    { message: message },
    token
  );
  return response;
};

export const updateMovie = async (movie: NewMovieInfo, token: string) => {
  const response: Response = await putRequest(
    `${uri}/Movies/update`,
    movie,
    token
  );
  return response;
};

export const addTag = async (tag: string, token: string) => {
  const response = await postRequest(
    `${uri}/Tags/create`,
    { name: tag },
    token
  );
  return response;
};

export const addPerson = async (person: NewPerson, token: string) => {
  const response: any = await postRequest(`${uri}/Cast/create`, person, token);
  return response;
};

export const getMovie = async (id: string) => {
  const response: AllMovieInfoType = await getRequest(`${uri}/Movies/${id}`);
  return response;
};

export const getMoviesFromTagAtPage = async (page: number, id: string) => {
  const response: MoviePage = await getRequest(
    `${uri}/Tags/${id}/movies/${page}`
  );
  return response;
};

export const getMoviesFromPersonAtPage = async (page: number, id: string) => {
  const response: MoviePage = await getRequest(
    `${uri}/Cast/${id}/movies/${page}`
  );
  return response;
};

export const getMoviesFromSearchAtPage = async (
  page: number,
  search: string
) => {
  const response: MoviePage = await getRequest(
    `${uri}/Movies/search/${search}/${page}`
  );
  return response;
};

export const getPerson = async (id: string) => {
  const response: PersonResponse = await getRequest(`${uri}/Cast/person/${id}`);
  response.birthday = new Date(response.birthday);
  return response;
};

export const getTag = async (id: string) => {
  const response: Tag = await getRequest(`${uri}/Tags/${id}`);
  return response;
};

export const getTags = async () => {
  const response: TagInfo[] = await getRequest(`${uri}/Tags`);
  return response;
};

export const deleteMovie = async (movieId: string, token: string) => {
  const response: Response = await deleteRequest(
    `${uri}/Movies/${movieId}`,
    undefined
  );
  return response;
};

export const deletePerson = async (personId: string, token: string) => {
  const response: Response = await deleteRequest(
    `${uri}/Cast/${personId}`,
    token
  );
  return response;
};

export const updatePerson = async (person: PersonResponse, token: string) => {
  const response: Response = await putRequest(
    `${uri}/Cast/update/`,
    person,
    token
  );
  return response;
};

export const deleteComment = async (commentId: string, token: string) => {
  const response: Response = await deleteRequest(
    `${uri}/Account/comment/${commentId}`,
    token
  );
  return response;
};

export const updateComment = async (comment: CommentType, token: string) => {
  const response: Response = await putRequest(
    `${uri}/Account/comment`,
    comment,
    token
  );
  return response;
};

export const getCast = async () => {
  const response: Cast[] = await getRequest(`${uri}/Cast`);
  return response;
};

export const addFavorite = async (
  movieId: string,
  desiredBool: boolean,
  token: string
) => {
  const response = await postRequest(
    `${uri}/Account/favorite`,
    { movieId, desiredBool },
    token
  );
  return response;
};

export const getIsFavorite = async (movieId: string, token: string) => {
  const response = await getRequest(
    `${uri}/Account/favorite/${movieId}`,
    token
  );
  return response;
};

export const getUserFavorites = async (
  page: number,
  userId: string,
  token: string
) => {
  const response: Response = await getRequest(
    `${uri}/Account/favorites/${userId}/${page}`,
    token
  );
  return response;
};

export const getProfile = async (userId: string, token?: string) => {
  let response: ProfileInfo = await getRequest(
    `${uri}/Account/profile/${userId}`,
    token
  );
  response = { ...response, creationDate: new Date(response.creationDate) };
  return response;
};
