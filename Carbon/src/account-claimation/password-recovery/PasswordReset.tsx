import React from 'react';
import { connect } from 'react-redux';
import _ from 'lodash';
import '../style.css';

import ReCAPTCHA from 'react-google-recaptcha';
import CarbonInform from '../../shared/CarbonInform';
import CarbonAlert from '../../shared/CarbonAlert';
import CarbonPreloader from '../../shared/CarbonPreloader';
import { IStatus } from '../../shared/CarbonAlert';
import { CONSTANTS } from '../../helpers/helper';

import { extractUrlParameters } from '../../helpers/helper';
import { resetPassword } from './redux/actions';
import {
    validateEmail, validatePasswordForResult,
    displayPasswordResetMessages } from '../utility';

const mapStateToProps = (state: any) => ({
    requestSending : state.RecoveryStore.resetPwRequestSending,
    requestSuccess : state.RecoveryStore.resetPwRequestSuccess,
    recoveryResult : state.RecoveryStore.resetPwResult
});

const mapDispatchToProps = {
    resetPassword
};

const PasswordReset = (props: any) => {
    const [resetter, setResetter] = React.useState({
        email : CONSTANTS.EMPTY,
        tempPassword : CONSTANTS.EMPTY,
        password : CONSTANTS.EMPTY,
        passwordConfirm : CONSTANTS.EMPTY,
        recoveryToken : CONSTANTS.EMPTY,
        captchaToken : CONSTANTS.EMPTY
    });

    const [pageError, setPageError] = React.useState({
        error : false,
        message : CONSTANTS.EMPTY
    });

    const [shouldEnableSubmit, setShouldEnableSubmit] = React.useState(false);
    const [status, setStatus] = React.useState({ messages : CONSTANTS.EMPTY, type : CONSTANTS.EMPTY } as IStatus);

    React.useEffect(() => {
        let url = window.location.href;
        let error = { page : false, params : false };
        let urlParams: any = {};

        try {
            urlParams = extractUrlParameters(url, ['email', 'token']);
            error.params = !validateEmail(urlParams.email) || _.isEmpty(urlParams.token);
        } catch { error.page = true; }

        if (error.page || error.params)
            setPageError({
                error: true,
                message : 'The recovery link is invalid. We\'re unable to retrieve your account details. Please request another Password Recovery email.'
            });
        else
            setResetter({
                ...resetter,
                email : urlParams.email,
                recoveryToken : urlParams.token
            });
    }, []);

    React.useEffect(() => {
        let result = validatePasswordForResult(resetter.password, resetter.passwordConfirm);
        if ((_.isArray(result) && result.length === 0) || _.isString(result))
            setStatus({ messages: CONSTANTS.EMPTY, type : CONSTANTS.EMPTY });
        else {
            setStatus({ messages: result, type: 'error' });
            setShouldEnableSubmit(false);
            return;
        }
        
        setShouldEnableSubmit(
            !_.isEmpty(resetter.tempPassword) &&
            !_.isEmpty(resetter.captchaToken)
        );
    }, [resetter]);

    React.useEffect(() => {
        setShouldEnableSubmit(
            !_.isEmpty(resetter.tempPassword) &&
            !_.isEmpty(resetter.captchaToken) && !props.requestSending
        );

        if (props.requestSending) setStatus({ messages : CONSTANTS.EMPTY, type : CONSTANTS.EMPTY });
        if (!props.requestSending && !props.requestSuccess &&
            !_.isEmpty(props.recoveryResult) && props.recoveryResult.hasOwnProperty('stack'))
            setStatus({ messages : 'We\'re unable to send your request to server due to a connection lost. Please check your network.', type : 'error' });
        
        let result = props.recoveryResult;
        displayPasswordResetMessages(result, setStatus);
    }, [props]);

    const sendResetPasswordRequest = () => {
        const { resetPassword } = props;
        resetPassword(resetter);
    }

    const setResetterAsync = (field: string, value: string) => {
        if (field === 'temp') setResetter({ ...resetter, tempPassword : value });
        if (field === 'password') setResetter({ ...resetter, password : value });
        if (field === 'confirm') setResetter({ ...resetter, passwordConfirm : value });
        if (field === 'captcha') setResetter({ ...resetter, captchaToken : value });
    }

    if (pageError.error) return <CarbonInform message={ pageError.message } />;

    return (
        <div className='claimation-wrapper'>
            <CarbonAlert { ...status } />
            <h5>Hello! { resetter.email }</h5>
            <p>Please fill in the below form to reset your password.</p>

            <div className='row'>
                <div className='input-field col m6 s12'>
                    <i className='material-icons prefix hidro-primary-icon'>lock_open</i>
                    <input id='temp-password' type='password' value={ resetter.tempPassword }
                        onChange={ (e: any) => { setResetterAsync('temp', e.target.value); }} />
                    <label htmlFor='temp-password'>Temporary Password</label>
                </div>
                <div className='col m12 s0'></div>
                <div className='input-field col m6 s12'>
                    <i className='material-icons prefix hidro-primary-icon'>lock</i>
                    <input id='password' type='password' value={ resetter.password }
                        onChange={ (e: any) => { setResetterAsync('password', e.target.value); }} />
                    <label htmlFor='password'>New Password</label>
                </div>
                <div className='input-field col m6 s12'>
                    <i className='material-icons prefix hidro-primary-icon'>confirmation_number</i>
                    <input id='confirm-password' type='password' value={ resetter.passwordConfirm }
                        onChange={ (e: any) => { setResetterAsync('confirm', e.target.value); }} />
                    <label htmlFor='password'>Confirm New Password</label>
                </div>
                <div className='col s12'>
                    <div className='row'>
                        <div className='col s12'>
                            <ReCAPTCHA
                                sitekey='6LeXhN4UAAAAAHKW6-44VxtUVMYSlMPj04WRoC8z'
                                onChange={ (e: any) => setResetterAsync('captcha', e) } />
                        </div>
                        <div className='col s12'>
                            <button className='btn' style={{ marginTop:'20px' }}
                                onClick={ sendResetPasswordRequest } disabled={ !shouldEnableSubmit }>
                                Submit
                            </button>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    );
}

export default connect(
    mapStateToProps,
    mapDispatchToProps
)(PasswordReset);