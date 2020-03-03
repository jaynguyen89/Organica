import React from 'react';
import '../style.css';

const SocialSignIn = () => {
    return (
        <div className='col m6 s12'>
            <h4>&nbsp;</h4>
            <div className='row social-row'>
                <h5>Or, sign in with:</h5>
                <div className='row'>
                    <div className='col s4'>
                        <i className='fab fa-facebook fa-5x social-icon fb-color'></i>
                    </div>
                    <div className='col s4'>
                        <i className='fab fa-google-plus fa-5x social-icon gg-color'></i>
                    </div>
                </div>
            </div>
        </div>
    );
}

export default SocialSignIn;