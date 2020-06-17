import React from 'react';
import { connect } from 'react-redux';
import './style.css';

import SearchBar from '../../shared/SearchBar';
import { Paper } from '@material-ui/core';
import CarbonAvatar from '../../shared/CarbonAvatar';
import AccountPane from './AccountPane';
import PreferencePane from './PreferencePane';

const HidroAccount = () => {
    return (
        <div className='account-wrapper'>
            <Paper style={{ paddingTop:'20px' }}>
                <SearchBar />
            </Paper>

            <hr style={{ width:'25%',border:'1px solid #49A9D6',margin:'30px auto' }} />

            <div className='account-content'>
                <Paper className='content-container'>
                    <h5>
                        <CarbonAvatar size='35px' />
                        &nbsp;&nbsp;Your Account
                    </h5>
                    <hr />
                    <div className='row'>
                        <AccountPane />
                        <PreferencePane />
                    </div>
                </Paper>
            </div>
        </div>
    );
}

export default HidroAccount;