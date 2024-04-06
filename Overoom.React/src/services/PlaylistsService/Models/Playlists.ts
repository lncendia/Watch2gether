export interface Playlists {
    playlists: Playlist[];
    countPages: number;
}

export interface Playlist {
    id: string
    name: string
    posterUrl: string
    description: string
    genres: string[]
    updated: Date
}