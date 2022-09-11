﻿namespace Watch2gether.Domain.Rooms.Exceptions;

public class MessageLengthException : Exception
{
    public MessageLengthException() : base("Message length must be between 1 and 1000 characters")
    {
    }
}