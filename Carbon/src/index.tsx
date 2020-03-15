import React from 'react';
import ReactDOM from 'react-dom';
import App from './foundation/App';
import { Provider } from 'react-redux';
import { store } from './foundation/store';
import * as serviceWorker from './serviceWorker';

import { loadAuthenticatedUser } from './authentication/redux/actions';
store.dispatch(loadAuthenticatedUser());

ReactDOM.render(
    <Provider store={ store }>
        <App />
    </Provider>,
    document.getElementById('root')
);

// Change unregister() to register() to allow app working offline (with some pitfalls)
serviceWorker.unregister();