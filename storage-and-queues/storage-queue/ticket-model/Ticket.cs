using System;

namespace ticket_model
{
    public class Ticket
    {
        public string Guid {get; set;} = System.Guid.NewGuid().ToString();
        public string Details {get; set;}
        public int Price {get; set;}
    }
}
