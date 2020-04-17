import React from 'react';
import _ from 'lodash';
import { connect } from 'react-redux';

import { CONSTANTS } from '../../../helpers/helper';
import { IStatus } from '../../../shared/CarbonAlert';

import CarbonAtavar from '../../../shared/CarbonAtavar';
import BiographyForm from './sub-contents/BiographyForm';
import BiographyShow from './sub-contents/BiographyShow';
import { useDropzone } from 'react-dropzone';
import CarbonAlert from '../../../shared/CarbonAlert';
import CarbonPreloader from '../../../shared/CarbonPreloader';

import { checkApiKeyResult, checkAvatarUploadResult } from './utility';
import { loadAuthenticatedUser } from '../../../authentication/redux/actions';
import { retrievePrivateProfile } from './redux/biography/actions';
import {
    retrieveApiKey,
    uploadAvatarToWater,
    replaceAvatarInWater,
    deleteAvatarInWater
} from '../../../helpers/water/avatar/actions';

const mapStateToProps = (state: any) => ({
    user : state.AuthenticationStore.authUser,
    apiKey : state.AvatarStore.apiKey,
    avatar : state.AvatarStore.avatarResult,
    removed : state.AvatarStore.deleteResult,
    profileResult : state.ProfileStore.getProfile
});

const mapDispatchToProps = {
    retrieveApiKey,
    uploadAvatarToWater,
    loadAuthenticatedUser,
    replaceAvatarInWater,
    deleteAvatarInWater,
    retrievePrivateProfile
};

const APIKEY_ACTIONS = {
    SAVE : 'save_avatar',
    UPDATE : 'replace_avatar',
    REMOVE : 'remove_avatar'
};

