import { encode, decode } from "@msgpack/msgpack";

export enum Kind {
    Unknown,
    BeginSessionRequest,
    Response,
}

export class MessageBase {
    constructor(public ProtocolVersion: number, public MessageKind: Kind) {
    }
}

export class BeginSessionRequest {
    constructor(public SessionId: string) {
        this.Message = new MessageBase(1, Kind.BeginSessionRequest);
    }

    public Message: MessageBase;
}

export class Client {
    constructor() {
        console.log("Intialized clinet");
        this.ws = new WebSocket("ws://localhost:9876");
        console.log(this.ws)
        this.ws.onopen = () => {
            console.log("Opened connection")
            let message = new BeginSessionRequest("Typescript Hello Session ID");
            let bytes = encode(message);
            console.log(bytes);
            this.ws.send(bytes);
        }
    }

    ws: WebSocket

    Send(message: MessageBase) {
        //let bytes = encode(message);
        // console.log(bytes);
        // this.ws.send(bytes);
    }
}