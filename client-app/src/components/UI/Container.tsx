type ContainerProps = {
    children?: JSX.Element
}

const Container = (props: ContainerProps) => {
  return <div className="container">{props.children}</div>;
};

export default Container;
