﻿using System;
using System.Collections.Generic;
using System.Text;

namespace MyMedicare
{
    public class User
    {
        private string username;
        private char[] password;
        private string firstName;
        private string lastName;
        private int age;
        private string address1;
        private string address2;
        private string phoneNumber;
        private string gpName;
        private float fontSize;
        private int fontColour;
        private int backgroundColour;

        /// <summary>Initializes a new instance of the User class.</summary>
        public User(string username, char[] password, string firstName, string lastName, int age, string address1, string address2, string phoneNumber, string gpName)
        {
            this.username = username;
            this.password = password;
            this.firstName = firstName;
            this.lastName = lastName;
            this.age = age;
            this.address1 = address1;
            this.address2 = address2;
            this.phoneNumber = phoneNumber;
            this.gpName = gpName;
        }

        public string Username
        {
            get { return username; }
            set { username = value; }
        }

        public char[] Password
        {
            get { return password; }
            set { password = value; }
        }

        public string FirstName
        {
            get { return firstName; }
            set { firstName = value; }
        }

        public string LastName
        {
            get { return lastName; }
            set { lastName = value; }
        }

        public int Age
        {
            get { return age; }
            set { age = value; }
        }

        public string Address1
        {
            get { return address1; }
            set { address1 = value; }
        }

        public string Address2
        {
            get { return address2; }
            set { address2 = value; }
        }

        public string PhoneNumber
        {
            get { return phoneNumber; }
            set { phoneNumber = value; }
        }

        public string GpName
        {
            get { return gpName; }
            set { gpName = value; }
        }

        public float FontSize
        {
            get { return fontSize; }
            set { fontSize = value; }
        }

        public int FontColour
        {
            get { return fontColour; }
            set { fontColour = value; }
        }

        public int BackgroundColour
        {
            get { return backgroundColour; }
            set { backgroundColour = value; }
        }
    }
}
