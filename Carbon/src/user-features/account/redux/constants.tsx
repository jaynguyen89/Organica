import { CONSTANTS } from "../../../helpers/helper";

export const GET_ACCOUNT_IDENTITY_BEGIN = 'GET_ACCOUNT_IDENTITY_BEGIN';
export type T_GET_ACCOUNT_IDENTITY_BEGIN = typeof GET_ACCOUNT_IDENTITY_BEGIN;

export const GET_ACCOUNT_IDENTITY_SUCCESS = 'GET_ACCOUNT_IDENTITY_SUCCESS';
export type T_GET_ACCOUNT_IDENTITY_SUCCESS = typeof GET_ACCOUNT_IDENTITY_SUCCESS;

export const GET_ACCOUNT_IDENTITY_FAILED = 'GET_ACCOUNT_IDENTITY_FAILED';
export type T_GET_ACCOUNT_IDENTITY_FAILED = typeof GET_ACCOUNT_IDENTITY_FAILED;

export const GET_ACCOUNT_TIMESTAMPS_BEGIN = 'GET_ACCOUNT_TIMESTAMPS_BEGIN';
export type T_GET_ACCOUNT_TIMESTAMPS_BEGIN = typeof GET_ACCOUNT_TIMESTAMPS_BEGIN;

export const GET_ACCOUNT_TIMESTAMPS_SUCCESS = 'GET_ACCOUNT_TIMESTAMPS_SUCCESS';
export type T_GET_ACCOUNT_TIMESTAMPS_SUCCESS = typeof GET_ACCOUNT_TIMESTAMPS_SUCCESS;

export const GET_ACCOUNT_TIMESTAMPS_FAILED = 'GET_ACCOUNT_TIMESTAMPS_FAILED';
export type T_GET_ACCOUNT_TIMESTAMPS_FAILED = typeof GET_ACCOUNT_TIMESTAMPS_FAILED;

export const GET_ACCOUNT_TWOFA_BEGIN = 'GET_ACCOUNT_TWOFA_BEGIN';
export type T_GET_ACCOUNT_TWOFA_BEGIN = typeof GET_ACCOUNT_TWOFA_BEGIN;

export const GET_ACCOUNT_TWOFA_SUCCESS = 'GET_ACCOUNT_TWOFA_SUCCESS';
export type T_GET_ACCOUNT_TWOFA_SUCCESS = typeof GET_ACCOUNT_TWOFA_SUCCESS;

export const GET_ACCOUNT_TWOFA_FAILED = 'GET_ACCOUNT_TWOFA_FAILED';
export type T_GET_ACCOUNT_TWOFA_FAILED = typeof GET_ACCOUNT_TWOFA_FAILED;

export const GET_PAYMENT_INFO_BEGIN = 'GET_PAYMENT_INFO_BEGIN';
export type T_GET_PAYMENT_INFO_BEGIN = typeof GET_PAYMENT_INFO_BEGIN;

export const GET_PAYMENT_INFO_SUCCESS = 'GET_PAYMENT_INFO_SUCCESS';
export type T_GET_PAYMENT_INFO_SUCCESS = typeof GET_PAYMENT_INFO_SUCCESS;

export const GET_PAYMENT_INFO_FAILED = 'GET_PAYMENT_INFO_FAILED';
export type T_GET_PAYMENT_INFO_FAILED = typeof GET_PAYMENT_INFO_FAILED;

export const SAVE_ACCOUNT_IDENTITY_BEGIN = 'SAVE_ACCOUNT_IDENTITY_BEGIN';
export type T_SAVE_ACCOUNT_IDENTITY_BEGIN = typeof SAVE_ACCOUNT_IDENTITY_BEGIN;

export const SAVE_ACCOUNT_IDENTITY_SUCCESS = 'SAVE_ACCOUNT_IDENTITY_SUCCESS';
export type T_SAVE_ACCOUNT_IDENTITY_SUCCESS = typeof SAVE_ACCOUNT_IDENTITY_SUCCESS;

export const SAVE_ACCOUNT_IDENTITY_FAILED = 'SAVE_ACCOUNT_IDENTITY_FAILED';
export type T_SAVE_ACCOUNT_IDENTITY_FAILED = typeof SAVE_ACCOUNT_IDENTITY_FAILED;

export const SAVE_PAYMENT_INFO_BEGIN = 'SAVE_PAYMENT_INFO_BEGIN';
export type T_SAVE_PAYMENT_INFO_BEGIN = typeof SAVE_PAYMENT_INFO_BEGIN;

