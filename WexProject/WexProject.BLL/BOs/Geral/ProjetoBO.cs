using System;
using System.Collections.Generic;
using System.Linq;
using WexProject.BLL.BOs.Custos;
using WexProject.BLL.BOs.RH;
using WexProject.BLL.Contexto;
using WexProject.BLL.DAOs.Custos;
using WexProject.BLL.DAOs.Geral;
using WexProject.BLL.DAOs.RH;
using WexProject.BLL.Entities.Geral;
using WexProject.BLL.Entities.RH;
using WexProject.BLL.Exceptions.Custos;
using WexProject.BLL.Exceptions.Geral;
using WexProject.BLL.Extensions.Custos;
using WexProject.BLL.Models.Custos;
using WexProject.BLL.Shared.DTOs.Custos;
using WexProject.BLL.Shared.DTOs.Projeto;
using WexProject.BLL.Util;
using WexProject.Library.Libs.Collection;
using WexProject.Library.Libs.Generic;

namespace WexProject.BLL.BOs.Geral
{
    public class ProjetoBo
    {
        private const int ProjetoBase = 1; 
		/// <summary>
		/// Singleton
		/// </summary>
        private static ProjetoBo instancia;

		/// <summary>
		/// Singleton
		/// </summary>
        private ProjetoBo()
        {
        }

		/// <summary>
		/// Singleton
		/// </summary>
        public static ProjetoBo Instancia
        {
            get { return instancia ?? (instancia = new ProjetoBo()); }
        }

        /// <summary>
        ///     Atualiza as datas de Inicio e Termino do projeto baseado nos aditivos
        /// </summary>
        /// <param name="projeto">Projeto a ser atualizado</param>
        /// <param name="aditivos">Aditivos do projeto</param>
        private void RecalcularDatasProjeto(Projeto projeto, List<Aditivo> aditivos)
        {
            DateTime? inicio;
            DateTime? termino;

            if (aditivos.Count == 0)
            {
                inicio = null;
                termino = null;
            }
            else
            {
                inicio = DateTime.MaxValue;
                termino = DateTime.MinValue;
                foreach (Aditivo aditivo in aditivos)
                {
                    if (aditivo.DtInicio < inicio)
                    {
                        inicio = aditivo.DtInicio;
                    }
                    if (aditivo.DtTermino > termino)
                    {
                        termino = aditivo.DtTermino;
                    }
                }
            }

            projeto.DtInicioPlan = inicio;
            projeto.DtTerminoPlan = termino;
        }

        /// <summary>
        ///     Atualiza o valor de um projeto baseado nos valores dos aditivos
        /// </summary>
        /// <param name="projeto">Projeto a ser atualizado</param>
        /// <param name="aditivos">Aditivos do projeto</param>
        private void RecalcularValorProjeto(Projeto projeto, List<Aditivo> aditivos)
        {
            projeto.NbValor = aditivos.Sum(a => a.NbOrcamento);
        }

		/// <summary>
		///     Atualiza as informações do Projeto baseado nos aditivos
		/// </summary>
		/// <param name="projetoOid">Id do projeto a ser atualizado</param>
		public void RecalcularDadosProjeto(Guid projetoOid)
		{
			Projeto projeto = ProjetoDao.Instancia.ConsultarProjetoPorOid(projetoOid);
			List<Aditivo> aditivos = AditivoDao.Instance.ConsultarAditivos(projetoOid);

			RecalcularDatasProjeto(projeto, aditivos);
			RecalcularValorProjeto(projeto, aditivos);

			ProjetoDao.Instancia.SalvarProjeto(ContextFactoryManager.CriarWexDb(), projeto);
		}

        /// <summary>
        ///     Recupera projetos de acordo com um modelo, filtrados por propriedades.
        /// </summary>
        /// <param name="modelo">Dicionário de Propriedades requisitadas.</param>
        /// <param name="filtro">Dicionário de Propriedades para filtrar os objetos.</param>
        /// <returns>Lista de Obejtos Dinâmicos com as propriedades requisitadas.</returns>
        public List<ObjetoDinamico> ListarProjetos(ModeloDinamico<ProjetoDto> modelo, Filtro<ProjetoDto> filtro)
        {
            List<ObjetoDinamico> projetosDto = (from p in ProjetoDao.Instancia.ListarProjetos()
                select p.ToDto())
                .Filtra(filtro)
                .Select(modelo.Objeto)
                .ToList();

            return projetosDto;
        }

