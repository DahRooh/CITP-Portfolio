import 'bootstrap/dist/css/bootstrap.min.css';

export function Pagination({index, total, setIndex}) {
  const handleIndex = (i) => setIndex(i);

  const toRender = [];

  // Left button
  if (index > 0) {
    toRender.push(
      <button
        className="pageButton"
        onClick={() => handleIndex(index - 1)}
        disabled={index === 0}
        key="left-arrow"
      >
        &larr;
      </button>
    );
  }

  // First button and ellipsis
  if (index > 2) {
    toRender.push( // push the first button and string of ... to indicate there are buttons hidden
      <>
        <button className='pageButton' key = {0} onClick = {() => handleIndex(0)} disabled = {index === 0}>
          1
        </button>
        <span style={{paddingLeft: '10px', paddingRight: '10px' }}>...</span> 
      </>
    );
  }

  // Middle buttons
  for (let i = index - 2; i < index + 3; i++) { 
    // we want [n - 2, ..., n + 2] as long as we dont hit last or first index
    if (i >= 0 && i < total) { // can't be larger or equal to total, and it can't be below minimum
      toRender.push( // we must then push each button that fit
        <>
          <button
            className={`pageButton ${index === i ? "active" : ""}`} // Fix: Add backticks for string interpolation
            key={i}
            onClick={() => handleIndex(i)}
            disabled={index === i}
          >
            {i + 1}
          </button>
        </>
      );
    }
  }
  

  // handle last button
  if (index < total - 3) {
    toRender.push(
    <>
      <span style={{paddingLeft: '10px', paddingRight: '10px' }}>...</span> 
      <button className='pageButton' key = {total} onClick = {() => handleIndex(total-1)} disabled = {index === total-1}>
        {total}
      </button>
    </>);
  }

  // handle right button
  if (index < total - 1) {
    toRender.push(
      <button // Right button (&rarr; right arrow)
        className="pageButton"
        onClick = {() => handleIndex(index + 1)}
        disabled={index === total}
      >
        &rarr; 
      </button> 
    );
  }

  return <>{toRender}</>;
}