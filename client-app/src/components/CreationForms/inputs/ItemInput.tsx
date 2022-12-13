import FormTag from "./ItemInputClick";
import { useState } from "react";
import ItemInputClick from "./ItemInputClick";

type ItemInputProps = {
  onChange: Function;
  dataItems: any[];
  items: any[];
  children?: JSX.Element;
  role?: string | null;
  placeHolder: string;
  itemId: string;
  dataItemId: string;
};

const ItemInput = (props: ItemInputProps) => {
  const [itemText, setItemText] = useState<string>("");

  const deleteItem = (item: any): void => {
    const newItems = props.items.filter((element: any) => element !== item);
    const newDataItems = [...props.dataItems, item];
    props.onChange(newItems, newDataItems);
  };

  const addItem = (event: React.SyntheticEvent) => {
    event.preventDefault();
    if (itemText === "") return;

    const item = props.dataItems.find((item) => item.name === itemText);

    if (props.role !== null) {
      if (String(props.role).trim().length !== 0) {
        item.role = props.role;
      } else return;
    }

    const newDataItems = [
      ...props.dataItems.filter((dataTag) => dataTag !== item),
    ];
    const newItems = [...props.items, item];
    props.onChange(newItems, newDataItems);
  };

  const itemTextChangeHandler = (event: any): void => {
    setItemText(event.target.value);
  };

  return (
    <div style={{ minHeight: "36px" }} className="input-dark input-add tag-box">
      {props.items.map((item) => {
        return (
          <ItemInputClick
            key={"selectedClick" + item[props.itemId]}
            item={item}
            onDelete={deleteItem}
          />
        );
      })}
      <div className="click-add-container">
        <select name="select" onChange={itemTextChangeHandler}>
          <option value={""}>{props.placeHolder}</option>
          {props.dataItems.map((item) => {
            if (item[props.dataItemId] == undefined) return;
            return (
              <option
                key={"optionClick" + item[props.dataItemId]}
                value={item.name}
              >
                {item.name}
              </option>
            );
          })}
        </select>
        <button className="button" onClick={addItem}>
          +
        </button>
        {props.children}
      </div>
    </div>
  );
};
export default ItemInput;
