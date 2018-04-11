using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace WexProject.Test.Scenario.Serializer
{
    [DataContract]
    class Pessoa
    {

        private string nome;
        private int? idade;
        private HashSet<Filho> filhos;

        public Pessoa(string nome, int? idade)
        {
            this.nome = nome;
            this.idade = idade;
            this.filhos = new HashSet<Filho>();
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

        [DataMember]
        public HashSet<Filho> Filhos
        {
            get { return filhos; }
            set { filhos = value; }
        }

    }
}
