import { Tag, TagInfo } from "../../types/Types";

export const TagInfoToTag = (info: TagInfo): Tag => {
  return {
    id: info.tagId,
    name: info.name,
  };
};
