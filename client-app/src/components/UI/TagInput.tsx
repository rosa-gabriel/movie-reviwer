import FormTag from "./FormTag";
import { useState, useEffect } from "react";
import { getTags } from "../../functions/MoviesData";
import { TagType } from "../../Type/Types";

type TagInputProps = {
  onChange: Function;
};

const TagInput = (props: TagInputProps) => {
  const [dataTags, setDataTags] = useState<TagType[]>([]);
  const [tags, setTags] = useState<TagType[]>([]);
  const [tagText, setTagText] = useState<string>("");

  useEffect(() => {
    (async () => {
      const data: TagType[] = await getTags();
      setDataTags([...data]);
      setTags([]);
    })();
  }, []);
  const deleteTag = (item: TagType): void => {
    setTags((prevTags: TagType[]) => {
      const newTags = prevTags.filter((tag: TagType) => tag != item);
      return newTags;
    });
    setDataTags((prevTags) => [...prevTags, item]);
  };

  const addTag = (event: React.SyntheticEvent) => {
    event.preventDefault();
    if (tagText == "") return;

    setTags((prevTags: any) => {
      const tag = dataTags.find((element) => element.name == tagText);
      setDataTags((prevDataTags) => [
        ...prevDataTags.filter((dataTag) => dataTag != tag),
      ]);
      return [...prevTags, tag];
    });
  };

  useEffect(() => {
    props.onChange(tags);
  }, [tags]);

  const tagTextChangeHandler = (event: any): void => {
    setTagText(event.target.value);
  };

  return (
    <div style={{ minHeight: "36px" }} className="input-dark input-add tag-box">
      {tags.map((tag) => {
        return <FormTag key={tag.name} tag={tag} onDelete={deleteTag} />;
      })}
      <select name="select" onChange={tagTextChangeHandler}>
        <option value={""}>Choose a tag</option>
        {dataTags.map((item) => {
          return (
            <option key={item.name} value={item.name}>
              {item.name}
            </option>
          );
        })}
      </select>
      <button className="add-tag-button button" onClick={addTag}>
        +
      </button>
    </div>
  );
};
export default TagInput;
