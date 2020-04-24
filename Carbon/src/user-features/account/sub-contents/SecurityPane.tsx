import React from 'react';
import { connect } from 'react-redux';
import _ from 'lodash';

import { Paper } from '@material-ui/core';
import SecurityForm from './SecurityForm';
import ReCAPTCHA from 'react-google-recaptcha';

import { ISecurityPane, ITwoFa, VOID_PW, IPasswordUpdate, VOID_TFA } from '../redux/constants';
import { saveNewPassword, enableOrRefreshTwoFa, disableTwoFa } from '../redux/actions';

const mapStateToProps = (state: any) => ({
    
});

const mapDispatchToProps = {
    saveNewPassword,
    enableOrRefreshTwoFa,
    disableTwoFa
};

const SecurityPane = (props: ISecurityPane) => {
    const [twoFa, setTwoFa] = React.useState(null as unknown as ITwoFa);
    const [passwordForm, setPasswordForm] = React.useState(VOID_PW as IPasswordUpdate);
    const [isPasswordUpdating, setIsPasswordUpdating] = React.useState(false);
    const [allowRenewTwoFa, setAllowRenewTwoFa] = React.useState(false);

    React.useEffect(() => {
        if (!_.isEmpty(props.twoFa)) setTwoFa(props.twoFa);
        else setTwoFa(VOID_TFA);
    }, [props.twoFa]);

    const updatePasswordFields = (field: string, value: string) => {
        if (field === 'password') setPasswordForm({ ...passwordForm, password : value });
        if (field === 'newpw') setPasswordForm({ ...passwordForm, newPassword : value });
        if (field === 'confirm') setPasswordForm({ ...passwordForm, passwordConfirm : value });
        if (field === 'captcha') setPasswordForm({ ...passwordForm, captchaToken : value });
    }

    const saveNewPassword = () => {
        const { saveNewPassword } = props;
        saveNewPassword(passwordForm);
    }

    const enableOrRefreshTwoFa = (token: string, isRefreshing: Boolean = false) => {
        let shouldDo = isRefreshing && window.confirm('We will generate a new QR Code for you. After that, you will need to scan it in Google Authenticator app again.');

        if ((isRefreshing && shouldDo) || !isRefreshing) {
            setTwoFa({ ...twoFa, id : props.user.userId, captchaToken : token });

            const { enableOrRefreshTwoFa } = props;
            enableOrRefreshTwoFa(twoFa);
        }
    }

    const disableTwoFa = () => {
        if (window.confirm('Are you sure to diable Two-Factor Authentication? Your account will be prone to intruders.')) {
            setTwoFa({ ...twoFa, id : props.user.userId });

            const { disableTwoFa } = props;
            disableTwoFa(twoFa);
        }
    }

    return (
        <Paper className='content-container'>
            <div className='row'>
                <div className={ (!isPasswordUpdating && 'col m5 s12') || 'col s12' }>
                    <div className='col s12'>
                        <p><b>Password</b></p>
                        {
                            (
                                !isPasswordUpdating &&
                                <button className='btn modal-trigger'
                                    data-target='security-form'
                                    onClick={ () => setIsPasswordUpdating(true) }>Update Password</button>
                            ) ||
                            <SecurityForm
                                passwordForm={ passwordForm }
                                updatePassword={ updatePasswordFields }
                                savePassword={ saveNewPassword }
                                setIsUpdating={ setIsPasswordUpdating } />
                        }
                    </div>
                </div>
                <div className={ (!isPasswordUpdating && 'col m7 s12') || 'col s0' }>
                    <p><b>Two-Factor Authentication</b></p>
                    {
                        (
                            (twoFa && twoFa.id === 0) &&
                            <>
                                <ReCAPTCHA
                                    sitekey='6LeXhN4UAAAAAHKW6-44VxtUVMYSlMPj04WRoC8z'
                                    onChange={ (v: string) => enableOrRefreshTwoFa(v) } />
                                <p className='instruction' style={{ marginTop:0 }}>
                                    <b>Click the reCaptcha to enable Two-Factor Authentication.</b>
                                </p>

                                <p className='instruction'>
                                    <i className='fas fa-question-circle hidro-primary-icon'></i>&nbsp;
                                    Protect your account better from attacks and intruders.
                                </p>
                                <p className='instruction'>
                                    <i className='fas fa-question-circle hidro-primary-icon'></i>&nbsp;
                                    Your account stays safe even if your password is compromised.
                                </p>
                                <p className='instruction'>
                                    <i className='fas fa-question-circle hidro-primary-icon'></i>&nbsp;
                                    You will only need to enter a 4-digit code on login.
                                </p>
                            </>
                        ) ||
                        <>
                            <p className='instruction'>
                                <i className='fas fa-question-circle hidro-primary-icon'></i>&nbsp;
                                Download <b>Google Authenticator</b> from <a href='/'>App Store</a> or <a href=''>Google Play</a>.
                            </p>
                            <p className='instruction'>
                                <i className='fas fa-question-circle hidro-primary-icon'></i>&nbsp;
                                Scan the above QR Code with <i>Google Authenticator</i> then <b>Save</b>.
                            </p>
                            
                            {
                                (
                                    allowRenewTwoFa &&
                                    <>
                                        <ReCAPTCHA
                                            sitekey='6LeXhN4UAAAAAHKW6-44VxtUVMYSlMPj04WRoC8z'
                                            onChange={ (v: string) => enableOrRefreshTwoFa(v, true) } />
                                        <a role='button' onClick={ () => setAllowRenewTwoFa(false) }>Cancel</a>
                                    </>
                                ) ||
                                <button className='btn' onClick={ () => setAllowRenewTwoFa(true) }>Renew QR Code</button>
                            }

                            <br />
                            <a role='button' className='red-text' onClick={ () => disableTwoFa() }>Disable Two-Factor</a>
                        </>
                    }
                </div>
            </div>
        </Paper>
    );
}

export default connect(
    mapStateToProps,
    mapDispatchToProps
)(SecurityPane);