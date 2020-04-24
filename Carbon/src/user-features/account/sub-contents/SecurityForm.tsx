import React from 'react';

import ReCAPTCHA from 'react-google-recaptcha';

import { ISecurityForm } from '../redux/constants';

const SecurityForm = (props: ISecurityForm) => {
    return (
        <div className='row'>
            <div className='input-field col s12'>
                <div className='input-field col s12'>
                    <i className='material-icons prefix hidro-primary-icon'>lock</i>
                    <input id='current-password' type='password' value={ props.passwordForm.password }
                        onChange={ (e: any) => props.updatePassword('password', e.target.value) } />
                    <label htmlFor='current-password'>Current Password</label>
                </div>
                <div className='input-field col s12'>
                    <i className='material-icons prefix hidro-primary-icon'>lock</i>
                    <input id='new-password' type='password' value={ props.passwordForm.newPassword }
                        onChange={ (e: any) => props.updatePassword('newpw', e.target.value) } />
                    <label htmlFor='new-password'>New Password</label>
                </div>
                <div className='input-field col s12'>
                    <i className='material-icons prefix hidro-primary-icon'>lock</i>
                    <input id='confirm-password' type='password' value={ props.passwordForm.passwordConfirm }
                        onChange={ (e: any) => props.updatePassword('confirm', e.target.value) } />
                    <label htmlFor='confirm-password'>Confirm New Password</label>
                </div>
                <div className='col s12'>
                    <ReCAPTCHA
                        sitekey='6LeXhN4UAAAAAHKW6-44VxtUVMYSlMPj04WRoC8z'
                        onChange={ (v: string) => props.updatePassword('captcha', v) } />
                </div>
                <div className='col s12' style={{ marginTop:'15px' }}>
                    <button className='btn' onClick={ () => props.savePassword() }>Update</button>
                    <button className='btn right grey darken-1' onClick={ () => props.setIsUpdating(false) }>Cancel</button>
                </div>
            </div>
        </div>
    );
}

export default SecurityForm;