export function Picture({ type, personPath, titlePath, width, height }) {
  const urls = {
    person: "https://image.tmdb.org/t/p/w500",
    title: ""
  };
  //console.log("pictuepath: ", urls[type]);
  const imagePath =
    type === "person" ? personPath?.profile_path : titlePath?.poster;

  const imgStyle = {
    width: `${width}px`,
    height: `${height}px`,
    border: "1px solid black",
  };
 
  const altMessage =
    type === "person"
      ? personPath?.name || "Image"
      : titlePath?._Title || "Image";
  
  return imagePath ? (
    <img
      style={imgStyle}
      src={type === "person" ? `${urls[type]}${imagePath}` : imagePath}
      alt={altMessage}
    />
  ) : (
    <div style={{ ...imgStyle, display: "flex", alignItems: "center", justifyContent: "center" }}>
      No Image Available
    </div>
  );
}
