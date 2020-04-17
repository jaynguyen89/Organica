import React from 'react';

import GoogleMapReact from 'google-map-react';

const AddressMap = () => {
    return (
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
    );
}

export default AddressMap;

interface IMarker {
    lat: number,
    lng: number,
    text: string
};

const Marker = ({ lat,lng,text }: IMarker) => <div>{text}</div>;