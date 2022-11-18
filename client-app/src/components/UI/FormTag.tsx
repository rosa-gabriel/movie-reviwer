import { Link } from "react-router-dom";
import { isPropertySignature } from "typescript";

const FormTag = (props: any) => {

  const deleteTag = () => {
    props.onDelete(props.TagItem);
  }

  return (
    <div className="form-tag">
      <div>
        <span>{props.TagItem.name}</span>
        <span>
          <button onClick={deleteTag} type='button'>X</button>
        </span>
      </div>
    </div>
  );
};

export default FormTag;