const BiographyPane = (props: any) => {
    const [shouldShowAvatarUpload, setShouldShowAvatarUpload] = React.useState(false);
    const [avatar, setAvatar] = React.useState([]);
    const { getRootProps, getInputProps} = useDropzone({
        accept: 'image/jpg,image/jpeg,image/png,image/gif',
        maxSize: 2000000, //2MB
        onDrop: (acceptedAvatar: any) => {
            setAvatar(acceptedAvatar.map((file: any) =>
                Object.assign(file, { preview: URL.createObjectURL(file) }
            )));
        },
        onFileDialogCancel: () => setAvatar([])
    });

    const [status, setStatus] = React.useState({ messages : CONSTANTS.EMPTY, type : CONSTANTS.EMPTY } as IStatus);
    const [apiKey, setApiKey] = React.useState(CONSTANTS.EMPTY);
    const [shouldAllowUpload, setShouldAllowUpload] = React.useState({
        hasImage : false,
        isUploading : false
    });

    const [shouldShowBioForm, setShouldShowBioForm] = React.useState(false);
  
    const avatarPreview = avatar.map((file:any) => (
        <div key={ file.name }>
            <img src={ file.preview } className='responsive-img' alt={ file.name } />
        </div>
    ));

    React.useEffect(() => {
        if (props.user) {
            const { retrievePrivateProfile } = props;
            retrievePrivateProfile(props.user.userId);
        }
    }, []);

    React.useEffect(() => {
        if (props.profileResult.isUpdated) {
            const { retrievePrivateProfile } = props;
            retrievePrivateProfile(props.user.userId);
        }
    }, [props.profileResult.isUpdated]);

    React.useEffect(() => {
        checkApiKeyResult(props.apiKey, setStatus, setApiKey);
    }, [props.apiKey]);

    React.useEffect(() => {
        checkAvatarUploadResult(props.avatar, setStatus, setShouldAllowUpload);
        if (!_.isEmpty(props.avatar)) {
            if (props.avatar.result === 1) {
                setStatus({ messages: 'Your avatar has been updated.', type : 'success' });
                setShouldShowAvatarUpload(false);
                setAvatar([]);

                const localStoredUser = localStorage.getItem('authentication');
                let authUser = JSON.parse(localStoredUser as string);

                authUser.avatar = props.avatar.message;
                localStorage.setItem('authentication', JSON.stringify(authUser));

                const { loadAuthenticatedUser } = props;
                loadAuthenticatedUser();
            }
            else setStatus({ messages: 'An error occurred while updating avatar.', type: 'error' });
        }
    }, [props.avatar]);

    React.useEffect(() => {
        if (props.removed && props.removed.result === 0) setStatus({ messages: props.removed.message, type: 'error' });
        else if (props.removed && props.removed.result !== 0) {
            setStatus({ messages: 'Your avatar has been removed.', type: 'success' });

            const localStoredUser = localStorage.getItem('authentication');
            let authUser = JSON.parse(localStoredUser as string);

            authUser.avatar = null;
            localStorage.setItem('authentication', JSON.stringify(authUser));

            const { loadAuthenticatedUser } = props;
            loadAuthenticatedUser();
        }
    }, [props.removed]);

    React.useEffect(() => () => {
        avatar.forEach((file:any) => URL.revokeObjectURL(file.preview));
        setShouldAllowUpload({
            hasImage : !_.isEmpty(avatar),
            isUploading : false,
        });
    }, [avatar]);

    React.useEffect(() => {
        if (_.isString(apiKey) && apiKey.length !== 0) {
            if (shouldAllowUpload.hasImage && shouldAllowUpload.isUploading) {
                let form = new FormData();
                form.append('file', avatar[0]);
                form.set('hidrogenianId', props.user.userId);
                form.set('apiKey', apiKey);

                if (_.isEmpty(props.user.avatar)) {
                    const { uploadAvatarToWater } = props;
                    uploadAvatarToWater(form);
                }
                else {
                    form.set('currentAvatar', props.user.avatar);

                    const { replaceAvatarInWater } = props;
                    replaceAvatarInWater(form);
                }
            }
            else {
                const { deleteAvatarInWater } = props;
                deleteAvatarInWater({
                    hidrogenianId : props.user.userId,
                    apikey : apiKey
                });
            }
        }
    }, [apiKey]);

    const uploadAvatar = () => {
        const { retrieveApiKey } = props;
        if (_.isEmpty(props.user.avatar)) retrieveApiKey(APIKEY_ACTIONS.SAVE);
        else retrieveApiKey(APIKEY_ACTIONS.UPDATE);

        setShouldAllowUpload({
            hasImage : true,
            isUploading : true
        });
    }

    const removeAvatar = () => {
        const { retrieveApiKey } = props;
        retrieveApiKey(APIKEY_ACTIONS.REMOVE);

        setShouldAllowUpload({
            hasImage : false,
            isUploading : true
        });
    }

    return (
        <div className='row'>
            <h6 className='content-caption'>
                <i className='fas fa-user-circle hidro-primary-icon'></i>&nbsp;&nbsp;Biography
            </h6>
            <div className='col m3 s12' style={{ textAlign:'center' }}>
                <CarbonAtavar size='200px' /><br />
                {
                    (
                        
                        !shouldShowAvatarUpload &&
                        <>
                            <a className='link-text' onClick={ () => setShouldShowAvatarUpload(true) }>Edit Avatar</a><br/>
                            {
                                props.user && props.user.avatar &&
                                <a className='link-text red-text' onClick={ () => removeAvatar() }>Remove</a>
                            }
                        </>
                    ) || <a className='link-text red-text' onClick={ () => setShouldShowAvatarUpload(false) }>Cancel</a>
                }

                <CarbonAlert { ...status } />
                {
                    shouldShowAvatarUpload &&
                    <section className="container">
                        <div {...getRootProps({ className: 'dropzone' })}>
                            <input { ...getInputProps() } multiple={ false } />
                            <p>Drag 'n' drop your photo here, or click to select a photo</p>
                        </div>
                        <aside>
                            {
                                (
                                    avatar.length !== 0 &&
                                    <>
                                        <h6>Selected Avatar</h6>
                                        { avatarPreview }
                                    </>
                                ) || <h6>Select a photo less than 2MB in size.</h6>
                            }
                        </aside>

                        {
                            avatar.length !== 0 &&
                            <button className='btn' onClick={ uploadAvatar } disabled={ !shouldAllowUpload.hasImage && shouldAllowUpload.isUploading }>Upload</button>
                        }

                        {
                            shouldAllowUpload.isUploading &&
                            <>
                                <br /><br />
                                <CarbonPreloader size='small' />
                                <p>Please wait...</p>
                            </>
                        }
                    </section>
                }
            </div>
            <div className='col m9 s12'>
                {
                    (
                        shouldShowBioForm &&
                        <BiographyForm showBioForm={ setShouldShowBioForm } />
                    ) ||
                    <BiographyShow showBioForm={ setShouldShowBioForm } />
                }
            </div>
        </div>
    );
}

export default connect(
    mapStateToProps,
    mapDispatchToProps
)(BiographyPane);