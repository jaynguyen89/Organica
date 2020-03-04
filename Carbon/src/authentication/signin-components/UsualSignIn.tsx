import React from 'react';
import '../style.css';

import ReCAPTCHA from 'react-google-recaptcha';

const UsualSignIn = () => {

    const reCaptchaClicked = (value : string) => {
        console.log(value);
    }

    return (
        <div className='col l8 m12'>
            <h4><i className='fas fa-user-circle'></i>&nbsp;Sign in</h4>
            <div className='card'>
                <div className='card-content'>
                    <div className='row'>
                        <div className='input-field col s12'>
                            <i className='material-icons prefix'>account_box</i>
                            <input id='username' type='text' className='validate' />
                            <label htmlFor='username'>Username</label>
                        </div>
                        <div className='input-field col s12'>
                            <i className='material-icons prefix'>lock</i>
                            <input id='password' type='password' className='validate' />
                            <label htmlFor='password'>Password</label>
                        </div>
                        <div className='col s12'>
                            <div className='row'>
                                <div className='col s12'>
                                    <ReCAPTCHA
                                        sitekey='6LeXhN4UAAAAAHKW6-44VxtUVMYSlMPj04WRoC8z'
                                        onChange={ reCaptchaClicked } />
                                </div>
                                <div className='col s12'>
                                    <a className='btn' style={{ marginTop:'20px' }}>Sign in</a>
                                    <p className='signin-assistance'>
                                        Forgot your password? <a href='/'>Reset</a> now.
                                    </p>
                                    <p>
                                        Problem signing into your account? <a href='/'>Contact</a> us.
                                    </p>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    );
}

export default UsualSignIn;