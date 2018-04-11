using System;
using System.Collections.Generic;
using System.Linq;
using DevExpress.Xpo;
using WexProject.BLL.Contexto;
using WexProject.BLL.DAOs.Escopo;
using WexProject.BLL.Models.Escopo;
using WexProject.BLL.Shared.Domains.Escopo;
using WexProject.BLL.Shared.DTOs.Escopo;

namespace WexProject.BLL.BOs.Escopo
{
    public class GraficoEscopoCompletudeBO
    {
        #region DevExpress

        public static List<GraficoEscopoCompletudeDTO> CalcularGraficoEscopoCompletude( Guid oidProjeto, Session session )
        {
            List<GraficoEscopoCompletudeDTO> lista = new List<GraficoEscopoCompletudeDTO>();
            Dictionary<string, GraficoEscopoCompletudeDTO> modulosDic = new Dictionary<string, GraficoEscopoCompletudeDTO>();

            //Adiciona os modulos na lista.
            foreach(Modulo item in Modulo.GetModuloPorProjeto( session, oidProjeto ))
            {
                GraficoEscopoCompletudeDTO dto = new GraficoEscopoCompletudeDTO();
                dto.Modulo = item.TxNome;
                dto.TotalPontosModulo = item.NbPontosTotal;
                lista.Add( dto );
                modulosDic.Add( dto.Modulo, dto );
            }

            //Preenche os dados com as estorias.
            foreach(Estoria est in Estoria.GetEstoriasPorProjetoByOid( session, oidProjeto ))
            {
                if(est.EstoriaFilho.Count <= 0)
                {
                    string keyMap = Modulo.RnRecuperarModuloRaiz( est.Modulo ).TxNome;
                    if(est.CsSituacao.Equals( CsEstoriaDomain.EmDesenv ))
                    {
                        modulosDic[keyMap].PontosEmDesenv += est.NbTamanho;
                    }
                    else if(est.CsSituacao.Equals( CsEstoriaDomain.Pronto ))
                    {
                        modulosDic[keyMap].PontosProntos += est.NbTamanho;
                    }
                    else if(est.CsEmAnalise)
                    {
                        modulosDic[keyMap].PontosEmAnalise += est.NbTamanho;
                    }
                    else
                    {
                        modulosDic[keyMap].PontoNaoIniciados += est.NbTamanho;
                    }

                    modulosDic[keyMap].SomaModulo += est.NbTamanho;
                }
            }

            foreach(GraficoEscopoCompletudeDTO dto in lista)
            {
                //Existe desvio ou solicitação de mudança.
                double diff = dto.SomaModulo - dto.TotalPontosModulo;
                if(dto.SomaModulo > dto.TotalPontosModulo)
                {
                    //SOLICITAÇÂO DE MUDANÇA.
                    if(dto.PontoNaoIniciados > 0)
                    {
                        if(dto.PontoNaoIniciados >= diff)
                        {
                            dto.PontoNaoIniciados = dto.PontoNaoIniciados - diff;
                            dto.PontosMudanca = diff;
                            diff = 0;
                        }
                        else
                        {
                            dto.PontosMudanca = dto.PontoNaoIniciados;
                            diff -= dto.PontoNaoIniciados;
                            dto.PontoNaoIniciados = 0;
                        }
                    }

                    if(diff > 0 && dto.PontosEmAnalise > 0)
                    {
                        if(dto.PontosEmAnalise >= diff)
                        {
                            dto.PontosEmAnalise = dto.PontosEmAnalise - diff;
                            dto.PontosMudanca += diff;
                            diff = 0;
                        }
                        else
                        {
                            dto.PontosMudanca += dto.PontosEmAnalise;
                            diff -= dto.PontosEmAnalise;
                            dto.PontosEmAnalise = 0;
                        }
                    }
                    //DESVIO...

                    if(dto.PontosProntos > 0 && dto.PontosProntos > dto.TotalPontosModulo)
                    {
                        double diffDesvio = dto.PontosProntos - dto.TotalPontosModulo;
                        if(dto.PontosProntos >= diffDesvio)
                        {
                            dto.PontosProntos = dto.PontosProntos - diffDesvio;
                            dto.PontosDesvio += diffDesvio;
                            diffDesvio = 0;
                        }
                        else
                        {
                            dto.PontosDesvio += dto.PontosProntos;
                            diffDesvio -= dto.PontosProntos;
                            dto.PontosProntos = 0;
                        }
                    }
                }
                else
                {
                    dto.PontoNaoIniciados += dto.TotalPontosModulo - dto.SomaModulo;
                }

                dto.PercNaoInciado = calcPerc( dto.PontoNaoIniciados, dto.TotalPontosModulo );
                dto.PercEmAnalise = calcPerc( dto.PontosEmAnalise, dto.TotalPontosModulo );
                dto.PercEmDesenv = calcPerc( dto.PontosEmDesenv, dto.TotalPontosModulo );
                dto.PercProntos = calcPerc( dto.PontosProntos, dto.TotalPontosModulo );
                dto.PercMudanca = calcPerc( dto.PontosMudanca, dto.TotalPontosModulo );
                dto.PercDesvio = calcPerc( dto.PontosDesvio, dto.TotalPontosModulo );
            }

            return lista;
        }

        #endregion

        #region Entity

