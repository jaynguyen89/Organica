import moment from 'moment-timezone';
import _ from 'lodash';
import md5 from 'md5';

export const CONSTANTS = {
    EMPTY : '',
    SPACE : ' ',
    NA : 'N/A',
    NIL : null,
    CREATE : 'create',
    UPDATE : 'update',
    DELETE : 'delete',
    FAILED : 0,
    SUCCESS : 1,
    INTERRUPTED : 2
};

export function sessionWatcher() {
    if (sessionStorage.getItem('expirationTime') &&
        Number(sessionStorage.getItem('expirationTime')) < moment().unix()
    ) {
        localStorage.clear();
        sessionStorage.clear();
        alert('Your session has expired. Please login again.');
        window.location.href = '/';

        return false;
    }

    return true;
}

// Pass in the URL and the query params in the exact same order as in the URL
export function extractUrlParameters(url: string, params: string[]) {
    let urlQuery = url.split('?' + params[0] + '=')[1];

    let data: any = {};
    params.map((element: string, i: number) => {
        var subQuery: any;
        if (i + 1 !== params.length) {
            subQuery = urlQuery.split('&' + params[i + 1] + '=');
            data[element] = subQuery[0];
        
            urlQuery = subQuery[1];
        }
        else
            data[element] = urlQuery;
    }, urlQuery);

    return data;
}

export function getGravatarURL(email: string, size:number = 130) {
    let defaults  = ['mp', 'identicon', 'monsterid', 'wavatar', 'retro', 'robohash'];

    if (/^\w+([.-]?\w+)*@\w+([.-]?\w+)*(.\w{2,3})+$/.test(email.toLowerCase()))
      return `https://www.gravatar.com/avatar/${md5(email.toLowerCase())}?s=${size}&d=${_.sample(defaults)}`;
    else
      return 'https://encrypted-tbn0.gstatic.com/images?q=tbn%3AANd9GcSXVwEuYLdkIYNy3YoI6AvPvc3K_7ELDrG2SSjxNmu9ktRi6E38';
}