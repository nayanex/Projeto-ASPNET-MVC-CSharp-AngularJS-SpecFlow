using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using WexProject.BLL.Entities.RH;

namespace WexProject.BLL.Entities.Planejamento
{
    public partial class TarefaResponsaveis
    {
        public Nullable<System.Guid> OidColaborador { get; set; }
        public Nullable<System.Guid> OidTarefa { get; set; }

        [Key]
        public System.Guid Oid { get; set; }
        public Colaborador Colaborador { get; set; }
        public Tarefa Tarefa { get; set; }
    }
}
