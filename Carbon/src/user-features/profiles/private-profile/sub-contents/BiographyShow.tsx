import React from 'react';
import { connect } from 'react-redux';

import CarbonPreloader from '../../../../shared/CarbonPreloader';
import CarbonAlert from '../../../../shared/CarbonAlert';

import { IBioShow, IProfile } from '../redux/constants';
import { CONSTANTS, GENDERS } from '../../../../helpers/helper';

const mapStateToProps = (state: any) => ({
    user : state.AuthenticationStore.authUser,
    profileResult : state.ProfileStore.getProfile
});

const mapDispatchToProps = {
    
};

const BiographyShow = (props: IBioShow) => {
    const [profile, setProfile] = React.useState(null as unknown as IProfile);

    React.useEffect(() => {
        if (props.profileResult.retrieveSuccess && props.profileResult.profileResult.result === CONSTANTS.SUCCESS)
            setProfile(props.profileResult.profileResult.message);
        else setProfile(null as unknown as IProfile);
    }, [props.profileResult]);

    return (
        <div className='row'>
            {
                props.profileResult.isUpdated &&
                <CarbonAlert messages='Your profile has been updated successfully.' type='success' />
            }
            {
                (
                    props.profileResult.profileRetrieving &&
                    <div className='col s12 center'>
                        <CarbonPreloader size='small' />
                        <p>Retrieving your profile information...</p>
                    </div>
                ) || (
                    (
                        (
                            props.profileResult.retrieveSuccess &&
                            <>
                                {
                                    (
                                        props.profileResult.profileResult.result === 1 &&
                                        <>
                                            <div className='col l3 m6 s12'>
                                                <p className='bioshow'>
                                                    <b>Full Name:</b>&nbsp;{ profile?.fullName }
                                                </p>
                                            </div>
                                            <div className='col l3 m6 s12'>
                                                <p className='bioshow'>
                                                    <b>Birth:</b>&nbsp;{ profile?.birthday.friendlyBirth }
                                                </p>
                                            </div>
                                            <div className='col l3 m6 s6'>
                                                <p className='bioshow'>
                                                    <b>Gender:</b>&nbsp;{ GENDERS[profile?.gender] }
                                                </p>
                                            </div>
                                            <div className='col l3 m6 s6'>
                                                <p className='bioshow'>
                                                    <b>Ethnicity:</b>&nbsp;{ profile?.ethnicity || CONSTANTS.NA }
                                                </p>
                                            </div>
                                            <div className='col m6 s12'>
                                                <p className='bioshow'>
                                                    <b>Company:</b>&nbsp;{ profile?.company || CONSTANTS.NA }
                                                </p>
                                            </div>
                                            <div className='col m6 s12'>
                                                <p className='bioshow'>
                                                    <b>Job Title:</b>&nbsp;{ profile?.jobTitle || CONSTANTS.NA }
                                                </p>
                                            </div>
                                            <div className='col m6 s12'>
                                                <p className='bioshow'>
                                                    <b>Website:</b>&nbsp;
                                                    <a href={ profile?.website } target='_blank'>{ profile?.website || CONSTANTS.NA }</a>
                                                </p>
                                            </div>
                                            {
                                                profile?.selfIntroduction &&
                                                <div className='col s12'>
                                                    <p><b>Self Introduction</b></p>
                                                    <div className='self-intro-box' dangerouslySetInnerHTML={{ __html : profile?.selfIntroduction }}></div>
                                                </div>
                                            }
                                            <div className='col s12'>
                                                <button className='btn' onClick={ () => props.showBioForm(true) }>Update</button>
                                            </div>
                                        </>
                                    ) ||
                                    <div className='col s12 center'>
                                        <CarbonAlert messages={ props.profileResult.profileResult.message } type='error' persistent={ true } />
                                    </div>
                                }
                            </>
                        ) ||
                        <div className='col s12 center'>
                            <CarbonAlert messages='Unable to get your profile details from Hidrogen due to network lost. Please check your network connection.'
                                type='warning' persistent={ true } />
                        </div>
                    )
                )
            }
        </div>
    );
}

export default connect(
    mapStateToProps,
    mapDispatchToProps
)(BiographyShow);