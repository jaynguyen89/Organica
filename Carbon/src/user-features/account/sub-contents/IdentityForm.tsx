import React from 'react';
import { IIdentity } from '../redux/constants';

const IdentityForm = (props: any) => {
    const [identity, setIdentity] = React.useState(null as unknown as IIdentity);

    React.useEffect(() => {
        if (props.identity) setIdentity(props.identity);
    }, [props.identity]);

    return (
        identity &&
        <div className='row'>
            <div className='input-field col s12'>
                <i className='fas fa-at prefix hidro-primary-icon'></i>
                <input id='identity-email' type='text' value={ identity.email }
                    onChange={ (e: any) => props.updateIdentity('email', e.target.value) } />
                <label htmlFor='identity-email'>Email</label>
            </div>
            <div className='input-field col s12'>
                <i className='fas fa-id-badge prefix hidro-primary-icon'></i>
                <input id='identity-username' type='text' value={ identity.userName }
                    onChange={ (e: any) => props.updateIdentity('username', e.target.value) } />
                <label htmlFor='identity-username'>Username</label>
            </div>
            <div className='input-field col s12'>
                <i className='fas fa-phone-alt prefix hidro-primary-icon'></i>
                <input id='identity-phone' type='text' value={ identity.phoneNumber as string }
                    onChange={ (e: any) => props.updateIdentity('phone', e.target.value) } />
                <label htmlFor='identity-phone'>Phone Number</label>
            </div>
            <div className='col s12'>
                <button className='btn' onClick={ () => props.saveIdentity() }>Update</button>
                <button className='btn right grey darken-1' onClick={ () => props.closeModal() }>Cancel</button>
            </div>
        </div>
    );
}

export default IdentityForm;