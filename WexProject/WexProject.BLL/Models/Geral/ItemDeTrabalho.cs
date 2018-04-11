using System;
using System.Collections.Generic;
using System.Linq;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Xpo;

namespace WexProject.BLL.Models.Geral
{
    /// <summary>
    /// Classe que será usada para analisar os impactos dos artefatos de um projeto.
    /// </summary>
    [OptimisticLocking(false)]
    public class ItemDeTrabalho : BaseObject
    {
        #region Atributos

        /// <summary>
        /// Propriedade que armazena o item de trabalho pai.
        /// </summary>
        private ItemDeTrabalho itemDeTrabalhoPai;

        #endregion

        #region Propriedades

        /// <summary>
        /// Propriedade que armazena quem é o Item de Trabalho Pai de outro determinado Item de Trabalho
        /// </summary>
        public ItemDeTrabalho ItemDeTrabalhoPai
        {
            get
            {
                return itemDeTrabalhoPai;
            }
            set
            {
                SetPropertyValue<ItemDeTrabalho>("ItemDeTrabalhoPai", itemDeTrabalhoPai);
            }
        }
        #endregion

        #region Construtor

        public ItemDeTrabalho(Session session)
            : base(session)
        {

        }

        #endregion
    }
}
