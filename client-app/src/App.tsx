import { Route, Routes } from "react-router-dom";
import Details from "./components/Details";
import Favorites from "./components/Favorites";
import Error from "./components/Error";
import NavBar from "./components/UI/NavBar";
import CreationOptions from "./components/CreationForms/CreationOptions";
import HomeList from "./components/HomeList";
import TagList from "./components/TagList";
import PersonInfo from "./components/PersonInfo";
import LoginForm from "./components/AccountForms/LoginForm";
import { UserContextProvider } from "./components/Context/UserContext";
import RegisterForm from "./components/AccountForms/RegisterForm";
import Profile from "./components/Profile";

export const uri = "http://localhost:5000";

function App() {
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
          <Route path="/account/register" element={<RegisterForm />} />

          <Route path="/account/profile/:id" element={<Profile />} />
          <Route path="/favorites" element={<Favorites />} />
          <Route path="/add/*" element={<CreationOptions />} />

          <Route path="/error" element={<Error />} />
          <Route path="/*" element={<Error />} />
        </Routes>
      </>
    </UserContextProvider>
  );
}

export default App;
