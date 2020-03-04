import React from 'react';
import ReCAPTCHA from 'react-google-recaptcha';

const UsualSignUp = () => {

    const reCaptchaClicked = (value : string) => {
        console.log(value);
    }

    return (
        <>
            <h4><i className='fas fa-user-plus'></i>&nbsp;Create your account</h4>
            <div className='card'>
                <div className='card-content'>
                    <div className='row'>
                        <div className='input-field col m6 s12'>
                            <i className='material-icons prefix'>account_box</i>
                            <input id='username' type='text' className='validate' />
                            <label htmlFor='username'>Username</label>
                        </div>
                        <div className='input-field col m6 s12'>
                            <i className='material-icons prefix'>alternate_email</i>
                            <input id='email' type='text' className='validate' />
                            <label htmlFor='email'>Email address</label>
                        </div>
                        <div className='input-field col m6 s12'>
                            <i className='material-icons prefix'>lock</i>
                            <input id='password' type='password' className='validate' />
                            <label htmlFor='password'>Password</label>
                        </div>
                        <div className='input-field col m6 s12'>
                            <i className='material-icons prefix'>lock</i>
                            <input id='confirm' type='text' className='validate' />
                            <label htmlFor='confirm'>Confirm password</label>
                        </div>
                        <div className='input-field col m6 s12'>
                            <i className='material-icons prefix'>assignment_ind</i>
                            <input id='family-name' type='text' className='validate' />
                            <label htmlFor='family-name'>Family Name</label>
                        </div>
                        <div className='input-field col m6 s12'>
                            <i className='material-icons prefix'>assignment_ind</i>
                            <input id='given-name' type='text' className='validate' />
                            <label htmlFor='given-name'>Given Name</label>
                        </div>
                        <div className='col s12'>
                            <div className='row'>
                                <div className='col s12'>
                                    <ReCAPTCHA
                                        sitekey='6LeXhN4UAAAAAHKW6-44VxtUVMYSlMPj04WRoC8z'
                                        onChange={ reCaptchaClicked } />
                                </div>
                                <div className='col s12'>
                                    <a className='btn' style={{ marginTop:'20px' }}>Submit</a>
                                </div>
                            </div>
                            
                            
                        </div>
                    </div>
                </div>
            </div>
        </>
    );
}

export default UsualSignUp;