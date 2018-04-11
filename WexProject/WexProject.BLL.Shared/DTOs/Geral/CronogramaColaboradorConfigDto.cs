using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WexProject.BLL.Shared.DTOs.Geral
{
    /// <summary>
    /// Classe de tranferencia de informações do usuário conectado no cronograma
    /// </summary>
    public class CronogramaColaboradorConfigDto
    {
        /// <summary>
        /// oid de identificação do colaborador
        /// </summary>
        public Guid OidColaborador { get; set; }

        /// <summary>
        /// oid de identificação do cronograma
        /// </summary>
        public Guid OidCronograma { get; set; }

        /// <summary>
        /// Nome completo do colaborador
        /// </summary>
        public string NomeCompletoColaborador { get; set; }

        /// <summary>
        /// login de identificação do colaborador
        /// </summary>
        public string Login { get; set; }

        /// <summary>
        /// Stream de imagem do colaborador
        /// </summary>
        public byte[] Foto { get; set; }

        /// <summary>
        /// Cor de identificação do colaborador conectado
        /// </summary>
        public string Cor { get; set; }
    }
}
