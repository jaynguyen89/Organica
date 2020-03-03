import React from 'react';
import './style.css';

import UsualSignUp from './signup-components/UsualSignUp';
import SocialSignUp from './signup-components/SocialSignUp';

const CarbonRegister = () => {
    return (
        <div className='register-wrapper'>
            <UsualSignUp />
            <SocialSignUp />
        </div>
    );
}

export default CarbonRegister;