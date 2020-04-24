import { combineReducers } from 'redux';

import RegistrationStore from '../account-registration/redux/reducer';
import ActivationStore from '../account-claimation/account-activation/redux/reducer';
import RecoveryStore from '../account-claimation/password-recovery/redux/reducer';
import AuthenticationStore from '../authentication/redux/reducer';
import AvatarStore from '../helpers/water/avatar/reducer';
import ProfileStore from '../user-features/profiles/private-profile/redux/biography/reducer';
import AddressStore from '../user-features/profiles/private-profile/redux/address/reducer';
import AccountStore from '../user-features/account/redux/reducer';

export default combineReducers({
    RegistrationStore,
    ActivationStore,
    RecoveryStore,
    AuthenticationStore,
    AvatarStore,
    ProfileStore,
    AddressStore,
    AccountStore
});