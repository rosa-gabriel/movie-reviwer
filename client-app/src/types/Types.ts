export type NewMovieInfo = {
  id?: string;
  name: string;
  coverUrl: string;
  releaseDate: Date;
  tags: Tag[];
  castMembers: CastInfo[];
};

export type MovieResponse = {
  id: string;
  name: string;
  coverUrl: string;
  releaseDate: Date;
  comments: CommentType[];
};

export type TagInfo = {
  tagId: string;
  name: string;
  entries: number;
};

export type Tag = {
  id: string;
  name: string;
};

export type NewTag = {
  name: string;
};

export type NewPerson = {
  name: string;
  profileImageUrl: string;
  birthday: Date;
  biography: string;
  gender: number;
};

export type Cast = {
  id: string;
  name: string;
  role: string;
};

export type CastInfo = {
  personId: string;
  name: string;
  role: string;
};

export type PersonResponse = {
  id: string;
  name: string;
  profileImageUrl: string;
  birthday: Date;
  biography: string;
  gender: number;
};

export type AllMovieInfoType = {
  id: string;
  name: string;
  coverUrl: string;
  releaseDate: Date;
  favorites: number;
  tags: TagInfo[];
  castMembers: CastInfo[];
};

//Account types
export type UserLogin = {
  login: string;
  password: string;
};

export type UserInfoContext = {
  id: string;
  email: string;
  token: string;
  username: string;
  profileImageUrl: string;
  isAdmin: boolean;
  confirmed: boolean;
};

export type UserRegister = {
  password: string;
  username: string;
  email: string;
};

export type ProfileInfo = {
  id: string;
  imageUrl: string;
  name: string;
  creationDate: Date;
  recentFavorites: MovieResponse[];
  bio: string;
  isFriend: boolean;
  hasRequested: boolean;
};

export type MoviePage = {
  movies: MovieResponse[];
  count: number;
};

export type NotificationMessage = {
  code: string;
  text: string;
  error: boolean;
};

export type CommentType = {
  id: string;
  creator: CreatorType;
  message: string;
  postDate: Date;
  wasEdited: boolean;
};

export type CreatorType = {
  id: string;
  name: string;
  profileImageUrl: string;
};

export type UserSettings = {
  username: string;
  email: string;
  profileImageUrl: string;
  bio: string;
  newPassword?: string;
  oldPassword?: string;
};

export type NewFriend = {
  receiverId: string;
  senderId: string;
};

export type Friend = {
  id: string;
  friend: CreatorType;
  sent: boolean;
};
