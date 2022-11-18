import { Link } from "react-router-dom";

const Movie = (props: any) => {
  return (
    <div className="movie-container">
      <div>
        <Link to={`/details/${props.item.id}`}>
          <>
            <img src={props.item.coverUrl} alt="Not found!" />
            <p>{props.item.name}</p>
          </>
        </Link>
      </div>
    </div>
  );
};

export default Movie;