        /// <summary>
        ///     Recupera projetos filtrados por propriedades.
        /// </summary>
        /// <param name="filtro">Dicionário de Propriedades para filtrar os objetos.</param>
        /// <returns>Lista de Projetos filtrados.</returns>
        public List<ProjetoDto> ListarProjetos(Filtro<ProjetoDto> filtro)
        {
            List<ProjetoDto> projetosDto = (from p in ProjetoDao.Instancia.ListarProjetos()
                select p.ToDto())
                .Filtra(filtro)
                .ToList();

            return projetosDto;
        }

        /// <summary>
        ///     Recupera Projeto de acordo com um modelo, com um certo Oid.
        /// </summary>
        /// <param name="modelo">Dicionário de Propriedades requisitadas.</param>
        /// <param name="projetoOid">Oid do Projeto a ser recuperado.</param>
        /// <returns>Obejto Dinâmico com as propriedades requisitadas.</returns>
        public ObjetoDinamico ConsultarProjeto(ModeloDinamico<ProjetoDto> modelo, Guid projetoOid)
        {
            ObjetoDinamico projetoDto = modelo.Objeto(ProjetoDao.Instancia.ConsultarProjetoPorOid(projetoOid,
                o => o.Gerente.Usuario.Person,
                o => o.EmpresaInstituicao1
                ).ToDto());

            return projetoDto;
        }

        /// <summary>
        ///     Recupera Projeto com um certo Oid.
        /// </summary>
        /// <param name="projetoOid">Oid do Projeto a ser recuperado.</param>
        /// <returns>Dto de Projeto.</returns>
		public ProjetoDto ConsultarProjeto(Guid projetoOid)
        {
            Projeto projeto = ProjetoDao.Instancia.ConsultarProjetoPorOid(projetoOid);          
            return projeto.ToDto();
        }

        /// <summary>
        ///     Recupera projetos, opcionalmente filtrado por situação.
        /// </summary>
        /// <param name="situacao">Situação dos projetos a serem recuperados.</param>
        /// <returns>Uma lista de DTOs de Projeto.</returns>
        public List<ProjetoDto> ListarProjetos()
        {
            List<ProjetoDto> projetosDto = (from p in ProjetoDao.Instancia.ListarProjetos()
                select p.ToDto())
                .ToList();

            return projetosDto;
        }

        /// <summary>
        /// Método para definir propriedades do projeto DTO
        /// </summary>
        /// <param name="projeto">Entidade projeto</param>
        /// <returns>DadosBasicoProjetoDto</returns>
        private DadosBasicoProjetoDto DefinirPropriedadesProjeto(Projeto projeto)
        {
            DadosBasicoProjetoDto projetoDto = new DadosBasicoProjetoDto
            {
                IdProjeto = projeto.Oid,
                Nome = projeto.TxNome,
                Situacao = projeto.CsSituacaoProjeto,
                InicioPlanejado = projeto.DtInicioPlan,
                InicioReal = projeto.DtInicioReal,
                TerminoReal = projeto.DtTerminoReal,
                Gerente = ColaboradorBo.Instancia.ConsultarColaboradorPorGuid(projeto.GerenteOid ?? System.Guid.Empty),
                HasProjetosMicros = ExistemProjetosMicros(projeto.Oid),
                Clientes = new List<ClienteDto>()
            };

            foreach (ProjetoCliente projetoCliente in projeto.ProjetoClientes)
            {
                ClienteDto clienteDto = new ClienteDto
                {
                    IdCliente = projetoCliente.IdCliente ?? Guid.Empty,
                    Nome = projetoCliente.EmpresaInstituicao.TxNome
                };

                projetoDto.Clientes.Add(clienteDto);
            }

            return projetoDto;
        }

