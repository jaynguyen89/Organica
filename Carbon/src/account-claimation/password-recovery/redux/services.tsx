import { sendRequestForResult } from '../../../providers/serviceProvider';
const CONTROLLER = 'authentication/';

export const sendForgotPasswordRequest = (recovery: any) => {
    return sendRequestForResult(CONTROLLER + 'forgot-password', null, recovery, 'POST');
}

export const sendResetPasswordRequest = (resetter: any) => {
    return sendRequestForResult(CONTROLLER + 'set-new-password', null, resetter, 'POST');
}