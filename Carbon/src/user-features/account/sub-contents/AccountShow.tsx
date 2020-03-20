import React from 'react';
import $ from 'jquery';
import M from 'materialize-css';

import { Paper } from '@material-ui/core';

const AccountShow = () => {
    React.useEffect(() => {
        M.Tooltip.init($('.tooltipped'), {});
    }, []);

    return (
        <Paper className='content-container'>
            <div className='row'>
                <div className='col s12'>
                    <p>
                        <b>Identity</b>
                        <i className='fas fa-edit tooltipped link-icon' data-position='right' data-tooltip='Update payments'></i>
                    </p>
                    <p className='account-show'>
                        <b>Email:</b> nguyen.le.kim.phuc@gmail.com
                    </p>
                </div>
                <div className='col m6 s12'>
                    <p className='account-show'>
                        <b>Username:</b> nlkp
                    </p>
                </div>
                <div className='col m6 s12'>
                    <p className='account-show'>
                        <b>Phone Number:</b> 0422 357 488
                    </p>
                </div>
                <div className='col s12'>
                    <p>
                        <b>Payment Types</b>
                        <i className='fas fa-edit tooltipped link-icon' data-position='right' data-tooltip='Update payments'></i>
                    </p>
                    <p className='account-show'><b>Card:</b> **** **** **** *234</p>
                    <p className='account-show'><b>Paypal:</b> nguyen.le.kim.phuc@gmail.com</p>
                </div>
            </div>
        </Paper>
    );
}

export default AccountShow;