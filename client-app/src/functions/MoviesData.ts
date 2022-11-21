import { uri } from "../App";
import { AllMovieInfoType, MovieType, TagType } from "../Type/Types";

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
      body: JSON.stringify({ Id: 0, Name: tag }),
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

export const getTags = async () => {
  try {
    const response: Response = await fetch(uri + "/Movies/tags", {
      method: "GET",
    });
    const data: TagType[] = await response.json();
    return data;
  } catch (ex) {
    throw connectionFailString;
  }
};
