import { Country } from "./country.model";
import { States } from "./state.model";

export interface Contacts{
    contactId:number,
    countryId:number,
    stateId:number,
    firstName:string,
    lastName:string,
    contactNumber:string,
    email:string,
    contactDescription:string,
    profilePic:string,
    gender:string,
    address:string,
    favourite:boolean,
    state:States,
    country:Country,
    imageByte:string,

}