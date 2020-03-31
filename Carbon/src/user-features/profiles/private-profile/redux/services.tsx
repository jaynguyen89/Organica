import axios from 'axios';
import sendRequestForResult from '../../../../providers/serviceProvider';
let CONTROLLER = 'profile/';

export const uploadAvatarToCloudinary = (avatar: any) => {
    const requestOptions = {
        method : 'GET',
        url : '/image/add',
        headers : {
            'Content-Type': 'multipart/form',
            'Accept': 'application/json'
        },
        body : { image : avatar }
    };

    const response = axios(requestOptions).then((result: any) => {
        if (result.status !== 200)
            return result.json().then((error: any) => { throw error; })
        else
            return result.data;
    });
    
    return response;
}

export const updateAvatarToHidrogen = (profile: any) => {
    return sendRequestForResult(CONTROLLER + 'update-avatar', null, profile, 'PUT');
}

export const removeAvatarFromHidrogen = (profileId: number) => {
    return sendRequestForResult(CONTROLLER + 'remove-avatar/' + profileId, null, null, 'DELETE');
}