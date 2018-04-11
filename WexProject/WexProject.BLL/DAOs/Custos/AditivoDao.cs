using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using WexProject.BLL.Contexto;
using WexProject.BLL.Entities.Geral;
using WexProject.BLL.Models.Custos;

namespace WexProject.BLL.DAOs.Custos
{
    public class AditivoDao
    {

        /// <summary>
        /// Singleton
        /// </summary>
        private static AditivoDao _instance;

        /// <summary>
        /// Singleton
        /// </summary>
        private AditivoDao()
        {
        }

        /// <summary>
        /// Singleton
        /// </summary>
        public static AditivoDao Instance
        {
            get { return _instance ?? (_instance = new AditivoDao()); }
        }
		
        /// <summary>
        ///     Adiciona um centro de custo ao aditivo
        /// </summary>
        /// <param name="aditivoId">Id do aditivo</param>
        /// <param name="centroCustoId">Id do centro de custo</param>
        /// <returns>Id da relação criada</returns>
        public int AssociarCentroCusto(int aditivoId, int centroCustoId)
        {
            WexDb contexto = ContextFactoryManager.CriarWexDb();
            var aditivoCentroCusto = new AditivoCentroCusto
            {
                AditivoId = aditivoId,
                CentroCustoId = centroCustoId
            };

            contexto.AditivosCentrosCusto.Add(aditivoCentroCusto);
            contexto.SaveChanges();

            return aditivoCentroCusto.AditivoCentroCustoId;
        }

        /// <summary>
        ///     Adiciona um patrocinador ao aditivo
        /// </summary>
        /// <param name="aditivoId">Id do aditivo</param>
        /// <param name="patrocinadorOid">Id do patrocinador</param>
        /// <returns>Id da relação criada</returns>
        public int AssociarPatrocinador(int aditivoId, Guid patrocinadorOid)
        {
            WexDb contexto = ContextFactoryManager.CriarWexDb();
            var aditivoPatrocinador = new AditivoPatrocinador
            {
                AditivoId = aditivoId,
                PatrocinadorOid = patrocinadorOid
            };

            contexto.AditivosPatrocinadores.Add(aditivoPatrocinador);
            contexto.SaveChanges();

            return aditivoPatrocinador.AditivoPatrocinadorId;
        }

        /// <summary>
        ///     Recupera aditivo por id
        /// </summary>
        /// <param name="aditivoId">Id do aditivo a ser recuperado</param>
        /// <returns>Aditivo com o id passado</returns>
        public Aditivo ConsultarAditivo(int aditivoId)
        {
            WexDb contexto = ContextFactoryManager.CriarWexDb();
            Aditivo aditivo = (from a in contexto.Aditivos.Include(a => a.Projeto)
                where a.AditivoId == aditivoId
                select a).First();

            return aditivo;
        }

        /// <summary>
        ///     Retorna todos os centros de custo de um aditivo
        /// </summary>
        /// <param name="aditivoId">Id do aditivo</param>
        /// <returns>Uma lista de CentroCusto</returns>
        public List<CentroCusto> ConsultarCentrosCustos(int aditivoId)
        {
            WexDb contexto = ContextFactoryManager.CriarWexDb();
            List<CentroCusto> centrosCusto = (from ac in contexto.AditivosCentrosCusto
                where ac.AditivoId == aditivoId
                select ac.CentroCusto).ToList();

            return centrosCusto;
        }

        /// <summary>
        ///     Retorna todos os patrocinadores de um aditivo
        /// </summary>
        /// <param name="aditivoId">Id do aditivo</param>
        /// <returns>Uma lista de DTOs de EmpresaInstituição</returns>
        public List<EmpresaInstituicao> ConsultarPatrocinadores(int aditivoId)
        {
            WexDb contexto = ContextFactoryManager.CriarWexDb();
            List<EmpresaInstituicao> patrocinadores = (from ap in contexto.AditivosPatrocinadores
                where ap.AditivoId == aditivoId
                select ap.Patrocinador).ToList();

            return patrocinadores;
        }


        public List<AditivoPatrocinador> ConsultarAditivosPatrocinadores(int aditivoId)
        {
            WexDb contexto = ContextFactoryManager.CriarWexDb();
            List<AditivoPatrocinador> aditivosPatrocinadores = (from ap in contexto.AditivosPatrocinadores
                                                       where ap.AditivoId == aditivoId
                                                       select ap).ToList();

            return aditivosPatrocinadores;
        }

