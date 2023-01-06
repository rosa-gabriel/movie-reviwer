import LoadingCircle from "./LoadingCircle";

type LoadingUserInfoProps = {
  error: boolean;
};

const LoadingUserInfo = (props: LoadingUserInfoProps) => {
  return (
    <div className="loading-container">
      {!props.error && (
        <>
          <h1 className="loading-title">Loading User Info</h1>
          <p className="faded">Wait for your user info to be loaded...</p>
          <LoadingCircle />
        </>
      )}
      {props.error && (
        <>
          <h1 className="loading-title">Failed to load user info</h1>
          <p className="error">
            Failed to load user info. Try refreshing the page. If the error
            continues the servers may be down.
          </p>
        </>
      )}
    </div>
  );
};

export default LoadingUserInfo;
