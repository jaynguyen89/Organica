import React from 'react';

import GoogleMapReact from 'google-map-react';

const AddressPane = () => {
    return (
        <div className='row'>
            <h6 className='content-caption'>
                <i className='fas fa-user-circle hidro-primary-icon'></i>&nbsp;&nbsp;Addresses
            </h6>
            <div className='col l3 m6 s12'>
                <div className='address-list'>
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
                        bootstrapURLKeys={{ key: 'AIzaSyDeataTmSWECGMeGzuh5-RbAadj_rMqbgI' }}
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