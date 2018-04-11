using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace WexProject.Test.Scenario.Serializer
{
    [DataContract]
    class ValoresNumericos
    {

        private double doubleValue;

        private bool boolValue;

        [DataMember]
        public bool BoolValue
        {
            get { return boolValue; }
            set { boolValue = value; }
        }


        public ValoresNumericos(double doubleValue, bool boolValue)
        {
            this.doubleValue = doubleValue;
            this.boolValue = boolValue;
        }

        [DataMember]
        public double DoubleValue
        {
            get { return doubleValue; }
            set { doubleValue = value; }
        }

    }
}
