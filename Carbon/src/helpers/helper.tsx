import moment from 'moment-timezone';

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