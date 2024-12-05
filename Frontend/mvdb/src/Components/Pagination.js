import 'bootstrap/dist/css/bootstrap.min.css';
import { Row, Col, Container, Pagination } from "react-bootstrap";


export function Paging({index, total, setIndex}) {
  const handleIndex = (i) => setIndex(i);

  const toRender = [];

  // Left button
  if (index > 0) {
    toRender.push(
      <Pagination.Prev onClick={() => handleIndex(index - 1)} key="prev" />
    );
  }

  // First button and ellipsis
  if (index > 2) {
    toRender.push(
      <Pagination.Item onClick={() => handleIndex(0)} active={index === 0} key={0}>
        1
      </Pagination.Item>
    );
    toRender.push(<Pagination.Ellipsis key="start-ellipsis" />);
  }

  // Middle buttons
  for (let i = index - 2; i <= index + 2; i++) {
    if (i >= 0 && i < total) {
      toRender.push(
        <Pagination.Item onClick={() => handleIndex(i)} active={index === i} key={i}>
          {i + 1}
        </Pagination.Item>
      );
    }
  }

  // handle last button
  if (index < total - 3) {
    toRender.push(<Pagination.Ellipsis key="end-ellipsis" />);
    toRender.push(
      <Pagination.Item onClick={() => handleIndex(total - 1)} active={index === total - 1} key={total - 1}>
        {total}
      </Pagination.Item>
    );
  }

  // handle right button
  if (index < total - 1) {
    toRender.push(
      <Pagination.Next onClick={() => handleIndex(index + 1)} key="next" />
    );
  }

  return <Pagination>{toRender}</Pagination>;
  
  }
