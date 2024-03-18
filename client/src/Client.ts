import { encode, decode } from "@msgpack/msgpack";

export enum Kind {
    Unknown,
    BeginSessionRequest,
    Response,
}

export class MessageBase {
    constructor(public MessageKind: Kind, public ProtocolVersion: number = 1) {
    }
}

export class BeginSessionRequest {
    constructor(public SessionId: string) {
        this.Message = new MessageBase(Kind.BeginSessionRequest);
    }

    public Message: MessageBase;
}

export class Client {
    constructor() {
    }

    ws?: WebSocket

    Connect(address: string) {
        return new Promise((resolve, reject) => {
            this.ws = new WebSocket(address);
            this.ws.onopen = () => resolve(this.ws);
            this.ws.onerror = reject;
        })
    }

    Send(message: any) {
        if (this.ws == null) {
            throw "Unable to send when websocket is not connected";
        }

        let bytes = encode(message);
        console.log(bytes);
        this.ws.send(bytes);
    }
}