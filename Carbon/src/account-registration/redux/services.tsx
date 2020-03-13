import sendRequestForResult from '../../providers/serviceProvider';
const CONTROLLER = 'authentication/';

const registerHidrogenianAccount = (registrationData: any) => {
    return sendRequestForResult(CONTROLLER + 'register-account', null, registrationData, 'POST');
}

export default registerHidrogenianAccount;