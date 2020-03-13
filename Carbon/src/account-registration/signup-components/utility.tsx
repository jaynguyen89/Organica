import _ from 'lodash';
import $ from 'jquery';
import { CONSTANTS } from '../../helpers/helper';

const INVALIDS:string[] = [
    '--', '_@', '-@', '.-', '-.', '._', '_.', ' ', '@_', '@-', '__', '..', '_-', '-_'
];

export const validateRegistrationInputs = (
    username: string, email: string, password: string,
    passwordConfirm: string, familyName: string, givenName: string
) => {
    let usernameResult = validateUsername(username);
    let emailResult = validateEmail(email);
    let passwordResult = validatePassword(password, passwordConfirm);
    let fnResult = validateFamilyName(familyName);
    let gnResult = validateGivenName(givenName);

    let messages:string[] = [];
    if (_.isArray(usernameResult)) messages.push(...(usernameResult as Array<string>));
    if (_.isArray(emailResult)) messages.push(...(emailResult as Array<string>));
    if (_.isArray(passwordResult)) messages.push(...(passwordResult as Array<string>));
    if (_.isArray(fnResult)) messages.push(...(fnResult as Array<string>));
    if (_.isArray(gnResult)) messages.push(...(gnResult as Array<string>));

    let hilights:string[] = [];
    if (_.isArray(usernameResult) && usernameResult.length !== 0) hilights.push('username');
    if (_.isArray(emailResult) && emailResult.length !== 0) hilights.push('email');
    if (_.isArray(passwordResult) && passwordResult.length !== 0) hilights.push('password');
    if (_.isArray(fnResult) && fnResult.length !== 0) hilights.push('family');
    if (_.isArray(gnResult) && gnResult.length !== 0) hilights.push('given');

    let valid = _.isString(usernameResult) && _.isString(emailResult) && _.isString(passwordResult) &&
                _.isString(fnResult) && _.isString(gnResult);

    return { errors : messages, fields : hilights, valid : valid };
}

export const highlightInputFields = (fields:string[]) => {
    if (fields.indexOf('username') !== -1) $('#username').css('background-color', '#FFEBEE');
    else $('#username').css('background-color', '#FFF');

    if (fields.indexOf('email') !== -1) $('#email').css('background-color', '#FFEBEE');
    else $('#email').css('background-color', '#FFF');

    if (fields.indexOf('password') !== -1) {
        $('#password').css('background-color', '#FFEBEE');
        $('#confirm').css('background-color', '#FFEBEE');
    }
    else {
        $('#password').css('background-color', '#FFF');
        $('#confirm').css('background-color', '#FFF');
    }

    if (fields.indexOf('family-name') !== -1) $('#family-name').css('background-color', '#FFEBEE');
    else $('#family-name').css('background-color', '#FFF');

    if (fields.indexOf('given-name') !== -1) $('#given-name').css('background-color', '#FFEBEE');
    else $('#given-name').css('background-color', '#FFF');
}

export const displayUsualRegistrationResult = (result: any, setStatus: any) => {
    if (_.isBoolean(result.result) && !result.result) {
        setStatus({ messages : 'ReCaptcha validation failed. Please confirm you are human.', type : 'error' });
        return;
    }
    
    if (result.result === CONSTANTS.FAILED && result.hasOwnProperty('error')) {
        setStatus({ messages : 'ReCaptcha validation failed. Please confirm you are human.', error : result.error, type : 'error' });
        return;
    }

    if (result.result === CONSTANTS.FAILED || result.result === CONSTANTS.INTERRUPTED) {
        setStatus({ messages : result.message, type : (result.result === CONSTANTS.FAILED && 'error') || 'success' });
        return;
    }

    if (result.result === CONSTANTS.SUCCESS)
        setStatus({ messages : 'Your account has been created successfully. Please check your email to activate account then start exploring Hidrogen.', type : 'success' });
}

const validateUsername = (username: string) => {
    if (username.length === 0)
        return [];

    username = username.trim().replace('/\s/g', CONSTANTS.EMPTY);
    if (username.length === 0)
        return ['Username is empty. No space is allowed.'];

    let errors:string[] = [];
    if (username.indexOf(CONSTANTS.SPACE) !== -1)
        errors.push('Username should not contain white space.');

    if (/.{3,20}/.test(username) === false)
        errors.push('Username should be 3 to 20 characters in length.');

    if (/^[0-9A-Za-z_\-.]*$/.test(username) === false)
        errors.push('Username can contain numbers, alphabet letters and these special characters: -_.');
    
    if (errors.length === 0 && INVALIDS.some((el:string) => username.indexOf(el) !== -1))
        errors.push('Username should not have adjacent special characters.');
    
    return errors.length === 0 ? username : errors;
}

const validateEmail = (email: string) => {
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

const validatePassword = (password: string, passwordConfirm: string) => {
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

const validateFamilyName = (familyName: string) => {
    if (familyName.length === 0)
        return [];

    let fnNoSpace = familyName.trim().replace('/\s/g', CONSTANTS.EMPTY);
    if (fnNoSpace.length === 0)
        return ['Family Name is empty.'];
    
    familyName = familyName.trim().replace('/\s\s+/g', CONSTANTS.SPACE);

    let errors:string[] = [];
    if (/.{1,30}/.test(familyName) === false)
        errors.push('Family Name has maximum 30 characters.');
    
    if (/^[A-Za-z_\-.'() ]*$/.test(familyName) === false)
        errors.push('Family Name should only contain alphabet letters and these special characters: _-.\'()');
    
        return errors.length === 0 ? familyName : errors;
}

const validateGivenName = (givenName: string) => {
    if (givenName.length === 0)
        return [];

    let fnNoSpace = givenName.trim().replace('/\s/g', CONSTANTS.EMPTY);
    if (fnNoSpace.length === 0)
        return ['Given Name is empty.'];
    
    givenName = givenName.trim().replace('/\s\s+/g', CONSTANTS.SPACE);

    let errors:string[] = [];
    if (/.{1,50}/.test(givenName) === false)
        errors.push('Given Name has maximum 50 characters.');
    
    if (/^[A-Za-z_\-.'() ]*$/.test(givenName) === false)
        errors.push('Given Name should only contain alphabet letters and these special characters: _-.\'()');
    
        return errors.length === 0 ? givenName : errors;
}