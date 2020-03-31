import axios from 'axios';

axios.defaults.timeout = 10000; //10 seconds
axios.defaults.withCredentials = true; //include all cookies

const LOCAL_ENDPOINT = 'https://localhost:5001/';

const sendRequestForResult = (action: string, auth: any, data: any, method = 'POST') => {
    if (auth == null)
        auth = sessionStorage.getItem('authToken');

    const requestOptions = {
        method : method,
        url : LOCAL_ENDPOINT + action,
        headers : {
            'Authorization': auth == null ? undefined
                : 'Bearer ' + (typeof (auth) === 'string' ? auth : auth.authToken),
            'Content-Type': 'application/json',
            'Accept': 'application/json',
        },
        body : (method !== 'GET' && data != null) ? JSON.stringify(data) : null
    };

    const response = axios(requestOptions).then((result: any) => {
        if (result.status !== 200)
            return result.json().then((error: any) => { throw error; })
        else
            return result.data;
    });
    
    return response;
};

export default sendRequestForResult;