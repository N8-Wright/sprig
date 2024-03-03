﻿namespace Sprig.Models;

using MessagePack;

[MessagePackObject]
public class Response : Message
{
    public Response()
    {
        ResponseCode = Code.Unknown;
    }

    public Response(Code code)
    {
        ResponseCode = code;
    }

    public enum Code
    {
        Unknown,
        Ok,
        GenericError,
        InvalidRequest,
    }

    [Key("ResponseCode")]
    public Code ResponseCode { get; set; }
}
