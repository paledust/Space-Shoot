using System.Collections;
using System.Collections.Generic;

public class baseEvent {
    // If you're using C# you can use delegates as the handlers
    // It seems natural to me to declare the handler type in
    // the event base class, but your tastes may vary
    public delegate void Handler(baseEvent e);
}
