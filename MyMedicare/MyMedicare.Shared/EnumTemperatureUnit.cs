using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace MyMedicare
{
    [DataContract(Name = "TemperatureUnit")]
    public enum EnumTemperatureUnit
    {
        [EnumMember]
        CELCIUS = 0,
        [EnumMember]
        FAHRENHEIT = 1
    }
}
