type CommentEditInputProps = {
  onCancel(): any;
  onConfirmEdit(): any;
  editMessage(n: any): any;
  value: string;
};

const CommentEditInput = (props: CommentEditInputProps) => {
  return (
    <div className="comment-edit-input">
      <input
        value={props.value}
        onChange={props.editMessage}
        className="input-dark"
      ></input>
      <button type="button" onClick={props.onCancel} className="button">
        Cancel
      </button>
      <button type="button" onClick={props.onConfirmEdit} className="button">
        Edit
      </button>
    </div>
  );
};

export default CommentEditInput;
