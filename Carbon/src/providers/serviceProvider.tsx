import axios from 'axios';
axios.defaults.withCredentials = true; //include all cookies

const LOCAL_ENDPOINT = 'https://localhost:5001/';

export const sendRequestForResult = (action: string, auth: any, data: any, method = 'POST') => {
    if (auth == null)
        auth = sessionStorage.getItem('authToken');
        
    const requestOptions = {
        timeout : 10000, // 10 seconds
        method : method,
        url : LOCAL_ENDPOINT + action,
        headers : {
            'Authorization': auth == null ? undefined
                : 'Bearer ' + (typeof (auth) === 'string' ? auth : auth.authToken),
            'Content-Type': 'application/json',
            'Accept': 'application/json',
        },
        data : method === 'POST' ? JSON.stringify(data) : null
    };

    const response = axios(requestOptions).then((result: any) => {
        if (result.status !== 200)
            return result.json().then((error: any) => { throw error; })
        else
            return result.data;
    });
    
    return response;
};

export const sendWaterRequest = (action: string, formData: any, method = 'POST') => {
    const requestOptions = {
        timeout : 30000, // 30 seconds
        method : method,
        url : LOCAL_ENDPOINT + action,
        headers : {
            'Authorization': 'Bearer ' + sessionStorage.getItem('authToken'),
            'Content-Type': 'multipart/form-data',
            'Accept': 'application/json',
        },
        data : formData
    };

    const response = axios(requestOptions).then((result: any) => {
        if (result.status !== 200)
            return result.json().then((error: any) => { throw error; })
        else
            return result.data;
    });
    
    return response;
}