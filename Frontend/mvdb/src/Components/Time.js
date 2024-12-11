export function Timestamp( {time} ) {
    let newTimestamp = time.split(".")[0].split("T");
    return (
        <>
            <p style={{fontWeight: 'bold', wordBreak: 'break-word'}}>
            Date: {newTimestamp[0]}
            </p>
            <p style={{fontWeight: 'bold', wordBreak: 'break-word'}}>
            Time: {newTimestamp[1]}
            </p>
        </>
    )
}