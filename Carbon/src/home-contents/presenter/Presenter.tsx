import React from 'react';
import Carousel from 'react-material-ui-carousel'
import { Paper, Button } from '@material-ui/core'
import './style.css';

const Presenter = () => {

    var items = [
        {
            title1: 'Shop everywhere!',
            title2: 'Just a click away^',
            subtitle1: 'Explore the smartest way to shop for millions of items on Hidrogen.',
            subtitle2: 'Find anything in an eye blink.',
            button: 'Shop now'
        },
        {
            title1: 'Sell everywhere!',
            title2: 'From your place^',
            subtitle1: 'List your items on Hidrogen to have it reach thousands of people in secs.',
            subtitle2: 'Reach out from your local.',
            button: 'Find out how'
        }
    ]

    return (
        <div className='presenter-wrapper'>
            <Carousel interval={ 10000 }>
                {
                    items.map(item =>
                        <Slide item={item} />
                    )
                }
            </Carousel>
        </div>
    );
}

export default Presenter;

const Slide = (props: any) => {
    return (
        <Paper>
            <div className='slide-wrapper'>
                <div className='row'>
                    <div className='col l6 m4 s12'>
                        <h4>{ props.item.title1 }</h4>
                        <h5>{ props.item.title2 }</h5>
                        <p style={{ marginTop:'25px' }}>{ props.item.subtitle1 }</p>
                        <p>{ props.item.subtitle2 }</p>
                        <button className='btn btn-large'>{ props.item.button }</button>
                    </div>
                    <div className='col l3 m8 s0'>
                        <div className='video-container'>
                            <iframe src='https://www.youtube.com/embed/nbKobG1hRSU' frameBorder='0' allow='accelerometer; autoplay; encrypted-media; gyroscope; picture-in-picture' allowFullScreen></iframe>
                        </div>
                    </div>
                    <div className='col l3 m8 s0'>
                        <div className='video-container'>
                            <iframe src='https://www.youtube.com/embed/OIi2ZEUEMe8' frameBorder='0' allow='accelerometer; autoplay; encrypted-media; gyroscope; picture-in-picture' allowFullScreen></iframe>
                        </div>
                    </div>
                </div>
            </div>
        </Paper>
    );
}