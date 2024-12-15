import { useState } from "react"
import {Button, ButtonGroup, Col, Image} from 'react-bootstrap';
import trashIcon from '../trash.png';

export function Popup( {deleter, message, functionMsg} ) {
    const [showMessage, setShowMessage] = useState(false);
    if (showMessage) {
        return (
            <Col md={3}>
                <p>{message}</p> 
                <ButtonGroup>
                    <Button onClick={() => {
                            deleter();
                            setShowMessage(false)
                        }}>Yes</Button>
                    <Button onClick={() => setShowMessage(false)}>No</Button>
                </ButtonGroup>
            </Col>
        )
    } else {
        return (
            <Col md={1}>
                {functionMsg}
                <Button className='btn-danger' onClick={() => setShowMessage(true)}>
                    <Image 
                        src={trashIcon} 
                        width={"100%"}
                        alt="Delete review"/>
                </Button>
            </Col>
        )
    }

}