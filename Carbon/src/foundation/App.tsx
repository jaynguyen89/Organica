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
import UserProfile from '../user-features/profiles/private-profile/PrivateProfile';
import HidroAccount from '../user-features/account/HidroAccount';
import MyPerformance from '../user-features/performance/MyPerformance';
import CustomerHome from '../customer-home/CustomerHome';

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
                    <Route path='/user-account' component={ HidroAccount } />
                    <Route path='/user-profile' component={ UserProfile } />
                    <Route path='/my-performance' component={ MyPerformance } />
                    <Route path='/customer-home' component={ CustomerHome } />
                </Switch>
            </BrowserRouter>
        </>
    );
}

export default App;