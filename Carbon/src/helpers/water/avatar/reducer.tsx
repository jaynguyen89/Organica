import * as waterConstants from './constants';
import produce from 'immer';

interface IAvatarStore {
    apiKey : object | null,
    avatarUploading : Boolean,
    avatarSuccess : Boolean,
    avatarResult : object | null,
    deleteSuccess : Boolean,
    deleteResult : object | null
}

const initialState : IAvatarStore = {
    apiKey : null,
    avatarUploading : false,
    avatarSuccess : false,
    avatarResult : null,
    deleteSuccess : false,
    deleteResult : null
}

const reducer = produce((draft, action) => {
    resetStore(draft);

    switch (action.type) {
        case waterConstants.GET_WATER_API_KEY:
            draft.apiKey = action.payload;
            return;
        case waterConstants.GET_WATER_API_KEY_FAILED:
            draft.apiKey = action.error;
            return;
        case waterConstants.UPLOAD_AVATAR_TO_WATER_BEGIN:
            draft.avatarUploading = true;
            return;
        case waterConstants.UPLOAD_AVATAR_TO_WATER_FAILED:
            draft.avatarSuccess = false;
            draft.avatarResult = action.error;
            return;
        case waterConstants.UPLOAD_AVATAR_TO_WATER_SUCCESS:
            draft.avatarSuccess = true;
            draft.avatarResult = action.payload;
            return;
        case waterConstants.UPDATE_AVATAR_REQUEST_SENDING:
            draft.avatarUploading = true;
            return;
        case waterConstants.UPDATE_AVATAR_REQUEST_SUCCESS:
            draft.avatarSuccess = true;
            draft.avatarResult = action.payload;
            return;
        case waterConstants.UPDATE_AVATAR_REQUEST_FAILED:
            draft.avatarSuccess = false;
            draft.avatarResult = action.error;
            return;
        case waterConstants.REMOVE_AVATAR_REQUEST_SUCCESS:
            draft.deleteSuccess = true;
            draft.deleteResult = action.payload;
            return;
        case waterConstants.REMOVE_AVATAR_REQUEST_FAILED:
            draft.deleteSuccess = false;
            draft.deleteResult = action.error;
            return;
        default:
            return;
    }
}, initialState);

export default reducer;

const resetStore = (draft: any) => {
    draft.apiKey = null;
    draft.avatarUploading = false;
    draft.avatarSuccess = false;
    draft.avatarResult = null;
    draft.deleteSuccess = false;
    draft.deleteResult = null;
}