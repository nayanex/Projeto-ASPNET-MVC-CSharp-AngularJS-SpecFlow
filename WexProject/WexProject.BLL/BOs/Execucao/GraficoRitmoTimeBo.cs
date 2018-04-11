using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WexProject.BLL.Shared.DTOs.Execucao;
using DevExpress.Xpo;
using WexProject.BLL.Models.Execucao;
using DevExpress.Data.Filtering;
using DevExpress.Xpo.DB;
using WexProject.BLL.Shared.Domains.Execucao;
using WexProject.BLL.DAOs.Execucao;
using WexProject.BLL.Contexto;

namespace WexProject.BLL.BOs.Execucao
{
    public class GraficoRitmoTimeBO
    {
        #region DevExpress

        public static List<GraficoRitmoTimeDTO> CalcularGraficoRitmoTimeProjeto( Guid oidProjeto, Session session )
        {
            List<GraficoRitmoTimeDTO> lista = new List<GraficoRitmoTimeDTO>();

            using(XPCollection<CicloDesenv> ciclos = new XPCollection<CicloDesenv>( session, CriteriaOperator.
                Parse( "Projeto.Oid = ?", oidProjeto ) ))
            {
                ciclos.Sorting.Add( new SortProperty( "NbCiclo", SortingDirection.Ascending ) );
                int cicloInicioTendencia = 0;
                foreach(CicloDesenv ciclo in ciclos)
                {
                    if(ciclo.CsSituacaoCiclo.Equals( CsSituacaoCicloDomain.Concluido )
                        || ciclo.CsSituacaoCiclo.Equals( CsSituacaoCicloDomain.Cancelado ))
                    {
                        lista.Add( new GraficoRitmoTimeDTO()
                        {
                            Ciclo = ciclo.NbCiclo,
                            Ritmo = ciclo.NbPontosRealizados,
                            Planejado = ciclo.NbPontosPlanejados,
                            Meta = ciclo.GetPontosMeta()
                        } );
                    }
                    else
                    {
                        lista.Add( new GraficoRitmoTimeDTO()
                        {
                            Ciclo = ciclo.NbCiclo,
                            Ritmo = 0,
                            Planejado = ciclo.NbPontosPlanejados,
                            Meta = ciclo.GetPontosMeta()
                        } );
                    }
                    if(ciclo.CsSituacaoCiclo.Equals( CsSituacaoCicloDomain.Concluido )
                       || ciclo.CsSituacaoCiclo.Equals( CsSituacaoCicloDomain.Cancelado ))
                    {
                        cicloInicioTendencia = ciclo.NbCiclo;
                    }
                }

                Boolean addNull = false;
                if(cicloInicioTendencia == 0)
                {
                    addNull = true;
                }
                foreach(GraficoRitmoTimeDTO item in lista)
                {
                    if(addNull)
                    {
                        item.Ritmo = null;
                        item.Planejado = null;
                        item.Meta = null;
                    }
                    if(item.Ciclo.Equals( cicloInicioTendencia ))
                    {
                        addNull = true;
                    }
                }
            }

            return lista;
        }

        #endregion

        #region Entity

        /// <summary>
        /// Método responsável por calcular o gráfico do ritmo do time por projeto
        /// </summary>
        /// <param name="contexto">Contexto do banco</param>
        /// <param name="oidProjeto">Oid do projeto que servirá para a pesquisa</param>
        /// <returns>Lista dos graficos em DTO</returns>
        public static List<GraficoRitmoTimeDTO> CalcularGraficoRitmoTimeProjeto( Guid oidProjeto )
        {
            List<GraficoRitmoTimeDTO> graficosDTO = new List<GraficoRitmoTimeDTO>();

            using(WexDb contexto = ContextFactoryManager.CriarWexDb())
            {
                List<WexProject.BLL.Entities.Execucao.CicloDesenv> ciclos = CicloDesenvDAO.ConsultarCiclosDesenvDoProjeto( contexto, oidProjeto );

                int cicloInicioTendencia = 0;

                foreach(WexProject.BLL.Entities.Execucao.CicloDesenv ciclo in ciclos)
                {
                    if(ciclo.CsSituacaoCiclo == Convert.ToInt32( CsSituacaoCicloDomain.Concluido ) || ciclo.CsSituacaoCiclo == Convert.ToInt32(  CsSituacaoCicloDomain.Cancelado ))
                    {
                        graficosDTO.Add( new GraficoRitmoTimeDTO()
                        {
                            Ciclo = Convert.ToInt32( ciclo.NbCiclo ),
                            Ritmo = ciclo.NbPontosRealizados,
                            Planejado = ciclo.NbPontosPlanejados,
                            Meta = GraficoRitmoTimeBO.ConsultarPontosMeta( ciclo.CicloDesenvEstorias.ToList() )
                        } );
                    }
                    else
                    {
                        graficosDTO.Add( new GraficoRitmoTimeDTO()
                        {
                            Ciclo = Convert.ToInt32( ciclo.NbCiclo ),
                            Ritmo = 0,
                            Planejado = ciclo.NbPontosPlanejados,
                            Meta = GraficoRitmoTimeBO.ConsultarPontosMeta( ciclo.CicloDesenvEstorias.ToList() )
                        } );
                    }

                    if(ciclo.CsSituacaoCiclo == Convert.ToInt32( CsSituacaoCicloDomain.Concluido ) || ciclo.CsSituacaoCiclo == Convert.ToInt32( CsSituacaoCicloDomain.Cancelado ))
                    {
                        cicloInicioTendencia = Convert.ToInt32( ciclo.NbCiclo );
                    }
                }

                Boolean addNull = false;

                if(cicloInicioTendencia == 0)
                {
                    addNull = true;
                }

                foreach(GraficoRitmoTimeDTO item in graficosDTO)
                {
                    if(addNull)
                    {
                        item.Ritmo = null;
                        item.Planejado = null;
                        item.Meta = null;
                    }

                    if(item.Ciclo.Equals( cicloInicioTendencia ))
                    {
                        addNull = true;
                    }
                }

                return graficosDTO;
            }
        }            

        /// <summary>
        /// Método responsável por 
        /// </summary>
        /// <param name="ciclosDesenvEstorias"></param>
        /// <returns></returns>
        public static Double ConsultarPontosMeta( List<WexProject.BLL.Entities.Execucao.CicloDesenvEstoria> ciclosDesenvEstorias )
        {
            Double pontosMeta = 0;

            foreach(WexProject.BLL.Entities.Execucao.CicloDesenvEstoria item in ciclosDesenvEstorias)
                if(Convert.ToBoolean( item.Meta ))
                    pontosMeta += Convert.ToDouble( item.Estoria1.NbTamanho );

            return pontosMeta;
        }

        #endregion
    }
}
