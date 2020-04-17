import React from 'react';

const LocationAutoDetect = (props: any) => {
    return (
        <div className='col s12'>
            <button className='btn blue darken-4' style={{ marginTop:'10px' }}
                onClick={ () => props.detectAddress() }>
                <i className='fas fa-search-location'></i>&nbsp;&nbsp;Auto Detect My Address
            </button>
        </div>
    );
}

export default LocationAutoDetect;