using System;

namespace WexProject.BLL.Shared.Domains.Custos
{
    [Flags]
    public enum CsClasseRubrica
    {
        Desenvolvimento = 1,
        Administrativo = 2,
        Aportes = 4,
        Pai = 8,
        RecursosHumanos = 16,
        Tudo = Desenvolvimento | Administrativo | Aportes
    }
}