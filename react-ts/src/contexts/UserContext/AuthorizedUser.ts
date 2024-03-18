export class AuthorizedUser {

    get name(): string {
        return this._name;
    }

    get avatarUrl(): string {
        return this._avatarUrl;
    }

    get roles(): string[] {
        return this._roles;
    }

    get email(): string {
        return this._email;
    }

    get locale(): string {
        return this._locale;
    }

    get id(): string {
        return this._id;
    }

    isInRole(role: string) {
        return this._roles.includes(role)
    }

    constructor(id: string, name: string, avatarUrl: string, roles: string[], email: string, locale: string) {
        this._id = id;
        this._name = name;
        this._avatarUrl = avatarUrl;
        this._roles = roles;
        this._email = email;
        this._locale = locale;
    }

    private readonly _id: string
    private readonly _name: string
    private readonly _avatarUrl: string
    private readonly _roles: string[]
    private readonly _email: string
    private readonly _locale: string
}