import * as activationConstants from './constants';
import produce from 'immer';

interface IActivationStore {
    activationRequestSending : boolean,
    activationRequestSuccess : boolean,
    activationResult : object,
    activatorRequestSending : boolean,
    activatorRequestSuccess : boolean,
    activatorResult : object
}

const initialState : IActivationStore = {
    activationRequestSending : false,
    activationRequestSuccess: false,
    activationResult : {},
    activatorRequestSending : false,
    activatorRequestSuccess : false,
    activatorResult : {}
}

const reducer = produce((draft, action) => {
    switch (action.type) {
        case activationConstants.ACCOUNT_ACTIVATION_REQUEST_SENDING:
            draft.activationRequestSending = true;
            draft.activationRequestSuccess = false;
            draft.activationResult = null;
            return;
        case activationConstants.ACCOUNT_ACTIVATION_REQUEST_SUCCESS:
            draft.activationRequestSending = false;
            draft.activationRequestSuccess = true;
            draft.activationResult = action.payload;
            return;
        case activationConstants.ACCOUNT_ACTIVATION_REQUEST_FAILED:
            draft.activationRequestSending = false;
            draft.activationRequestSuccess = false;
            draft.activationResult = action.error;
            return;
        case activationConstants.NEW_ACTIVATOR_REQUEST_SENDING:
            draft.activatorRequestSending = true;
            draft.activatorRequestSuccess = false;
            draft.activatorResult = null;
            return;
        case activationConstants.NEW_ACTIVATOR_REQUEST_SUCCESS:
            draft.activatorRequestSending = false;
            draft.activatorRequestSuccess = true;
            draft.activatorResult = action.payload;
            return;
        case activationConstants.NEW_ACTIVATOR_REQUEST_FAILED:
            draft.activatorRequestSending = false;
            draft.activatorRequestSuccess = false;
            draft.activatorResult = action.error;
            return;
        default:
            return initialState;
    }
}, initialState);

export default reducer;