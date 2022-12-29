import { Tag, TagInfo } from "../../types/Types";

export const TagInfoToTag = (info: TagInfo): Tag => {
  return {
    id: info.tagId,
    name: info.name,
  };
};

export const IdToGender = (id: number): string => {
  switch (id) {
    case 0:
      return "Unknown";
    case 1:
      return "Male";
    case 2:
      return "Female";
    case 3:
      return "Other";
    default:
      throw new Error("Gender id not found!");
  }
};
