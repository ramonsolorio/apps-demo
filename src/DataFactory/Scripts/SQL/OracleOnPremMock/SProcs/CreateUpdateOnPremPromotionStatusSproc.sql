IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND OBJECT_ID = OBJECT_ID('dbo.SP_UPDATE_ON_PREM_PROMOTION_STATUS'))
	DROP PROCEDURE [dbo].[SP_UPDATE_ON_PREM_PROMOTION_STATUS]
GO

CREATE PROCEDURE [dbo].[SP_UPDATE_ON_PREM_PROMOTION_STATUS]
	@BatchID DECIMAL = 182599,
	@Location DECIMAL = 924,
	@LoadWeek DECIMAL = 9,
	@LoadBatchID DECIMAL = 122176113,
	@CloudPrmStatus nvarchar(1),
	@WmTargetPrm nvarchar(255)
AS
BEGIN
	UPDATE [dbo].[POS_PRM_HEAD]
	SET [CLOUD_PRM_STATUS] = @CloudPrmStatus,
		[WM_TARGET_PRM] = @WmTargetPrm
	WHERE [BATCH_ID] = @BatchID and [LOCATION] = @Location and [LOAD_WEEK] = @LoadWeek and [LOAD_BATCH_ID] = @LoadBatchID
END
