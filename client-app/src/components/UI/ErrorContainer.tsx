type ErrorContextProps = {
  error: string | null;
  children?: JSX.Element;
};

const ErrorContainer = (props: ErrorContextProps) => {
  return (
    <>
      {props.error && <p className="error centered-message">{props.error}</p>}
      {!props.error && props.children}
    </>
  );
};

export default ErrorContainer;
