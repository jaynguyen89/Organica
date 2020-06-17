import React from 'react';
import './style.css';

import { Paper } from '@material-ui/core';
import SearchBar from '../../shared/SearchBar';
import CarbonAvatar from '../../shared/CarbonAvatar';
import SellerRating from './sub-contents/ratings/SellerRating';
import BuyerFeedback from './sub-contents/feedbacks/BuyerFeedback';
import CashFlow from './sub-contents/cashflow/CashFlow';
import ActivitySummary from './sub-contents/activity-summary/ActivitySummary';

const MyPerformance = (props: any) => {
    return (
        <div className='performance-wrapper'>
            <Paper style={{ paddingTop:'20px' }}>
                <SearchBar />
            </Paper>

            <hr style={{ width:'25%',border:'1px solid #49A9D6',margin:'30px auto' }} />

            <div className='performance-content'>
                <Paper className='content-container'>
                    <h5>
                        <CarbonAvatar size='35px' />
                        &nbsp;&nbsp;Your Performance
                    </h5>
                    <hr />

                    <div className='row'>
                        <CashFlow />
                        <ActivitySummary />
                        <SellerRating />
                        <BuyerFeedback />
                    </div>
                </Paper>
            </div>
        </div>
    );
}

export default MyPerformance;