        public static List<GraficoEscopoCompletudeDTO> CalcularGraficoEscopoCompletude( Guid oidProjeto )
        {
            List<GraficoEscopoCompletudeDTO> graficos = new List<GraficoEscopoCompletudeDTO>();
            Dictionary<string, GraficoEscopoCompletudeDTO> modulos = new Dictionary<string, GraficoEscopoCompletudeDTO>();

            using(WexDb contexto = ContextFactoryManager.CriarWexDb())
            {
                //Adiciona os modulos na lista.
                foreach(WexProject.BLL.Entities.Escopo.Modulo item in ModuloDAO.ConsultarModulos( contexto, oidProjeto ))
                {
                    GraficoEscopoCompletudeDTO dto = new GraficoEscopoCompletudeDTO();
                    dto.Modulo = item.TxNome;
                    dto.TotalPontosModulo = item.NbPontosTotal.HasValue ? item.NbPontosTotal.Value : 0;
                    graficos.Add( dto );
                    modulos.Add( dto.Modulo, dto );
                }

                //Preenche os dados com as estorias.
                foreach(WexProject.BLL.Entities.Escopo.Estoria item in EstoriaDAO.ConsultarEstoriasPorProjeto( contexto, oidProjeto ))
                {

                    if(item.Estorias.Count <= 0)
                    {

                        string keyMap = ModuloBO.RecuperarModuloRaiz( item.Modulo ).TxNome;
                        bool emAnalise = item.CsEmAnalise.HasValue ? item.CsEmAnalise.Value : false;

                        if(item.CsSituacao == Convert.ToInt32( CsEstoriaDomain.EmDesenv ))
                        {
                            modulos[keyMap].PontosEmDesenv += Convert.ToDouble( item.NbTamanho );
                        }
                        else if(item.CsSituacao == Convert.ToInt32( CsEstoriaDomain.Pronto ))
                        {
                            modulos[keyMap].PontosProntos += Convert.ToDouble( item.NbTamanho );
                        }
                        else if(emAnalise)
                        {
                            modulos[keyMap].PontosEmAnalise += Convert.ToDouble( item.NbTamanho );
                        }
                        else
                        {
                            modulos[keyMap].PontoNaoIniciados += Convert.ToDouble( item.NbTamanho );
                        }

                        modulos[keyMap].SomaModulo += Convert.ToDouble( item.NbTamanho );
                    }
                }

                foreach(GraficoEscopoCompletudeDTO dto in graficos)
                {
                    //Existe desvio ou solicitação de mudança.
                    double diff = dto.SomaModulo - dto.TotalPontosModulo;

                    if(dto.SomaModulo > dto.TotalPontosModulo)
                    {
                        //SOLICITAÇÂO DE MUDANÇA.
                        if(dto.PontoNaoIniciados > 0)
                        {
                            if(dto.PontoNaoIniciados >= diff)
                            {
                                dto.PontoNaoIniciados = dto.PontoNaoIniciados - diff;
                                dto.PontosMudanca = diff;
                                diff = 0;
                            }
                            else
                            {
                                dto.PontosMudanca = dto.PontoNaoIniciados;
                                diff -= dto.PontoNaoIniciados;
                                dto.PontoNaoIniciados = 0;
                            }
                        }

                        if(diff > 0 && dto.PontosEmAnalise > 0)
                        {
                            if(dto.PontosEmAnalise >= diff)
                            {
                                dto.PontosEmAnalise = dto.PontosEmAnalise - diff;
                                dto.PontosMudanca += diff;
                                diff = 0;
                            }
                            else
                            {
                                dto.PontosMudanca += dto.PontosEmAnalise;
                                diff -= dto.PontosEmAnalise;
                                dto.PontosEmAnalise = 0;
                            }
                        }
                        //DESVIO...

                        if(dto.PontosProntos > 0 && dto.PontosProntos > dto.TotalPontosModulo)
                        {
                            double diffDesvio = dto.PontosProntos - dto.TotalPontosModulo;
                            if(dto.PontosProntos >= diffDesvio)
                            {
                                dto.PontosProntos = dto.PontosProntos - diffDesvio;
                                dto.PontosDesvio += diffDesvio;
                                diffDesvio = 0;
                            }
                            else
                            {
                                dto.PontosDesvio += dto.PontosProntos;
                                diffDesvio -= dto.PontosProntos;
                                dto.PontosProntos = 0;
                            }
                        }
                    }
                    else
                    {
                        dto.PontoNaoIniciados += dto.TotalPontosModulo - dto.SomaModulo;
                    }

                    dto.PercNaoInciado = calcPerc( dto.PontoNaoIniciados, dto.TotalPontosModulo );
                    dto.PercEmAnalise = calcPerc( dto.PontosEmAnalise, dto.TotalPontosModulo );
                    dto.PercEmDesenv = calcPerc( dto.PontosEmDesenv, dto.TotalPontosModulo );
                    dto.PercProntos = calcPerc( dto.PontosProntos, dto.TotalPontosModulo );
                    dto.PercMudanca = calcPerc( dto.PontosMudanca, dto.TotalPontosModulo );
                    dto.PercDesvio = calcPerc( dto.PontosDesvio, dto.TotalPontosModulo );
                }

                return graficos;
            }
        }

        #endregion

        public static double calcPerc( double valor, double total )
        {
            if(valor == 0)
                return 0;

            return Math.Round( ( 100 * valor ) / total, 2, MidpointRounding.ToEven );
        }
    }
}
