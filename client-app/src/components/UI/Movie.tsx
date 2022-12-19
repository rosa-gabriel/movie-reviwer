import { Link } from "react-router-dom";
import { MovieType } from "../../types/Types";

type MovieProps = {
  movie: MovieType;
};

const Movie = (props: MovieProps) => {
  return (
    <div className="movie-container">
      <div>
        <Link to={`/details/${props.movie.id}`}>
          <>
            <img src={props.movie.coverUrl} alt="Not found!" />
            <p>{props.movie.name}</p>
          </>
        </Link>
      </div>
    </div>
  );
};

export default Movie;
