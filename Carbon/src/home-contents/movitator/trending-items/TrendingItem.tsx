import React from 'react';
import './style.css';

const TrendingItem = () => {
    return (
        <div className='trending-wrapper'>
            <div className='trending-list'>
                <h5>Get inspired</h5>
                <p>Explore the trend in our categories</p>
            </div>
            <div className='categories'>
                <div className='row'>
                    <div className='col m2 s4'>
                        <a href='/'><i className='fas fa-laptop fa-2x'></i></a><br/>
                        <a href='/'>Electronics</a>
                    </div>
                    <div className='col m2 s4'>
                        <a href='/'><i className='fas fa-laptop fa-2x'></i></a><br/>
                        <a href='/'>Electronics</a>
                    </div>
                    <div className='col m2 s4'>
                        <a href='/'><i className='fas fa-laptop fa-2x'></i></a><br/>
                        <a href='/'>Electronics</a>
                    </div>
                    <div className='col m2 s4'>
                        <a href='/'><i className='fas fa-laptop fa-2x'></i></a><br/>
                        <a href='/'>Electronics</a>
                    </div>
                    <div className='col m2 s4'>
                        <a href='/'><i className='fas fa-laptop fa-2x'></i></a><br/>
                        <a href='/'>Electronics</a>
                    </div>
                    <div className='col m2 s4'>
                        <a href='/'><i className='fas fa-laptop fa-2x'></i></a><br/>
                        <a href='/'>Electronics</a>
                    </div>
                </div>
            </div>
        </div>
    );
}

export default TrendingItem;