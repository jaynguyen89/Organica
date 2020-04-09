import { sendRequestForResult } from '../../../providers/serviceProvider';
const CONTROLLER = 'authentication/';

export const activateHidrogenianAccount = (activator: any) => {
    return sendRequestForResult(CONTROLLER + 'activate-account', null, activator, 'POST');
}

export const requestNewActivationEmail = (requester: any) => {
    return sendRequestForResult(CONTROLLER + 'request-new-activation-email', null, requester, 'POST');
}