import { sendRequestForResult } from '../../../providers/serviceProvider';
import { IIdentity, IPaymentInfo, IPasswordUpdate, ITwoFa } from './constants';

const ACCOUNT_CONTROLLER = 'account/';
const PAYMENT_CONTROLLER = 'payment/';

export const getAccountIdentity = (hidrogenianId: number) => {
    return sendRequestForResult(ACCOUNT_CONTROLLER + 'get-identity/' + hidrogenianId, null, null, 'GET');
}

export const getTwoFaData = (hidrogenianId: number) => {
    return sendRequestForResult(ACCOUNT_CONTROLLER + 'get-two-fa/' + hidrogenianId, null, null, 'GET');
}

export const getTimeStamps = (hidrogenianId: number) => {
    return sendRequestForResult(ACCOUNT_CONTROLLER + 'get-time-logs/' + hidrogenianId, null, null, 'GET');
}

export const getPaymentInfo = (hidrogenianId: number) => {
    return sendRequestForResult(PAYMENT_CONTROLLER + 'payment-details/' + hidrogenianId, null, null, 'GET');
}

export const updateAccountIdentity = (identity: IIdentity) => {
    return sendRequestForResult(ACCOUNT_CONTROLLER + 'update-identity', null, identity, 'POST');
}

export const savePaymentMethod = (paymentMethod: IPaymentInfo) => {
    return sendRequestForResult(PAYMENT_CONTROLLER + 'update-payment-details', null, paymentMethod, 'POST');
}

export const updateAccountPassword = (pwSetter: IPasswordUpdate) => {
    return sendRequestForResult(ACCOUNT_CONTROLLER + 'update-security', null, pwSetter, 'POST');
}

export const enableOrRefreshTwoFa = (twoFa: ITwoFa) => {
    return sendRequestForResult(ACCOUNT_CONTROLLER + 'enable-or-refresh-two-fa', null, twoFa, 'POST');
}

export const disableTwoFa = (twoFa: ITwoFa) => {
    return sendRequestForResult(ACCOUNT_CONTROLLER + 'disable-two-fa/', null, twoFa, 'POST');
}