UPDATE [WexProject_Producao].[dbo].[Projeto]
   SET [CsSituacaoProjeto] = 1
 WHERE TxNome NOT IN ('ATC Control', 'CTI', 'Smart IDE', 'GIP', 'Deskmedia', 'PosInova', 'PosiEduca', 'Gestavi', 'Wifi Module', 'Android Toys', 'B2C', 'Localização', 'Time Industrial', 'Java Teambox', 'Test Tools')
GO

UPDATE [WexProject_Producao].[dbo].[Projeto]
   SET [CsSituacaoProjeto] = 0
 WHERE TxNome IN ('ATC Control', 'CTI', 'Smart IDE', 'GIP', 'Deskmedia', 'PosInova', 'PosiEduca', 'Gestavi', 'Wifi Module', 'Android Toys', 'B2C', 'Localização', 'Time Industrial', 'Java Teambox', 'Test Tools')
GO

SELECT * FROM [WexProject_Producao].[dbo].[Projeto]
 WHERE TxNome IN ('ATC Control',  'CTI', 'Smart IDE', 'GIP', 'Deskmedia', 'PosInova', 'PosiEduca', 'Gestavi', 'Wifi Module', 'Android Toys', 'B2C', 'Localização', 'Time Industrial', 'Java Teambox', 'Test Tools')
GO