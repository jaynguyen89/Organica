import React from 'react';
import { connect } from 'react-redux';
import _ from 'lodash';
import '../style.css';

import ReCAPTCHA from 'react-google-recaptcha';
import CarbonAlert from '../../shared/CarbonAlert';
import CarbonPreloader from '../../shared/CarbonPreloader';
import { IStatus } from '../../shared/CarbonAlert';
import { CONSTANTS } from '../../helpers/helper';

import { signInWithCredentials } from '../redux/actions';
import { displayAuthMessages } from './utility';

const mapStateToProps = (state: any) => ({
    authSending : state.AuthenticationStore.authSending,
    authSuccess : state.AuthenticationStore.authSuccess,
    authResult : state.AuthenticationStore.authResult
});

const mapDispatchToProps = {
    signInWithCredentials
};

const UsualSignIn = (props : any) => {
    const [account, setAccount] = React.useState({
        username : null,
        email : null,
        password : CONSTANTS.EMPTY,
        trustedAuth : false,
        captchaToken : CONSTANTS.EMPTY
    });

    const [shouldEnableBtn, setShouldEnableBtn] = React.useState(false);
    const [status, setStatus] = React.useState({ messages : CONSTANTS.EMPTY, type : CONSTANTS.EMPTY } as IStatus);

    React.useEffect(() => {
        setShouldEnableBtn(
            (!_.isEmpty(account.email) || !_.isEmpty(account.username)) &&
            !_.isEmpty(account.password) &&
            !_.isEmpty(account.captchaToken)
        );
    }, [account]);

    React.useEffect(() => {
        setShouldEnableBtn(shouldEnableBtn && !props.authSending);
        displayAuthMessages(props, setStatus);
    }, [props]);

    const setAccountAsync = (field: string, value: any) => {
        if (field === 'password') setAccount({ ...account, password : value });
        if (field === 'trusted') setAccount({ ...account, trustedAuth : value });
        if (field === 'captcha') setAccount({ ...account, captchaToken : value });

        if (field === 'identity') {
            if (value.indexOf('@') !== -1) setAccount({ ...account, email : value.toLowerCase(), username : null });
            else setAccount({ ...account, username : value, email : null });
        }
    }

    const loginHidrogenian = () => {
        if (shouldEnableBtn) {
            const { signInWithCredentials } = props;
            signInWithCredentials(account);
        }
        else
            setStatus({
                messages : 'Please enter your login email/username and password.',
                type : 'warning'
            });
    }

    return (
        <div className='col l8 m12'>
            <CarbonAlert { ...status } />
            {
                props.authSending &&
                <>
                    <CarbonPreloader size='small' />
                    <span style={{ marginLeft:'10px' }}>Please wait while we're logging you in...</span>
                </>
            }

            <h4><i className='fas fa-user-circle hidro-primary-icon'></i>&nbsp;Sign in</h4>
            <div className='card'>
                <div className='card-content'>
                    <div className='row'>
                        <div className='input-field col s12'>
                            <i className='material-icons prefix hidro-primary-icon'>account_box</i>
                            <input id='username' type='text' className='validate'
                                onChange={ (e: any) => setAccountAsync('identity', e.target.value) }
                                onKeyDown={ (e: any) => { if (e.keyCode === 13) loginHidrogenian(); }} />
                            <label htmlFor='username'>Email or Username</label>
                        </div>
                        <div className='input-field col s12'>
                            <i className='material-icons prefix hidro-primary-icon'>lock</i>
                            <input id='password' type='password' className='validate'
                                onChange={ (e: any) => setAccountAsync('password', e.target.value) }
                                onKeyDown={ (e: any) => { if (e.keyCode === 13) loginHidrogenian(); }} />
                            <label htmlFor='password'>Password</label>
                        </div>
                        <div className='col s12' style={{ marginBottom:'10px' }}>
                            <label>
                                <input type='checkbox' className='filled-in' checked={ account.trustedAuth }
                                    onClick={ (e: any) => setAccountAsync('trusted', e.target.checked) } />
                                <span>Yoo! This is your personal computer.</span>
                            </label>
                        </div>
                        <div className='col s12'>
                            <div className='row'>
                                <div className='col s12'>
                                    <ReCAPTCHA
                                        sitekey='6LeXhN4UAAAAAHKW6-44VxtUVMYSlMPj04WRoC8z'
                                        onChange={ (e: any) => setAccountAsync('captcha', e) } />
                                </div>
                                <div className='col s12'>
                                    <button className='btn' style={{ marginTop:'20px' }}
                                        disabled={ !shouldEnableBtn } onClick={ loginHidrogenian }>
                                        Sign in
                                    </button>
                                    <p className='signin-assistance'>
                                        Forgot your password? <a href='/forgot-password'>Reset</a> now.
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

export default connect(
    mapStateToProps,
    mapDispatchToProps
)(UsualSignIn);