import * as profileConstants from './constants';
import * as profileServices from './services';

export const retrievePrivateProfile = (hidrogenianId: number) => {
    return (dispatch: (arg0: { type: string; payload?: any; error?: any; }) => void) => {
        dispatch({ type : profileConstants.GET_PRIVATE_PROFILE_BEGIN });

        profileServices.retrievePrivateProfile(hidrogenianId)
        .then((result: any) => {
            dispatch({
                type : profileConstants.GET_PRIVATE_PROFILE_SUCCESS,
                payload : result
            });
        })
        .catch((error: any) => {
            dispatch({ type : profileConstants.GET_PRIVATE_PROFILE_FAILED, error });
        });
    };
}

export const updatePrivateProfile = (profile: profileConstants.IProfile) => {
    return (dispatch: (arg0: { type: string; payload?: any; error?: any; }) => void) => {
        dispatch({ type : profileConstants.UPDATE_PRIVATE_PROFILE_BEGIN });

        profileServices.updatePrivateProfile(profile)
        .then((result: any) => {
            dispatch({
                type : profileConstants.UPDATE_PRIVATE_PROFILE_SUCCESS,
                payload : result
            });
        })
        .catch((error: any) => {
            dispatch({ type : profileConstants.UPDATE_PRIVATE_PROFILE_FAILED, error });
        });
    };
}

export const bioShowShouldUpdate = () => {
    return (dispatch: (arg0: { type: string; }) => void) => {
        dispatch({
            type : profileConstants.SET_NEW_PROFILE_AFTER_UPDATE
        });
    };
}