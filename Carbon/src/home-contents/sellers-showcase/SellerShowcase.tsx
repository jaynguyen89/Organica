import React from 'react';
//import './style.css';
import M from 'materialize-css';
import $ from 'jquery';

import FeaturedStore from './featured-stores/FeaturedStore';
import TopDeal from './top-deals/TopDeal';

const SellerShowcase = () => {
    React.useEffect(() => {
        M.Tabs.init($('.tabs'), {});
    }, []);

    return (
        <div className='showcase-wrapper'>
            <div className='row'>
                <div className='col s12'>
                    <ul className='tabs'>
                        <li className='tab col s6'><a href='#featured-stores'>Featured Stores</a></li>
                        <li className='tab col s6'><a href='#top-deals'>Today's Deals</a></li>
                    </ul>
                </div>

                <div className='col s12' id='featured-stores'>
                    <FeaturedStore />
                </div>
                <div className='col s12' id='top-deals'>
                    <TopDeal />
                </div>
            </div>
        </div>
    );
}

export default SellerShowcase;