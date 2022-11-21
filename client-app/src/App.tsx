import { Route, Routes, Navigate } from "react-router-dom";
import Details from "./components/Details";
import Favorites from "./components/Favorites";
import Error from "./components/Error";
import MovieList from "./components/MovieList";
import NavBar from "./components/UI/NavBar";
import CreationOptions from "./components/CreationForms/CreationOptions";

export const uri = "http://localhost:5000";

function App() {
  return (
    <>
      <NavBar />
      <div className="container">
        <Routes>
          <Route path="/" element={<MovieList />} />
          <Route path="/details/:movieId" element={<Details />} />
          <Route path="/add/*" element={<CreationOptions/>} />
          <Route path="/:userId/favorites/" element={<Favorites />} />
          <Route path="/error" element={<Error />} />
          <Route path="/*" element={<Navigate to={"/Error"} />} />
        </Routes>
      </div>
    </>
  );
}

export default App;
