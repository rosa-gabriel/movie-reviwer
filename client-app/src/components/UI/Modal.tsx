type ModalProps = {
  title: string;
  text: string;
  onCancel: any;
  onConfirm: any;
};

const Modal = (props: ModalProps) => {
  return (
    <div className="modal-background" onClick={props.onCancel}>
      <div className="modal-box">
        <h2 className="title ">{props.title}</h2>
        <p className={"faded centered-message"}>{props.text}</p>
        <div className="modal-buttons">
          <button className="button" onClick={props.onCancel}>
            Cancel
          </button>
          <button className="button" onClick={props.onConfirm}>
            Confirm
          </button>
        </div>
      </div>
    </div>
  );
};

export default Modal;
