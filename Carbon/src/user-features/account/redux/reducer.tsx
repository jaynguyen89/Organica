import * as accountConstants from './constants';
import produce from 'immer';

interface IAccountStore {
    getIdentity : {
        isSending : Boolean,
        getSuccess : Boolean,
        getResult : object | null
    },
    getTimeStamps : {
        isSending : Boolean,
        getSuccess : Boolean,
        getResult : object | null
    },
    getTwoFa : {
        isSending : Boolean,
        getSuccess : Boolean,
        getResult : object | null
    },
    getPaymentInfo : {
        isSending : Boolean,
        getSuccess : Boolean,
        getResult : object | null
    },
    saveIdentity : {
        isSending : Boolean,
        saveSuccess : Boolean,
        saveResult : object | null
    },
    savePaymentInfo : {
        isSending : Boolean,
        saveSuccess : Boolean,
        saveResult : object | null
    },
    savePassword : {
        isSending : Boolean,
        saveSuccess : Boolean,
        saveResult : object | null
    },
    toggleFa : {
        isSending : Boolean,
        toggleSuccess : Boolean,
        faResult : object | null
    }
};

const initialState : IAccountStore = {
    getIdentity : {
        isSending : false,
        getSuccess : false,
        getResult : null
    },
    getTimeStamps : {
        isSending : false,
        getSuccess : false,
        getResult : null
    },
    getTwoFa : {
        isSending : false,
        getSuccess : false,
        getResult : null
    },
    getPaymentInfo : {
        isSending : false,
        getSuccess : false,
        getResult : null
    },
    saveIdentity : {
        isSending : false,
        saveSuccess : false,
        saveResult : null
    },
    savePaymentInfo : {
        isSending : false,
        saveSuccess : false,
        saveResult : null
    },
    savePassword : {
        isSending : false,
        saveSuccess : false,
        saveResult : null
    },
    toggleFa : {
        isSending : false,
        toggleSuccess : false,
        faResult : null
    }
};

