import { Route, Routes } from "react-router";
import { NavLink } from "react-router-dom";
import AddMovieForm from "./AddMovieForm";
import AddTagForm from "./AddTagForm";

export const active = ({ isActive }: any) => {
  return "nav-link " + (isActive ? "nav-link-selected" : "");
};

const CreationOptions = () => {
  return (
    <>
      <nav className="creation_nav">
        <NavLink className={active} to={"movie"}>
          Movie
        </NavLink>
        <NavLink className={active} to={"person"}>
          Person
        </NavLink>
        <NavLink className={active} to={"tag"}>
          Tag
        </NavLink>
      </nav>
      <Routes>
        <Route path="movie" element={<AddMovieForm />} />
        <Route path="tag" element={<AddTagForm/>} />
        <Route path="person" element={<></>} />
      </Routes>
    </>
  );
};
export default CreationOptions;
