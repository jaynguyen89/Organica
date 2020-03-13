import * as claimationConstants from './constants';
import * as activationServices from './services';

export const activateHidrogenian = (activator: any) => {
    return (dispatch: (arg0: { type: string; payload?: any; error?: any; }) => void) => {
        dispatch({ type : claimationConstants.ACCOUNT_ACTIVATION_REQUEST_SENDING });
        
        activationServices.activateHidrogenianAccount(activator)
        .then((result: any) => {
            dispatch({
                type : claimationConstants.ACCOUNT_ACTIVATION_REQUEST_SUCCESS,
                payload : result
            });
        })
        .catch((error: any) => {
            dispatch({ type : claimationConstants.ACCOUNT_ACTIVATION_REQUEST_FAILED, error });
        });
    };
}

export const requestNewActivator = (requester: any) => {
    return (dispatch: (arg0: { type: string; payload?: any; error?: any; }) => void) => {
        dispatch({ type : claimationConstants.NEW_ACTIVATOR_REQUEST_SENDING });
        
        activationServices.requestNewActivationEmail(requester)
        .then((result: any) => {
            dispatch({
                type : claimationConstants.NEW_ACTIVATOR_REQUEST_SUCCESS,
                payload : result
            });
        })
        .catch((error: any) => {
            dispatch({ type : claimationConstants.NEW_ACTIVATOR_REQUEST_FAILED, error });
        });
    };
}