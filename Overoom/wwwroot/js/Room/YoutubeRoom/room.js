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

class RemoveVideoReceiveEvent extends ReceiveEvent {
    constructor(id, videoId) {
        super('RemoveVideo', id)
        this.VideoId = videoId
    }
}

class AddAccessReceiveEvent extends ReceiveEvent {
    constructor(id, add) {
        super('AddAccess', id)
        this.AddAccess = add
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

class RemoveVideoUserEvent extends UserEvent {
    constructor(id) {
        super('RemoveVideo')
        this.Id = id
    }
}

class AddAccessUserEvent extends UserEvent {
    constructor(add) {
        super('AddAccess')
        this.AddAccess = add
    }
}