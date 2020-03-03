import React from 'react';

import TopDealCard from './TopDealCard';

const TopDeal = () => {
    return (
        <div className='listings-wrapper'>
            <div className='row'>
                <TopDealCard />
                <TopDealCard />
                <TopDealCard />
                <TopDealCard />
                <TopDealCard />
                <TopDealCard />
                <TopDealCard />
            </div>
            <div className='row' style={{ textAlign:'center' }}>
                <button className='btn'>Show more</button>
            </div>
        </div>
    );
}

export default TopDeal;