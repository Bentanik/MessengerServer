﻿namespace MessengerServer.Src.Contracts.Abstractions;

public class Result<T>
{
    public int Error { get; set; }
    public string Message { get; set; }
    public T Data { get; set; }
}