export const SAVE_PAYMENT_INFO_SUCCESS = 'SAVE_PAYMENT_INFO_SUCCESS';
export type T_SAVE_PAYMENT_INFO_SUCCESS = typeof SAVE_PAYMENT_INFO_SUCCESS;

export const SAVE_PAYMENT_INFO_FAILED = 'SAVE_PAYMENT_INFO_FAILED';
export type T_SAVE_PAYMENT_INFO_FAILED = typeof SAVE_PAYMENT_INFO_FAILED;

export const SAVE_PASSWORD_UPDATE_BEGIN = 'SAVE_PASSWORD_UPDATE_BEGIN';
export type T_SAVE_PASSWORD_UPDATE_BEGIN = typeof SAVE_PASSWORD_UPDATE_BEGIN;

export const SAVE_PASSWORD_UPDATE_SUCCESS = 'SAVE_PASSWORD_UPDATE_SUCCESS';
export type T_SAVE_PASSWORD_UPDATE_SUCCESS = typeof SAVE_PASSWORD_UPDATE_SUCCESS;

export const SAVE_PASSWORD_UPDATE_FAILED = 'SAVE_PASSWORD_UPDATE_FAILED';
export type T_SAVE_PASSWORD_UPDATE_FAILED = typeof SAVE_PASSWORD_UPDATE_FAILED;

export const ENABLE_TWO_FA_BEGIN = 'ENABLE_TWO_FA_BEGIN';
export type T_ENABLE_TWO_FA_BEGIN = typeof ENABLE_TWO_FA_BEGIN;

export const ENABLE_TWO_FA_SUCCESS = 'ENABLE_TWO_FA_SUCCESS';
export type T_ENABLE_TWO_FA_SUCCESS = typeof ENABLE_TWO_FA_SUCCESS;

export const ENABLE_TWO_FA_FAILED = 'ENABLE_TWO_FA_FAILED';
export type T_ENABLE_TWO_FA_FAILED = typeof ENABLE_TWO_FA_FAILED;

export const DISABLE_TWO_FA_BEGIN = 'DISABLE_TWO_FA_BEGIN';
export type T_DISABLE_TWO_FA_BEGIN = typeof DISABLE_TWO_FA_BEGIN;

export const DISABLE_TWO_FA_SUCCESS = 'DISABLE_TWO_FA_SUCCESS';
export type T_DISABLE_TWO_FA_SUCCESS = typeof DISABLE_TWO_FA_SUCCESS;

export const DISABLE_TWO_FA_FAILED = 'DISABLE_TWO_FA_FAILED';
export type T_DISABLE_TWO_FA_FAILED = typeof DISABLE_TWO_FA_FAILED;

export interface IIdentity {
    id : number,
    email : string,
    emailConfirmed : boolean,
    userName : string,
    phoneNumber : string | null,
    phoneConfirmed : boolean,
    captchaToken : string | null
}

export interface IAccountShow {
    user : any,
    identity : IIdentity,
    getPaymentInfo : any,
    getPaymentInformation : any,
    saveAccountIdentity : any,
    savePaymentInfo : any
}

export interface ITwoFa {
    id : number,
    qrImageUrl : string | null,
    manualQrCode : string | null,
    captchaToken : string | null
}

export interface ISecurityPane {
    user : any,
    twoFa : ITwoFa,
    saveNewPassword : any,
    enableOrRefreshTwoFa : any,
    disableTwoFa : any
}

export interface IPasswordUpdate {
    id : number,
    password : string,
    newPassword : string,
    passwordConfirm : string,
    captchaToken : string | null
}

export interface ISecurityForm {
    setIsUpdating : any,
    updatePassword : any,
    savePassword : any,
    passwordForm : IPasswordUpdate
}

export interface ICreditCard {
    holderName : string,
    cardNumber : string,
    expiryDate : string,
    securityCode : string
}

export interface IPaypal {
    email : string,
    addedOn : string
}

interface IPaymentMethod {
    id : number,
    accountBalance : number,
    balanceDate : string | null,
    creditCard : ICreditCard | null,
    paypal : IPaypal | null
}

export interface IPaymentInfo {
    hidrogenianId : number,
    paymentMethod : IPaymentMethod
}

export const VOID_PW : IPasswordUpdate = {
    id : 0,
    password : CONSTANTS.EMPTY,
    newPassword : CONSTANTS.EMPTY,
    passwordConfirm : CONSTANTS.EMPTY,
    captchaToken : null
};

export const VOID_TFA : ITwoFa = {
    id : 0,
    qrImageUrl : null,
    manualQrCode : null,
    captchaToken : null
};