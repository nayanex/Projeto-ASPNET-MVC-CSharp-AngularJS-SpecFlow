using System;
using System.Collections.Generic;
using System.Linq;
using DevExpress.Xpo;
using WexProject.BLL.Models.Execucao;
using DevExpress.Xpo.DB;
using DevExpress.Data.Filtering;
using WexProject.BLL.Shared.Domains.Execucao;
using WexProject.BLL.Shared.DTOs.Escopo;
using WexProject.BLL.Contexto;
using WexProject.BLL.DAOs.Execucao;

namespace WexProject.BLL.BOs.Escopo
{
    public class GraficoEstimadoRealizadoBO
    {
        #region DevExpress

        public static List<GraficoEstimadoRealizadoDTO> CalcularGraficoEstimadoVsRealizadoProjeto( Guid oidProjeto, Session session )
        {
            List<GraficoEstimadoRealizadoDTO> lista = new List<GraficoEstimadoRealizadoDTO>();

            int cicloInicioTendencia = 0;
            uint ritimoTime = 0;

            using(XPCollection<CicloDesenv> ciclos = new XPCollection<CicloDesenv>( session, CriteriaOperator.Parse( "Projeto.Oid = ?", oidProjeto ) ))
            {
                ciclos.Sorting.Add( new SortProperty( "NbCiclo", SortingDirection.Ascending ) );
                if(ciclos.Count <= 0)
                {
                    return lista;
                }
                foreach(CicloDesenv ciclo in ciclos)
                {
                    ritimoTime = ciclo.Projeto.NbRitmoTime;
                    if(lista.Count > 0)
                    {
                        GraficoEstimadoRealizadoDTO newGraficoEstimadoRealizado = new GraficoEstimadoRealizadoDTO()
                        {
                            ProjetoOid = ciclo.Projeto.Oid,
                            Ciclo = ciclo.NbCiclo,
                            Estimado = ciclo.Projeto.NbTamanhoTotal / ciclo.Projeto.NbCicloTotalPlan + lista[lista.Count - 1].Estimado
                        };

                        if(ciclo.CsSituacaoCiclo.Equals( CsSituacaoCicloDomain.Concluido )
                            || ciclo.CsSituacaoCiclo.Equals( CsSituacaoCicloDomain.Cancelado ))
                        {
                            newGraficoEstimadoRealizado.Realizado = ciclo.NbPontosRealizados + lista[lista.Count - 1].Realizado;
                        }
                        else
                        {
                            newGraficoEstimadoRealizado.Realizado = lista[lista.Count - 1].Realizado;
                        }

                        //Ultimo ciclo do projeto.
                        if(ciclo.NbCiclo == ciclos.Count)
                        {
                            if(newGraficoEstimadoRealizado.Estimado < ciclo.Projeto.NbTamanhoTotal)
                            {
                                newGraficoEstimadoRealizado.Estimado += ciclo.Projeto.NbTamanhoTotal - newGraficoEstimadoRealizado.Estimado;
                            }
                            else if(newGraficoEstimadoRealizado.Estimado > ciclo.Projeto.NbTamanhoTotal)
                            {
                                newGraficoEstimadoRealizado.Estimado -= ( newGraficoEstimadoRealizado.Estimado - ciclo.Projeto.NbTamanhoTotal );
                            }
                        }

                        lista.Add( newGraficoEstimadoRealizado );
                    }
                    else
                    {
                        lista.Add( new GraficoEstimadoRealizadoDTO()
                        {
                            ProjetoOid = ciclo.Projeto.Oid,
                            Ciclo = ciclo.NbCiclo,
                            Estimado = ciclo.Projeto.NbTamanhoTotal / ciclo.Projeto.NbCicloTotalPlan,
                            Realizado = ciclo.NbPontosRealizados
                        } );
                    }

                    if(ciclo.CsSituacaoCiclo.Equals( CsSituacaoCicloDomain.Concluido )
                        || ciclo.CsSituacaoCiclo.Equals( CsSituacaoCicloDomain.Cancelado ))
                    {
                        cicloInicioTendencia = ciclo.NbCiclo;
                    }

                }

                double? tendenciaAcumulada = 0;
                Boolean addtendencia = false;

                if(cicloInicioTendencia == 0)
                {
                    addtendencia = true;
                    lista[0].RitimoSugerido = CalcularRitmoSugerido( null, ciclos[0].Projeto.NbTamanhoTotal, ciclos.Count );
                }
                if(cicloInicioTendencia < lista.Count)
                {
                    foreach(GraficoEstimadoRealizadoDTO item in lista)
                    {
                        if(cicloInicioTendencia.Equals( item.Ciclo ))
                        {
                            item.Tendencia = item.Realizado;
                            tendenciaAcumulada += item.Tendencia;
                            addtendencia = true;
                            item.RitimoSugerido = CalcularRitmoSugerido( item, ciclos[0].Projeto.NbTamanhoTotal, ciclos.Count );
                        }
                        else
                        {
                            if(addtendencia)
                            {
                                item.Tendencia = tendenciaAcumulada + ritimoTime;
                                tendenciaAcumulada += ritimoTime;
                                item.Realizado = null;
                            }
                            else
                            {
                                item.Tendencia = null;
                            }
                        }

                    }

                }
            }
            return lista;
        }

        #endregion

        #region Entity

