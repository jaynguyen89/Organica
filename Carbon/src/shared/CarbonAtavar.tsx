import React from 'react';
import { connect } from 'react-redux';

import { getGravatarURL, CONSTANTS } from '../helpers/helper';

const mapStateToProps = (state: any) => ({
    auth : state.AuthenticationStore.authUser
});

const CarbonAvatar = (props: any) => {
    return (
        (
            props.auth && props.auth.avatar &&
            <img className='avatar-round responsive-img' src={ 'https://water.jaydeveloper.com/files/avatars/' + props.auth.avatar } width={ props.size } />
        ) || (
            props.auth && <img className='avatar-round' src={ getGravatarURL(props.auth.email || CONSTANTS.EMPTY, props.size) } />
        )
    );
}

export default connect(
    mapStateToProps
)(CarbonAvatar);