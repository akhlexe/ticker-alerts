export enum AlertState {
    PENDING = 0,
    TRIGGERED = 1,
    NOTIFIED = 2,
    RECEIVED = 3,
    CANCELED = 4
}

export const AlertStateConfig = {
    [AlertState.PENDING]: { label: 'Pending', cssClass: 'alert-pending' },
    [AlertState.TRIGGERED]: { label: 'Triggered', cssClass: 'alert-triggered' },
    [AlertState.NOTIFIED]: { label: 'Notified', cssClass: 'alert-notified' },
    [AlertState.RECEIVED]: { label: 'Received', cssClass: 'alert-received' },
    [AlertState.CANCELED]: { label: 'Canceled', cssClass: 'alert-canceled' }
};