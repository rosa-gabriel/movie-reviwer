import { TagType } from "../../Type/Types";

type FormTagProps = {
  tag: TagType;
  onDelete: Function;
};

const FormTag = (props: FormTagProps) => {
  const deleteTag = (): void => {
    props.onDelete(props.tag);
  };

  return (
    <div className="form-tag">
      <div>
        <span>{props.tag.name}</span>
        <span>
          <button onClick={deleteTag} type="button">
            X
          </button>
        </span>
      </div>
    </div>
  );
};

export default FormTag;
