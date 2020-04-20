import { CONSTANTS } from "../../../../../helpers/helper";

export const GET_DROPDOWN_COUNTRIES_BEGIN = 'GET_DROPDOWN_COUNTRIES_BEGIN';
export type T_GET_DROPDOWN_COUNTRIES_BEGIN = typeof GET_DROPDOWN_COUNTRIES_BEGIN;

export const GET_DROPDOWN_COUNTRIES_SUCCESS = 'GET_DROPDOWN_COUNTRIES_SUCCESS';
export type T_GET_DROPDOWN_COUNTRIES_SUCCESS = typeof GET_DROPDOWN_COUNTRIES_SUCCESS;

export const GET_DROPDOWN_COUNTRIES_FAILED = 'GET_DROPDOWN_COUNTRIES_FAILED';
export type T_GET_DROPDOWN_COUNTRIES_FAILED = typeof GET_DROPDOWN_COUNTRIES_FAILED;

export const GET_ADDRESS_LIST_BEGIN = 'GET_ADDRESS_LIST_BEGIN';
export type T_GET_ADDRESS_LIST_BEGIN = typeof GET_ADDRESS_LIST_BEGIN;

export const GET_ADDRESS_LIST_SUCCESS = 'GET_ADDRESS_LIST_SUCCESS';
export type T_GET_ADDRESS_LIST_SUCCESS = typeof GET_ADDRESS_LIST_SUCCESS;

export const GET_ADDRESS_LIST_FAILED = 'GET_ADDRESS_LIST_FAILED';
export type T_GET_ADDRESS_LIST_FAILED = typeof GET_ADDRESS_LIST_FAILED;

export const SAVE_NEW_ADDRESS_BEGIN = 'SAVE_NEW_ADDRESS_BEGIN';
export type T_SAVE_NEW_ADDRESS_BEGIN = typeof SAVE_NEW_ADDRESS_BEGIN;

export const SAVE_NEW_ADDRESS_SUCCESS = 'SAVE_NEW_ADDRESS_SUCCESS';
export type T_SAVE_NEW_ADDRESS_SUCCESS = typeof SAVE_NEW_ADDRESS_SUCCESS;

export const SAVE_NEW_ADDRESS_FAILED = 'SAVE_NEW_ADDRESS_FAILED';
export type T_SAVE_NEW_ADDRESS_FAILED = typeof SAVE_NEW_ADDRESS_FAILED;

export const UPDATE_ADDRESS_BEGIN = 'UPDATE_ADDRESS_BEGIN';
export type T_UPDATE_ADDRESS_BEGIN = typeof UPDATE_ADDRESS_BEGIN;

export const UPDATE_ADDRESS_SUCCESS = 'UPDATE_ADDRESS_SUCCESS';
export type T_UPDATE_ADDRESS_SUCCESS = typeof UPDATE_ADDRESS_SUCCESS;

export const UPDATE_ADDRESS_FAILED = 'UPDATE_ADDRESS_FAILED';
export type T_UPDATE_ADDRESS_FAILED = typeof UPDATE_ADDRESS_FAILED;

export const SET_ADDRESS_FIELD_BEGIN = 'SET_ADDRESS_FIELD_BEGIN';
export type T_SET_ADDRESS_FIELD_BEGIN = typeof SET_ADDRESS_FIELD_BEGIN;

export const SET_ADDRESS_FIELD_SUCCESS = 'SET_ADDRESS_FIELD_SUCCESS';
export type T_SET_ADDRESS_FIELD_SUCCESS = typeof SET_ADDRESS_FIELD_SUCCESS;

export const SET_ADDRESS_FIELD_FAILED = 'SET_ADDRESS_FIELD_FAILED';
export type T_SET_ADDRESS_FIELD_FAILED = typeof SET_ADDRESS_FIELD_FAILED;

export const DELETE_ADDRESS_BEGIN = 'DELETE_ADDRESS_BEGIN';
export type T_DELETE_ADDRESS_BEGIN = typeof DELETE_ADDRESS_BEGIN;

export const DELETE_ADDRESS_SUCCESS = 'DELETE_ADDRESS_SUCCESS';
export type T_DELETE_ADDRESS_SUCCESS = typeof DELETE_ADDRESS_SUCCESS;

