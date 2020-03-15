import * as authenticationConstants from './constants';
import produce from 'immer';

interface IAuthenticationStore {
    authSending : boolean,
    authSuccess : boolean,
    authResult : object | null,
    unAuthSending : boolean,
    unAuthSuccess : boolean,
    unAuthResult : object | null,
    authUser : object | null
}

const initialState : IAuthenticationStore = {
    authSending : false,
    authSuccess : false,
    authResult : null,
    unAuthSending : false,
    unAuthSuccess : false,
    unAuthResult : null,
    authUser : null
}

const reducer = produce((draft, action) => {
    switch (action.type) {
        case authenticationConstants.AUTHENTICATION_SENDING:
            draft.authSending = true;
            draft.authSuccess = false;
            draft.authResult = null;
            draft.unAuthSending = false;
            draft.unAuthSuccess = false;
            draft.unAuthResult = null;
            draft.authUser = null;
            return;
        case authenticationConstants.AUTHENTICATED:
            draft.authSending = false;
            draft.authSuccess = true;
            draft.authResult = action.payload;
            draft.unAuthSending = false;
            draft.unAuthSuccess = false;
            draft.unAuthResult = null;
            draft.authUser = null;
            return;
        case authenticationConstants.SIGN_OUT_SENDING:
            draft.authSending = false;
            draft.authSuccess = false;
            draft.authResult = null;
            draft.unAuthSending = true;
            draft.unAuthSuccess = false;
            draft.unAuthResult = null;
            return;
        case authenticationConstants.SIGN_OUT_FAILED:
            draft.authSending = false;
            draft.authSuccess = false;
            draft.authResult = null;
            draft.unAuthSending = false;
            draft.unAuthSuccess = false;
            draft.unAuthResult = action.error;
            return;
        case authenticationConstants.UNAUTHENTICATED: //sign-in failed
            draft.authSending = false;
            draft.authSuccess = false;
            draft.authResult = action.error;
            draft.unAuthSending = false;
            draft.unAuthSuccess = false;
            draft.unAuthResult = null;
            draft.authUser = null;
            return;
        case authenticationConstants.NO_AUTHENTICATION: //sign-out success
            draft.authSending = false;
            draft.authSuccess = false;
            draft.authResult = null;
            draft.unAuthSending = false;
            draft.unAuthSuccess = true;
            draft.unAuthResult = action.payload;
            draft.authUser = null;
            return;
        case authenticationConstants.LOAD_AUTH_USER:
            draft.authSending = false;
            draft.authSuccess = false;
            draft.authResult = null;
            draft.unAuthSending = false;
            draft.unAuthSuccess = false;
            draft.unAuthResult = null;
            draft.authUser = action.payload;
            return;
        case authenticationConstants.LOAD_AUTH_USER_FAILED:
            return initialState;
        default:
            return;
    }
}, initialState);

export default reducer;