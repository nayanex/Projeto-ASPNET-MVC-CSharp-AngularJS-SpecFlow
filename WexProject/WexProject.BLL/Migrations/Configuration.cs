namespace WexProject.BLL.Migrations
{
	using System;
	using System.Data.Entity;
	using System.Data.Entity.Migrations;
	using System.Linq;
	using WexProject.BLL.Models.Custos;
	using WexProject.BLL.Entities.Geral;
	using WexProject.BLL.Shared.Domains.Custos;
	using WexProject.BLL.Contexto;

    internal sealed class Configuration : DbMigrationsConfiguration<WexDb>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(WexDb context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //

			context.ClassesProjetos.AddOrUpdate(
				new ClasseProjeto { ClasseProjetoId = 1, TxNome = "Projeto Patrocinado" },
				new ClasseProjeto { ClasseProjetoId = 2, TxNome = "Projeto sem Patrocínio" },
				new ClasseProjeto { ClasseProjetoId = 3, TxNome = "Setor" }
			);

			context.TiposProjetos.AddOrUpdate(
				new TipoProjeto { TipoProjetoId = 1, ClasseProjetoId = 1, TxNome = "Projeto Base" },
				new TipoProjeto { TipoProjetoId = 2, ClasseProjetoId = 3, TxNome = "Setor de Administração" },
				new TipoProjeto { TipoProjetoId = 3, ClasseProjetoId = 2, TxNome = "Projeto Base" }
			);

			context.TiposRubrica.AddOrUpdate(
				/* Projeto Patrocinado | Projeto Base */
				new TipoRubrica { TipoRubricaId = 1, CsClasse = CsClasseRubrica.Desenvolvimento, TipoProjetoId = 1, TxNome = "Viagens e Deslocamentos" },
				new TipoRubrica { TipoRubricaId = 2, CsClasse = CsClasseRubrica.Desenvolvimento, TipoProjetoId = 1, TxNome = "Contratações (fora de Manaus)" },
				new TipoRubrica { TipoRubricaId = 3, CsClasse = CsClasseRubrica.Desenvolvimento, TipoProjetoId = 1, TxNome = "Treinamentos" },
				new TipoRubrica { TipoRubricaId = 4, CsClasse = CsClasseRubrica.Desenvolvimento, TipoProjetoId = 1, TxNome = "Manutenção (após entrega)" },
				new TipoRubrica { TipoRubricaId = 5, CsClasse = CsClasseRubrica.Desenvolvimento, TipoProjetoId = 1, TxNome = "Livros e Periódicos" },
				new TipoRubrica { TipoRubricaId = 6, CsClasse = CsClasseRubrica.Desenvolvimento, TipoProjetoId = 1, TxNome = "Software" },
				new TipoRubrica { TipoRubricaId = 7, CsClasse = CsClasseRubrica.Desenvolvimento, TipoProjetoId = 1, TxNome = "Hardware" },
				new TipoRubrica { TipoRubricaId = 8, CsClasse = CsClasseRubrica.Desenvolvimento, TipoProjetoId = 1, TxNome = "Obras de Infra-Estrutura" },
				new TipoRubrica { TipoRubricaId = 9, CsClasse = CsClasseRubrica.Desenvolvimento, TipoProjetoId = 1, TxNome = "Terceiros" },
				new TipoRubrica { TipoRubricaId = 10, CsClasse = CsClasseRubrica.Desenvolvimento | CsClasseRubrica.RecursosHumanos, TipoProjetoId = 1, TxNome = "RH GDC" },
				new TipoRubrica { TipoRubricaId = 11, CsClasse = CsClasseRubrica.Desenvolvimento | CsClasseRubrica.RecursosHumanos, TipoProjetoId = 1, TxNome = "RH TI" },
				new TipoRubrica { TipoRubricaId = 12, CsClasse = CsClasseRubrica.Desenvolvimento | CsClasseRubrica.RecursosHumanos, TipoProjetoId = 1, TxNome = "RH Designer" },
				new TipoRubrica { TipoRubricaId = 13, CsClasse = CsClasseRubrica.Desenvolvimento | CsClasseRubrica.RecursosHumanos, TipoProjetoId = 1, TxNome = "RH Testes" },
				new TipoRubrica { TipoRubricaId = 14, CsClasse = CsClasseRubrica.Desenvolvimento | CsClasseRubrica.RecursosHumanos, TipoProjetoId = 1, TxNome = "RH Qualidade" },
				new TipoRubrica { TipoRubricaId = 15, CsClasse = CsClasseRubrica.Desenvolvimento | CsClasseRubrica.RecursosHumanos, TipoProjetoId = 1, TxNome = "Mão de Obra Direta (Exclusiva)" },
				new TipoRubrica { TipoRubricaId = 16, CsClasse = CsClasseRubrica.Administrativo, TipoProjetoId = 1, TxNome = "Custo Fixo FPF (Rateio)" },
				new TipoRubrica { TipoRubricaId = 17, CsClasse = CsClasseRubrica.Administrativo, TipoProjetoId = 1, TxNome = "Depreciação de Equipamentos" },
				//new TipoRubrica { TipoRubricaId = 18, CsClasse = CsClasseRubrica.Administrativo, TipoProjetoId = 1, TxNome = "Terceiros" },
				new TipoRubrica { TipoRubricaId = 19, CsClasse = CsClasseRubrica.Administrativo, TipoProjetoId = 1, TxNome = "Taxa de Administração" },
				new TipoRubrica { TipoRubricaId = 20, CsClasse = CsClasseRubrica.Administrativo, TipoProjetoId = 1, TxNome = "Impostos e Encargos" },
				new TipoRubrica { TipoRubricaId = 21, CsClasse = CsClasseRubrica.Administrativo, TipoProjetoId = 1, TxNome = "Apoio a Clientes" },
                new TipoRubrica { TipoRubricaId = 99, CsClasse = CsClasseRubrica.Administrativo, TipoProjetoId = 1, TxNome = "Apoio a Clientes 2" },
				new TipoRubrica { TipoRubricaId = 22, CsClasse = CsClasseRubrica.Administrativo, TipoProjetoId = 1, TxNome = "FACN" },
				new TipoRubrica { TipoRubricaId = 23, CsClasse = CsClasseRubrica.Aportes, TipoProjetoId = 1, TxNome = "Aportes" },
				new TipoRubrica { TipoRubricaId = 24, CsClasse = CsClasseRubrica.Desenvolvimento | CsClasseRubrica.RecursosHumanos, TipoProjetoId = 1, TxNome = "RH Direto (Dissídio)" },

				/* Setor | Setor Administração */
				new TipoRubrica { TipoRubricaId = 25, CsClasse = CsClasseRubrica.Administrativo | CsClasseRubrica.Pai, TipoProjetoId = 2, TxNome = "Pessoal" },
				new TipoRubrica { TipoRubricaId = 26, CsClasse = CsClasseRubrica.Administrativo, TipoProjetoId = 2, TxNome = "Salários", TipoPaiId = 25 },
				new TipoRubrica { TipoRubricaId = 27, CsClasse = CsClasseRubrica.Administrativo, TipoProjetoId = 2, TxNome = "Rescisão / Férias", TipoPaiId = 25 },
				new TipoRubrica { TipoRubricaId = 28, CsClasse = CsClasseRubrica.Administrativo, TipoProjetoId = 2, TxNome = "Estagiários", TipoPaiId = 25 },
				new TipoRubrica { TipoRubricaId = 29, CsClasse = CsClasseRubrica.Administrativo, TipoProjetoId = 2, TxNome = "Encargos Sociais", TipoPaiId = 25 },
				new TipoRubrica { TipoRubricaId = 30, CsClasse = CsClasseRubrica.Administrativo, TipoProjetoId = 2, TxNome = "Benefícios Sociais", TipoPaiId = 25 },
				new TipoRubrica { TipoRubricaId = 31, CsClasse = CsClasseRubrica.Administrativo, TipoProjetoId = 2, TxNome = "PQC - Programa de Qualificação Colaboradores", TipoPaiId = 25 },
				new TipoRubrica { TipoRubricaId = 32, CsClasse = CsClasseRubrica.Administrativo, TipoProjetoId = 2, TxNome = "Transporte de funcionários", TipoPaiId = 25 },
				new TipoRubrica { TipoRubricaId = 33, CsClasse = CsClasseRubrica.Administrativo, TipoProjetoId = 2, TxNome = "Exames Laboatoriais", TipoPaiId = 25 },
				new TipoRubrica { TipoRubricaId = 34, CsClasse = CsClasseRubrica.Administrativo, TipoProjetoId = 2, TxNome = "Ginástica Laboral", TipoPaiId = 25 },
				new TipoRubrica { TipoRubricaId = 35, CsClasse = CsClasseRubrica.Administrativo | CsClasseRubrica.Pai, TipoProjetoId = 2, TxNome = "Material de Consumo" },
				new TipoRubrica { TipoRubricaId = 36, CsClasse = CsClasseRubrica.Administrativo, TipoProjetoId = 2, TxNome = "Escritório", TipoPaiId = 35 },
				new TipoRubrica { TipoRubricaId = 37, CsClasse = CsClasseRubrica.Administrativo, TipoProjetoId = 2, TxNome = "Limpeza", TipoPaiId = 35 },
				new TipoRubrica { TipoRubricaId = 38, CsClasse = CsClasseRubrica.Administrativo, TipoProjetoId = 2, TxNome = "Copa/Cozinha", TipoPaiId = 35 },
				new TipoRubrica { TipoRubricaId = 39, CsClasse = CsClasseRubrica.Administrativo, TipoProjetoId = 2, TxNome = "Prevenção para Segurança e Sinalização", TipoPaiId = 35 },
				new TipoRubrica { TipoRubricaId = 40, CsClasse = CsClasseRubrica.Administrativo, TipoProjetoId = 2, TxNome = "Manutenção", TipoPaiId = 35 },
				new TipoRubrica { TipoRubricaId = 41, CsClasse = CsClasseRubrica.Administrativo, TipoProjetoId = 2, TxNome = "Informática", TipoPaiId = 35 },
				new TipoRubrica { TipoRubricaId = 42, CsClasse = CsClasseRubrica.Administrativo, TipoProjetoId = 2, TxNome = "Material de Jardinagem", TipoPaiId = 35 },
				new TipoRubrica { TipoRubricaId = 43, CsClasse = CsClasseRubrica.Administrativo, TipoProjetoId = 2, TxNome = "Enfermaria", TipoPaiId = 35 },
				new TipoRubrica { TipoRubricaId = 44, CsClasse = CsClasseRubrica.Administrativo, TipoProjetoId = 2, TxNome = "Uniformes", TipoPaiId = 35 },
				new TipoRubrica { TipoRubricaId = 45, CsClasse = CsClasseRubrica.Administrativo | CsClasseRubrica.Pai, TipoProjetoId = 2, TxNome = "TI e Comunicação" },
				new TipoRubrica { TipoRubricaId = 46, CsClasse = CsClasseRubrica.Administrativo, TipoProjetoId = 2, TxNome = "Telefone", TipoPaiId = 45 },
				new TipoRubrica { TipoRubricaId = 47, CsClasse = CsClasseRubrica.Administrativo, TipoProjetoId = 2, TxNome = "Comunicação", TipoPaiId = 45 },
				new TipoRubrica { TipoRubricaId = 48, CsClasse = CsClasseRubrica.Administrativo | CsClasseRubrica.Pai, TipoProjetoId = 2, TxNome = "Serviços de Manut. Predial" },
				new TipoRubrica { TipoRubricaId = 49, CsClasse = CsClasseRubrica.Administrativo, TipoProjetoId = 2, TxNome = "Limpeza e Conservação", TipoPaiId = 48 },
				new TipoRubrica { TipoRubricaId = 50, CsClasse = CsClasseRubrica.Administrativo, TipoProjetoId = 2, TxNome = "Segurança Física d Patrimonial", TipoPaiId = 48 },
				new TipoRubrica { TipoRubricaId = 51, CsClasse = CsClasseRubrica.Administrativo, TipoProjetoId = 2, TxNome = "Jardinagem", TipoPaiId = 48 },
				new TipoRubrica { TipoRubricaId = 52, CsClasse = CsClasseRubrica.Administrativo, TipoProjetoId = 2, TxNome = "Informática", TipoPaiId = 48 },
				new TipoRubrica { TipoRubricaId = 53, CsClasse = CsClasseRubrica.Administrativo, TipoProjetoId = 2, TxNome = "Manutenção de Máquinas e Eqptos", TipoPaiId = 48 },
				new TipoRubrica { TipoRubricaId = 54, CsClasse = CsClasseRubrica.Administrativo, TipoProjetoId = 2, TxNome = "Conservação e reformas", TipoPaiId = 48 },
				new TipoRubrica { TipoRubricaId = 55, CsClasse = CsClasseRubrica.Administrativo | CsClasseRubrica.Pai, TipoProjetoId = 2, TxNome = "Assessoria e Consultoria" },
				new TipoRubrica { TipoRubricaId = 56, CsClasse = CsClasseRubrica.Administrativo, TipoProjetoId = 2, TxNome = "Consultoria PJ", TipoPaiId = 55 },
				new TipoRubrica { TipoRubricaId = 57, CsClasse = CsClasseRubrica.Administrativo, TipoProjetoId = 2, TxNome = "Treinamentos Funcionários", TipoPaiId = 55 },
				new TipoRubrica { TipoRubricaId = 58, CsClasse = CsClasseRubrica.Administrativo, TipoProjetoId = 2, TxNome = "Outros", TipoPaiId = 55 },
				new TipoRubrica { TipoRubricaId = 59, CsClasse = CsClasseRubrica.Administrativo | CsClasseRubrica.Pai, TipoProjetoId = 2, TxNome = "Despesas Gerais" },
				new TipoRubrica { TipoRubricaId = 60, CsClasse = CsClasseRubrica.Administrativo, TipoProjetoId = 2, TxNome = "Serviços Públicos", TipoPaiId = 59 },
				new TipoRubrica { TipoRubricaId = 61, CsClasse = CsClasseRubrica.Administrativo, TipoProjetoId = 2, TxNome = "Aquisição/Locação Máq. Equiptos.", TipoPaiId = 59 },
				new TipoRubrica { TipoRubricaId = 62, CsClasse = CsClasseRubrica.Administrativo, TipoProjetoId = 2, TxNome = "Assinaturas Livros/Revistas/Jornais", TipoPaiId = 59 },
				new TipoRubrica { TipoRubricaId = 63, CsClasse = CsClasseRubrica.Administrativo, TipoProjetoId = 2, TxNome = "Cópias/Reprográficas", TipoPaiId = 59 },
				new TipoRubrica { TipoRubricaId = 64, CsClasse = CsClasseRubrica.Administrativo, TipoProjetoId = 2, TxNome = "Conduções (Taxi)", TipoPaiId = 59 },
				new TipoRubrica { TipoRubricaId = 65, CsClasse = CsClasseRubrica.Administrativo, TipoProjetoId = 2, TxNome = "Lanches/Refeições", TipoPaiId = 59 },
				new TipoRubrica { TipoRubricaId = 66, CsClasse = CsClasseRubrica.Administrativo, TipoProjetoId = 2, TxNome = "Taxas Banc/Financ.", TipoPaiId = 59 },
				new TipoRubrica { TipoRubricaId = 67, CsClasse = CsClasseRubrica.Administrativo, TipoProjetoId = 2, TxNome = "Parcelamento de Impostos", TipoPaiId = 59 },
				new TipoRubrica { TipoRubricaId = 68, CsClasse = CsClasseRubrica.Administrativo, TipoProjetoId = 2, TxNome = "Correios", TipoPaiId = 59 },
				new TipoRubrica { TipoRubricaId = 69, CsClasse = CsClasseRubrica.Administrativo, TipoProjetoId = 2, TxNome = "Seguro Predial", TipoPaiId = 59 },
				new TipoRubrica { TipoRubricaId = 70, CsClasse = CsClasseRubrica.Administrativo | CsClasseRubrica.Pai, TipoProjetoId = 2, TxNome = "Despesas de Viagens e Hospedagens" },
				new TipoRubrica { TipoRubricaId = 71, CsClasse = CsClasseRubrica.Administrativo, TipoProjetoId = 2, TxNome = "Viagens e hospedagens", TipoPaiId = 70 },
				new TipoRubrica { TipoRubricaId = 72, CsClasse = CsClasseRubrica.Administrativo | CsClasseRubrica.Pai, TipoProjetoId = 2, TxNome = "Despesas MKT/ENDOMKT" },
				new TipoRubrica { TipoRubricaId = 73, CsClasse = CsClasseRubrica.Administrativo, TipoProjetoId = 2, TxNome = "Eventos", TipoPaiId = 72 },
				new TipoRubrica { TipoRubricaId = 74, CsClasse = CsClasseRubrica.Administrativo, TipoProjetoId = 2, TxNome = "Brindes/homenagens", TipoPaiId = 72 },

				/* Projeto sem Patrocínio | Projeto Base */
				new TipoRubrica { TipoRubricaId = 75, CsClasse = CsClasseRubrica.Desenvolvimento, TipoProjetoId = 3, TxNome = "Viagens e Deslocamentos" },
				new TipoRubrica { TipoRubricaId = 76, CsClasse = CsClasseRubrica.Desenvolvimento, TipoProjetoId = 3, TxNome = "Contratações (fora de Manaus)" },
				new TipoRubrica { TipoRubricaId = 77, CsClasse = CsClasseRubrica.Desenvolvimento, TipoProjetoId = 3, TxNome = "Treinamentos" },
				new TipoRubrica { TipoRubricaId = 78, CsClasse = CsClasseRubrica.Desenvolvimento, TipoProjetoId = 3, TxNome = "Manutenção (após entrega)" },
				new TipoRubrica { TipoRubricaId = 79, CsClasse = CsClasseRubrica.Desenvolvimento, TipoProjetoId = 3, TxNome = "Livros e Periódicos" },
				new TipoRubrica { TipoRubricaId = 80, CsClasse = CsClasseRubrica.Desenvolvimento, TipoProjetoId = 3, TxNome = "Software" },
				new TipoRubrica { TipoRubricaId = 81, CsClasse = CsClasseRubrica.Desenvolvimento, TipoProjetoId = 3, TxNome = "Hardware" },
				new TipoRubrica { TipoRubricaId = 82, CsClasse = CsClasseRubrica.Desenvolvimento, TipoProjetoId = 3, TxNome = "Obras de Infra-Estrutura" },
				new TipoRubrica { TipoRubricaId = 83, CsClasse = CsClasseRubrica.Desenvolvimento, TipoProjetoId = 3, TxNome = "Terceiros" },
				new TipoRubrica { TipoRubricaId = 84, CsClasse = CsClasseRubrica.Desenvolvimento | CsClasseRubrica.RecursosHumanos, TipoProjetoId = 3, TxNome = "RH GDC" },
				new TipoRubrica { TipoRubricaId = 85, CsClasse = CsClasseRubrica.Desenvolvimento | CsClasseRubrica.RecursosHumanos, TipoProjetoId = 3, TxNome = "RH TI" },
				new TipoRubrica { TipoRubricaId = 86, CsClasse = CsClasseRubrica.Desenvolvimento | CsClasseRubrica.RecursosHumanos, TipoProjetoId = 3, TxNome = "RH Designer" },
				new TipoRubrica { TipoRubricaId = 87, CsClasse = CsClasseRubrica.Desenvolvimento | CsClasseRubrica.RecursosHumanos, TipoProjetoId = 3, TxNome = "RH Testes" },
				new TipoRubrica { TipoRubricaId = 88, CsClasse = CsClasseRubrica.Desenvolvimento | CsClasseRubrica.RecursosHumanos, TipoProjetoId = 3, TxNome = "RH Qualidade" },
				new TipoRubrica { TipoRubricaId = 89, CsClasse = CsClasseRubrica.Desenvolvimento | CsClasseRubrica.RecursosHumanos, TipoProjetoId = 3, TxNome = "Mão de Obra Direta (Exclusiva)" },
				new TipoRubrica { TipoRubricaId = 90, CsClasse = CsClasseRubrica.Administrativo, TipoProjetoId = 3, TxNome = "Custo Fixo FPF (Rateio)" },
				new TipoRubrica { TipoRubricaId = 91, CsClasse = CsClasseRubrica.Administrativo, TipoProjetoId = 3, TxNome = "Depreciação de Equipamentos" },
				new TipoRubrica { TipoRubricaId = 92, CsClasse = CsClasseRubrica.Administrativo, TipoProjetoId = 3, TxNome = "Taxa de Administração" },
				new TipoRubrica { TipoRubricaId = 93, CsClasse = CsClasseRubrica.Administrativo, TipoProjetoId = 3, TxNome = "Impostos e Encargos" },
				new TipoRubrica { TipoRubricaId = 94, CsClasse = CsClasseRubrica.Administrativo, TipoProjetoId = 3, TxNome = "Apoio a Clientes" },
                new TipoRubrica { TipoRubricaId = 98, CsClasse = CsClasseRubrica.Administrativo, TipoProjetoId = 3, TxNome = "Apoio a Clientes 2" },
				new TipoRubrica { TipoRubricaId = 95, CsClasse = CsClasseRubrica.Administrativo, TipoProjetoId = 3, TxNome = "FACN" },
				new TipoRubrica { TipoRubricaId = 96, CsClasse = CsClasseRubrica.Aportes, TipoProjetoId = 3, TxNome = "Aportes" },
				new TipoRubrica { TipoRubricaId = 97, CsClasse = CsClasseRubrica.Desenvolvimento | CsClasseRubrica.RecursosHumanos, TipoProjetoId = 3, TxNome = "RH Direto (Dissídio)" }
			);

			var terceirosAdm = context.TiposRubrica.Find(18);

			if (terceirosAdm != null)
			{
				context.TiposRubrica.Remove(terceirosAdm);
			}

			context.Projetos.ToList().ForEach(delegate(Projeto projeto)
			{
				if (projeto.TipoProjetoId == null)
				{
					projeto.TipoProjetoId = 1;
				}
			});
        }
    }
}