        /// <summary>
        /// Método para criar projeto DTO
        /// </summary>
        /// <param name="projeto">Entidade projeto</param>
        /// <returns>DadosBasicoProjetoDto</returns>
        private DadosBasicoProjetoDto CriarProjetoDto(Projeto projeto)
        {
            if (projeto == null)
            {
                return null;
            }

            DadosBasicoProjetoDto projetoDto = DefinirPropriedadesProjeto(projeto);

            if (projeto.CentroCusto != null)
            {               
                projetoDto.CentroCusto = CentroCustoBo.Instance.ConsultarCentroCustosPorId(projeto.CentroCusto.CentroCustoId);
            }

            if (VerificarProjetoMicro(projeto.Oid))
            {
                projetoDto.ProjetoMacro = CriarProjetoDto(projeto.ProjetoMacro);
                projetoDto.IsMacro = false;
            }
            else
            {
                projetoDto.IsMacro = true;
            }

            return projetoDto;
        }

        /// <summary>
        /// Verifica se o projeto possui um Gerente de Projeto
        /// </summary>
        /// <param name="projeto">Projeto DTO</param>
        /// <returns>Boolean</returns>
        public bool ExisteGerenteProjeto(DadosBasicoProjetoDto projeto)
        {
            return ((projeto.Gerente != null) && !(Guid.Empty.Equals(projeto.Gerente.OidColaborador)));
        }

        /// <summary>
        /// Verifica se o projeto possui um Centro de Custo
        /// </summary>
        /// <param name="projeto">Projeto DTO</param>
        /// <returns>Boolean</returns>
        public bool ExisteCentroCusto(DadosBasicoProjetoDto projeto)
        {
            return ((projeto.CentroCusto != null) && (projeto.CentroCusto.IdCentroCusto > 0));
        }

        /// <summary>
        /// Verifica se o projeto possui clientes associados
        /// </summary>
        /// <param name="projeto">Projeto DTO</param>
        /// <returns>Boolean</returns>
        public bool ExistemClientes(DadosBasicoProjetoDto projeto)
        {
            return (projeto.Clientes != null && projeto.Clientes.Count > 0);
        }

        /// <summary>
        /// Verifica se o projeto possui projetos micros
        /// </summary>
        /// <param name="IdProjeto">Id do Projeto</param>
        /// <returns>Boolean</returns>
        public bool ExistemProjetosMicros(Guid IdProjeto)
        {
            WexDb db = ContextFactoryManager.CriarWexDb();
            return ProjetoDao.Instancia.ExistemProjetosMicros(db, IdProjeto);
        }

        /// <summary>
        /// Verifica se o projeto é um projeto macro
        /// </summary>
        /// <param name="IdProjeto">Id do projeto</param>
        /// <returns>Boolean</returns>
        public bool VerificarProjetoMacro(Guid IdProjeto)
        {
            WexDb db = ContextFactoryManager.CriarWexDb();
            Projeto projeto = ProjetoDao.Instancia.ConsultarProjetoPorGuid(db, IdProjeto);
            return (projeto != null && projeto.ProjetoMacro == null);
        }

        /// <summary>
        /// Verifica se o projeto é um projeto macro
        /// </summary>
        /// <param name="projeto">Projeto DTO</param>
        /// <returns>Boolean</returns>
        public bool VerificarProjetoMacro(DadosBasicoProjetoDto projeto)
        {
            return (projeto.ProjetoMacro == null || Guid.Empty.Equals(projeto.ProjetoMacro.IdProjeto));
        }

        /// <summary>
        /// Verifica se o projeto é um projeto micro
        /// </summary>
        /// <param name="IdProjeto">Id do projeto</param>
        /// <returns>Boolean</returns>
        public bool VerificarProjetoMicro(Guid IdProjeto)
        {
            WexDb db = ContextFactoryManager.CriarWexDb();
            Projeto projeto = ProjetoDao.Instancia.ConsultarProjetoPorGuid(db, IdProjeto);
            return (projeto != null && projeto.ProjetoMacro != null);
        }

