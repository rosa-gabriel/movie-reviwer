export type MovieType = {
  id?: number;
  name: string;
  coverUrl: string;
  releaseDate: Date;
};

export type NewMovieType = {
  name: string;
  coverUrl: string;
  releaseDate: string;
};

export type TagType = {
  tagId?: number;
  name: string;
  entries: number;
};

export type CastType = {
  castId?: number;
  name: string;
  role: string;
};

export type AllMovieInfoType = {
  movie: MovieType;
  favorites: number;
  tags: TagType[];
  castMembers: CastType[];
};
