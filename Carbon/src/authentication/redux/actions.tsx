import * as authenticationServices from './services';
import * as authenticationConstants from './constants';

export const signInWithCredentials = (credentials: any) => {
    return (dispatch: (arg0: { type: string; payload?: any; error?: any; }) => void) => {
        dispatch({ type : authenticationConstants.AUTHENTICATION_SENDING });
        
        authenticationServices.signInHidrogenianByCredentials(credentials)
        .then((result: any) => {
            dispatch({
                type : authenticationConstants.AUTHENTICATED,
                payload : result
            });
        })
        .catch((error: any) => {
            dispatch({ type : authenticationConstants.UNAUTHENTICATED, error });
        });
    };
}

export const universalSignOut = (auth: any) => {
    return (dispatch: (arg0: { type: string; payload?: any; error?: any; }) => void) => {
        dispatch({ type : authenticationConstants.SIGN_OUT_SENDING });
        
        authenticationServices.signOutHidrogenian(auth)
        .then((result: any) => {
            dispatch({
                type : authenticationConstants.NO_AUTHENTICATION,
                payload : result
            });
        })
        .catch((error: any) => {
            dispatch({ type : authenticationConstants.SIGN_OUT_FAILED, error });
        });
    };
}

export const loadAuthenticatedUser = () => {
    return ((dispatch: (arg0: { type: string; payload?: any; }) => void) => {
        try {
            const localStoredUser = localStorage.getItem('authentication');
            let authUser = JSON.parse(localStoredUser as string);

            dispatch({ type : authenticationConstants.LOAD_AUTH_USER, payload : authUser });
        } catch {
            dispatch({ type : authenticationConstants.LOAD_AUTH_USER_FAILED });
        }
    });
}