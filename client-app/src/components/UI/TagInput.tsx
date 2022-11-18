import FormTag from "./FormTag";
import { useState } from "react";
import { Form } from "react-router-dom";

type teste = {
  tagId: number;
  name: string;
};

const init: teste[] = [
  {
    tagId: 10,
    name: 'sus'
  }
];

const TagInput = (props: any) => {
  const [dataTags, setDataTags] = useState(init);
  const [tags, setTags] = useState(init);
  const [tagText, setTagText] = useState("");

  const deleteTag = (item: any) => {
    setTags((prevTags: any) => {
      const newTags = prevTags.filter((tag: any) => tag != item);
      return newTags;
    });
  };

  const addTag = (event: any) => {
    event.preventDefault();

    setTags((prevTags: any) => {
      return [...prevTags, { name: tagText, entries: 10 }];
    });
  };

  const tagTextChangeHandler = (event: any) => {
    setTagText(event.target.value);
  };

  return (
    <form
      style={{ minHeight: "36px" }}
      className="input-dark input-add tag-box"
      onSubmit={addTag}
    >
      {tags.map((item) => {
        return <FormTag key={item.name} TagItem={item} onDelete={deleteTag} />;
      })}
      <select name="select" onChange={tagTextChangeHandler}>
        {dataTags.map((item) => {
          return (
            <option id={item.name} value={item.name}>
              {item.name}
            </option>
          );
        })}
      </select>
    </form>
  );
};
export default TagInput;
