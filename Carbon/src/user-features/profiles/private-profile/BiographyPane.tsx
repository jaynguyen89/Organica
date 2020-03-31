import React from 'react';
import { connect } from 'react-redux';
import { useDropzone } from 'react-dropzone';

import { uploadProfileAvatarToImagebb } from './redux/actions';

import CarbonAtavar from '../../../shared/CarbonAtavar';
import BiographyForm from './sub-contents/BiographyForm';
import BiographyShow from './sub-contents/BiographyShow';

const mapStateToProps = (state: any) => ({

});

const mapDispatchToProps = {
    uploadProfileAvatarToImagebb
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
  
    const avatarPreview = avatar.map((file:any) => (
        <div key={ file.name }>
            <img src={ file.preview } className='responsive-img' alt={ file.name } />
        </div>
    ));

    React.useEffect(() => () => {
        avatar.forEach((file:any) => URL.revokeObjectURL(file.preview));
    }, [avatar]);

    const uploadAvatar = () => {
        let form = new FormData();
        form.append('test', avatar[0]);

        const { uploadProfileAvatarToImagebb } = props;
        uploadProfileAvatarToImagebb(form);
    }

    return (
        <div className='row'>
            <h6 className='content-caption'>
                <i className='fas fa-user-circle hidro-primary-icon'></i>&nbsp;&nbsp;Biography
            </h6>
            <div className='col m3 s12' style={{ textAlign:'center' }}>
                <CarbonAtavar /><br />
                {
                    (
                        !shouldShowAvatarUpload &&
                        <a className='link-text' onClick={ () => setShouldShowAvatarUpload(true) }>Edit Avatar</a>
                    ) || <a className='link-text red-text' onClick={ () => setShouldShowAvatarUpload(false) }>Cancel</a>
                }

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
                            <button className='btn' onClick={ uploadAvatar }>Upload</button>
                        }
                    </section>
                }
            </div>
            <div className='col m9 s12'>
                <BiographyShow />
                <BiographyForm />
                <img src='https://i.ibb.co/jZmmFy8/Hinh-The.jpg' className='responsive-img' />
            </div>
        </div>
    );
}

export default connect(
    mapStateToProps,
    mapDispatchToProps
)(BiographyPane);