import React from 'react';
import { connect } from 'react-redux';
import _ from 'lodash';
import '../style.css';

import ReCAPTCHA from 'react-google-recaptcha';
import CarbonInform from '../../shared/CarbonInform';
import CarbonPreloader from '../../shared/CarbonPreloader';
import { CONSTANTS } from '../../helpers/helper';

import { validateEmail } from '../utility';
import { extractUrlParameters } from '../../helpers/helper';
import { activateHidrogenian } from './redux/actions';

const mapStateToProps = (state: any) => ({
    requestSending : state.ActivationStore.activationRequestSending,
    requestSuccess : state.ActivationStore.activationRequestSuccess,
    activationResult : state.ActivationStore.activationResult
});

const mapDispatchToProps = {
    activateHidrogenian
};

const AccountClaimer = (props: any) => {
    const [activator, setActivator] = React.useState({
        email : CONSTANTS.EMPTY,
        activationToken : CONSTANTS.EMPTY,
        captchaToken : CONSTANTS.EMPTY
    });

    const [pageError, setPageError] = React.useState({
        error : false,
        message : CONSTANTS.EMPTY
    });

    React.useEffect(() => {
        let url = window.location.href;
        let error = { page : false, email : false };
        let urlParams: any = {};

        try {
            urlParams = extractUrlParameters(url, ['email', 'token']);
            error.email = !validateEmail(urlParams.email);
            setActivator({
                ...activator,
                email : urlParams.email,
                activationToken : urlParams.token
            });
        } catch { error.page = true; }

        if (error.page || error.email)
            setPageError({
                error: true,
                message : 'The activation link is invalid. We\'re unable to retrieve your account details. Please request another Account Activation email.'
            });
    }, []);

    React.useEffect(() => {
        if (!pageError.error && activator.captchaToken) {
            const { activateHidrogenian } = props;
            activateHidrogenian(activator);
        }
    }, [activator]);

    React.useEffect(() => {
        if (!props.requestSending &&
            props.activationResult !== null &&
            !_.isEmpty(props.activationResult) &&
            props.activationResult.result === CONSTANTS.SUCCESS) {
                alert("Congratulation! Your account has been activated.");
                window.location.href = '/';
            }
    }, [props]);

    const setReCaptcha = (value: string) => {
        setActivator({
            ...activator,
            captchaToken : value
        });
    }

    if (pageError.error) return <CarbonInform message={ pageError.message } />;

    return (
        <div className='claimation-wrapper'>
            {
                (
                    activator.email === CONSTANTS.EMPTY &&
                    (props.activationResult === null || _.isEmpty(props.activationResult)) &&
                    <>
                        <h5>Please wait while we are retrieving your information...</h5>
                        <CarbonPreloader size='big' />
                    </>
                ) ||
                <>
                    <h5>Hello! { activator.email }</h5>
                    <p>Please confirm ReCaptcha to activate your account</p>
                    <ReCAPTCHA
                        sitekey='6LeXhN4UAAAAAHKW6-44VxtUVMYSlMPj04WRoC8z'
                        onChange={ setReCaptcha } />
                </>
            }

            {
                props.requestSending &&
                <>
                    <p>Activating your account...</p>
                    <CarbonPreloader size='big' />
                </>
            }

            {
                !props.requestSending &&
                props.activationResult !== null &&
                !_.isEmpty(props.activationResult) &&
                props.activationResult.hasOwnProperty('hostName') &&
                <>
                    <h5 className='claimation-error'>ReCaptcha failed.</h5>
                    <p>Failed to check your humanity. Please verify the ReCaptcha again.</p>
                </>
            }

            {
                (
                    !props.requestSending && props.requestSuccess &&
                    props.activationResult.result !== CONSTANTS.SUCCESS &&
                    !props.activationResult.hasOwnProperty('Error') &&
                    <>
                        <h5 className='claimation-error'>Activation failed.</h5>
                        <p>{ props.activationResult.message }</p>
                    </>
                ) || (
                    !props.requestSending && props.requestSuccess &&
                    props.activationResult.result !== CONSTANTS.SUCCESS &&
                    props.activationResult.hasOwnProperty('Error') &&
                    <>
                        <h5 className='claimation-error'>Error { props.activationResult.error }</h5>
                        <p>A problem has occurred while attempting to activate your account. Please try again later.</p>
                    </>
                ) || (
                    !props.requestSending && !props.requestSuccess && !_.isEmpty(props.recoveryResult) && props.activationResult.hasOwnProperty('stack') &&
                    <>
                        <h5 className='claimation-error'>{ props.activationResult.message }</h5>
                        <p>We're unable to send activation data to server due to a connection lost. Please check your network.</p>
                    </>
                )
            }
        </div>
    );
}

export default connect(
    mapStateToProps,
    mapDispatchToProps
)(AccountClaimer);