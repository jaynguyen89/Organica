import React from 'react';
import './style.css';

import { Paper } from '@material-ui/core';
import SearchBar from '../shared/SearchBar';
import YourReminder from './sub-contents/YourReminder';

const CustomerHome = (props: any) => {
    return (
        <div className='home-wrapper'>
            <Paper style={{ paddingTop:'20px' }}>
                <SearchBar />
            </Paper>

            <hr style={{ width:'25%',border:'1px solid #49A9D6',margin:'30px auto' }} />

            <div className='home-content'>
                <Paper className='content-container'>
                    <YourReminder />
                </Paper>
            </div>
        </div>
    );
}

export default CustomerHome;