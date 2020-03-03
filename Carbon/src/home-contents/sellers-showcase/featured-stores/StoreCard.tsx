import React from 'react';

const StoreCard = () => {
    return (
        <div className='col l6 m12'>
            <div className='card horizontal'>
            <div className='card-image'>
                <img src='https://d1q0twczwkl2ie.cloudfront.net/wp-content/uploads/2016/09/12105964_1000216096695458_1007538799526354232_n.jpg' />
            </div>
            <div className='card-stacked'>
                <div className='card-content'>
                    <p className='card-title'>Store Name</p>
                    <p>A featured store is one that has signed up to be advertised on the Home Page.</p>
                    <b>Use code: ABC123</b>
                </div>
                <div className='card-action'>
                    <a className='btn' href='/'>Explore</a>
                </div>
            </div>
            </div>
        </div>
    );
}

export default StoreCard;