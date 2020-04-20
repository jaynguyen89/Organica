import React from 'react';
import M from 'materialize-css';

import LocationAutoDetect from './LocationDetector';
import { VOID_ADDRESS, IAddress, ICountry } from '../redux/address/constants';

const LocalAddressForm = (props: any) => {
    const [address, setAddress] = React.useState(VOID_ADDRESS as IAddress);
    const [countries, setCountries] = React.useState([]);

    React.useEffect(() => {
        setAddress(props.address);
        setCountries(props.countries);

        M.updateTextFields();
    }, [props]);

    return (
        <div className='row address-form-body'>
            <LocationAutoDetect detectAddress={ props.detectAddress } />
            <div className='input-field col l4 m6 s12'>
                <i className='fas fa-quote-left prefix hidro-primary-icon'></i>
                <input id='l-address-title' type='text' value={ address.title }
                    onChange={ (e: any) => props.updateAddress('local', 'title', e.target.value) } />
                <label htmlFor='l-address-title'>Address Title</label>
            </div>
            <div className='input-field col l4 m6 s12'>
                <i className='fas fa-building prefix hidro-primary-icon'></i>
                <input id='l-address-pobox' type='text' value={ address.lAddress?.poBox as string }
                    onChange={ (e: any) => props.updateAddress('local', 'pobox', e.target.value) } />
                <label htmlFor='l-address-pobox'>PO Box</label>
            </div>
            <div className='input-field col l4 m6 s12'>
                <i className='fas fa-building prefix hidro-primary-icon'></i>
                <input id='l-address-building' type='text' value={ address.lAddress?.buildingName as string }
                    onChange={ (e: any) => props.updateAddress('local', 'building', e.target.value) } />
                <label htmlFor='l-address-building'>Building Address</label>
            </div>
            <div className='input-field col m6 s12'>
                <i className='fas fa-road prefix hidro-primary-icon'></i>
                <input id='l-address-lane' type='text' value={ address.lAddress?.lane as string }
                    onChange={ (e: any) => props.updateAddress('local', 'lane', e.target.value) } />
                <label htmlFor='l-address-lane'>Lane</label>
            </div>
            <div className='input-field col m6 s12'>
                <i className='fas fa-home prefix hidro-primary-icon'></i>
                <input id='l-address-street' type='text' value={ address.lAddress?.streetAddress }
                    onChange={ (e: any) => props.updateAddress('local', 'street', e.target.value) } />
                <label htmlFor='l-address-street'>Street Address</label>
            </div>
            <div className='input-field col m6 s12'>
                <i className='fas fa-users prefix hidro-primary-icon'></i>
                <input id='l-address-group' type='text' value={ address.lAddress?.group as string }
                    onChange={ (e: any) => props.updateAddress('local', 'group', e.target.value) } />
                <label htmlFor='l-address-group'>Group</label>
            </div>
            <div className='input-field col m6 s12'>
                <i className='fas fa-torii-gate prefix hidro-primary-icon'></i>
                <input id='l-address-quarter' type='text' value={ address.lAddress?.quarter as string }
                    onChange={ (e: any) => props.updateAddress('local', 'quarter', e.target.value) } />
                <label htmlFor='l-address-quarter'>Quarter</label>
            </div>
            <div className='input-field col l4 m6 s12'>
                <i className='fas fa-landmark prefix hidro-primary-icon'></i>
                <input id='l-address-ward' type='text' value={ address.lAddress?.ward as string }
                    onChange={ (e: any) => props.updateAddress('local', 'ward', e.target.value) } />
                <label htmlFor='l-address-ward'>Ward</label>
            </div>
            <div className='input-field col l4 m6 s12'>
                <i className='fas fa-landmark prefix hidro-primary-icon'></i>
                <input id='l-address-hamlet' type='text' value={ address.lAddress?.hamlet as string }
                    onChange={ (e: any) => props.updateAddress('local', 'hamlet', e.target.value) } />
                <label htmlFor='l-address-hamlet'>Hamlet</label>
            </div>
            <div className='input-field col l4 m6 s12'>
                <i className='fas fa-landmark prefix hidro-primary-icon'></i>
                <input id='l-address-commute' type='text' value={ address.lAddress?.commute as string }
                    onChange={ (e: any) => props.updateAddress('local', 'commute', e.target.value) } />
                <label htmlFor='l-address-commute'>Commute</label>
            </div>
            <div className='input-field col m6 s12'>
                <i className='fas fa-place-of-worship prefix hidro-primary-icon'></i>
                <input id='l-address-town' type='text' value={ address.lAddress?.town as string }
                    onChange={ (e: any) => props.updateAddress('local', 'town', e.target.value) } />
                <label htmlFor='l-address-town'>Town</label>
            </div>
            <div className='input-field col m6 s12'>
                <i className='fas fa-place-of-worship prefix hidro-primary-icon'></i>
                <input id='l-address-district' type='text' value={ address.lAddress?.district as string }
                    onChange={ (e: any) => props.updateAddress('local', 'district', e.target.value) } />
                <label htmlFor='l-address-district'>District</label>
            </div>
            <div className='input-field col m6 s12'>
                <i className='fas fa-map-marked-alt prefix hidro-primary-icon'></i>
                <input id='l-address-city' type='text' value={ address.lAddress?.city as string }
                    onChange={ (e: any) => props.updateAddress('local', 'city', e.target.value) } />
                <label htmlFor='l-address-city'>City</label>
            </div>
            <div className='input-field col m6 s12'>
                <i className='fas fa-map-marked-alt prefix hidro-primary-icon'></i>
                <input id='l-address-province' type='text' value={ address.lAddress?.province as string }
                    onChange={ (e: any) => props.updateAddress('local', 'province', e.target.value) } />
                <label htmlFor='l-address-province'>Province</label>
            </div>
            <div className='input-field col m6 s12'>
                <select className='browser-default' id='s-address-country' value={ address.lAddress?.country?.id }
                    onChange={ (e: any) => props.updateAddress('local', 'country', e.target.value) }>
                    {
                        countries.map((country: ICountry) => 
                            <option key={ country.id } value={ country.id }>{ country.combinedName }</option>
                        )
                    }
                </select>
            </div>
            <div className='input-field col m6 s12'>
                <i className='fas fa-shoe-prints prefix hidro-primary-icon'></i>
                <input id='l-address-alt' type='text' value={ address.lAddress?.alternateAddress as string }
                    onChange={ (e: any) => props.updateAddress('local', 'alternate', e.target.value) } />
                <label htmlFor='l-address-alt'>Alternate Address</label>
            </div>
        </div>
    );
}

export default LocalAddressForm;