        /// <summary>
        /// Validação dos dados obrigatórios do projeto.
        /// @exception WexProject.BLL.Web.ValidationException Quando houver algum problema com os dados do projeto
        /// </summary>
        /// <param name="projeto">Projeto DTO</param>
        public void ValidarDadosProjeto(DadosBasicoProjetoDto projeto)
        {
            List<Validation> validacoes = new List<Validation>();

            if (!ExistemClientes(projeto))
            {
                validacoes.Add(new Validation("Clientes", "O projeto não possui cliente(s)."));
            }

            if (!Guid.Empty.Equals(projeto.IdProjeto) && projeto.ProjetoMacro != null && projeto.IdProjeto.Equals(projeto.ProjetoMacro.IdProjeto))
            {
                validacoes.Add(new Validation("ProjetoMacro", "O projeto não pode ser associado a ele mesmo"));
            }

            if (validacoes.Count > 0)
            {
                ValidationException exception = new ValidationException();
                exception.Validacoes = validacoes;
                throw exception;
            }
        }

        /// <summary>
        /// Consulta os dados de um projeto por Id
        /// </summary>
        /// <param name="IdProjeto">Id do projeto</param>
        /// <returns>Projeto DTO</returns>
		public DadosBasicoProjetoDto DadosProjeto(Guid IdProjeto)
        {
            WexDb db = ContextFactoryManager.CriarWexDb();
            Projeto projeto = ProjetoDao.Instancia.ConsultarProjetoPorGuid(db, IdProjeto);
            return CriarProjetoDto(projeto);
        }

        /// <summary>
        /// Lista todos os projetos do tipo Macro
        /// </summary>
        /// <returns>Lista de Projeto DTO</returns>
        public List<DadosBasicoProjetoDto> ListarProjetosMacro()
        {
            WexDb db = ContextFactoryManager.CriarWexDb();
            List<DadosBasicoProjetoDto> projetosMacros = new List<DadosBasicoProjetoDto>();
            ProjetoDao.Instancia.ListarProjetosMacro(db).ForEach(delegate(Entities.Geral.Projeto p)
            {
                projetosMacros.Add(CriarProjetoDto(p));
            });

            return projetosMacros;
        }

        /// <summary>
        /// Lista todos os projetos do tipo Micro referente ao projeto Macro
        /// </summary>
        /// <param name="IdProjetoMacro">Id do projeto macro</param>
        /// <returns>Lista de Projeto DTO</returns>
        public List<DadosBasicoProjetoDto> ListarProjetosMicro(Guid IdProjetoMacro)
        {
            WexDb db = ContextFactoryManager.CriarWexDb();
            List<DadosBasicoProjetoDto> projetosMicros = new List<DadosBasicoProjetoDto>();
            ProjetoDao.Instancia.ListarProjetosMicro(db, IdProjetoMacro).ForEach(delegate(Entities.Geral.Projeto p)
            {
                projetosMicros.Add(CriarProjetoDto(p));
            });

            return projetosMicros;
        }

        /// <summary>
        /// Verifica se existem projetos micros associados a um projeto 
        /// </summary>
        /// <param name="db">Contexto</param>
        /// <param name="projetoDto">Projeto DTO</param>
        private void VerificarExistenciaProjetosMicrosAssociados(WexDb db, DadosBasicoProjetoDto projetoDto)
        {
            if (ProjetoDao.Instancia.ExistemProjetosMicros(db, projetoDto.IdProjeto))
            {
                if (projetoDto.ProjetoMacro != null && !Guid.Empty.Equals(projetoDto.ProjetoMacro.IdProjeto))
                {
                    ValidarExistenciaProjetosMicrosAssociados();
                }
            }
        }

        /// <summary>
        /// Exibe mensagem de exceção
        /// </summary>
        private void ValidarExistenciaProjetosMicrosAssociados()
        {          
            ValidationException exception = new ValidationException();
            exception.Validacoes = new List<Validation>();
            exception.Validacoes.Add(new Validation("ProjetoMacro", "O projeto macro não pode ser convertido para projeto micro pois o mesmo contém projetos micros associados."));
            throw exception;
        }

