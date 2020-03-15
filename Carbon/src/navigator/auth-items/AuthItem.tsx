import React from 'react';
import './style.css';

import { ClickAwayListener } from '@material-ui/core';
import CarbonAvatar from '../../shared/CarbonAtavar';

const AuthItem = (props: any) => {
    const [open, setOpen] = React.useState(false);

    return (
        (
            props.auth &&
            <ClickAwayListener onClickAway={ () => setOpen(false) }>
                <li>
                    <div className='navbar-auth' onClick={ () => setOpen(true) }>
                        <span>
                            G'day! { props.auth.fullName }&nbsp;&nbsp;
                        </span>
                        <i className='fas fa-caret-down'></i>
                        {
                            open &&
                            <div className='navbar-expand'>
                                <CarbonAvatar />
                                Some more details
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

export default AuthItem;