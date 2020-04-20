import React from 'react';
import { makeStyles } from '@material-ui/core/styles';
import { Backdrop, Tooltip, Zoom } from '@material-ui/core';
import SpeedDial from '@material-ui/lab/SpeedDial';
import SpeedDialAction from '@material-ui/lab/SpeedDialAction';
import { CONSTANTS } from '../helpers/helper';

// Usage: pass data to this component in the following way:
//
// <CarbonFab
//     fab={ ---------------------------------------------> required
//         tooltip, icon: string
//         handleFabClicked={ your delegate } ------------> required if `actions` is not provided, otherwise, must be ommitted
//     }
//     actions={------------------------------------------> must provide if `handleFabClicked` is omitted, otherwise, must not provide
//         [ ---------------------------------------------> notice, array of JSON objects
//         {
//             name, icon : string -----------------------> requried
//             handleActionClicked={ your delegate } -----> required
//         }, ...
//         ]
//     }
// />

const useStyles = makeStyles(theme => ({
    root: {
        position: 'absolute',
        bottom: '3%',
        right: '3%',
        transform: 'translateZ(0px)',
        flexGrow: 1,
    },
    speedDial: {
        position: 'absolute',
        bottom: theme.spacing(0),
        right: theme.spacing(0),
    }
}));

const CarbonFab = (props: any) => {
    const classes = useStyles();
    const [open, setOpen] = React.useState(false);
    const [hidden] = React.useState(false);

    return (
        <div className={ classes.root }>
            <Backdrop open={ open } />
            <Tooltip TransitionComponent={ Zoom }
                     title={ props.actions ? CONSTANTS.EMPTY : ((props.fab && props.fab.tooltip) || CONSTANTS.EMPTY) }
                     placement="top">

                <SpeedDial
                    id="carbon-fab"
                    ariaLabel="Actions"
                    direction='left'
                    className={ classes.speedDial }
                    hidden={ hidden }
                    icon={ <i className={ (props.fab && props.fab.icon) || 'fas fa-pencil' } /> }
                    onClose={ () => setOpen(false) }
                    onOpen={ () => setOpen(true) }
                    open={ open }
                    onClick={ () => { if (!props.actions) props.fab.handleFabClicked(); }}>
                {
                    props.actions && props.actions.map((a: any) =>
                        <SpeedDialAction
                            onClick={ () => { a.handleActionClicked(); setOpen(false); }}
                            tooltipTitle={ a.name }
                            tooltipPlacement='top'
                            key={ a.name }
                            icon={ <i className={ a.icon || 'fas fa-question' } /> }
                        />
                    )
                }
                </SpeedDial>
            </Tooltip>
        </div>
    );
}

export default CarbonFab;