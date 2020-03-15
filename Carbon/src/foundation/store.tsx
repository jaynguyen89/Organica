import { createStore, applyMiddleware, compose } from 'redux';
import thunk from 'redux-thunk';
import reducers from '../providers/reducerIndex';
import * as authConstants from '../authentication/redux/constants';

interface IActionResult {
    type: string;
    payload: {
        hasOwnProperty: (arg0: string) => any;
        result: number;
        message: any;
    };
}

const composeEnhancers = (
    window && (window as any).__REDUX_DEVTOOLS_EXTENSION_COMPOSE__
) || compose;

export const setCookies = () =>
                          (next: (arg0: any) => void) =>
                          (action: IActionResult) =>
{
    if (
        action.type === authConstants.AUTHENTICATED && action.payload &&
        action.payload.hasOwnProperty('result') && action.payload.result === 1
    ) {    
        localStorage.setItem(
          'authentication',
          JSON.stringify(action.payload.message)
        );
        
        let auth = action.payload.message;
        
        sessionStorage.setItem('authToken', auth.authToken);
        sessionStorage.setItem('expirationTime', auth.expirationTime);
    }
    
    if (action.type === authConstants.NO_AUTHENTICATION) {
        localStorage.removeItem('authentication');
        sessionStorage.clear();
    }
    
    next(action);
}

const enhancer = composeEnhancers(
    applyMiddleware(thunk, setCookies)
);

export const store = createStore(reducers, enhancer);