import React from 'react';

import { Paper } from '@material-ui/core';

const SecurityPane = () => {
    return (
        <Paper className='content-container'>
            <div className='row'>
                <div className='col m6 s12'>
                    <p><b>Password</b></p>
                    <button className='btn'>Update Password</button>
                </div>
                <div className='col m6 s12'>
                    <p><b>Two-Factor Authentication</b></p>
                    <button className='btn'>Enable Two-Factor</button>
                </div>
                <div className='col s12'>
                    <p style={{ marginBottom:0 }}><b>Open a store</b></p>
                    <p className='account-show'>
                        Let's start the first step to create an awesome store on Hidrogen.
                    </p>
                    <button className='btn'>Open Store&nbsp;&nbsp;<i className='fas fa-external-link-alt'></i></button>
                </div>
            </div>
        </Paper>
    );
}

export default SecurityPane;