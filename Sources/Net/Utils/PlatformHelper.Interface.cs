using System;

namespace Advexp
{
    interface IPlatformHelper : ILogger
    {
        object ToUnderlyingObject(object obj, Type preferedDestinationType);

    }
}