import { IndividualConfig } from "ngx-toastr";

export const ToastrConfigs = {
    defaultConfig: {
        positionClass: 'toast-bottom-left',
        timeOut: 3000,
        closeButton: true,
        progressBar: true,
    } as Partial<IndividualConfig>,

    importantConfig: {
        positionClass: 'toast-top-center',
        timeOut: 5000,
        closeButton: true,
        progressBar: true,
        tapToDismiss: false,
        disableTimeOut: true
    } as Partial<IndividualConfig>,
};