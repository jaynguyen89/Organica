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
import { requestNewActivator } from './redux/actions';

const mapStateToProps = (state: any) => ({
    requestSending : state.ActivationStore.activatorRequestSending,
    requestSuccess : state.ActivationStore.activatorRequestSuccess,
    activatorResult : state.ActivationStore.activatorResult
});

const mapDispatchToProps = {
    requestNewActivator
};

const RequestActivator = (props: any) => {
    const [requester, setRequester] = React.useState({
        email : CONSTANTS.EMPTY,
        captchaToken : CONSTANTS.EMPTY
    });

    const [pageError, setPageError] = React.useState({
        error : false,
        message : CONSTANTS.EMPTY
    });

    const [shouldShowEmailInput, setShouldShowEmailInput] = React.useState(false);
    const [status, setStatus] = React.useState({ messages : CONSTANTS.EMPTY, type : CONSTANTS.EMPTY } as IStatus);

    React.useEffect(() => {
        let url = window.location.href;
        let error = false;
        let urlParams: any = {};

        try {
            urlParams = extractUrlParameters(url, ['email']);
            setRequester({ ...requester, email : urlParams.email });
        } catch { error = true; }

        setPageError({
            error: error,
            message : 'The request link is invalid. We\'re unable to retrieve your account details. Please request another Account Activation email.'
        });

        try {
            error = !validateEmail(urlParams.email);
        } catch { error = true; }
        
        if (error) setRequester({ ...requester, email : CONSTANTS.EMPTY });
        setShouldShowEmailInput(error);
    }, []);

    React.useEffect(() => {
        if (shouldShowEmailInput) {
            var result = validateEmailForResult(requester.email);
            if ((_.isArray(result) && result.length === 0) || _.isString(result))
                setStatus({ messages: CONSTANTS.EMPTY, type : CONSTANTS.EMPTY });
            else
                setStatus({ messages: result, type: 'error' });
        }
        
        if (!validateEmail(requester.email) && requester.captchaToken) {
            const { requestNewActivator } = props;
            requestNewActivator(requester);
        }
    }, [requester]);

    React.useEffect(() => {
        if (!props.requestSending &&
            props.activatorResult !== null &&
            !_.isEmpty(props.activatorResult) &&
            props.activatorResult.result === CONSTANTS.SUCCESS) {
                alert("Congratulation! An email with new activation link has been sent to you. Please check your inbox.");
                window.location.href = '/';
            }
    }, [props]);

    const setActivatorAsync = (field: string, value: string) => {
        if (field === 'email') setRequester({ ...requester, email : value });
        if (field === 'captcha') setRequester({ ...requester, captchaToken : value });
    }

    if (pageError.error) return <CarbonInform message={ pageError.message } />;

    return (
        <div className='claimation-wrapper'>
            <CarbonAlert { ...status } />
            <h5>Hello! { requester.email || 'Hidrogenian.' }</h5>
            {
                (
                    requester.email && <p>Please confirm ReCaptcha then we will send you a new Account Activation email.</p>
                ) || <p>Please provide the email associated with your account and confirm ReCaptcha to get a new Account Activation email.</p>
            }

            <div className='row'>
                {
                    (
                        shouldShowEmailInput &&
                        <div className='input-field col s6'>
                            <i className='material-icons prefix hidro-primary-icon'>alternate_email</i>
                            <input id='email' type='text' value={ requester.email }
                                onChange={ (e: any) => { setActivatorAsync('email', e.target.value); }} />
                            <label htmlFor='email'>Email address</label>
                        </div>
                    )
                }

                <ReCAPTCHA
                    sitekey='6LeXhN4UAAAAAHKW6-44VxtUVMYSlMPj04WRoC8z'
                    onChange={ (e: any) => setActivatorAsync('captcha', e) } />

                {
                    props.requestSending &&
                    <>
                        <p>Sending new Activation Account email...</p>
                        <CarbonPreloader size='big' />
                    </>
                }

                {
                    !props.requestSending &&
                    props.activatorResult !== null &&
                    !_.isEmpty(props.activatorResult) &&
                    props.activatorResult.hasOwnProperty('hostName') &&
                    <>
                        <h5 className='claimation-error'>ReCaptcha failed.</h5>
                        <p>Failed to check your humanity. Please verify the ReCaptcha again.</p>
                    </>
                }

                {
                    (
                        !props.requestSending && props.requestSuccess &&
                        props.activatorResult.result !== CONSTANTS.SUCCESS &&
                        !props.activatorResult.hasOwnProperty('Error') &&
                        <>
                            <h5 className='claimation-error'>Request failed.</h5>
                            <p>{ props.activatorResult.message }</p>
                        </>
                    ) || (
                        !props.requestSending && props.requestSuccess &&
                        props.activatorResult.result !== CONSTANTS.SUCCESS &&
                        props.activatorResult.hasOwnProperty('Error') &&
                        <>
                            <h5 className='claimation-error'>Error { props.activatorResult.error }</h5>
                            <p>A problem has occurred while preparing your account for a new activation. Please try again later.</p>
                        </>
                    ) || (
                        !props.requestSending && !props.requestSuccess && !_.isEmpty(props.recoveryResult) && props.activatorResult.hasOwnProperty('stack') &&
                        <>
                            <h5 className='claimation-error'>{ props.activatorResult.message }</h5>
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
)(RequestActivator);