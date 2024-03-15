interface FilmRoom {
    title: string;
    cdnName: string;
    cdnUrl: string;
    isSerial: boolean;
    id: string;
    ownerId: string;
    viewers: FilmViewer[];
}

interface FilmViewer {
    season?: number;
    series?: number;
    id: string;
    username: string;
    photoUrl?: string;
    pause: boolean;
    fullScreen: boolean;
    online: boolean;
    second: number;
    allows: Allows;
}

interface Allows {
    beep: boolean;
    scream: boolean;
    change: boolean;
}