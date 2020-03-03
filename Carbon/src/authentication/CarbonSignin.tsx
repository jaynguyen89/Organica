import React from 'react';
import './style.css';

import UsualSignIn from './signin-components/UsualSignIn';
import SocialSignIn from './signin-components/SocialSignIn';

const CarbonSignIn = () => {
    return (
        <div className='signin-wrapper'>
            <div className='row'>
                <UsualSignIn />
                <SocialSignIn />
            </div>
        </div>
    );
}

export default CarbonSignIn;