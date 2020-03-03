import React from 'react';
import $ from 'jquery';
import M from 'materialize-css';

import SearchArea from './search-area/SearchArea';
import Presenter from './presenter/Presenter';
import ListingShowcase from './listings-showcase/ListingShowcase';
import SellerShowcase from './sellers-showcase/SellerShowcase';
import SellingEase from './movitator/selling-ease/SellingEase';
import TrendingItem from './movitator/trending-items/TrendingItem';
import CarbonSiteMap from './CarbonSiteMap';

const CarbonHome = () => {
    React.useEffect(() => {
        M.Sidenav.init($('.sidenav'), {});
        M.FormSelect.init($('#search-categories'), {});
    }, []);

    return (
        <>
            <SearchArea />
            <Presenter />
            <ListingShowcase />
            <SellerShowcase />
            <SellingEase />
            <TrendingItem />
            <CarbonSiteMap />
        </>
    );
}

export default CarbonHome;