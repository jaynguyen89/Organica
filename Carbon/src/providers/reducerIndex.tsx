import { combineReducers } from 'redux';

import RegistrationStore from '../account-registration/redux/reducer';
import ActivationStore from '../account-claimation/account-activation/redux/reducer';
import RecoveryStore from '../account-claimation/password-recovery/redux/reducer';
import AuthenticationStore from '../authentication/redux/reducer';

import AvatarStore from '../helpers/water/avatar/reducer';

export default combineReducers({
    RegistrationStore,
    ActivationStore,
    RecoveryStore,
    AuthenticationStore,
    AvatarStore
});