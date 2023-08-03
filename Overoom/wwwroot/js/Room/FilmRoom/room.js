class FilmUser extends User {
    constructor(id, name, avatar, second, pause, fullScreen, allowBeep, allowScream, allowChange, season, series) {
        super(id, name, avatar, second, pause, fullScreen, allowBeep, allowScream, allowChange);
        this.Season = season;
        this.Series = series;
    }
}

class FilmRoom extends Room {
    constructor(currentId, ownerId, id, open, url, type) {
        super(currentId, ownerId, id, open)
        this.Url = url;
        this.Type = type;
    }
}


class ConnectFilmReceiveEvent extends ConnectReceiveEvent {
    constructor(id, username, avatar, time, beep, scream, change, season, series) {
        super(id, username, avatar, time, beep, scream, change);
        this.Season = season
        this.Series = series
    }
}


class ChangeSeriesReceiveEvent extends ReceiveEvent {
    constructor(id, season, series) {
        super('ChangeSeries', id)
        this.Season = season
        this.Series = series
    }
}


class ChangeSeriesUserEvent extends UserEvent {
    constructor(season, series) {
        super('ChangeSeries')
        this.Season = season
        this.Series = series
    }
}