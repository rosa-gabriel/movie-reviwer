import { useContext, useState } from "react";
import { useNavigate } from "react-router";
import { addPerson } from "../../functions/requests/MovieRequests";
import { NotificationContext } from "../../contexts/NotificationContext";
import { UserContext } from "../../contexts/UserContext";
import SubmitButton from "../../components/UI/SubmitButton";
import FormPair from "../../components/UI/FormPair";

const AddPersonForm = () => {
  //States
  const [error, setError] = useState<string | null>(null);
  const [isLoading, setIsLoading] = useState<boolean>(false);
  const [name, setName] = useState<string>("");
  const [url, setUrl] = useState<string>("");
  const [bio, setBio] = useState<string>("");
  const [date, setDate] = useState<Date>(new Date());
  const [gender, setGender] = useState<number>(0);

  //Contexts
  const notification = useContext(NotificationContext);

  //Hooks
  const navigate = useNavigate();
  const context = useContext(UserContext);

  //Input Handlers
  const nameChangeHandler = (event: any) => {
    setName(event.target.value);
  };
  const urlChangeHandler = (event: any) => {
    setUrl(event.target.value);
  };
  const bioChangeHandler = (event: any) => {
    setBio(event.target.value);
  };
  const genderChangeHandler = (event: any) => {
    setGender(event.target.value);
  };
  const dateChangeHandler = (event: any) => {
    setDate(new Date(event.target.value));
  };

  //Submit Handler
  const submitHandler = async (event: any) => {
    event.preventDefault();

    if (name.trim() === "") {
      setError("Name field is required.");
      return;
    }

    if (url.trim() === "") {
      setError("Url field is required.");
      return;
    }
    if (gender == 0) {
      setError("You need to choose a gender!");
      return;
    }
    setIsLoading(true);
    try {
      await addPerson(
        {
          name: name,
          profileImageUrl: url,
          birthday: date,
          biography: bio,
          gender: gender,
        },
        String(context.userInfo?.token)
      );
      setError(null);
      notification.addNotification({
        code: "ADDED",
        text: "Person was successfully added.",
        error: false,
      });
      navigate("/add/movie");
    } catch (ex: any) {
      setError(ex.message);
    } finally {
      setIsLoading(false);
    }
  };

  return (
    <form onSubmit={submitHandler}>
      <h1 className="title">Add new Person</h1>

      {error !== null && <p className="error centered-message">{error}</p>}

      <div className="form-pair">
        <label>NAME</label>
        <input
          onChange={nameChangeHandler}
          className="input-dark input-add"
          type={"text"}
        />
      </div>

      <div className="form-pair">
        <label>IMAGE URL</label>
        <input
          onChange={urlChangeHandler}
          className="input-dark input-add"
          type={"text"}
        />
      </div>

      <FormPair
        title={"BIOGRAPHY"}
        value={bio}
        type={"text"}
        onChange={bioChangeHandler}
      ></FormPair>

      <FormPair
        title={"BIRTHDAY"}
        value={date.toISOString()}
        type={"date"}
        onChange={dateChangeHandler}
      ></FormPair>

      <div className="form-pair">
        <label>GENDER</label>
        <select
          onChange={genderChangeHandler}
          className="input-dark input-add"
          placeholder="Gender"
          value={gender}
        >
          <option value={0}>-- Choose a Gender --</option>
          <option value={1}>Male</option>
          <option value={2}>Female</option>
          <option value={3}>Different</option>
        </select>
      </div>

      <SubmitButton loading={isLoading} buttonText={"Add"}></SubmitButton>
    </form>
  );
};

export default AddPersonForm;
