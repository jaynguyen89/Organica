import * as profileConstants from './constants';
import * as profileServices from './services';

export const uploadProfileAvatarToImagebb = (avatar: any) => {
    return (dispatch: (arg0: { type: string; payload?: any; error?: any; }) => void) => {
        dispatch({ type : profileConstants.UPLOAD_AVATAR_TO_IMAGEBB_BEGIN });
        
        profileServices.uploadAvatarToCloudinary(avatar)
        .then((result: any) => {
            dispatch({
                type : profileConstants.UPLOAD_AVATAR_TO_IMAGEBB_SUCCESS,
                payload : result
            });
        })
        .catch((error: any) => {
            dispatch({ type : profileConstants.UPLOAD_AVATAR_TO_IMAGEBB_FAILED, error });
        });
    };
}

export const updateAvatarToHidrogen = () => {

}