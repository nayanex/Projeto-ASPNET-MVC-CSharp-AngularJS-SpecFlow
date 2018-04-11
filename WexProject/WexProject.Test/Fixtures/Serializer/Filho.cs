using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace WexProject.Test.Scenario.Serializer
{
    [DataContract]
    class Filho
    {
        private int? idade;
        private string nome;

        public Filho(String nome, int idade) {
            this.idade = idade;
            this.nome = nome;
        }


        [DataMember]
        public int? Idade
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
