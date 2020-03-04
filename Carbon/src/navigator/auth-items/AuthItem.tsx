import React from 'react';
import './style.css';

const AuthItem = () => {
    return (
        <>
            <li><a className='btn' href='/carbon-signin'>Sign in</a></li>
            <li><a className='btn' href='/carbon-register'>Register</a></li>
        </>
    );
}

export default AuthItem;