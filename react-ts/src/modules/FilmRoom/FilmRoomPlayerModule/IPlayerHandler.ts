export interface IPlayerHandler {
    handler(event: MessageEvent<any>): Promise<void>
}