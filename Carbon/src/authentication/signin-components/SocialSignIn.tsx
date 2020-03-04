import React from 'react';
import '../style.css';

const SocialSignIn = () => {
    return (
        <div className='col l4 m12'>
            <h4>&nbsp;</h4>
            <div className='row social-row'>
                <h5>Or, sign in with:</h5>
                <div className='row'>
                    <div className='col l6 m4 s6'>
                        <i className='fab fa-facebook fa-5x social-icon fb-color'></i>
                    </div>
                    <div className='col l6 m4 s6'>
                        <i className='fab fa-google-plus fa-5x social-icon gg-color'></i>
                    </div>
                </div>
            </div>
        </div>
    );
}

export default SocialSignIn;