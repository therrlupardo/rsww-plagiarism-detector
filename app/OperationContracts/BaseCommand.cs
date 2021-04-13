using System;

namespace OperationContracts
{
    public abstract record BaseCommand(Guid Id, Guid UserId, DateTime IssuedOn) { }
}