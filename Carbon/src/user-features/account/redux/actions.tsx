import * as accountServices from './services';
import * as accountConstants from './constants';

export const getAccountIdentity = (hidrogenianId: number) => {
    return (dispatch: (arg0: { type: string; payload?: any; error?: any; }) => void) => {
        dispatch({ type : accountConstants.GET_ACCOUNT_IDENTITY_BEGIN });

        accountServices.getAccountIdentity(hidrogenianId)
        .then((result: any) => {
            dispatch({
                type : accountConstants.GET_ACCOUNT_IDENTITY_SUCCESS,
                payload : result
            });
        })
        .catch((error: any) => {
            dispatch({ type : accountConstants.GET_ACCOUNT_IDENTITY_FAILED, error });
        });
    };
}

export const getAccountTimeStamps = (hidrogenianId: number) => {
    return (dispatch: (arg0: { type: string; payload?: any; error?: any; }) => void) => {
        dispatch({ type : accountConstants.GET_ACCOUNT_TIMESTAMPS_BEGIN });

        accountServices.getTimeStamps(hidrogenianId)
        .then((result: any) => {
            dispatch({
                type : accountConstants.GET_ACCOUNT_TIMESTAMPS_SUCCESS,
                payload : result
            });
        })
        .catch((error: any) => {
            dispatch({ type : accountConstants.GET_ACCOUNT_TIMESTAMPS_FAILED, error });
        });
    };
}

export const getAccountTwoFaData = (hidrogenianId : number) => {
    return (dispatch: (arg0: { type: string; payload?: any; error?: any; }) => void) => {
        dispatch({ type : accountConstants.GET_ACCOUNT_TWOFA_BEGIN });

        accountServices.getTwoFaData(hidrogenianId)
        .then((result: any) => {
            dispatch({
                type : accountConstants.GET_ACCOUNT_TWOFA_SUCCESS,
                payload : result
            });
        })
        .catch((error: any) => {
            dispatch({ type : accountConstants.GET_ACCOUNT_TWOFA_FAILED, error });
        });
    };
}

export const getPaymentInformation = (hidrogenianId : number) => {
    return (dispatch: (arg0: { type: string; payload?: any; error?: any; }) => void) => {
        dispatch({ type : accountConstants.GET_PAYMENT_INFO_BEGIN });

        accountServices.getPaymentInfo(hidrogenianId)
        .then((result: any) => {
            dispatch({
                type : accountConstants.GET_PAYMENT_INFO_SUCCESS,
                payload : result
            });
        })
        .catch((error: any) => {
            dispatch({ type : accountConstants.GET_PAYMENT_INFO_FAILED, error });
        });
    };
}

export const saveAccountIdentity = (identity: accountConstants.IIdentity) => {
    return (dispatch: (arg0: { type: string; payload?: any; error?: any; }) => void) => {
        dispatch({ type : accountConstants.SAVE_ACCOUNT_IDENTITY_BEGIN });

        accountServices.updateAccountIdentity(identity)
        .then((result: any) => {
            dispatch({
                type : accountConstants.SAVE_ACCOUNT_IDENTITY_SUCCESS,
                payload : result
            });
        })
        .catch((error: any) => {
            dispatch({ type : accountConstants.SAVE_ACCOUNT_IDENTITY_FAILED, error });
        });
    };
}

export const savePaymentInfo = (paymentInfo: accountConstants.IPaymentInfo) => {
    return (dispatch: (arg0: { type: string; payload?: any; error?: any; }) => void) => {
        dispatch({ type : accountConstants.SAVE_PAYMENT_INFO_BEGIN });

        accountServices.savePaymentMethod(paymentInfo)
        .then((result: any) => {
            dispatch({
                type : accountConstants.SAVE_PAYMENT_INFO_SUCCESS,
                payload : result
            });
        })
        .catch((error: any) => {
            dispatch({ type : accountConstants.SAVE_PAYMENT_INFO_FAILED, error });
        });
    };
}

export const saveNewPassword = (pwSetter: accountConstants.IPasswordUpdate) => {
    return (dispatch: (arg0: { type: string; payload?: any; error?: any; }) => void) => {
        dispatch({ type : accountConstants.SAVE_PASSWORD_UPDATE_BEGIN });

        accountServices.updateAccountPassword(pwSetter)
        .then((result: any) => {
            dispatch({
                type : accountConstants.SAVE_PASSWORD_UPDATE_SUCCESS,
                payload : result
            });
        })
        .catch((error: any) => {
            dispatch({ type : accountConstants.SAVE_PASSWORD_UPDATE_FAILED, error });
        });
    };
}

export const enableOrRefreshTwoFa = (twoFa: accountConstants.ITwoFa) => {
    return (dispatch: (arg0: { type: string; payload?: any; error?: any; }) => void) => {
        dispatch({ type : accountConstants.ENABLE_TWO_FA_BEGIN });

        accountServices.enableOrRefreshTwoFa(twoFa)
        .then((result: any) => {
            dispatch({
                type : accountConstants.ENABLE_TWO_FA_SUCCESS,
                payload : result
            });
        })
        .catch((error: any) => {
            dispatch({ type : accountConstants.ENABLE_TWO_FA_FAILED, error });
        });
    };
}

export const disableTwoFa = (twoFa: accountConstants.ITwoFa) => {
    return (dispatch: (arg0: { type: string; payload?: any; error?: any; }) => void) => {
        dispatch({ type : accountConstants.DISABLE_TWO_FA_BEGIN });

        accountServices.disableTwoFa(twoFa)
        .then((result: any) => {
            dispatch({
                type : accountConstants.DISABLE_TWO_FA_SUCCESS,
                payload : result
            });
        })
        .catch((error: any) => {
            dispatch({ type : accountConstants.DISABLE_TWO_FA_FAILED, error });
        });
    };
}