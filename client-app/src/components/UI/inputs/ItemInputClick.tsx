type ItemInputClickProps = {
  item: any;
  onDelete(item: any): void;
};

const ItemInputClick = (props: ItemInputClickProps) => {

  let content: string = props.item.name;

  if (props.item.role) {
    content += ` (${props.item.role})`;
  }
  const deleteItem = (): void => {
    props.onDelete(props.item);
  };

  return (
    <div className="form-tag">
      <div>
        <span>{content}</span>
        <span>
          <button onClick={deleteItem} type="button">
            X
          </button>
        </span>
      </div>
    </div>
  );
};

export default ItemInputClick;
