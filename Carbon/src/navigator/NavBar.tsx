import React from 'react';

const NavBar = () => {
    return (
        <div className='navbar-fixed'>
            <nav>
                <div className='nav-wrapper'>
                    <a href='/' className='brand-logo'>Hidrogen</a>
                    <a href='#' data-target='mobile-demo' className='sidenav-trigger'>
                        <i className='fas fa-bars'></i>
                    </a>
                    <ul className='right hide-on-med-and-down'>
                        <li><a href='/'>Products</a></li>
                        <li><a href='/'>Categories</a></li>
                        <li><a href='/'>Markets</a></li>
                        <li><a href='/'>About</a></li>
                        <li><a className='btn' href='/carbon-signin'>Sign in</a></li>
                        <li><a className='btn' href='/carbon-register'>Register</a></li>
                    </ul>
                </div>
            </nav>

            <ul className='sidenav' id='mobile-demo'>
                <li><a href='/'>Products</a></li>
                <li><a href='/'>Categories</a></li>
                <li><a href='/'>Markets</a></li>
                <li><a href='/'>About</a></li>
                <li><a className='btn'>Sign in</a></li>
                <li><a className='btn'>Register</a></li>
            </ul>
        </div>
    );
}

export default NavBar;