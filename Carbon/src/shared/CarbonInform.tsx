import React from 'react';

interface IInform {
    message?: string
}

const CarbonInform = ({ message } : IInform) => {
    return (
        <div className='card-panel inform'>
            <h5><i className='fas fa-grin-beam-sweat fa-2x'></i>&nbsp;&nbsp;Something seems to be wrong...</h5>
            {
                (
                    message === undefined &&
                    <>
                        <p>An error has happenned on the page. No worries. We will look into this very soon.</p>
                        <p>We are truely sorry about this inconvenience. We will greatly appreciate if you could spare sometime to <a href='/'>report this problem</a> to us so we can make Hidrogen better for everyone.</p>
                    </>
                ) ||
                <p>{ message }</p>
            }
            <p>Please press F5 to reload Hidrogen and or <a href='/'>Click Here</a> to continue with your session.</p>
        </div>
    );
}

export default CarbonInform;