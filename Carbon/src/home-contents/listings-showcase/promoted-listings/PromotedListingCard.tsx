import React from 'react';

import PriceTag from '../../../shared/PriceTag';
import InfoTag from '../../../shared/InfoTag';

const PromotedListingCard = () => {
    return (
        <div className='col l3 m6 s6'>
            <div className='card'>
                <div className='card-image'>
                    <img src='https://d1q0twczwkl2ie.cloudfront.net/wp-content/uploads/2016/09/12105964_1000216096695458_1007538799526354232_n.jpg'/>
                    <a className='btn-floating halfway-fab'><i className='material-icons'>turned_in</i></a>
                    <PriceTag />
                </div>
                <div className='card-content'>
                    <span className='card-title'>Item name is very long for the card, and even longer.</span>
                    <InfoTag />
                </div>
            </div>
        </div>
    );
}

export default PromotedListingCard;