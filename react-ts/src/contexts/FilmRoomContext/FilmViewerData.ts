interface FilmViewerData {
    id: string;
    username: string;
    photoUrl: string;
    beep: boolean;
    scream: boolean;
    change: boolean;
}

interface FilmViewerParams {
    id: string;
    season?: number;
    series?: number;
    pause: boolean;
    fullScreen: boolean;
    online: boolean;
    second: number;
    typing: boolean;
    typingTimeout?: NodeJS.Timeout
}