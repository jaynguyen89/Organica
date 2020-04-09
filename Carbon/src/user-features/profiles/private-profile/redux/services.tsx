import { sendRequestForResult } from '../../../../providers/serviceProvider';

let PROFILE_CONTROLLER = 'profile/';

export const updateAvatarToHidrogen = (profile: any) => {
    return sendRequestForResult(PROFILE_CONTROLLER + 'update-avatar', null, profile, 'PUT');
}

export const removeAvatarFromHidrogen = (profileId: number) => {
    return sendRequestForResult(PROFILE_CONTROLLER + 'remove-avatar/' + profileId, null, null, 'DELETE');
}