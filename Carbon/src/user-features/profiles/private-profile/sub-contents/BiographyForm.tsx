import React from 'react';
import { connect } from 'react-redux';
import moment from 'moment-timezone';
import _ from 'lodash';
import $ from 'jquery';
import M from 'materialize-css';

import CKEditor from '@ckeditor/ckeditor5-react';
import InlineEditor from '@ckeditor/ckeditor5-build-inline';

import { IBioForm, VOID_PROFILE } from '../redux/constants';
import { CONSTANTS, GENDERS } from '../../../../helpers/helper';
import CarbonAlert, { IStatus } from '../../../../shared/CarbonAlert';

import { updatePrivateProfile, bioShowShouldUpdate } from '../redux/actions';
import { checkProfileResult } from '../utility';

const mapStateToProps = (state: any) => ({
    user : state.AuthenticationStore.authUser,
    profileResult : state.ProfileStore.getProfile,
    updateResult : state.ProfileStore.updateProfile
});

const mapDispatchToProps = {
    updatePrivateProfile,
    bioShowShouldUpdate
};

const BiographyForm = (props : IBioForm) => {
    const [profile, setProfile] = React.useState(VOID_PROFILE);
    const [selectedDate, setSelectedDate] = React.useState(moment());
    const [status, setStatus] = React.useState({ messages : CONSTANTS.EMPTY, type : CONSTANTS.EMPTY } as IStatus);
    const [shouldCloseBioForm, setShouldCloseBioForm] = React.useState(false);

    React.useEffect(() => {
        M.FormSelect.init($('select'), {});

        M.Datepicker.init($('#birthday'), {
            format : 'dd mmm yyyy',
            onSelect : (selectedDate: any) => setSelectedDate(moment(selectedDate)),
            minDate : moment('01/01/1900').toDate(),
            maxDate : moment().toDate(),
            yearRange : [moment().toDate().getFullYear() - 100, moment().toDate().getFullYear()]
        });

        if (props.profileResult.retrieveSuccess && props.profileResult.profileResult.result === CONSTANTS.SUCCESS)
            setProfile(props.profileResult.profileResult.message);
    }, []);

    React.useEffect(() => {
        if (selectedDate.format('DD MMM YYYY') !== moment().format('DD MMM YYYY'))
            setProfile({
                ...profile,
                birthday : {
                    friendlyBirth : selectedDate.format('DD MMM YYYY'),
                    birth : selectedDate.format('YYYY-MM-DD')
                }
            });
    }, [selectedDate]);

    React.useEffect(() => {
        let result = checkProfileResult(props.updateResult, setStatus);
        if (result && !_.isEmpty(props.updateResult.newProfile)) {
            const { bioShowShouldUpdate } = props;
            bioShowShouldUpdate();

            setShouldCloseBioForm(true);
        }
    }, [props.updateResult]);

    React.useEffect(() => {
        if (shouldCloseBioForm) props.showBioForm(false);
    }, [shouldCloseBioForm]);

    const updateProfileFields = (field: string, value: any) => {
        if (field === 'family-name') setProfile({ ...profile, familyName : value });
        if (field === 'given-name') setProfile({ ...profile, givenName : value });
        if (field === 'gender') setProfile({ ...profile, gender : value });
        if (field === 'ethnicity') setProfile({ ...profile, ethnicity : value });
        if (field === 'company') setProfile({ ...profile, company : value });
        if (field === 'job-title') setProfile({ ...profile, jobTitle : value });
        if (field === 'website') setProfile({ ...profile, website : value });
        if (field === 'self-intro') setProfile({ ...profile, selfIntroduction : value });
    }

    const cancelUpdate = () => {
        if (window.confirm('Changes you made to the profile will be lost. Continue?')) {
            setProfile(VOID_PROFILE);
            props.showBioForm(false);
        }
    }

    const saveProfile = () => {
        let clonedProfile = _.cloneDeep(profile);
        clonedProfile.gender = +profile.gender;

        const { updatePrivateProfile } = props;
        updatePrivateProfile(clonedProfile);
    }

    return (
        <div className='row'>
            <CarbonAlert { ...status } />
            <div className='input-field col m6 s12'>
                <i className='fas fa-file-signature prefix hidro-primary-icon'></i>
                <input id='family-name' type='text' value={ profile?.familyName || CONSTANTS.EMPTY }
                    onChange={ (e: any) => updateProfileFields('family-name', e.target.value) } />
                <label htmlFor='family-name'>Family Name</label>
            </div>
            <div className='input-field col m6 s12'>
                <i className='fas fa-file-signature prefix hidro-primary-icon'></i>
                <input id='given-name' type='text' value={ profile?.givenName || CONSTANTS.EMPTY }
                    onChange={ (e: any) => updateProfileFields('given-name', e.target.value) } />
                <label htmlFor='given-name'>Given Name</label>
            </div>
            <div className='input-field col l4 m6 s12'>
                <i className='fas fa-venus-mars prefix hidro-primary-icon'></i>
                <select id='gender' value={ profile?.gender }
                    onChange={ (e: any) => updateProfileFields('gender', e.target.value) }>
                    {
                        GENDERS.map((sex: string, i: number) =>
                            <option key={ i } value={ i } selected={ profile?.gender === i }>{ sex }</option>
                        )
                    }
                </select>
            </div>
            <div className='input-field col l4 m6 s12'>
                <i className='fas fa-calendar-alt prefix hidro-primary-icon'></i>
                <input id='birthday' type='text' className='date'
                    value={ profile?.birthday.birth ? profile?.birthday.friendlyBirth : CONSTANTS.EMPTY } />
                <label htmlFor='birthday'>Date of Birth</label>
            </div>
            <div className='input-field col l4 m6 s12'>
                <i className='fas fa-user prefix hidro-primary-icon'></i>
                <input id='ethnicity' type='text' value={ profile?.ethnicity || CONSTANTS.EMPTY }
                    onChange={ (e: any) => updateProfileFields('ethnicity', e.target.value) } />
                <label htmlFor='ethnicity'>Ethnicity</label>
            </div>
            <div className='input-field col l4 m6 s12'>
                <i className='fas fa-building prefix hidro-primary-icon'></i>
                <input id='company' type='text' value={ profile?.company || CONSTANTS.EMPTY }
                    onChange={ (e: any) => updateProfileFields('company', e.target.value) } />
                <label htmlFor='company'>Company</label>
            </div>
            <div className='input-field col l4 m6 s12'>
                <i className='fas fa-user-tie prefix hidro-primary-icon'></i>
                <input id='job-title' type='text' value={ profile?.jobTitle || CONSTANTS.EMPTY }
                    onChange={ (e: any) => updateProfileFields('job-title', e.target.value) } />
                <label htmlFor='job-title'>Job Title</label>
            </div>
            <div className='input-field col l4 m12'>
                <i className='fas fa-link prefix hidro-primary-icon'></i>
                <input id='website' type='text' value={ profile?.website || CONSTANTS.EMPTY }
                    onChange={ (e: any) => updateProfileFields('website', e.target.value) } />
                <label htmlFor='website'>Website</label>
            </div>
            <div className='col s12' style={{ marginBottom:'10px' }}>
                <h6>Self Introduction</h6>
                <div className='self-intro'>
                    <CKEditor
                        editor={ InlineEditor } onChange={ (e: any, editor: any) => updateProfileFields('self-intro', editor.getData()) }
                        data={ profile?.selfIntroduction || CONSTANTS.EMPTY } />
                </div>
            </div>
            <div className='col s12 last-col'>
                {
                    !props.updateResult.profileUpdating &&
                    <button className='btn grey darken-1 right' onClick={ cancelUpdate }>Cancel</button>
                }
                <button className='btn' onClick={ saveProfile } disabled={ props.updateResult.profileUpdating }>Save Changes</button>
            </div>
        </div>
    );
}

export default connect(
    mapStateToProps,
    mapDispatchToProps
)(BiographyForm);