import { sendRequestForResult, sendWaterRequest } from '../../../providers/serviceProvider';

let PROFILE_CONTROLLER = 'profile/';
let WATER_CONTROLLER = 'water/';

export const getApiKey = (action: string) => {
    return sendRequestForResult(WATER_CONTROLLER + 'get-api-key/' + action, null, null, 'GET');
}

export const uploadAvatarToWater = (avatar: any) => {
    return sendWaterRequest(PROFILE_CONTROLLER + 'save-avatar', avatar, 'POST');
}

export const replaceAvatarInWater = (avatar: any) => {
    return sendWaterRequest(PROFILE_CONTROLLER + 'update-avatar', avatar, 'PUT');
}

export const removeAvatarFromWaterFor = (profile: any) => {
    return sendRequestForResult(PROFILE_CONTROLLER + 'remove-avatar/' + profile.hidrogenianId + '/' + profile.apikey, null, null, 'DELETE');
}