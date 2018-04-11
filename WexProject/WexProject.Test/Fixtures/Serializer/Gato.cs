using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace WexProject.Test.Scenario.Serializer
{
    [DataContract]
    class Gato
    {

        public Gato(string nome, string nomeCachorro, int idade)
        {
            this.nome = nome;
            this.cachorro = new Cachorro();
            this.cachorro.Nome = nomeCachorro;
            this.cachorro.Idade = idade;
        }

        private string nome;

        [DataMember]
        public string Nome
        {
            get { return nome; }
            set { nome = value; }
        }

        private Cachorro cachorro;

        [DataMember]
        public Cachorro Cachorro
        {
            get { return cachorro; }
            set { cachorro = value; }
        }
        
    }
}
