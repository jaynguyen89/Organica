import { CONSTANTS } from "../helpers/helper";
import _ from 'lodash';

export const validateEmail = (email: string) => {
    if (email.length === 0)
        return false;
        
    email = email.trim().replace('/\s/g', CONSTANTS.EMPTY);
    if (email.length === 0) return false;
    
    return /^([\w\.\-]+)+\@([\w\-]+)((\.(\w){2,3})+)$/.test(email);
}

export const validateEmailForResult = (email: string) => {
    if (email.length === 0)
        return [];
    
    email = email.trim().replace('/\s/g', CONSTANTS.EMPTY);
    if (email.length === 0)
        return ['Email is empty. No space is allowed.'];
    
    let errors:string[] = [];
    if (/.{6,50}/.test(email) === false)
        errors.push('Email should be 6 to 50 characters in length.');
    
    if (/^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$/.test(email) === false)
        errors.push('Email seems to be invalid.');
    
    if (errors.length === 0) {
        var domain = email.split('@')[1];
        if (domain.split('.').length > 3)
            errors.push('Email seems to be invalid.');
    }

    return errors.length === 0 ? email : errors;
}

export const validatePasswordForResult = (password: string, passwordConfirm: string) => {
    if (password.length === 0)
        return [];
    
    let pwNoSpace = password.trim().replace('/\s/g', CONSTANTS.EMPTY);
    if (pwNoSpace.length === 0)
        return ['Password and Confirm Password are both required.'];

    let errors:string[] = [];
    if (/.{6,20}/.test(password) === false)
        errors.push('Password length must be between 6 and 20 characters.');

    if (/[a-z]+/.test(password) === false)
        errors.push('Password must contain at least 1 lowercase character.');
    
    if (/[A-Z]+/.test(password) === false)
        errors.push('Password must contain at least 1 uppercase character.');

    if (/[\d]+/.test(password) === false)
        errors.push('Password must contain at least 1 number.');

    if (/[!@#$%^&*_+\.]+/.test(password) === false)
        errors.push('Password must contain at least 1 of these special characters: !@#$%^&*_+.');

    if (password.indexOf(CONSTANTS.SPACE) !== -1)
        errors.push('Password should not contain whitespace.');
    
    if (password !== passwordConfirm)
        errors.push('Password and Confirm Password must be the same.');

    return errors.length === 0 ? password : errors;
}

export const displayPasswordResetMessages = (result: any, setStatus: any) => {
    if (_.isBoolean(result.result) && result.result === false && result.hasOwnProperty('hostName'))
        setStatus({ messages : 'Failed to check your humanity. Please verify the ReCaptcha again.', type : 'error' });
    
    if (result.result === CONSTANTS.FAILED && result.hasOwnProperty('error'))
        setStatus({ messages : 'Error ' + result.error + ': A problem has occurred while setting new password for your account. Please try again.', type : 'error' });

    if (result.result === CONSTANTS.FAILED || result.result === CONSTANTS.INTERRUPTED)
        setStatus({ messages : result.message, type : (result.result === CONSTANTS.FAILED && 'error') || 'success' });
    
    if (result.result === CONSTANTS.SUCCESS)
        setStatus({ messages : 'Congratulation! Your new password has been set successfully. You can now login.', type : 'success' });
}