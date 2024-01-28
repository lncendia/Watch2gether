class YoutubeUser extends User {
    constructor(id, name, avatar, second, pause, fullScreen, allowBeep, allowScream, allowChange, videoId) {
        super(id, name, avatar, second, pause, fullScreen, allowBeep, allowScream, allowChange);
        this.VideoId = videoId
    }
}

class YoutubeRoom extends Room {
    constructor(currentId, ownerId, id, open, ids) {
        super(currentId, ownerId, id, open)
        this.Ids = ids;
    }
}

class ConnectYoutubeReceiveEvent extends ConnectReceiveEvent {
    constructor(id, username, avatar, time, beep, scream, change, videoId) {
        super(id, username, avatar, time, beep, scream, change);
        this.VideoId = videoId;
    }
}

class ChangeVideoReceiveEvent extends ReceiveEvent {
    constructor(id, videoId) {
        super('ChangeVideo', id)
        this.VideoId = videoId
    }
}

class AddVideoReceiveEvent extends ReceiveEvent {
    constructor(id, videoId) {
        super('AddVideo', id)
        this.VideoId = videoId
    }
}



class ChangeVideoUserEvent extends UserEvent {
    constructor(id) {
        super('ChangeVideo')
        this.Id = id
    }
}

class AddVideoUserEvent extends UserEvent {
    constructor(url) {
        super('AddVideo')
        this.Url = url
    }
}