type FormPairProps = {
  title: string;
  value: string;
  type: string;
  onChange(e: any): void;
};

const FormPair = (props: FormPairProps) => {
  return (
    <div className="form-pair">
      <div className="form-pair">
        <label>{props.title}</label>
        <input
          type={props.type}
          className="input-add input-dark"
          value={props.value}
          onChange={props.onChange}
          placeholder={props.title}
        />
      </div>
    </div>
  );
};
export default FormPair;
