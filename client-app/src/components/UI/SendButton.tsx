import LoadingCircle from "./LoadingCircle";

type SendButtonProps = {
  className: string;
  text: string;
  isLoading: boolean;
  type: "button" | "submit";
  onClick?(e: any): any;
};

const SendButton = (props: SendButtonProps) => {
  return (
    <>
      {props.isLoading && (
        <button className={"button " + props.className} disabled>
          <LoadingCircle />
        </button>
      )}
      {!props.isLoading && (
        <button
          onClick={props.onClick}
          type={props.type}
          className={"button " + props.className}
        >
          {props.text}
        </button>
      )}
    </>
  );
};

export default SendButton;
