IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND OBJECT_ID = OBJECT_ID('dbo.spGetXxfcMaestroDeCrsV'))
	DROP PROCEDURE [dbo].[spGetXxfcMaestroDeCrsV]
GO

CREATE PROCEDURE [dbo].[spGetXxfcMaestroDeCrsV]
	@Location DECIMAL = 924
AS
BEGIN
	SELECT *
	FROM [dbo].[XXFC_MAESTRO_DE_CRS_V]
	WHERE [RETEK_CR] = @Location
END
