import * as registrationConstants from './constants';
import registerHidrogenianAccount from './services';

export const registerHidrogenian = (registration: any) => {
    return (dispatch: (arg0: { type: string; payload?: any; error?: any; }) => void) => {
        dispatch({ type : registrationConstants.REGISTRATION_REQUEST_SENDING });
        
        registerHidrogenianAccount(registration)
        .then((result: any) => {
            dispatch({
                type : registrationConstants.REGISTRATION_REQUEST_SUCCESS,
                payload : result
            });
        })
        .catch((error: any) => {
            dispatch({ type : registrationConstants.REGISTRATION_REQUEST_FAILED, error });
        });
    };
}