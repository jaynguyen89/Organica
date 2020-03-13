import React from 'react';
import { BrowserRouter,  Route,  Switch } from 'react-router-dom';

import NavBar from '../navigator/NavBar';
import CarbonHome from '../home-contents/CarbonHome';
import CarbonSignIn from '../authentication/CarbonSignin';
import CarbonRegister from '../account-registration/CarbonRegister';
import AccountClaimer from '../account-claimation/account-activation/AccountClaimer';
import RequestActivator from '../account-claimation/account-activation/RequestActivator';
import ForgotPassword from '../account-claimation/password-recovery/ForgotPassword';
import PasswordReset from '../account-claimation/password-recovery/PasswordReset';

const App = () => {
    return (
        <>
            <NavBar />
            <BrowserRouter>
                <Switch>
                    <Route path='/' exact={ true } component={ CarbonHome } />
                    <Route path='/signout' component={ CarbonHome } />
                    <Route path='/carbon-signin' component={ CarbonSignIn } />
                    <Route path='/carbon-register' component={ CarbonRegister } />
                    <Route path='/activate-account' component={ AccountClaimer } />
                    <Route path='/request-activation-email' component={ RequestActivator } />
                    <Route path='/forgot-password' component={ ForgotPassword } />
                    <Route path='/reset-password' component={ PasswordReset } />
                </Switch>
            </BrowserRouter>
        </>
    );
}

export default App;