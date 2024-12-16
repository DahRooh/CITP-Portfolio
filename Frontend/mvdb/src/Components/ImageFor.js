import { useEffect, useState } from "react";

function ImageFor( {item, width = "100%", height="100%"} ) {
    const [poster, setPoster] = useState(null);
    const apiKey = "c8190d104e34c4f62a2be88afa477327";
    const fetchPoster = `https://api.themoviedb.org/3/find/${item.id}?api_key=${apiKey}&external_source=imdb_id`;
    useEffect(() => {
        fetch(fetchPoster)
        .then(res =>  res.json())
        .then(data => {
            handleData(data, setPoster);
        });
    }, [fetchPoster]);

    function handleData(data, setData) {
        var keys = Object.keys(data);
        keys.forEach(key => {
            if (data[key].length > 0) {
                const path = data[key][0].profile_path || data[key][0].poster_path;
                if (path) setData(path);
            }
        });
    }
    var posterUrl = "https://image.tmdb.org/t/p/w500";
    const selectSrc = () => {
        if (item.poster && item.poster !== "N/A") {
            posterUrl = item.poster;

        }
        else if (poster) {
            posterUrl = posterUrl+poster;
        }
        else {
            posterUrl = "https://media.istockphoto.com/id/911590226/vector/movie-time-vector-illustration-cinema-poster-concept-on-red-round-background-composition-with.jpg?s=612x612&w=0&k=20&c=QMpr4AHrBgHuOCnv2N6mPUQEOr5Mo8lE7TyWaZ4r9oo=";
        }
        return posterUrl;
    }
    

    return <img 
                className="selectionImage" 
                src={selectSrc()} 
                style={{width: width, height: height}}   
                alt="Poster" />
}

export default ImageFor;