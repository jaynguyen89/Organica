import * as registrationConstants from './constants';
import produce from 'immer';

interface IRegistrationStore {
    registrationType : boolean,
    requestSending : boolean,
    requestSuccess : boolean,
    registrationResult : object
}

const initialState : IRegistrationStore = {
    registrationType : false,
    requestSending : false,
    requestSuccess: false,
    registrationResult : {}
}

const reducer = produce((draft, action) => {
    switch (action.type) {
        case registrationConstants.REGISTRATION_REQUEST_SENDING:
            draft.requestSending = true;
            return;
        case registrationConstants.REGISTRATION_REQUEST_SUCCESS:
            draft.requestSending = false;
            draft.requestSuccess = true;
            draft.registrationResult = action.payload;
            return;
        case registrationConstants.REGISTRATION_REQUEST_FAILED:
            draft.requestSending = false;
            draft.requestSuccess = false;
            draft.registrationResult = null;
            return;
        default:
            return;
    }
}, initialState);

export default reducer;