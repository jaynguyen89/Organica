import React from 'react';
import { connect } from 'react-redux';
import _ from 'lodash';

import { CONSTANTS } from '../../helpers/helper';
import { IStatus } from '../../shared/CarbonAlert';
import ReCAPTCHA from 'react-google-recaptcha';
import CarbonAlert from '../../shared/CarbonAlert';
import CarbonPreloader from '../../shared/CarbonPreloader';

import {
    validateRegistrationInputs,
    highlightInputFields,
    displayUsualRegistrationResult } from './utility';
import { registerHidrogenian } from '../redux/actions';

const mapStateToProps = (state: any) => ({
    registrationType : state.RegistrationStore.registrationType,
    requestSending : state.RegistrationStore.requestSending,
    requestSuccess: state.RegistrationStore.requestSuccess,
    registrationResult : state.RegistrationStore.registrationResult,
});

const mapDispatchToProps = {
    registerHidrogenian
};

const UsualSignUp = (props : any) => {
    const [register, setRegister] = React.useState({
        username : CONSTANTS.EMPTY,
        email : CONSTANTS.EMPTY,
        password : CONSTANTS.EMPTY,
        passwordConfirm : CONSTANTS.EMPTY,
        familyName : CONSTANTS.EMPTY,
        givenName : CONSTANTS.EMPTY,
        captchaToken : CONSTANTS.EMPTY
    });
    
    const [shouldEnableSubmit, setShouldEnableSubmit] = React.useState(false);
    const [status, setStatus] = React.useState({ messages : CONSTANTS.EMPTY, type : CONSTANTS.EMPTY } as IStatus);

    const setReCaptcha = (value:string) => {
        setRegister({ ...register, captchaToken : value });
    }

    const setRegistrationData = (field: string, e:any) => {
        let value = e.target.value;

        if (field === 'username') setRegister({ ...register, username : value });
        if (field === 'email') setRegister({ ...register, email : value });
        if (field === 'password') setRegister({ ...register, password : value });
        if (field === 'confirm') setRegister({ ...register, passwordConfirm : value });
        if (field === 'familyName') setRegister({ ...register, familyName : value });
        if (field === 'givenName') setRegister({ ...register, givenName : value });
    }

    React.useEffect(() => {
        checkRegistrationData();
    }, [register]);

    React.useEffect(() => {
        if (!props.requestSuccess && !_.isEmpty(props.registrationResult))
            setStatus({ messages : 'Unable to contact Hidrogen Server at the moment. Please try again later.', type : 'warning' });
        else {
            let result = props.registrationResult;
            displayUsualRegistrationResult(result, setStatus);
        }
    }, [props]);

    const checkRegistrationData = () => {
        let result = validateRegistrationInputs(
            register.username, register.email, register.password,
            register.passwordConfirm, register.familyName, register.givenName
        );

        if (result.errors.length !== 0) setStatus({ messages : result.errors, type : 'error' });
        else setStatus({ messages : '', type : '' } as IStatus);
        
        highlightInputFields(result.fields);
        setShouldEnableSubmit(register.captchaToken != null && register.captchaToken !== CONSTANTS.EMPTY && result.valid);
    }

    const submitRegistration = () => {
        const { registerHidrogenian } = props;
        registerHidrogenian(register);
    }

    return (
        <>
            <CarbonAlert { ...status } />
            {
                props.requestSending &&
                <div className='row' style={{ textAlign:'center' }}>
                    <CarbonPreloader size='big' />
                </div>
            }
            
            <h4><i className='fas fa-user-plus hidro-primary-icon'></i>&nbsp;Create your account</h4>
            <div className='card'>
                <div className='card-content'>
                    <p className='input-error'></p>
                    <div className='row'>
                        <div className='input-field col m6 s12'>
                            <i className='material-icons prefix hidro-primary-icon'>account_box</i>
                            <input id='username' type='text' value={ register.username }
                                onChange={ (e: any) => { setRegistrationData('username', e); }} />
                            <label htmlFor='username'>Username</label>
                        </div>
                        <div className='input-field col m6 s12'>
                            <i className='material-icons prefix hidro-primary-icon'>alternate_email</i>
                            <input id='email' type='text' value={ register.email }
                                onChange={ (e: any) => { setRegistrationData('email', e); }} />
                            <label htmlFor='email'>Email address</label>
                        </div>
                        <div className='input-field col m6 s12'>
                            <i className='material-icons prefix hidro-primary-icon'>lock</i>
                            <input id='password' type='password' value ={ register.password }
                                onChange={ (e: any) => { setRegistrationData('password', e); }} />
                            <label htmlFor='password'>Password</label>
                        </div>
                        <div className='input-field col m6 s12'>
                            <i className='material-icons prefix hidro-primary-icon'>confirmation_number</i>
                            <input id='confirm' type='password' value={ register.passwordConfirm }
                                onChange={ (e: any) => { setRegistrationData('confirm', e); }} />
                            <label htmlFor='confirm'>Confirm password</label>
                        </div>
                        <div className='input-field col m6 s12'>
                            <i className='material-icons prefix hidro-primary-icon'>perm_identity</i>
                            <input id='family-name' type='text' value={ register.familyName }
                                onChange={ (e: any) => { setRegistrationData('familyName', e); }} />
                            <label htmlFor='family-name'>Family Name</label>
                        </div>
                        <div className='input-field col m6 s12'>
                            <i className='material-icons prefix hidro-primary-icon'>perm_identity</i>
                            <input id='given-name' type='text' value={ register.givenName }
                                onChange={ (e: any) => { setRegistrationData('givenName', e); }} />
                            <label htmlFor='given-name'>Given Name</label>
                        </div>
                        <div className='col s12'>
                            <div className='row'>
                                <div className='col s12'>
                                    <ReCAPTCHA
                                        sitekey='6LeXhN4UAAAAAHKW6-44VxtUVMYSlMPj04WRoC8z'
                                        onChange={ setReCaptcha } />
                                </div>
                                <div className='col s12'>
                                    <button className='btn' style={{ marginTop:'20px' }} onClick={ submitRegistration }
                                        disabled={ !shouldEnableSubmit || props.requestSending }>
                                            Submit
                                    </button>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </>
    );
}

export default connect(
    mapStateToProps,
    mapDispatchToProps
)(UsualSignUp);