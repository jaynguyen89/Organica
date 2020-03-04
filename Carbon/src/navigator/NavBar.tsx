import React from 'react';
import $ from 'jquery';
import M from 'materialize-css';

import AuthItem from './auth-items/AuthItem';

const NavBar = () => {

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
                        <li><a href='/'>Products</a></li>
                        <li><a href='/'>Categories</a></li>
                        <li><a href='/'>Markets</a></li>
                        <li><a href='/'>About</a></li>
                        <AuthItem />
                    </ul>
                </div>
            </nav>

            <ul className='sidenav' id='navbar-collapsed'>
                <li><a href='/'>Products</a></li>
                <li><a href='/'>Categories</a></li>
                <li><a href='/'>Markets</a></li>
                <li><a href='/'>About</a></li>
                <AuthItem />
            </ul>
        </div>
    );
}

export default NavBar;