        /// <summary>
        ///     Lista todos o aditivos de um projeto
        /// </summary>
        /// <param name="projetoOid">Id do projeto dos aditivos</param>
        /// <returns>Lista com os aditivos do projeto</returns>
        public List<Aditivo> ConsultarAditivos(Guid projetoOid)
        {
            WexDb contexto = ContextFactoryManager.CriarWexDb();
            List<Aditivo> aditivos =
                contexto.Aditivos.Include(aditivo => aditivo.Rubricas)
                    .Where(aditivo => aditivo.ProjetoOid == projetoOid)
                    .ToList();
            return aditivos;
        }

        /// <summary>
        /// </summary>
        /// <param name="aditivo"></param>
        /// <returns></returns>
        public int SalvarAditivo(Aditivo aditivo)
        {
            using (WexDb contexto = ContextFactoryManager.CriarWexDb())
            {
                if (aditivo.AditivoId == 0)
                {
                    contexto.Aditivos.Add(aditivo);

                    contexto.Entry(aditivo).Reference(a => a.Projeto).Load();

                    List<TipoRubrica> tiposRubricas = (from tr in contexto.TiposRubrica
                        where tr.TipoPaiId == null && aditivo.Projeto.TipoProjetoId == tr.TipoProjetoId
                        select tr).ToList();

                    foreach (TipoRubrica tipoRubrica in tiposRubricas)
                    {
                        var rubrica = new Rubrica
                        {
                            TipoRubricaId = tipoRubrica.TipoRubricaId,
                            AditivoId = aditivo.AditivoId
                        };

                        contexto.Rubricas.Add(rubrica);

                        foreach (TipoRubrica tipoRubricaFilha in tipoRubrica.TiposFilhos)
                        {
                            rubrica.Filhos.Add(new Rubrica
                            {
                                TipoRubricaId = tipoRubricaFilha.TipoRubricaId,
                                AditivoId = aditivo.AditivoId
                            });
                        }
                    }
                }
                else
                {
                    // aditivo está no estado Detached e não existe nenhum outro objeto Aditivo no contexto com a mesma chave primária que aditivo.
                    // Neste caso basta adicionar aditivo ao contexto e definir seu estado como Modified.
                    // Caso o já existisse um objeto Aditivo com a mesma chave primária no contexto seria preciso executar a segunite linha:
                    //		_db.Entry(_db.Aditivos.Find(aditivo.AditivoId)).CurrentValues.SetValues(aditivo);
                    contexto.Entry(aditivo).State = EntityState.Modified;
                }

                contexto.SaveChanges();
            }

            return aditivo.AditivoId;
        }

        /// <summary>
        ///     Remove um aditivo do banco
        /// </summary>
        /// <param name="aditivoId">Id do aditivo</param>
		/// <returns>Id do aditivo removido</returns>
        public int RemoverAditivo(int aditivoId)
        {
			Aditivo aditivo;

            using (WexDb contexto = ContextFactoryManager.CriarWexDb())
            {
                aditivo = contexto.Aditivos.Find(aditivoId);
                contexto.Aditivos.Remove(aditivo);
                contexto.SaveChanges();
            }

			return aditivo.AditivoId;
        }

        /// <summary>
        ///     Remove um centro de custo de um aditivo
        /// </summary>
        /// <param name="aditivoId">Id do aditivo</param>
        /// <param name="centroCustoId">Id do centro de custo</param>
        /// <returns>Id da relação removida</returns>
        public int DesassociarCentroCusto(int aditivoId, int centroCustoId)
        {
            WexDb contexto = ContextFactoryManager.CriarWexDb();
            AditivoCentroCusto aditivoCentroCusto = (from ac in contexto.AditivosCentrosCusto
                where ac.AditivoId == aditivoId && ac.CentroCustoId == centroCustoId
                select ac).First();

            contexto.AditivosCentrosCusto.Remove(aditivoCentroCusto);
            contexto.SaveChanges();

            return aditivoCentroCusto.AditivoCentroCustoId;
        }

        /// <summary>
        ///     Remove um patrocinador de um aditivo
        /// </summary>
        /// <param name="aditivoId">Id do Aditivo</param>
        /// <param name="patrocinadorOid">Id do Patrocinador</param>
        /// <returns>Id da relação removida</returns>
        public int DesassociarPatrocinador(int aditivoId, Guid patrocinadorOid)
        {
            WexDb contexto = ContextFactoryManager.CriarWexDb();
            AditivoPatrocinador aditivoPatrocinador = (from ap in contexto.AditivosPatrocinadores
                where ap.AditivoId == aditivoId && ap.PatrocinadorOid == patrocinadorOid
                select ap).First();

            contexto.AditivosPatrocinadores.Remove(aditivoPatrocinador);
            contexto.SaveChanges();

            return aditivoPatrocinador.AditivoPatrocinadorId;
        }
	}
}
