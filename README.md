# MovieApp

Still in Development... May not work properly

A movie website created using REACT and ASP.NET, this project was inspired by sites like MyAnimeList, IMDb, and Rotten Tomates, this was a very good opportunity to concepts like requests, full-stack, CRUD, UML, and database design in a medium-sized website.

# Prerequisites

## Softwares:

1. SDK version 6.0.403
2. MySQL version 8.0.30 (if using a local database)
3. NPM version 8.19.2
4. React and React.dom package version 18.2.0
5. TypeScript package version 4.9.4

These are the versions used in the site development. Older versions may not work properly.

# Future updates

1. Let the user be able to change profile information.
2. Comment Section to movies.
3. Friend other profiles.
4. Bether Search functionality, search from the movie tags also.
5. Add better response messages so the user understands why errors occurred.
6. Fix timezone issues when changing dates.

# Instalation

### 1. Clone the repository in a new folder.

```sh
https://github.com/GabrielWaif/MovieApp.git
```

## If on windows:

### 2. Open the cloned folder and open the "StartMoviesApp.bat" file, this file will open 2 terminals each one part of the application.

## If on another OS:

### 2. Inside the cloned folder there is a filter names 'client-app' run:

```sh
npm start
```

This will start the local front-end server.

### 3. Keep this terminal open and open a new one.

### 4. Go back to the main folder and find the API folder, there run the command:

```sh
dotnet run --launch-profile GitHub
```

This will start the local backend API server.

# Possible bugs

## Port already in use:

If you see this terminal response:

<center>
<img src="https://i.imgur.com/yy1ifgX.png" width="70%"/>
</center>

This means that some other process is already running on port 3000 (which is the front-end REACT application port), running the application on another port will cause the back-end CORS policy to reject any request made by the new port. The recommended step is to kill the current process running on port 3000.

<a href="https://medium.com/@javatechie/how-to-kill-the-process-currently-using-a-port-on-localhost-in-windows-31ccdea2a3ea">For more information in how to do that.</a>

# Pages

### 1. Home

Here the newest movies are shown in an endless scroll list, you can click enter the details of each movie by clicking on them.

<center>
<img src="https://i.imgur.com/p9U9E8e.png" width="70%"/>
</center>

### 2. Details

Inside the movie details page, you can see the movie tags and cast.
Users that are logged in can favorite a movie by clicking the heart icon.
Admin users can edit the movie by clicking on the pen icon on the top right.

<center>
<img src="https://i.imgur.com/XR4IftY.png" width="70%"/>
</center>

### 3. Edit Movie

Admin users can edit existing movies by typing the new info and clicking confirm or delete the movie by clicking the trash can icon on the top right.

<center>
<img src="https://i.imgur.com/EHFCw0V.png" width="70%"/>
</center>

### 4. Tag List

Shows all the movies that have that certain tab ordered by release date.

<center>
<img src="https://i.imgur.com/vj3Ey9t.png" width="70%"/>
</center>

### 5. Search

By searching a string(word or phrase) the search page shows a movie that contains that string on its name ordered by release date.

<center>
<img src="https://i.imgur.com/mtRNTo2.png" width="70%"/>
</center>

### 6. Person

Shows a person's page with their name, photo, and all the movies in which they participated.

<center>
<img src="https://i.imgur.com/zuyd2bw.png" width="70%"/>
</center>

### 7. Adding Movies, Tags, and People

On this page, an admin user can add tags, people, and movies using a form.

<center>
<img src="https://i.imgur.com/4acmx5T.png" width="70%"/>
</center>

### 8. User profile

The user profile shows some user info, their profile picture, and the 5 last movies that the user favorites.

<center>
<img src="https://i.imgur.com/Eq4rIa2.png" width="70%"/>
</center>

### 9. Login

On this page the user can log in with preexisting accounts, passwords have prerequisites and the login can be made using the username or email. Login tokens last 7 days.

<center>
<img src="https://i.imgur.com/suCiTG7.png" width="70%"/>
</center>

### 10. Register

Here new accounts are created, and new accounts need to have a unique username and email, and a valid password.

<center>
<img src="https://i.imgur.com/BbmaRhS.png" width="70%"/>
</center>