        public static List<GraficoEstimadoRealizadoDTO> CalcularGraficoEstimadoVsRealizadoProjeto( Guid oidProjeto )
        {
            List<GraficoEstimadoRealizadoDTO> graficos = new List<GraficoEstimadoRealizadoDTO>();

            int cicloInicioTendencia = 0;
            uint ritmoTime = 0;

            using(WexDb contexto = ContextFactoryManager.CriarWexDb())
            {
                List<WexProject.BLL.Entities.Execucao.CicloDesenv> ciclos = CicloDesenvDAO.ConsultarCiclosDesenvDoProjeto( contexto, oidProjeto );

                if(ciclos.Count <= 0)
                    return graficos;

                foreach( WexProject.BLL.Entities.Execucao.CicloDesenv ciclo in ciclos)
                {
                    ritmoTime = Convert.ToUInt32( ciclo.Projeto1.NbRitmoTime );

                    if(graficos.Count > 0)
                    {
                        GraficoEstimadoRealizadoDTO newGraficoEstimadoRealizado = new GraficoEstimadoRealizadoDTO()
                        {
                            ProjetoOid = ciclo.Projeto1.Oid,
                            Ciclo = Convert.ToInt32( ciclo.NbCiclo ),
                            Estimado = Convert.ToInt32( ciclo.Projeto1.NbTamanhoTotal ) / Convert.ToInt32( ciclo.Projeto1.NbCicloTotalPlan ) + graficos[graficos.Count - 1].Estimado
                        };

                        if(ciclo.CsSituacaoCiclo == Convert.ToInt32( CsSituacaoCicloDomain.Concluido ) || ciclo.CsSituacaoCiclo == Convert.ToInt32( CsSituacaoCicloDomain.Cancelado ))
                        {
                            newGraficoEstimadoRealizado.Realizado = ciclo.NbPontosRealizados + graficos[graficos.Count - 1].Realizado;
                        }
                        else
                        {
                            newGraficoEstimadoRealizado.Realizado = graficos[graficos.Count - 1].Realizado;
                        }

                        //Ultimo ciclo do projeto.
                        if(ciclo.NbCiclo == ciclos.Count)
                        {
                            if(newGraficoEstimadoRealizado.Estimado < ciclo.Projeto1.NbTamanhoTotal)
                            {
                                newGraficoEstimadoRealizado.Estimado += (  Convert.ToInt32( ciclo.Projeto1.NbTamanhoTotal ) - newGraficoEstimadoRealizado.Estimado );
                            }
                            else if(newGraficoEstimadoRealizado.Estimado > ciclo.Projeto1.NbTamanhoTotal)
                            {
                                newGraficoEstimadoRealizado.Estimado -= ( Convert.ToDouble( newGraficoEstimadoRealizado.Estimado ) - Convert.ToInt32( ciclo.Projeto1.NbTamanhoTotal ) );
                            }
                        }

                        graficos.Add( newGraficoEstimadoRealizado );
                    }
                    else
                    {
                        graficos.Add( new GraficoEstimadoRealizadoDTO()
                        {
                            ProjetoOid = ciclo.Projeto1.Oid,
                            Ciclo = Convert.ToInt32( ciclo.NbCiclo ),
                            Estimado = Convert.ToInt32( ciclo.Projeto1.NbTamanhoTotal ) / Convert.ToDouble( ciclo.Projeto1.NbCicloTotalPlan ),
                            Realizado = ciclo.NbPontosRealizados
                        } );
                    }

                    if(ciclo.CsSituacaoCiclo == Convert.ToInt32( CsSituacaoCicloDomain.Concluido )
                        || ciclo.CsSituacaoCiclo == Convert.ToInt32( CsSituacaoCicloDomain.Cancelado ))
                    {
                        cicloInicioTendencia = Convert.ToInt32( ciclo.NbCiclo );
                    }
                }

                double? tendenciaAcumulada = 0;
                Boolean addtendencia = false;

                if(cicloInicioTendencia == 0)
                {
                    addtendencia = true;
                    graficos[0].RitimoSugerido = CalcularRitmoSugerido( null, Convert.ToInt32( ciclos[0].Projeto1.NbTamanhoTotal ), ciclos.Count );
                }
                if(cicloInicioTendencia < graficos.Count)
                {
                    foreach(GraficoEstimadoRealizadoDTO item in graficos)
                    {
                        if(cicloInicioTendencia == Convert.ToInt32( item.Ciclo ))
                        {
                            item.Tendencia = item.Realizado;
                            tendenciaAcumulada += item.Tendencia;
                            addtendencia = true;
                            item.RitimoSugerido = CalcularRitmoSugerido( item, Convert.ToInt32( ciclos[0].Projeto1.NbTamanhoTotal ), ciclos.Count );
                        }
                        else
                        {
                            if(addtendencia)
                            {
                                item.Tendencia = tendenciaAcumulada + ritmoTime;
                                tendenciaAcumulada += ritmoTime;
                                item.Realizado = null;
                            }
                            else
                            {
                                item.Tendencia = null;
                            }
                        }
                    }
                }
            }

            return graficos;
        }

        #endregion

        public static Double? CalcularRitmoSugerido( GraficoEstimadoRealizadoDTO ciclo, double tamanhoTotal, int totalCiclos )
        {
            if(totalCiclos <= 0)
            {
                return null;
            }
            else
            {
                if(ciclo == null)
                {
                    return Math.Round( tamanhoTotal / totalCiclos, 2, MidpointRounding.ToEven );
                }
                else
                {
                    if(ciclo.Ciclo == Convert.ToInt32( totalCiclos ))
                    {
                        return tamanhoTotal - ciclo.Realizado;
                    }
                    int ciclosRestantes = totalCiclos - ciclo.Ciclo;
                    Double? pontosRestantes = tamanhoTotal - ciclo.Realizado;
                    Double res = Double.Parse( ( pontosRestantes / ciclosRestantes ).ToString() );
                    return Math.Round( res, 2, MidpointRounding.ToEven );
                }
            }
        }
    }
}
