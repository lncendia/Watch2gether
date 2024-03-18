interface FilmViewerData {
    season?: number;
    series?: number;
    id: string;
    username: string;
    photoUrl?: string;
    pause: boolean;
    fullScreen: boolean;
    online: boolean;
    second: number;
    typing: boolean;
    allows: Allows;
}

interface Allows {
    beep: boolean;
    scream: boolean;
    change: boolean;
}