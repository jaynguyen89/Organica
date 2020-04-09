import { sendRequestForResult } from '../../providers/serviceProvider';
const CONTROLLER = 'authentication/';

export const signInHidrogenianByCredentials = (credentials: any) => {
    return sendRequestForResult(CONTROLLER + 'authenticate', null, credentials, 'POST');
}

export const signOutHidrogenian = (auth: any) => {
    return sendRequestForResult(CONTROLLER + 'sign-out', auth, null, 'GET');
}