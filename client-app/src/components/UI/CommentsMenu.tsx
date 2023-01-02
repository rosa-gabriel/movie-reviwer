type CommentsMenuProps = {
  onEdit(): any;
  onDelete(): any;
};

const CommentsMenu = (props: CommentsMenuProps) => {
  return (
    <div className="comment-menu">
      <ul>
        <li onClick={props.onEdit}>Edit</li>
        <li onClick={props.onDelete}>Delete</li>
      </ul>
    </div>
  );
};

export default CommentsMenu;
