import React from 'react';
import { connect } from 'react-redux';
import $ from 'jquery';
import M from 'materialize-css';

import GuestItems from './auth-items/GuestItems';
import CustomerItems from './auth-items/CustomerItems';
import AuthItem from './auth-items/AuthItem';

const mapStateToProps = (state: any) => ({
    auth : state.AuthenticationStore.authUser
});

const NavBar = (props: any) => {
    React.useEffect(() => {
        M.Sidenav.init($('.sidenav'), {});
    }, []);

    return (
        <div className='navbar-fixed'>
            <nav>
                <div className='nav-wrapper'>
                    <a href='/' className='brand-logo'>Hidrogen</a>
                    <a href='#' data-target='navbar-collapsed' className='sidenav-trigger'>
                        <i className='fas fa-bars'></i>
                    </a>
                    <ul className='right hide-on-med-and-down'>
                        {
                            (props.auth && <CustomerItems />) || <GuestItems />
                        }
                        <AuthItem { ...props } />
                    </ul>
                </div>
            </nav>

            <ul className='sidenav' id='navbar-collapsed'>
                {
                    (props.auth && <CustomerItems />) || <GuestItems />
                }
                <AuthItem { ...props } />
            </ul>
        </div>
    );
}

export default connect(
    mapStateToProps
)(NavBar);