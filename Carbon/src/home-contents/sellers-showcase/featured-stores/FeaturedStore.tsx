import React from 'react';

import StoreCard from './StoreCard';

const FeaturedStore = () => {
    return (
        <div className='listings-wrapper'>
            <div className='row'>
                <StoreCard />
                <StoreCard />
                <StoreCard />
                <StoreCard />
            </div>
        </div>
    );
}

export default FeaturedStore;