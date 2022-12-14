import LoadingCircle from "./LoadingCircle";

type SubmitButtonProps = {
  loading: boolean;
  buttonText: string;
};

const SubmitButton = (props: SubmitButtonProps) => {
  return (
    <>
      {!props.loading && (
        <button type="submit" className="add-button button">
          {props.buttonText}
        </button>
      )}

      {props.loading && (
        <button style={{ display: "flex" }} className="add-button" disabled>
          <LoadingCircle />
        </button>
      )}
    </>
  );
};

export default SubmitButton;
