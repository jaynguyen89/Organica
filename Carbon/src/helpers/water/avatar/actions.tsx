import * as waterConstants from './constants';
import * as waterServices from './services';

export const retrieveApiKey = (action: string) => {
    return (dispatch: (arg0: { type: string; payload?: any; error?: any; }) => void) => {
        waterServices.getApiKey(action)
        .then((result: any) => {
            dispatch({
                type : waterConstants.GET_WATER_API_KEY,
                payload : result
            });
        })
        .catch((error: any) => {
            dispatch({ type : waterConstants.GET_WATER_API_KEY_FAILED, error });
        });
    };
}

export const uploadAvatarToWater = (avatar: any) => {
    return (dispatch: (arg0: { type: string; payload?: any; error?: any; }) => void) => {
        dispatch({ type : waterConstants.UPLOAD_AVATAR_TO_WATER_BEGIN });
        
        waterServices.uploadAvatarToWater(avatar)
        .then((result: any) => {
            dispatch({
                type : waterConstants.UPLOAD_AVATAR_TO_WATER_SUCCESS,
                payload : result
            });
        })
        .catch((error: any) => {
            dispatch({ type : waterConstants.UPLOAD_AVATAR_TO_WATER_FAILED, error });
        });
    };
}

export const replaceAvatarInWater = (avatar: any) => {
    return (dispatch: (arg0: { type: string; payload?: any; error?: any; }) => void) => {
        dispatch({ type : waterConstants.UPDATE_AVATAR_REQUEST_SENDING });
        
        waterServices.replaceAvatarInWater(avatar)
        .then((result: any) => {
            dispatch({
                type : waterConstants.UPDATE_AVATAR_REQUEST_SUCCESS,
                payload : result
            });
        })
        .catch((error: any) => {
            dispatch({ type : waterConstants.UPDATE_AVATAR_REQUEST_FAILED, error });
        });
    };
}

export const deleteAvatarInWater = (profile: any) => {
    return (dispatch: (arg0: { type: string; payload?: any; error?: any; }) => void) => {        
        waterServices.removeAvatarFromWaterFor(profile)
        .then((result: any) => {
            dispatch({
                type : waterConstants.REMOVE_AVATAR_REQUEST_SUCCESS,
                payload : result
            });
        })
        .catch((error: any) => {
            dispatch({ type : waterConstants.REMOVE_AVATAR_REQUEST_FAILED, error });
        });
    };
}