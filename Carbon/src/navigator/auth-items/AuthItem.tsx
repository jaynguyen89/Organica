import React from 'react';
import { connect } from 'react-redux';
import './style.css';

import { ClickAwayListener } from '@material-ui/core';
import CarbonAvatar from '../../shared/CarbonAvatar';

import { universalSignOut } from '../../authentication/redux/actions';

const mapStateToProps = (state: any) => ({
    requestSending : state.AuthenticationStore.unAuthSending,
    requestSuccess : state.AuthenticationStore.unAuthSuccess,
    requestResult : state.AuthenticationStore.unAuthResult
});

const mapDispatchToProps = {
    universalSignOut
};

const AuthItem = (props: any) => {
    const [open, setOpen] = React.useState(false);

    React.useEffect(() => {
        if (props.requestSuccess && props.requestResult &&
            props.requestResult.hasOwnProperty('result') && props.requestResult.result === 1)
            window.location.href = '/';
        else if (!props.requestSuccess && props.requestResult && props.requestResult.hasOwnProperty('stack'))
            alert('Network connection lost. Please check your internet and try again.');
    }, [props]);

    return (
        (
            props.auth &&
            <ClickAwayListener onClickAway={ () => setOpen(false) }>
                <li>
                    <div className='navbar-auth' onClick={ () => setOpen(true) }>
                        <span>
                            G'day! { props.auth.fullName }&nbsp;&nbsp;
                        </span>
                        <i className='fas fa-caret-down expand-toggle'></i>
                        {
                            open &&
                            <div className='navbar-expand'>
                                <div className='nav-expand-item' onClick={ () => window.location.href = '/user-account' }>
                                    <CarbonAvatar size='30px' />
                                    <span>My Account</span>
                                </div>
                                <div className='nav-expand-item' onClick={ () => window.location.href = '/user-profile' }>
                                    <i className='fas fa-user-circle hidro-primary-icon'></i>
                                    <span>My Profile</span>
                                </div>
                                <div className='nav-expand-item' onClick={ () => window.location.href = '/my-performance' }>
                                    <i className='fas fa-seedling hidro-primary-icon'></i>
                                    <span>My Performance</span>
                                </div>
                                <div className='nav-expand-item' onClick={ () => window.location.href = '/user-profile' }>
                                    <i className='fas fa-comment-dots hidro-primary-icon'></i>
                                    <span>My Messages</span>
                                </div>
                                <div className='nav-expand-item' onClick={ () => window.location.href = '/user-profile' }>
                                    <i className='fas fa-shopping-bag hidro-primary-icon'></i>
                                    <span>My Purchases</span>
                                </div>
                                <div className='nav-expand-item' onClick={ () => props.universalSignOut(props.auth.authToken) }>
                                    <i className='fas fa-sign-out-alt hidro-primary-icon'></i>
                                    <span>Sign Out</span>
                                </div>
                            </div>
                        }
                    </div>
                </li>
            </ClickAwayListener>
        ) ||
        <>
            <li><a className='btn' href='/carbon-signin'>Sign in</a></li>
            <li><a className='btn' href='/carbon-register'>Register</a></li>
        </>
    );
}

export default connect(
    mapStateToProps,
    mapDispatchToProps
)(AuthItem);