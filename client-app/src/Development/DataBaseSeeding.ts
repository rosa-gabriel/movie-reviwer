import {
  addMovie,
  addPerson,
  addTag,
} from "../functions/requests/MovieRequests";
import { AllMovieInfoType } from "../types/Types";

export const seedTags = async () => {
  const response = await fetch(
    "https://api.themoviedb.org/3/genre/movie/list?api_key=c70c967bbc4fbb88330e678847fac7aa&language=en-US"
  );
  const data = await response.json();
  const tagJson: any[] = [];
  data.genres.forEach(async (tag: any) => {
    const tagData = await addTag(tag.name, "sus");
    tagData.imdbId = tag.id;
    tagJson.push(tagData);
  });
  return tagJson;
};

export const seedCast = async (movieId: number, savesCast: any[]) => {
  const response = await fetch(
    "https://api.themoviedb.org/3/movie/" +
      movieId +
      "/credits?api_key=c70c967bbc4fbb88330e678847fac7aa&language=en-US"
  );
  const data = await response.json();
  let allCast = [...savesCast];
  const castJson: any[] = [];
  data.cast.forEach(async (person: any) => {
    const test = allCast.find((cj) => cj.name == person.name);
    if (!test) {
      const castData = await addPerson(
        {
          name: person.name,
          profileImageUrl: `https://image.tmdb.org/t/p/w500${person.profile_path}`,
        },
        "sus"
      );
      castData.imdbId = person.id;
      castData.role = person.character;
      castJson.push(castData);
      allCast.push(castData);
    }
  });

  return castJson;
};

export const seedMovies = async (tagJson: any) => {
  let data: any = { results: [] };
  for (let i = 1; i <= 2; i++) {
    const response = await fetch(
      "https://api.themoviedb.org/3/movie/popular?api_key=c70c967bbc4fbb88330e678847fac7aa&language=en-US&page=" +
        i
    );
    const bufferData = await response.json();
    data = { results: [...data.results, ...bufferData.results] };
  }
  let allCast: any[] = [];
  data.results.forEach(async (result: any) => {
    const tags: any = [];
    result.genre_ids.forEach(async (tag: number) => {
      const newTag = tagJson.find((ts: any) => ts.imdbId === tag);
      tags.push({
        tagId: newTag.id,
        name: newTag.name,
      });
    });

    const castMembers = await seedCast(result.id, allCast);
    console.log(castMembers);
    allCast = [...allCast, ...castMembers];
    setTimeout(async () => {
      const newMovie: AllMovieInfoType = {
        movie: {
          id: "",
          name: result.original_title,
          coverUrl: `https://image.tmdb.org/t/p/w500${result.poster_path}`,
          releaseDate: result.release_date,
        },
        favorites: 0,
        tags: tags,
        castMembers: castMembers,
      };

      await addMovie(newMovie, "sus");
    }, 10000);
  });
};

const addingMoviesInfo = () => {};
