import React from 'react';
import { connect } from 'react-redux';

import { getGravatarURL } from '../helpers/helper';

const mapStateToProps = (state: any) => ({
    auth : state.AuthenticationStore.authUser
});

const CarbonAvatar = (props: any) => {
    return (
        (
            props.auth.avatar &&
            <img className='avatar-round' src={ props.auth.avatar } />
        ) || <img className='avatar-round' src={ getGravatarURL(props.auth.email) } />
    );
}

export default connect(
    mapStateToProps
)(CarbonAvatar);