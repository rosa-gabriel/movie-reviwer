import { Route, Routes } from "react-router";
import { NavLink } from "react-router-dom";
import AddPersonForm from "./AddPersonForm";
import AddMovieForm from "./AddMovieForm";
import AddTagForm from "./AddTagForm";
import Container from "../../components/UI/Container";
import AccountCheck from "../../components/account/AccountCheck";

export const active = ({ isActive }: any) => {
  return "nav-link " + (isActive ? "nav-link-selected" : "");
};

const CreationOptions = () => {
  return (
    <Container>
      <>
        <AccountCheck />
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
          <Route path="tag" element={<AddTagForm />} />
          <Route path="person" element={<AddPersonForm />} />
        </Routes>
      </>
    </Container>
  );
};
export default CreationOptions;
