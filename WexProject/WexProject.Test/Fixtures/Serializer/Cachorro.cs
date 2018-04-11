using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace WexProject.Test.Scenario.Serializer
{
    [DataContract]
    class Cachorro
    {
        private int idade;
        private string nome;

        [DataMember]
        public int Idade
        {
            get { return idade; }
            set { idade = value; }
        }

        [DataMember]
        public string Nome
        {
            get { return nome; }
            set { nome = value; }
        }
    }
}
