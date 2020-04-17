import { sendRequestForResult } from '../../../../../providers/serviceProvider';
import { IProfile } from './constants';

let PROFILE_CONTROLLER = 'profile/';

export const retrievePrivateProfile = (hidrogenianId: number) => {
    return sendRequestForResult(PROFILE_CONTROLLER + 'get-private-profile/' + hidrogenianId, null, null, 'GET');
}

export const updatePrivateProfile = (profile: IProfile) => {
    return sendRequestForResult(PROFILE_CONTROLLER + 'update-private-profile', null, profile, 'POST');
}