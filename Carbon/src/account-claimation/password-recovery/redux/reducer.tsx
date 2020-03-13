import * as recoveryConstants from './constants';
import produce from 'immer';

interface IRecoveryStore {
    forgotPwRequestSending : boolean,
    forgotPwRequestSuccess : boolean,
    forgotPwResult : object,
    resetPwRequestSending : boolean,
    resetPwRequestSuccess : boolean,
    resetPwResult : object
}

const initialState : IRecoveryStore = {
    forgotPwRequestSending : false,
    forgotPwRequestSuccess : false,
    forgotPwResult : {},
    resetPwRequestSending : false,
    resetPwRequestSuccess : false,
    resetPwResult : {}
}

const reducer = produce((draft, action) => {
    switch (action.type) {
        case recoveryConstants.FORGOT_PASSWORD_REQUEST_SENDING:
            draft.forgotPwRequestSending = true;
            draft.forgotPwRequestSuccess = false;
            draft.forgotPwResult = null;
            return;
        case recoveryConstants.FORGOT_PASSWORD_REQUEST_FAILED:
            draft.forgotPwRequestSending = false;
            draft.forgotPwRequestSuccess = false;
            draft.forgotPwResult = action.error;
            return;
        case recoveryConstants.FORGOT_PASSWORD_REQUEST_SUCCESS:
            draft.forgotPwRequestSending = false;
            draft.forgotPwRequestSuccess = true;
            draft.forgotPwResult = action.payload;
            return;
        case recoveryConstants.PASSWORD_RECOVERY_REQUEST_SENDING:
            draft.resetPwRequestSending = true;
            draft.resetPwRequestSuccess = false;
            draft.resetPwResult = null;
            return;
        case recoveryConstants.PASSWORD_RECOVERY_REQUEST_FAILED:
            draft.resetPwRequestSending = false;
            draft.resetPwRequestSuccess = false;
            draft.resetPwResult = action.error;
            return;
        case recoveryConstants.PASSWORD_RECOVERY_REQUEST_SUCCESS:
            draft.resetPwRequestSending = false;
            draft.resetPwRequestSuccess = true;
            draft.resetPwResult = action.payload;
            return;
        default:
            return initialState;
    }
}, initialState);

export default reducer;