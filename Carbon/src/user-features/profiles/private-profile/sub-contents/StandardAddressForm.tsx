import React from 'react';
import M from 'materialize-css';

import LocationAutoDetect from './LocationDetector';
import { VOID_ADDRESS, IAddress, ICountry } from '../redux/address/constants';

const StandardAddressForm = (props: any) => {
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
                <input id='s-address-title' type='text' value={ address.title }
                    onChange={ (e: any) => props.updateAddress('standard', 'title', e.target.value) } />
                <label htmlFor='s-address-title'>Address Title</label>
            </div>
            <div className='input-field col l4 m6 s12'>
                <i className='fas fa-building prefix hidro-primary-icon'></i>
                <input id='s-address-building' type='text' value={ address.sAddress?.buildingName as string }
                    onChange={ (e: any) => props.updateAddress('standard', 'building', e.target.value) } />
                <label htmlFor='s-address-building'>Building Address</label>
            </div>
            <div className='input-field col l4 m6 s12'>
                <i className='fas fa-home prefix hidro-primary-icon'></i>
                <input id='s-address-street' type='text' value={ address.sAddress?.streetAddress }
                    onChange={ (e: any) => props.updateAddress('standard', 'street', e.target.value) } />
                <label htmlFor='s-address-street'>Street Address</label>
            </div>
            <div className='input-field col m6 s12'>
                <i className='fas fa-map-marked-alt prefix hidro-primary-icon'></i>
                <input id='s-address-suburb' type='text' value={ address.sAddress?.suburb }
                    onChange={ (e: any) => props.updateAddress('standard', 'suburb', e.target.value) } />
                <label htmlFor='s-address-suburb'>Suburb</label>
            </div>
            <div className='input-field col m6 s12'>
                <i className='fas fa-map-marked-alt prefix hidro-primary-icon'></i>
                <input id='s-address-state' type='text' value={ address.sAddress?.state }
                    onChange={ (e: any) => props.updateAddress('standard', 'state', e.target.value) } />
                <label htmlFor='s-address-state'>State</label>
            </div>
            <div className='input-field col m6 s12'>
                <i className='fas fa-qrcode prefix hidro-primary-icon'></i>
                <input id='s-address-postcode' type='text' value={ address.sAddress?.postcode }
                    onChange={ (e: any) => props.updateAddress('standard', 'postcode', e.target.value) } />
                <label htmlFor='s-address-postcode'>Postcode</label>
            </div>
            <div className='input-field col m6 s12'>
                <select className='browser-default' id='s-address-country' value={ address.sAddress?.country.id }
                    onChange={ (e: any) => props.updateAddress('standard', 'country', e.target.value) }>
                    {
                        countries.map((country: ICountry) => 
                            <option key={ country.id } value={ country.id }>{ country.combinedName }</option>
                        )
                    }
                </select>
            </div>
            <div className='input-field col l12 m6 s12'>
                <i className='fas fa-shoe-prints prefix hidro-primary-icon'></i>
                <input id='s-address-alt' type='text' value={ address.sAddress?.alternateAddress as string }
                    onChange={ (e: any) => props.updateAddress('standard', 'alternate', e.target.value) } />
                <label htmlFor='s-address-alt'>Alternate Address</label>
            </div>
        </div>
    );
}

export default StandardAddressForm;