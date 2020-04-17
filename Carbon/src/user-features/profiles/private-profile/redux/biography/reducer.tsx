import * as profileConstants from './constants';
import produce from 'immer';

interface IProfileStore {
    getProfile : {
        profileRetrieving : Boolean,
        retrieveSuccess : Boolean,
        profileResult : object | null,
        isUpdated : Boolean
    },
    updateProfile : {
        profileUpdating : Boolean,
        updateSuccess : Boolean,
        newProfile : object | null
    }
};

const initialState : IProfileStore = {
    getProfile : {
        profileRetrieving : false,
        retrieveSuccess : false,
        profileResult : null,
        isUpdated : false
    },
    updateProfile : {
        profileUpdating : false,
        updateSuccess : false,
        newProfile : null
    }
};

const reducer = produce((draft, action) => {
    switch (action.type) {
        case profileConstants.GET_PRIVATE_PROFILE_BEGIN:
            draft.getProfile.profileRetrieving = true;
            draft.getProfile.retrieveSuccess = false;
            draft.getProfile.profileResult = null;
            draft.getProfile.isUpdated = false;

            draft.updateProfile.profileUpdating = false;
            draft.updateProfile.updateSuccess = false;
            draft.updateProfile.newProfile = null;
            return;
        case profileConstants.GET_PRIVATE_PROFILE_FAILED:
            draft.getProfile.profileRetrieving = false;
            draft.getProfile.retrieveSuccess = false;
            draft.getProfile.profileResult = action.error;
            draft.getProfile.isUpdated = false;
            return;
        case profileConstants.GET_PRIVATE_PROFILE_SUCCESS:
            draft.getProfile.profileRetrieving = false;
            draft.getProfile.retrieveSuccess = true;
            draft.getProfile.profileResult = action.payload;
            draft.getProfile.isUpdated = false;
            return;
        case profileConstants.UPDATE_PRIVATE_PROFILE_BEGIN:
            draft.updateProfile.profileUpdating = true;
            draft.updateProfile.updateSuccess = false;
            draft.updateProfile.newProfile = null;
            return;
        case profileConstants.UPDATE_PRIVATE_PROFILE_FAILED:
            draft.updateProfile.profileUpdating = false;
            draft.updateProfile.updateSuccess = false;
            draft.updateProfile.newProfile = action.error;
            return;
        case profileConstants.UPDATE_PRIVATE_PROFILE_SUCCESS:
            draft.updateProfile.profileUpdating = false;
            draft.updateProfile.updateSuccess = true;
            draft.updateProfile.newProfile = action.payload;
            return;
        case profileConstants.SET_NEW_PROFILE_AFTER_UPDATE:
            draft.getProfile.isUpdated = true;
            return;
        default:
            return;
    }
}, initialState);

export default reducer;