import React from 'react';

import PromotedListingCard from './PromotedListingCard';

const PromotedListing = () => {
    return (
        <div className='listings-wrapper'>
            <div className='row'>
                <PromotedListingCard />
                <PromotedListingCard />
                <PromotedListingCard />
                <PromotedListingCard />
                <PromotedListingCard />
                <PromotedListingCard />
            </div>
            <div className='row' style={{ textAlign:'center' }}>
                <button className='btn'>Show more</button>
            </div>
        </div>
    );
}

export default PromotedListing;