import React from 'react';
import { BrowserRouter,  Route,  Switch } from 'react-router-dom';

import NavBar from '../navigator/NavBar';
import CarbonHome from '../home-contents/CarbonHome';
import CarbonSignIn from '../authentication/CarbonSignin';
import CarbonRegister from '../account-registration/CarbonRegister';

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
                </Switch>
            </BrowserRouter>
        </>
    );
}

export default App;
