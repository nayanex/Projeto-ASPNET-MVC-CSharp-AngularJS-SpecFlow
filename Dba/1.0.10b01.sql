DBCC DBREINDEX ('FeriasPlanejamento')
DBCC DBREINDEX ('Colaborador')
DBCC DBREINDEX ('CasoTeste')
DBCC DBREINDEX ('SolicitacaoOrcamento')
DBCC DBREINDEX ('CicloDesenv')
DBCC DBREINDEX ('Requisito')
DBCC DBREINDEX ('Modulo')
DBCC DBREINDEX ('Estoria')

ALTER TABLE dbo.SolicitacaoOrcamento
ALTER COLUMN TxUltimoComentario nvarchar(max) NULL

ALTER TABLE dbo.SolicitacaoOrcamentoHistorico
ALTER COLUMN Comentario nvarchar(max) NULL