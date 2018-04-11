DELETE FROM [dbo].[ColaboradorPeriodoAquisitivo]
WHERE Colaborador = '<id_colaborador>'

DELETE FROM [dbo].[Colaborador]
WHERE Oid = '<id_colaborador>'

DELETE FROM [dbo].[UserUsers_RoleRoles]
WHERE Users = '<id_user>'

DELETE FROM [dbo].[User]
WHERE Oid = '<id_user>'