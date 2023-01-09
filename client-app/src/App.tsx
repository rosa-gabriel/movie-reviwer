import { Route, Routes } from "react-router-dom";
import Details from "./pages/details/Details";
import Favorites from "./pages/user/Favorites";
import Error from "./components/UI/Error";
import NavBar from "./components/UI/NavBar";
import CreationOptions from "./pages/creationForms/CreationOptions";
import HomeList from "./pages/movieLists/HomeList";
import TagList from "./pages/movieLists/TagList";
import PersonInfo from "./pages/details/PersonInfo";
import LoginForm from "./pages/user/LoginForm";
import { UserContextProvider } from "./contexts/UserContext";
import RegisterForm from "./pages/user/RegisterForm";
import Profile from "./pages/user/Profile";
import SearchList from "./pages/movieLists/SearchList";
import EditDetails from "./pages/details/EditDetails";
import { ModalContextProvider } from "./contexts/ModalContext";
import { NotificationContextProvider } from "./contexts/NotificationContext";
import DataBaseSeeding from "./development/DatabaseSeedingO";
import Settings from "./pages/user/Settings";
import FriendsList from "./pages/user/FriendsList";
import EditPersonInfo from "./pages/details/EditPersonInfo";
import ConfirmEmail from "./pages/user/ConfirmEmail";
import ConfirmEmailRequest from "./pages/user/ConfirmEmailRequest";

export const uri = "https://movieapp-production-8f6a.up.railway.app";

function App() {
  return (
    <UserContextProvider>
      <ModalContextProvider>
        <NotificationContextProvider>
          <>
            <NavBar />
            <Routes>
              <Route path="/MovieApp" element={<HomeList />} />
              <Route path="/MovieApp/details/:movieId" element={<Details />} />
              <Route
                path="/MovieApp/details/:movieId/edit"
                element={<EditDetails />}
              />
              <Route path="/MovieApp/tag/:id" element={<TagList />} />
              <Route path="/MovieApp/search/:search" element={<SearchList />} />
              <Route path="/MovieApp/person/:id" element={<PersonInfo />} />
              <Route
                path="/MovieAppperson/:personId/edit"
                element={<EditPersonInfo />}
              />
              <Route path="/MovieApp/account/login" element={<LoginForm />} />
              <Route
                path="/MovieApp/account/register"
                element={<RegisterForm />}
              />

              <Route
                path="/MovieApp/account/message/confirm"
                element={<ConfirmEmailRequest />}
              />

              <Route
                path="/MovieApp/account/confirm/:token"
                element={<ConfirmEmail />}
              />
              <Route path="/MovieApp/account/settings" element={<Settings />} />

              <Route
                path="/MovieApp/account/profile/:id"
                element={<Profile />}
              />
              <Route
                path="/MovieApp/account/friends/"
                element={<FriendsList />}
              />
              <Route path="/MovieApp/favorites/" element={<Favorites />} />
              <Route
                path="/MovieApp/favorites/:Username"
                element={<Favorites />}
              />
              <Route path="/MovieApp/add/*" element={<CreationOptions />} />

              <Route path="/MovieApp/error" element={<Error />} />
              <Route path="/MovieApp/*" element={<Error />} />
            </Routes>
          </>
        </NotificationContextProvider>
      </ModalContextProvider>
    </UserContextProvider>
  );
}

export default App;
