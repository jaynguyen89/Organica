import * as recoveryConstants from './constants';
import * as recoveryServices from './services';

export const requestForgotPassword = (recovery: any) => {
    return (dispatch: (arg0: { type: string; payload?: any; error?: any; }) => void) => {
        dispatch({ type : recoveryConstants.FORGOT_PASSWORD_REQUEST_SENDING });
        
        recoveryServices.sendForgotPasswordRequest(recovery)
        .then((result: any) => {
            dispatch({
                type : recoveryConstants.FORGOT_PASSWORD_REQUEST_SUCCESS,
                payload : result
            });
        })
        .catch((error: any) => {
            dispatch({ type : recoveryConstants.FORGOT_PASSWORD_REQUEST_FAILED, error });
        });
    };
}

export const resetPassword = (resetter: any) => {
    return (dispatch: (arg0: { type: string; payload?: any; error?: any; }) => void) => {
        dispatch({ type : recoveryConstants.PASSWORD_RECOVERY_REQUEST_SENDING });
        
        recoveryServices.sendResetPasswordRequest(resetter)
        .then((result: any) => {
            dispatch({
                type : recoveryConstants.PASSWORD_RECOVERY_REQUEST_SUCCESS,
                payload : result
            });
        })
        .catch((error: any) => {
            dispatch({ type : recoveryConstants.PASSWORD_RECOVERY_REQUEST_FAILED, error });
        });
    };
}