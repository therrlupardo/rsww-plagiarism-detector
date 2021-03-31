using System;

namespace Commands
{
    public abstract record BaseCommand(Guid Id, Guid UserId)
    {
    }
}