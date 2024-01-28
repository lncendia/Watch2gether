class User {
    constructor(id, name, avatar, second, pause, fullScreen, allowBeep, allowScream, allowChange) {
        this.Name = name
        this.Pause = pause
        this.FullScreen = fullScreen
        this.AllowBeep = allowBeep
        this.AllowScream = allowScream
        this.AllowChange = allowChange
        this.Second = second
        this.Id = id
        this.Avatar = avatar
    }
}


class Room {
    constructor(currentId, ownerId, id, open) {
        this.Id = id
        this.CurrentId = currentId
        this.OwnerId = ownerId
        this.Open = open
        this.Users = []
        this.ReceiveEvents = []
        this.UserEvents = []
    }

    RegisterUserEvent(name, callback) {
        this.UserEvents.push({Name: name, Process: callback})
    }

    RegisterReceiveEvent(name, callback) {
        this.ReceiveEvents.push({Name: name, Process: callback})
    }

    ProcessUserEvent(event) {
        console.log('process user: ' + event.Name)
        for (let i = 0; i < this.UserEvents.length; i++) {
            if (event.Name === this.UserEvents[i].Name) this.UserEvents[i].Process(this, event)
        }
    }

    ProcessReceiveEvent(event) {
        console.log('process receive: ' + event.Name)
        for (let i = 0; i < this.ReceiveEvents.length; i++) {
            if (event.Name === this.ReceiveEvents[i].Name) this.ReceiveEvents[i].Process(this, event)
        }
    }
}


class UserEvent {
    constructor(name) {
        this.Name = name;
    }
}

class SyncUserEvent extends UserEvent {
    constructor() {
        super('Sync')
    }
}

class LoadUserEvent extends UserEvent {
    constructor() {
        super('Load')
    }
}

class SeekUserEvent extends UserEvent {
    constructor(second) {
        super('Seek')
        this.Second = second
    }
}

class PauseUserEvent extends UserEvent {
    constructor(pause, second) {
        super('Pause')
        this.Second = second
        this.Pause = pause
    }
}

class FullScreenUserEvent extends UserEvent {
    constructor(fullScreen) {
        super('FullScreen')
        this.FullScreen = fullScreen
    }
}


class MessageUserEvent extends UserEvent {
    constructor(text) {
        super('Send')
        this.Text = text
    }
}

class TypeUserEvent extends UserEvent {
    constructor() {
        super('Type')
    }
}

class BeepUserEvent extends UserEvent {
    constructor(target) {
        super('Beep')
        this.Target = target
    }
}

class KickUserEvent extends UserEvent {
    constructor(target) {
        super('Kick')
        this.Target = target
    }
}


class ScreamUserEvent extends UserEvent {
    constructor(target) {
        super('Scream')
        this.Target = target
    }
}

class ChangeUserEvent extends UserEvent {
    constructor(target, name) {
        super('Change')
        this.Target = target
        this.Username = name
    }
}


class ReceiveEvent {
    constructor(name, id) {
        this.Name = name;
        this.Id = id;
    }
}

class SeekReceiveEvent extends ReceiveEvent {
    constructor(id, second) {
        super('Seek', id)
        this.Second = second
    }
}

class PauseReceiveEvent extends ReceiveEvent {
    constructor(id, pause, second) {
        super('Pause', id)
        this.Second = second
        this.Pause = pause
    }
}

class FullScreenReceiveEvent extends ReceiveEvent {
    constructor(id, fullScreen) {
        super('FullScreen', id)
        this.FullScreen = fullScreen
    }
}

class LeaveReceiveEvent extends ReceiveEvent {
    constructor(id) {
        super('Leave', id)
    }
}

class ConnectReceiveEvent extends ReceiveEvent {
    constructor(id, username, avatar, time, beep, scream, change) {
        super('Connect', id)
        this.Username = username
        this.Avatar = avatar
        this.Time = time
        this.Beep = beep
        this.Scream = scream
        this.Change = change
    }
}

class MessageReceiveEvent extends ReceiveEvent {
    constructor(id, text) {
        super('Message', id)
        this.Text = text
    }
}

class TypeReceiveEvent extends ReceiveEvent {
    constructor(id) {
        super('Type', id)
    }
}

class BeepReceiveEvent extends ReceiveEvent {
    constructor(id, target) {
        super('Beep', id)
        this.Target = target
    }
}

class ScreamReceiveEvent extends ReceiveEvent {
    constructor(id, target) {
        super('Scream', id)
        this.Target = target
    }
}

class KickReceiveEvent extends ReceiveEvent {
    constructor(id, target) {
        super('Kick', id)
        this.Target = target
    }
}

class ChangeReceiveEvent extends ReceiveEvent {
    constructor(id, target, name) {
        super('Change', id)
        this.Target = target
        this.Username = name
    }
}


class ErrorReceiveEvent extends ReceiveEvent {
    constructor(id, text) {
        super('Error', id)
        this.Text = text
    }
}