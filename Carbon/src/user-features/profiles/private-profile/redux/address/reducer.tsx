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
    },
    updateAddress : {
        isUpdating : Boolean,
        updateSuccess : Boolean,
        updatedAddress : object | null
    },
    setField : {
        isSetting : Boolean,
        settingSuccess : Boolean,
        result : object | null
    },
    deleteAddress : {
        isDeleting : Boolean,
        deleteSuccess : Boolean,
        result : object | null
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
    },
    updateAddress : {
        isUpdating : false,
        updateSuccess : false,
        updatedAddress : null
    },
    setField : {
        isSetting : false,
        settingSuccess : false,
        result : null
    },
    deleteAddress : {
        isDeleting : false,
        deleteSuccess : false,
        result : null
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
        case addressConstants.UPDATE_ADDRESS_BEGIN:
            draft.updateAddress.isUpdating = true;
            draft.updateAddress.updateSuccess = false;
            draft.updateAddress.updatedAddress = null;
            return;
        case addressConstants.UPDATE_ADDRESS_FAILED:
            draft.updateAddress.isUpdating = false;
            draft.updateAddress.updateSuccess = false;
            draft.updateAddress.updatedAddress = action.error;
            return;
        case addressConstants.UPDATE_ADDRESS_SUCCESS:
            draft.updateAddress.isUpdating = false;
            draft.updateAddress.updateSuccess = true;
            draft.updateAddress.updatedAddress = action.payload;
            return;
        case addressConstants.SET_ADDRESS_FIELD_BEGIN:
            draft.setField.isSetting = true;
            draft.setField.settingSuccess = false;
            draft.setField.result = null;
            return;
        case addressConstants.SET_ADDRESS_FIELD_FAILED:
            draft.setField.isSetting = false;
            draft.setField.settingSuccess = false;
            draft.setField.result = action.error;
            return;
        case addressConstants.SET_ADDRESS_FIELD_SUCCESS:
            draft.setField.isSetting = false;
            draft.setField.settingSuccess = true;
            draft.setField.result = action.payload;
            return;
        case addressConstants.DELETE_ADDRESS_BEGIN:
            draft.deleteAddress.isDeleting = true;
            draft.deleteAddress.deleteSuccess = false;
            draft.deleteAddress.result = null;
            return;
        case addressConstants.DELETE_ADDRESS_FAILED:
            draft.deleteAddress.isDeleting = false;
            draft.deleteAddress.deleteSuccess = false;
            draft.deleteAddress.result = action.error;
            return;
        case addressConstants.DELETE_ADDRESS_SUCCESS:
            draft.deleteAddress.isDeleting = false;
            draft.deleteAddress.deleteSuccess = true;
            draft.deleteAddress.result = action.payload;
            return;
        default:
            return;
    }
}, initialState);

export default reducer;