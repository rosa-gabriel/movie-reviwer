import { uri } from "../App";
import {
  AllMovieInfoType,
  CastType,
  MovieType,
  PersonType,
  TagEntriesType,
  TagType,
} from "../Type/Types";

const connectionFailString: string =
  "Failed to connect to the database! Try again later.";

export const getMovies = async () => {
  try {
    const response: Response = await fetch(uri + "/Movies/", { method: "GET" });
    const data: MovieType[] = await response.json();
    return data;
  } catch (ex) {
    throw connectionFailString;
  }
};

export const addMovie = async (movie: AllMovieInfoType) => {
  console.log(movie);
  try {
    const response: Response = await fetch(uri + "/Movies", {
      method: "POST",
      headers: {
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

export const addTag = async (tag: string) => {
  try {
    const response: Response = await fetch(uri + "/Movies/tags", {
      headers: {
        "Content-Type": "application/json",
      },
      method: "POST",
      body: JSON.stringify({ Name: tag }),
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

export const addPerson = async (person: PersonType) => {
  try {
    const response: Response = await fetch(uri + "/Movies/person", {
      headers: {
        "Content-Type": "application/json",
      },
      method: "POST",
      body: JSON.stringify(person),
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

export const getMovie = async (id: string | number) => {
  try {
    const response: Response = await fetch(uri + "/Movies/" + id);
    const data: AllMovieInfoType = await response.json();
    return data;
  } catch (ex) {
    throw connectionFailString;
  }
};

export const getMoviesFromTag = async (id: string | number) => {
  try {
    const response: Response = await fetch(uri + "/Movies/from/tag/" + id);
    const data: TagType[] = await response.json();
    return data;
  } catch (ex) {
    throw connectionFailString;
  }
};

export const getPerson = async (id: string | number) => {
  try {
    const response: Response = await fetch(uri + "/Movies/person/" + id);
    const data: PersonType = await response.json();

    if (!response.ok) throw "";

    return data;
  } catch (ex) {
    throw connectionFailString;
  }
};
export const getTag = async (id: string | number) => {
  try {
    const response: Response = await fetch(uri + "/Movies/tag/" + id);
    const data: TagType = await response.json();

    if (!response.ok) throw "";

    return data;
  } catch (ex) {
    throw connectionFailString;
  }
};

export const getTags = async () => {
  try {
    const response: Response = await fetch(uri + "/Movies/tags", {
      method: "GET",
    });
    const data: TagEntriesType[] = await response.json();

    if (!response.ok) throw "";

    return data;
  } catch (ex) {
    throw new Error(connectionFailString);
  }
};

export const getCast = async () => {
  try {
    const response: Response = await fetch(uri + "/Movies/cast", {
      method: "GET",
    });
    const data: CastType[] = await response.json();

    if (!response.ok) throw "";

    return data;
  } catch (ex) {
    throw new Error(connectionFailString);
  }
};
