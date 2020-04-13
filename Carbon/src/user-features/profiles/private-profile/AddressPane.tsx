import React from 'react';
import M from 'materialize-css';
import $ from 'jquery';

import GoogleMapReact from 'google-map-react';
import { ModalOptions } from '../../../helpers/helper';

const AddressPane = () => {

    React.useEffect(() => {
        M.Modal.init($('.modal'), ModalOptions);
    }, []);

    return (
        <div className='row'>
            <h6 className='content-caption'>
                <i className='fas fa-user-circle hidro-primary-icon'></i>&nbsp;&nbsp;Addresses
            </h6>
            <div className='col l3 m6 s12'>
                <div className='address-list'>
                    <div className='add-address-row modal-trigger' data-target='address-form'>
                        <i className='fas fa-plus-circle'></i>&nbsp;New address
                    </div>
                    <div className='address-row'>
                        <div className='ribbon ribbon-primary'>Primary</div>
                        <h6>Address Title</h6>
                        <p>Ap.5, Bldg 12A, Block 1B, 111 Somewhere Street, Somewhere Place, VIC 3020</p>
                        <a className='address-edit'><i className='fas fa-edit'></i></a>
                        <a className='address-delete'><i className='fas fa-trash red-text darken-4'></i></a>
                    </div>
                </div>
            </div>
            <div className='col l9 m6 s0'>
                <div className='address-map'>
                    <GoogleMapReact
                        bootstrapURLKeys={{ key: 'AIzaSyCHwksVRlz52XzP6BNf_Js8EdFta3xCgSs' }}
                        defaultCenter={{
                            lat: 59.95,
                            lng: 30.33
                        }}
                        defaultZoom={ 10 }>
                        <Marker
                            lat={59.955413}
                            lng={30.337844}
                            text='My Marker'/>
                    </GoogleMapReact>
                </div>
            </div>

            <div id='address-form' className='modal'>
                <div className='modal-content'>
                    <h4>Add Address</h4>
                    <p>This is a modal.</p>
                </div>
            </div>
        </div>
    );
}

export default AddressPane;

interface IMarker {
    lat: number,
    lng: number,
    text: string
};

const Marker = ({ lat,lng,text }: IMarker) => <div>{text}</div>;