const reducer = produce((draft, action) => {
    switch (action.type) {
        case accountConstants.GET_ACCOUNT_IDENTITY_BEGIN:
            draft.getIdentity.isSending = true;
            draft.getIdentity.getSuccess = false;
            draft.getIdentity.getResult = null;
            return;
        case accountConstants.GET_ACCOUNT_IDENTITY_FAILED:
            draft.getIdentity.isSending = false;
            draft.getIdentity.getSuccess = false;
            draft.getIdentity.getResult = action.error;
            return;
        case accountConstants.GET_ACCOUNT_IDENTITY_SUCCESS:
            draft.getIdentity.isSending = false;
            draft.getIdentity.getSuccess = true;
            draft.getIdentity.getResult = action.payload;
            return;
        case accountConstants.GET_ACCOUNT_TIMESTAMPS_BEGIN:
            draft.getTimeStamps.isSending = true;
            draft.getTimeStamps.getSuccess = false;
            draft.getTimeStamps.getResult = null;
            return;
        case accountConstants.GET_ACCOUNT_TIMESTAMPS_FAILED:
            draft.getTimeStamps.isSending = false;
            draft.getTimeStamps.getSuccess = false;
            draft.getTimeStamps.getResult = action.error;
            return;
        case accountConstants.GET_ACCOUNT_TIMESTAMPS_SUCCESS:
            draft.getTimeStamps.isSending = false;
            draft.getTimeStamps.getSuccess = true;
            draft.getTimeStamps.getResult = action.payload;
            return;
        case accountConstants.GET_ACCOUNT_TWOFA_BEGIN:
            draft.getTwoFa.isSending = true;
            draft.getTwoFa.getSuccess = false;
            draft.getTwoFa.getResult = null;
            return;
        case accountConstants.GET_ACCOUNT_TWOFA_FAILED:
            draft.getTwoFa.isSending = false;
            draft.getTwoFa.getSuccess = false;
            draft.getTwoFa.getResult = action.error;
            return;
        case accountConstants.GET_ACCOUNT_TWOFA_SUCCESS:
            draft.getTwoFa.isSending = false;
            draft.getTwoFa.getSuccess = true;
            draft.getTwoFa.getResult = action.payload;
            return;
        case accountConstants.GET_PAYMENT_INFO_BEGIN:
            draft.getPaymentInfo.isSending = true;
            draft.getPaymentInfo.getSuccess = false;
            draft.getPaymentInfo.getResult = null;
            return;
        case accountConstants.GET_PAYMENT_INFO_FAILED:
            draft.getPaymentInfo.isSending = false;
            draft.getPaymentInfo.getSuccess = false;
            draft.getPaymentInfo.getResult = action.error;
            return;
        case accountConstants.GET_PAYMENT_INFO_SUCCESS:
            draft.getPaymentInfo.isSending = false;
            draft.getPaymentInfo.getSuccess = true;
            draft.getPaymentInfo.getResult = action.payload;
            return;
        case accountConstants.SAVE_ACCOUNT_IDENTITY_BEGIN:
            draft.saveIdentity.isSending = true;
            draft.saveIdentity.saveSuccess = false;
            draft.saveIdentity.saveResult = null;
            return;
        case accountConstants.SAVE_ACCOUNT_IDENTITY_FAILED:
            draft.saveIdentity.isSending = false;
            draft.saveIdentity.saveSuccess = false;
            draft.saveIdentity.saveResult = action.error;
            return;
        case accountConstants.SAVE_ACCOUNT_IDENTITY_SUCCESS:
            draft.saveIdentity.isSending = false;
            draft.saveIdentity.saveSuccess = true;
            draft.saveIdentity.saveResult = action.payload;
            return;
        case accountConstants.SAVE_PAYMENT_INFO_BEGIN:
            draft.savePaymentInfo.isSending = true;
            draft.savePaymentInfo.saveSuccess = false;
            draft.savePaymentInfo.saveResult = null;
            return;
        case accountConstants.SAVE_PAYMENT_INFO_FAILED:
            draft.savePaymentInfo.isSending = false;
            draft.savePaymentInfo.saveSuccess = false;
            draft.savePaymentInfo.saveResult = action.error;
            return;
        case accountConstants.SAVE_PAYMENT_INFO_SUCCESS:
            draft.savePaymentInfo.isSending = false;
            draft.savePaymentInfo.saveSuccess = true;
            draft.savePaymentInfo.saveResult = action.payload;
            return;
        case accountConstants.SAVE_PASSWORD_UPDATE_BEGIN:
            draft.savePassword.isSending = true;
            draft.savePassword.saveSuccess = false;
            draft.savePassword.saveResult = null;
            return;
        case accountConstants.SAVE_PASSWORD_UPDATE_FAILED:
            draft.savePassword.isSending = false;
            draft.savePassword.saveSuccess = false;
            draft.savePassword.saveResult = action.error;
            return;
        case accountConstants.SAVE_PASSWORD_UPDATE_SUCCESS:
            draft.savePassword.isSending = false;
            draft.savePassword.saveSuccess = true;
            draft.savePassword.saveResult = action.payload;
            return;
        case accountConstants.ENABLE_TWO_FA_BEGIN:
            draft.toggleFa.isSending = true;
            draft.toggleFa.toggleSuccess = false;
            draft.toggleFa.faResult = null;
            return;
        case accountConstants.ENABLE_TWO_FA_FAILED:
            draft.toggleFa.isSending = false;
            draft.toggleFa.toggleSuccess = false;
            draft.toggleFa.faResult = action.error;
            return;
        case accountConstants.ENABLE_TWO_FA_SUCCESS:
            draft.toggleFa.isSending = false;
            draft.toggleFa.toggleSuccess = true;
            draft.toggleFa.faResult = action.payload;
            return;
        case accountConstants.DISABLE_TWO_FA_BEGIN:
            draft.toggleFa.isSending = true;
            draft.toggleFa.toggleSuccess = false;
            draft.toggleFa.faResult = null;
            return;
        case accountConstants.DISABLE_TWO_FA_FAILED:
            draft.toggleFa.isSending = false;
            draft.toggleFa.toggleSuccess = false;
            draft.toggleFa.faResult = action.error;
            return;
        case accountConstants.DISABLE_TWO_FA_SUCCESS:
            draft.toggleFa.isSending = false;
            draft.toggleFa.toggleSuccess = true;
            draft.toggleFa.faResult = action.payload;
            return;
        default:
            return;
    }
}, initialState);

export default reducer;