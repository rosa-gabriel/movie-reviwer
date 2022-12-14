import { Route, Routes } from "react-router-dom";
import Details from "./pages/Details/Details";
import Favorites from "./pages/User/Favorites";
import Error from "./components/UI/Error";
import NavBar from "./components/UI/NavBar";
import CreationOptions from "./pages/CreationForms/CreationOptions";
import HomeList from "./pages/MovieLists/HomeList";
import TagList from "./pages/MovieLists/TagList";
import PersonInfo from "./pages/Details/PersonInfo";
import LoginForm from "./pages/User/LoginForm";
import { UserContextProvider } from "./Context/UserContext";
import RegisterForm from "./pages/User/RegisterForm";
import Profile from "./pages/User/Profile";
import SearchList from "./pages/MovieLists/SearchList";
import EditDetails from "./pages/Details/EditDetails";
import { ModalContextProvider } from "./Context/ModalContext";
import { NotificationContextProvider } from "./Context/NotificationContext";

export const uri = "http://localhost:5000";

function App() {
  return (
    <UserContextProvider>
      <ModalContextProvider>
        <NotificationContextProvider>
          <>
            <NavBar />
            <Routes>
              <Route path="/" element={<HomeList />} />
              <Route path="/details/:movieId" element={<Details />} />
              <Route path="/details/:movieId/edit" element={<EditDetails />} />
              <Route path="/tag/:id" element={<TagList />} />
              <Route path="/search/:search" element={<SearchList />} />
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
        </NotificationContextProvider>
      </ModalContextProvider>
    </UserContextProvider>
  );
}

export default App;
