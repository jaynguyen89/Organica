import React from 'react';

const InfoTag = () => {
    return (
        <>
            <div className='row'>
                <div className='col s12'>
                    <p className='listing-tag left'>Location</p>
                    <p className='listing-tag right'>2km</p>
                </div>
            </div>
            <div className='row'>
                <div className='col s12'>
                    <p className='listing-tag left'>12 watches - 4 left</p>
                    <p className='listing-tag right'>2 sold in 1 hour</p>
                </div>
            </div>
        </>
    );
}

export default InfoTag;