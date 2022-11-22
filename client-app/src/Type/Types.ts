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

export type TagEntriesType = {
  tagId: number;
  name: string;
  entries: number;
};

export type TagType = {
  id: number;
  name: string;
};

export type CastType = {
  personId?: number;
  name: string;
  role: string;
};

export type PersonType = {
  id?: number,
  name: string,
  profileImageUrl: string
}

export type AllMovieInfoType = {
  movie: MovieType;
  favorites: number;
  tags: TagEntriesType[];
  castMembers: CastType[];
};
