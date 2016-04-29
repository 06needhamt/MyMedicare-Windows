using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace MyMedicare
{
    [DataContract(Name = "RiskLevel")]
    public enum EnumRiskLevel
    {
        [EnumMember]
        LOW = 0,
        [EnumMember]
        MEDIUM = 1,
        [EnumMember]
        HIGH = 2,
    }
}