        /// <summary>
        /// Método para consultar um projeto ou criar um novo projeto
        /// </summary>
        /// <param name="db">Contexto</param>
        /// <param name="idProjeto">Id do projeto</param>
        /// <returns>Projeto</returns>
        private Projeto ConsultarOuCriarProjeto(WexDb db, Guid idProjeto)
        {
            Projeto projeto;

            if (Guid.Empty.Equals(idProjeto))
            {
                projeto = new Projeto();
            }
            else
            {
                projeto = ProjetoDao.Instancia.ConsultarProjetoPorGuid(db, idProjeto);
            }

            return projeto;
        }

        /// <summary>
        /// Método para definir propriedades do projeto macro
        /// </summary>
        /// <param name="db">Contexto</param>
        /// <param name="projetoDto">Projeto DTO</param>
        /// <param name="projeto">Entidade projeto</param>
        /// <param name="gerente">Gerente</param>
        /// <param name="centroCusto">Centro Custo</param>
        private void DefinirPropriedadesProjetoMacro(WexDb db, DadosBasicoProjetoDto projetoDto, Projeto projeto,
                out Colaborador gerente, out CentroCusto centroCusto)
        {
			gerente = null;
			centroCusto = null;

            if (ExisteGerenteProjeto(projetoDto))
            {
                gerente = ColaboradorDAO.Instancia.ConsultarColaboradorPorGuid(db, projetoDto.Gerente.OidColaborador);
            }

            if (ExisteCentroCusto(projetoDto))
            {
                // TODO: Refatorar esta chamada
                centroCusto = db.CentrosCusto.Find(projetoDto.CentroCusto.IdCentroCusto);
            }

            projeto.TipoProjetoId = ProjetoBase;
        }

        /// <summary>
        /// Método para verificar se o projeto era macro
        /// </summary>
        /// <param name="projeto">Projeto</param>
        private void VerificarSeProjetoEraMacro(Projeto projeto)
        {
            // Projeto está sendo salvo como Micro. Era Macro antes?
            if (projeto.ProjetoMacroOid == null && !VerificarProjetoVazio(projeto))
            {
                throw new ProjetoNaoVazioException(String.Format("Projeto Macro '{0}'({1}) não está vazio e não pode ser movido como Projeto Micro.", projeto.TxNome, projeto.Oid));
            }
        }

        /// <summary>
        /// Método para configurar as propriedades do projeto
        /// </summary>
        /// <param name="projeto">Entidade projeto</param>
        /// <param name="projetoDto">Projeto DTO</param>
        /// <param name="gerente">Gerente</param>
        /// <param name="centroCusto">Centro Custo</param>
        /// <param name="projetoMacro">ProjetoMacro</param>
        private void ConfigurarPropriedadesProjeto(Projeto projeto, DadosBasicoProjetoDto projetoDto, Colaborador gerente,
            CentroCusto centroCusto, Projeto projetoMacro)
        {
            projeto.TxNome = projetoDto.Nome;
            projeto.DtInicioPlan = projetoDto.InicioPlanejado;
            projeto.DtInicioReal = projetoDto.InicioReal;
            projeto.DtTerminoReal = projetoDto.TerminoReal;
            projeto.CsSituacaoProjeto = projetoDto.Situacao;

            if (gerente != null && !Guid.Empty.Equals(gerente.Oid))
            {
                projeto.GerenteOid = gerente.Oid;
            }

            if (centroCusto != null && centroCusto.CentroCustoId != 0)
            {
                projeto.CentroCustoId = centroCusto.CentroCustoId;
            }

            if (projetoMacro != null)
            {
                projeto.ProjetoMacroOid = projetoMacro.Oid;
            }
        }

        /// <summary>
        /// Método para associar clientes ao projeto
        /// </summary>
        /// <param name="db">Contexto</param>
        /// <param name="projeto">Projeto</param>
        /// <param name="clientes">Lista de clientes</param>
        private void AssociarClientesAoProjeto(WexDb db, Projeto projeto, List<ClienteDto> clientes)
        {
            List<Guid> idsClientes = new List<Guid>();

            foreach (ClienteDto cliente in clientes)
            {
                idsClientes.Add(cliente.IdCliente);
            }

            ProjetoDao.Instancia.AssociarClientes(db, projeto, idsClientes);
        }

