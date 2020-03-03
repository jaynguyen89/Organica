import React from 'react';

import NearbyListingCard from './NearbyListingCard';

const NearbyListing = () => {
    return (
        <div className='listings-wrapper'>
            <div className='row'>
                <NearbyListingCard />
                <NearbyListingCard />
                <NearbyListingCard />
                <NearbyListingCard />
                <NearbyListingCard />
                <NearbyListingCard />
                <NearbyListingCard />
                <NearbyListingCard />
            </div>
        </div>
    );
}

export default NearbyListing;