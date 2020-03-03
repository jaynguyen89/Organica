import React from 'react';
import { Route, Redirect } from 'react-router-dom';
import { sessionWatcher } from '../helpers/helper';

interface IProps {
    component : any
}

const SessionRoute = ({ component : Component, ...rest } : IProps) => (
    <Route { ...rest } render={props => (
        sessionWatcher()
            ? <Component { ...props } />
            : <Redirect to={{ pathname: '/', state: { from: props.location } }} />
    )} />
)

export default SessionRoute;