import React from 'react';

import AccountShow from './sub-contents/AccountShow';
import SecurityPane from './sub-contents/SecurityPane';

const AccountPane = () => {
    return (
        <div className='row'>
            <h6 className='content-caption'>
                <i className='fas fa-user-circle hidro-primary-icon'></i>&nbsp;&nbsp;Signup Information
            </h6>
            <div className='col l6 m12'>
                <AccountShow />
            </div>
            <div className='col l6 m12'>
                <SecurityPane />
            </div>
            <div className='col s12'>
                <hr style={{ width:'100%' }} />
                <div className='row'>
                    
                    <div className='col m3 s6'>
                        <p className='account-show'><b>Signup On:</b> 13 Feb 2020</p>
                    </div>
                    <div className='col m3 s6'>
                        <p className='account-show'><b>Last Update:</b> 10 Mar 2020</p>
                    </div>
                    <div className='col m3 s6'>
                        <p className='account-show'><b>Last Online:</b> 18 Mar 2020 19:31</p>
                    </div>
                    <div className='col m3 s6'>
                        <p className='account-show'><b>Last Signout:</b> 19 Mar 2020 01:44</p>
                    </div>
                </div>
            </div>
        </div>
    );
}

export default AccountPane;