        /// <summary>
        /// Método para salvar um projeto
        /// </summary>
        /// <param name="projetoDto">Projeto DTO</param>
        /// <returns>Id do projeto</returns>
        public Guid SalvarProjeto(DadosBasicoProjetoDto projetoDto)
        {
            ValidarDadosProjeto(projetoDto);

            WexDb db = ContextFactoryManager.CriarWexDb();

            VerificarExistenciaProjetosMicrosAssociados(db, projetoDto);

            Projeto projeto = ConsultarOuCriarProjeto(db, projetoDto.IdProjeto);

            Projeto projetoMacro = null;
            CentroCusto centroCusto = null;
            Colaborador gerente = null;

            if (VerificarProjetoMacro(projetoDto))
            {
                DefinirPropriedadesProjetoMacro(db, projetoDto, projeto, out gerente, out centroCusto);
            }
            else
            {
                VerificarSeProjetoEraMacro(projeto);
                // projetos micros possuem o mesmo centro de custo e gerente do projeto macro
                projetoMacro = ProjetoDao.Instancia.ConsultarProjetoPorGuid(db, projetoDto.ProjetoMacro.IdProjeto);

                centroCusto = projetoMacro.CentroCusto;

                gerente = projetoMacro.Gerente;
            }

            ConfigurarPropriedadesProjeto(projeto, projetoDto, gerente, centroCusto, projetoMacro);

            Guid projetoId = ProjetoDao.Instancia.SalvarProjeto(db, projeto);

            AssociarClientesAoProjeto(db, projeto, projetoDto.Clientes);

            db.SaveChanges();

            return projetoId;

        }

        /// <summary>
        /// Método para verificar se projeto está vazio
        /// </summary>
        /// <param name="projeto">Projeto</param>
        /// <returns>Boolean</returns>
        public bool VerificarProjetoVazio(Projeto projeto)
        {
            var aditivos = AditivoDao.Instance.ConsultarAditivos(projeto.Oid);

            if (aditivos.Count > 0)
            {
                return false;
            }

            return true;
        }

        public void ExisteTipoProjeto(Projeto projeto)
        {
            if (!projeto.TipoProjetoId.HasValue)
            {
                throw new ProjetoSemTipoException(String.Format("Projeto {0}({1}) não possui tipo.",
                    projeto.TxNome, projeto.Oid));
            }
        }

        public void CompararTipoProjetoEmProjetoETipoRubrica(Projeto projeto, TipoProjeto tipoProjeto)
        {
            if (projeto.TipoProjetoId.Value != tipoProjeto.TipoProjetoId)
            {
                TipoProjeto TipoProjeto = TipoProjetoDao.GetTipoProjetoPorId(projeto.TipoProjetoId.Value);
                ClasseProjeto classeProjetoRubrica =
                    ClasseProjetoDao.GetClasseProjetoPorId(tipoProjeto.ClasseProjetoId);

                throw new RubricaTipoDiferenteException(
                    String.Format(
                        "Rubrica é do tipo '{0}' da classe '{1}', mas Projeto é do tipo '{2}' da classe '{3}'",
                        tipoProjeto.TxNome, classeProjetoRubrica.TxNome, TipoProjeto.TxNome,
                        TipoProjeto.ClasseProjeto.TxNome));
            }
        }

		/// <summary>
		/// Método para verificar existência de TipoProjeto 
		/// </summary>
		/// <param name="aditivoId">Id do aditivo</param>
		/// <param name="tipoRubricaId">Id do TipoRubrica</param>
		public void ValidarTipoProjeto(int aditivoId, int tipoRubricaId)
		{
			Aditivo aditivo = AditivoDao.Instance.ConsultarAditivo(aditivoId);
			TipoRubrica tipoRubrica = TipoRubricaDao.Instance.ConsultarTipoRubrica(tipoRubricaId);

			ProjetoBo.Instancia.ExisteTipoProjeto(aditivo.Projeto);

			ProjetoBo.Instancia.CompararTipoProjetoEmProjetoETipoRubrica(aditivo.Projeto, tipoRubrica.TipoProjeto);
		}
    }
}