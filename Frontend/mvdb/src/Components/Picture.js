export function Picture({picturePath, width, height}) {

    var url = "https://image.tmdb.org/t/p/w500";
    var imageUrl = url + picturePath.profile_path; 
    var imgStyle= {width: `${width}px`, height: `${height}px`, border: `1px solid black`}
  
    return (
      <img 
        style={imgStyle} 
        src={imageUrl} 
        alt={picturePath?.name} 
      />
    );
  }