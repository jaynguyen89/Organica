import React from 'react';
import './style.css';
import M from 'materialize-css';
import $ from 'jquery';

import PromotedListing from './promoted-listings/PromotedListing';
import NearbyListing from './nearby-listings/NearbyListing';

const ListingShowcase = () => {
    React.useEffect(() => {
        M.Tabs.init($('.tabs'), {});
    }, []);

    return (
        <div className='showcase-wrapper'>
            <div className='row'>
                <div className='col s12'>
                    <ul className='tabs'>
                        <li className='tab col s6'><a href='#promoted-listings'>Promoted Listings</a></li>
                        <li className='tab col s6'><a href='#nearby-listings'>Near-by Listings</a></li>
                    </ul>
                </div>

                <div className='col s12' id='promoted-listings'>
                    <PromotedListing />
                </div>
                <div className='col s12' id='nearby-listings'>
                    <NearbyListing />
                </div>
            </div>
        </div>
    );
}

export default ListingShowcase;