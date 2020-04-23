using System;

namespace Example.Models
{
    public class Person
    {
        public string Name { get; set; }
        public int Age { get; set; }
        public Address Address { get; set; }

        public MaterialStatus MarriedStatus { get; set; }

        public string Employeed { get; set; }

        public bool CanDrive { get; set; }
    }

    public enum MaterialStatus
    {
        Married, UnMarried
    }

    public class Address
    {
        public string Street { get; set; }
        public string Postcode { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
    }
}
