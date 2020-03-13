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

import { validateEmail, validateEmailForResult } from '../utility';
import { extractUrlParameters } from '../../helpers/helper';
import { requestForgotPassword } from './redux/actions';

const mapStateToProps = (state: any) => ({
    requestSending : state.RecoveryStore.forgotPwRequestSending,
    requestSuccess : state.RecoveryStore.forgotPwRequestSuccess,
    recoveryResult : state.RecoveryStore.forgotPwResult
});

const mapDispatchToProps = {
    requestForgotPassword
};

const ForgotPassword = (props: any) => {
    const [recovery, setRecovery] = React.useState({
        email : CONSTANTS.EMPTY,
        captchaToken : CONSTANTS.EMPTY,
        reattempt : false
    });

    const [pageError, setPageError] = React.useState({
        error : false,
        message : CONSTANTS.EMPTY
    });

    const [shouldShowEmailInput, setShouldShowEmailInput] = React.useState(true);
    const [status, setStatus] = React.useState({ messages : CONSTANTS.EMPTY, type : CONSTANTS.EMPTY } as IStatus);

    React.useEffect(() => {
        let urlQuery = props.location.search;
        setShouldShowEmailInput(urlQuery === CONSTANTS.EMPTY);

        if (urlQuery !== CONSTANTS.EMPTY) {
            let error = { page : false, email : false };
            let urlParams: any = {};

            try {
                urlParams = extractUrlParameters(urlQuery, ['email', 'reattempt']);
                error.email = !validateEmail(urlParams.email) || urlParams.reattempt === undefined;
                setRecovery({
                    ...recovery,
                    email : urlParams.email,
                    reattempt : urlParams.reattempt === '1'
                });
            } catch { error.page = true; }

            if (error.page || error.email)
                setPageError({
                    error: true,
                    message : 'The request link is invalid. We\'re unable to retrieve your account details. Please contact Hidrogen support to help with your case.'
                });
        }
    }, []);

    React.useEffect(() => {
        if (shouldShowEmailInput) {
            var result = validateEmailForResult(recovery.email);
            if ((_.isArray(result) && result.length === 0) || _.isString(result))
                setStatus({ messages: CONSTANTS.EMPTY, type : CONSTANTS.EMPTY });
            else
                setStatus({ messages: result, type: 'error' });
        }

        if (!validateEmail(recovery.email) && recovery.captchaToken) {
            const { requestForgotPassword } = props;
            requestForgotPassword(recovery);
        }
    }, [recovery]);

    React.useEffect(() => {
        if (!props.requestSending &&
            props.recoveryResult !== null &&
            !_.isEmpty(props.recoveryResult) &&
            props.recoveryResult.result === CONSTANTS.SUCCESS) {
                alert("Congratulation! An email has been sent to you with instruction on resetting password. Please check your inbox.");
                window.location.href = '/';
            }
    }, [props]);

    const setRecoveryAsync = (field: string, value: string) => {
        if (field === 'email') setRecovery({ ...recovery, email : value });
        if (field === 'captcha') setRecovery({ ...recovery, captchaToken : value });
    }

    if (pageError.error) return <CarbonInform message={ pageError.message } />;

    return (
        <div className='claimation-wrapper'>
            <CarbonAlert { ...status } />
            <h5>Hello! { recovery.email || 'Hidrogenian.' }</h5>
            {
                (
                    recovery.email && <p>Please confirm ReCaptcha to reset your password.</p>
                ) || <p>Please enter your registration email and confirm ReCaptcha to reset your password.</p>
            }

            <div className='row'>
                {
                    (
                        shouldShowEmailInput &&
                        <div className='input-field col s6'>
                            <i className='material-icons prefix hidro-primary-icon'>alternate_email</i>
                            <input id='email' type='text' value={ recovery.email }
                                onChange={ (e: any) => { setRecoveryAsync('email', e.target.value); }} />
                            <label htmlFor='email'>Email address</label>
                        </div>
                    )
                }

                <ReCAPTCHA
                    sitekey='6LeXhN4UAAAAAHKW6-44VxtUVMYSlMPj04WRoC8z'
                    onChange={ (e: any) => setRecoveryAsync('captcha', e) } />
                
                {
                    props.requestSending &&
                    <>
                        <p>Sending Password Recovery email...</p>
                        <CarbonPreloader size='big' />
                    </>
                }

                {
                    !props.requestSending &&
                    props.recoveryResult !== null &&
                    !_.isEmpty(props.recoveryResult) &&
                    props.recoveryResult.hasOwnProperty('hostName') &&
                    <>
                        <h5 className='claimation-error'>ReCaptcha failed.</h5>
                        <p>Failed to check your humanity. Please verify the ReCaptcha again.</p>
                    </>
                }

                {
                    (
                        !props.requestSending && props.requestSuccess &&
                        props.recoveryResult.result !== CONSTANTS.SUCCESS &&
                        !props.recoveryResult.hasOwnProperty('Error') &&
                        <>
                            <h5 className='claimation-error'>Request failed.</h5>
                            <p>{ props.recoveryResult.message }</p>
                        </>
                    ) || (
                        !props.requestSending && props.requestSuccess &&
                        props.recoveryResult.result !== CONSTANTS.SUCCESS &&
                        props.recoveryResult.hasOwnProperty('Error') &&
                        <>
                            <h5 className='claimation-error'>Error { props.recoveryResult.error }</h5>
                            <p>A problem has occurred while preparing your account for password recovering. Please try again later.</p>
                        </>
                    ) || (
                        !props.requestSending && !props.requestSuccess && !_.isEmpty(props.recoveryResult) && props.recoveryResult.hasOwnProperty('stack') &&
                        <>
                            <h5 className='claimation-error'>{ props.recoveryResult.message }</h5>
                            <p>We're unable to send your request to server due to a connection lost. Please check your network.</p>
                        </>
                    )
                }
            </div>
        </div>
    );
}

export default connect(
    mapStateToProps,
    mapDispatchToProps
)(ForgotPassword);