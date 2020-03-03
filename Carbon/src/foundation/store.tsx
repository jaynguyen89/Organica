import { createStore, applyMiddleware, compose } from 'redux';
import thunk from 'redux-thunk';
import reducers from '../indexes/reducerIndex';
import * as authConstants from '../authentication/constants';

const composeEnhancers = (
    window && (window as any).__REDUX_DEVTOOLS_EXTENSION_COMPOSE__
) || compose;

export const setCookies = () =>
                          (next: (arg0: any) => void) =>
                          (action: { type: string; payload: { authToken: any; }; }) =>
{
    if (
        action.type === authConstants.AUTHENTICATED 
    ) {    
        localStorage.setItem(
          'currentUser',
          JSON.stringify(action.payload)
        );
        
        let authToken = action.payload.authToken;
    
        sessionStorage.setItem('userToken', authToken.token);
        sessionStorage.setItem('expirationTime', authToken.tokenExpirationTime);
    }
    
    if (action.type === authConstants.UNAUTHENTICATED)
        localStorage.removeItem('currentUser');
    
    next(action);
}

const enhancer = composeEnhancers(
    applyMiddleware(thunk, setCookies)
);

export const store = createStore(reducers, enhancer);