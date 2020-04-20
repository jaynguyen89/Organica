import React from 'react';
import _ from 'lodash';
import $ from 'jquery';

import GoogleMapReact from 'google-map-react';
import { IAddressMap, IAddress } from '../redux/address/constants';
import CarbonPreloader from '../../../../shared/CarbonPreloader';

interface ICoordination {
    lat : number,
    lng : number,
    title : string
}

const AddressMap = (props: IAddressMap) => {
    const [coordinations, setCoordinations] = React.useState([] as any);
    const [center, setCenter] = React.useState({} as ICoordination);

    React.useEffect(() => {
        if (!_.isEmpty(props.addresses)) {
            props.addresses.forEach((address: IAddress) => {
                let addressUrl = address.normalizedAddress.split(" ").join("+");;
                let requestURL = "https://maps.googleapis.com/maps/api/geocode/json?address=" + addressUrl + "&key=AIzaSyCHwksVRlz52XzP6BNf_Js8EdFta3xCgSs";

                let coords: ICoordination[] = [];
                let latSum = 0;
                let lngSum = 0;
                $.getJSON(requestURL, function (data) {
                    if (data['status'] === 'OK') {
                        let latitude = +(data["results"][0]["geometry"]["location"]["lat"]);
                        let longitude = +(data["results"][0]["geometry"]["location"]["lng"]);

                        latSum += latitude;
                        lngSum += longitude;

                        coords.push({ lat : latitude, lng : longitude, title : address.title } as ICoordination);
                    }
                });

                setCoordinations(coords);
                // setCenter({ lat : latSum/props.addresses.length, lng : lngSum/props.addresses.length } as ICoordination);
                setCenter({ lat : -37.81, lng : 144.9645 } as ICoordination);
            });
        }
    }, [props.addresses]);

    return (
        (
            !_.isEmpty(props.addresses) &&
            <div className='address-map'>
                <GoogleMapReact bootstrapURLKeys={{ key: 'AIzaSyCHwksVRlz52XzP6BNf_Js8EdFta3xCgSs' }}
                    defaultCenter={ center } defaultZoom={ 12 }>
                    {
                        coordinations.map((coord: ICoordination, i: number) =>
                            <Marker key={ i } lat={ coord.lat } lng={ coord.lng } text={ coord.title } />
                        )
                    }
                </GoogleMapReact>
            </div>
        ) || <CarbonPreloader />
    );
}

export default AddressMap;

interface IMarker {
    lat: number,
    lng: number,
    text: string
};

const Marker = ({ lat, lng, text }: IMarker) => <div>{text}</div>;