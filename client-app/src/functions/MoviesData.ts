import { uri } from "../App";

const connectionFailString: string =
  "Failed to connect to the database! Try again later.";

export const getMovies = async () => {
  try {
    const response = await fetch(uri + "/Movies/", { method: "GET" });
    const data = await response.json();
    return data;
  } catch (ex) {
    throw connectionFailString;
  }
};

export const addMovie = async (movie: any) => {
  try {
      const response: Response  = await fetch(uri + "/Movies", {
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

export const getMovie = async (id: string | number) => {
  try {
    const response = await fetch(uri + "/Movies/" + id);
    const data = await response.json();
    return data;
  } catch (ex) {
    throw connectionFailString;
  }
};
