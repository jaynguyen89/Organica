import _ from 'lodash';

import { CONSTANTS } from "../../helpers/helper"
import { ICreditCard, IPaypal } from './redux/constants';

export const checkIdentityResult = (result: any) => {
    if (result.isSending) return CONSTANTS.EMPTY;

    if (!result.isSending && !result.getSuccess && !_.isEmpty(result.getResult) && result.getResult.hasOwnProperty('stack'))
        return 'network lost';

    if (!result.isSending && result.getSuccess && !_.isEmpty(result.getResult) && result.getResult.hasOwnProperty('result'))
        return result.getResult.message;
}

export const checkTwoFaResult = (result: any) => {
    if (result.isSending) return CONSTANTS.EMPTY;

    if (!result.isSending && !result.getSuccess && !_.isEmpty(result.getResult) && result.getResult.hasOwnProperty('stack'))
        return 'network lost';

    if (!result.isSending && result.getSuccess && !_.isEmpty(result.getResult) && result.getResult.hasOwnProperty('result')) {
        if (result.getResult.result !== 1) return result.getResult.message;

        if (!result.getResult.hasOwnProperty('message')) return 'twofa unset';

        return result.getResult.message;
    }
}

export const updateIdentityFields = (backup: any, setBackup: any, field: string, value: string) => {
    if (field === 'email') setBackup({ ...backup, identity : { ...backup.identity, email : value } });
    if (field === 'username') setBackup({ ...backup, identity : { ...backup.identity, userName : value } });
    if (field === 'phone') setBackup({ ...backup, identity : { ...backup.identity, phoneNumber : value } });
}

export const updatePaymentFields = (backup: any, setBackup: any, field: string, value: string) => {
    if (field === 'holder') setBackup({ ...backup, paymentInfo : { ...backup.paymentInfo, paymentMethod: { ...backup.paymentInfo.paymentMethod, creditCard : { ...backup.paymentInfo.paymentMethod.creditCard, holderName : value } as ICreditCard }}});
    if (field === 'number') setBackup({ ...backup, paymentInfo : { ...backup.paymentInfo, paymentMethod: { ...backup.paymentInfo.paymentMethod, creditCard : { ...backup.paymentInfo.paymentMethod.creditCard, cardNumber : value } as ICreditCard }}});
    if (field === 'code') setBackup({ ...backup, paymentInfo : { ...backup.paymentInfo, paymentMethod: { ...backup.paymentInfo.paymentMethod, creditCard : { ...backup.paymentInfo.paymentMethod.creditCard, securityCode : value } as ICreditCard }}});

    if (field === 'email') setBackup({ ...backup, paymentInfo : { ...backup.paymentInfo, paymentMethod : { ...backup.paymentInfo.paymentMethod, paypal : { ...backup.paymentInfo.paymentMethod.paypal, email : value } as IPaypal}}});

    if (field === 'month' || field === 'year') {
        let expiryDate = backup.paymentInfo.paymentMethod.creditCard?.expiryDate;

        if (expiryDate) {
            if (field === 'month') expiryDate = value + '/' + expiryDate?.split('/')[1];
            if (field === 'year') expiryDate = expiryDate?.split('/')[0] + '/' + value;
        }
        else expiryDate = field === 'month' ? value + '/' : (field === 'year' ? '/' + value : '');

        setBackup({ ...backup, paymentInfo : { ...backup.paymentInfo, paymentMethod : { ...backup.paymentInfo.paymentMethod, creditCard : { ...backup.paymentInfo.paymentMethod.creditCard, expiryDate : expiryDate } as ICreditCard}}});
    }
}