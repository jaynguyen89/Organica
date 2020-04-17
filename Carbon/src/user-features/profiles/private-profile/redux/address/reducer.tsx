import * as addressConstants from './constants';
import produce from 'immer';

interface IAddressStore {
    addressList : {
        isRetrieving : Boolean,
        retrieveSuccess : Boolean,
        retrieveResult : object | null
    },
    countryList : {
        isRetrieving : Boolean,
        retrieveSuccess : Boolean,
        retrieveResult : object | null
    },
    saveAddress : {
        isSending : Boolean,
        sendSuccess : Boolean,
        newAddress : object | null
    }
};

const initialState : IAddressStore = {
    addressList : {
        isRetrieving : false,
        retrieveSuccess : false,
        retrieveResult : null
    },
    countryList : {
        isRetrieving : false,
        retrieveSuccess : false,
        retrieveResult : null
    },
    saveAddress : {
        isSending : false,
        sendSuccess : false,
        newAddress : null
    }
};

const reducer = produce((draft, action) => {
    switch (action.type) {
        case addressConstants.GET_ADDRESS_LIST_BEGIN:
            draft.addressList.isRetrieving = true;
            draft.addressList.retrieveSuccess = false;
            draft.addressList.retrieveResult = null;
            return;
        case addressConstants.GET_ADDRESS_LIST_FAILED:
            draft.addressList.isRetrieving = false;
            draft.addressList.retrieveSuccess = false;
            draft.addressList.retrieveResult = action.error;
            return;
        case addressConstants.GET_ADDRESS_LIST_SUCCESS:
            draft.addressList.isRetrieving = false;
            draft.addressList.retrieveSuccess = true;
            draft.addressList.retrieveResult = action.payload;
            return;
        case addressConstants.GET_DROPDOWN_COUNTRIES_BEGIN:
            draft.countryList.isRetrieving = true;
            draft.countryList.retrieveSuccess = false;
            draft.countryList.retrieveResult = null;
            return;
        case addressConstants.GET_DROPDOWN_COUNTRIES_SUCCESS:
            draft.countryList.isRetrieving = false;
            draft.countryList.retrieveSuccess = true;
            draft.countryList.retrieveResult = action.payload;
            return;
        case addressConstants.GET_DROPDOWN_COUNTRIES_FAILED:
            draft.countryList.isRetrieving = false;
            draft.countryList.retrieveSuccess = false;
            draft.countryList.retrieveResult = action.error;
            return;
        case addressConstants.SAVE_NEW_ADDRESS_BEGIN:
            draft.saveAddress.isSending = true;
            draft.saveAddress.sendSuccess = false;
            draft.saveAddress.newAddress = null;
            return;
        case addressConstants.SAVE_NEW_ADDRESS_FAILED:
            draft.saveAddress.isSending = false;
            draft.saveAddress.sendSuccess = false;
            draft.saveAddress.newAddress = action.error;
            return;
        case addressConstants.SAVE_NEW_ADDRESS_SUCCESS:
            draft.saveAddress.isSending = false;
            draft.saveAddress.sendSuccess = true;
            draft.saveAddress.newAddress = action.payload;
            return;
        default:
            return;
    }
}, initialState);

export default reducer;