import { Route, Routes, Navigate } from "react-router-dom";
import Details from "./components/Details";
import Favorites from "./components/Favorites";
import Error from "./components/Error";
import NavBar from "./components/UI/NavBar";
import CreationOptions from "./components/CreationForms/CreationOptions";
import HomeList from "./components/HomeList";
import TagList from "./components/TagList";
import PersonInfo from "./components/PersonInfo";

export const uri = "http://localhost:5000";

function App() {
  return (
    <>
      <NavBar />
        <Routes>
          <Route path="/" element={<HomeList />} />
          <Route path="/details/:movieId" element={<Details />} />

          <Route path="/tag/:id" element={<TagList/>} />
          <Route path="/person/:id" element={<PersonInfo/>} />

          <Route path="/add/*" element={<CreationOptions/>} />
          <Route path="/:userId/favorites/" element={<Favorites />} />
          <Route path="/error" element={<Error />} />
          <Route path="/*" element={<Navigate to={"/Error"} />} />
        </Routes>
    </>
  );
}

export default App;
