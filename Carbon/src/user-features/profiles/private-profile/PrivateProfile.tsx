import React from 'react';
import { connect } from 'react-redux';
import './style.css';

import SearchBar from '../../../shared/SearchBar';
import { Paper } from '@material-ui/core';
import CarbonAvatar from '../../../shared/CarbonAvatar';
import BiographyPane from './BiographyPane';
import AddressPane from './AddressPane';

const PrivateProfile = (props: any) => {
    return (
        <div className='profile-wrapper'>
            <Paper style={{ paddingTop:'20px' }}>
                <SearchBar />
            </Paper>

            <hr style={{ width:'25%',border:'1px solid #49A9D6',margin:'30px auto' }} />
            
            <div className='profile-content'>
                <Paper className='content-container'>
                    <h5>
                        <CarbonAvatar size='35px' />
                        &nbsp;&nbsp;Your Profile
                    </h5>
                    
                    <div className='row'>
                        <BiographyPane />
                        <AddressPane />
                    </div>
                </Paper>
            </div>
        </div>
    );
}

export default PrivateProfile;