export const DELETE_ADDRESS_FAILED = 'DELETE_ADDRESS_FAILED';
export type T_DELETE_ADDRESS_FAILED = typeof DELETE_ADDRESS_FAILED;

export interface IAddressBinder {
    hidrogenianId : number,
    localAddress : object | null,
    standardAddress : object | null
}

export interface IAddressList {
    user : any,
    addresses : any,
    countries : any,
    saveAddressFor : any,
    saveAddress : any,
    onSaveSuccess : any,
    onUpdateSuccess : any,
    onSetFieldOrDeleteSuccess : any,
    updateAddressFor : any,
    deleteAddress : any,
    updateAddressField : any,
    updating : any,
    setField : any,
    deleting : any
};

export interface IAddressForm {
    address : IAddress,
    countries : any
    updateAddress : any,
    detectAddress : any,
    currentTab : any,
    setCurrentTab : any,
    saveAddress : any,
    closeModal : any,
    actionError : string,
    isUpdating : Boolean
}

export interface IAddressMap {
    addresses : any
}

export interface ICountry {
    id : number,
    name : string,
    code : string,
    combinedName : string,
    continent : string,
    currenry : string
}

interface IGenericLocation {
    id : number,
    poBox : string | null,
    buildingName : string | null,
    streetAddress : string,
    alternateAddress : string | null,
    country : ICountry
}

export interface IStandardLocation extends IGenericLocation {
    suburb : string,
    postcode : string,
    state : string
}

export interface ILocalLocation extends IGenericLocation {
    group : string | null,
    lane : string | null,
    quarter : string | null,
    hamlet : string | null,
    commute : string | null,
    ward : string | null,
    district : string | null,
    town : string | null,
    province : string | null,
    city : string | null
}

export interface IAddress {
    id : number,
    title : string,
    isPrimary : Boolean,
    forDelivery : Boolean,
    isStandard : Boolean,
    isRefined : Boolean,
    lastUpdate : string,
    lAddress : ILocalLocation | null,
    sAddress : IStandardLocation | null,
    normalizedAddress : string
}

export interface IFieldSetter {
    id : number,
    hidrogenianId : number,
    field : string
}

export interface IPOBox {
    id : number,
    title : string
}

const VOID_COUNTRY = {
    id : 0,
    name : CONSTANTS.EMPTY,
    code : CONSTANTS.EMPTY,
    combinedName : CONSTANTS.EMPTY,
    continent : CONSTANTS.EMPTY,
    currenry : CONSTANTS.EMPTY
};

export const VOID_SLOCATION = {
    id : 0,
    poBox : CONSTANTS.EMPTY,
    buildingName : CONSTANTS.EMPTY,
    streetAddress : CONSTANTS.EMPTY,
    alternateAddress : CONSTANTS.EMPTY,
    country : VOID_COUNTRY,
    suburb : CONSTANTS.EMPTY,
    postcode : CONSTANTS.EMPTY,
    state : CONSTANTS.EMPTY
};

export const VOID_LLOCATION = {
    id : 0,
    poBox : CONSTANTS.EMPTY,
    buildingName : CONSTANTS.EMPTY,
    streetAddress : CONSTANTS.EMPTY,
    alternateAddress : CONSTANTS.EMPTY,
    country : VOID_COUNTRY,
    group : CONSTANTS.EMPTY,
    lane : CONSTANTS.EMPTY,
    quarter : CONSTANTS.EMPTY,
    hamlet : CONSTANTS.EMPTY,
    commute : CONSTANTS.EMPTY,
    ward : CONSTANTS.EMPTY,
    district : CONSTANTS.EMPTY,
    town : CONSTANTS.EMPTY,
    province : CONSTANTS.EMPTY,
    city : CONSTANTS.EMPTY
};

export const VOID_ADDRESS = {
    id : 0,
    title : CONSTANTS.EMPTY,
    isPrimary : false,
    forDelivery : false,
    isStandard : false,
    isRefined : false,
    lastUpdate : CONSTANTS.EMPTY,
    lAddress : VOID_LLOCATION,
    sAddress : VOID_SLOCATION,
    normalizedAddress : CONSTANTS.EMPTY
};