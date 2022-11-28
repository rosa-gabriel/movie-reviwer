import { Route, Routes, Navigate, UNSAFE_RouteContext } from "react-router-dom";
import Details from "./components/Details";
import Favorites from "./components/Favorites";
import Error from "./components/Error";
import NavBar from "./components/UI/NavBar";
import CreationOptions from "./components/CreationForms/CreationOptions";
import HomeList from "./components/HomeList";
import TagList from "./components/TagList";
import PersonInfo from "./components/PersonInfo";
import { seedMovies, seedTags } from "./functions/DataBaseSeeding";
import LoginForm from "./components/AccountForms/LoginForm";
import { UserContext, UserContextProvider } from "./components/Context/UserContext";
import { useContext } from "react";
import RegisterForm from "./components/AccountForms/RegisterForm";

export const uri = "http://localhost:5000";

function App() {
  const context =  useContext(UserContext)

  return (
    <UserContextProvider>
      <>
        <NavBar />
        <Routes>
          <Route path="/" element={<HomeList />} />
          <Route path="/details/:movieId" element={<Details />} />
          <Route path="/tag/:id" element={<TagList />} />
          <Route path="/person/:id" element={<PersonInfo />} />
          <Route path="/account/login" element={<LoginForm />} />
          <Route path="/account/register" element={<RegisterForm/>} />

          <Route path="/:userId/favorites/" element={<Favorites />} />
          <Route path="/add/*" element={<CreationOptions />} />

          <Route path="/error" element={<Error />} />
          <Route path="/*" element={<Navigate to={"/Error"} />} />
        </Routes>
      </>
    </UserContextProvider>
  );
}

export default App;
