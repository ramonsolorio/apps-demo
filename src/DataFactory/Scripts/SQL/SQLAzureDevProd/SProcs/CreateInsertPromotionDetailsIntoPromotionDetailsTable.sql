IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND OBJECT_ID = OBJECT_ID('dbo.SP_INSERT_PROMOTION_DATA_INTO_PROMOTION_DETAILS'))
	DROP PROCEDURE [dbo].[SP_INSERT_PROMOTION_DATA_INTO_PROMOTION_DETAILS]
GO

CREATE PROCEDURE [dbo].[SP_INSERT_PROMOTION_DATA_INTO_PROMOTION_DETAILS]
	@PromotionJson nvarchar(MAX),
	@ForeignKeyId int
AS
BEGIN
	INSERT INTO [dbo].[CO_PRM_HEADD] WITH (ROWLOCK) (ID_PRM_HDR_OX,NMB_PRM,FO_PRM,CTGY_ITM,TYP_PRM,SRT_DT_PRM,FNS_DT_PRM,SKU,CTGY_PRM,TYP_SPR,QTY_REQT_PRM,DSC,TYP_DSC,OPT_PRM,DSCP_ITM,SUB_CTGY_ITM,SGMT_ITM,SUB_SGMT_ITM,DRN_CD,GP_PRM,PRCT_PRM,FS_OPTN_PRN,SND_PRM,TYP_PRM_AD,CD_PRM,MSG_PRM,FL,LCNT,ID_OPR_CRT,TM_CRT,ID_OPR_MDY,TM_MDY)
		SELECT
			@ForeignKeyId,
			NMB_PRM,
			FO_PRM,
			CTGY_ITM,
			TYP_PRM,
			SRT_DT_PRM,
			FNS_DT_PRM,
			SKU,
			CTGY_PRM,
			TYP_SPR,
			QTY_REQT_PRM,
			DSC,
			TYP_DSC,
			OPT_PRM,
			DSCP_ITM,
			SUB_CTGY_ITM,
			SGMT_ITM,
			SUB_SGMT_ITM,
			DRN_CD,
			GP_PRM,
			PRCT_PRM,
			FS_OPTN_PRN,
			SND_PRM,
			TYP_PRM_AD,
			CD_PRM,
			MSG_PRM,
			1 AS FL,
			0 AS LCNT,
			'276F47E8-98B7-4B28-9EE5-42E097FFBA7B' AS ID_OPR_CRT,
			GETDATE() AS TM_CRT,
			'276F47E8-98B7-4B28-9EE5-42E097FFBA7B' AS ID_OPR_MDY,
			'' AS TM_MDY
		FROM
			OPENJSON(@PromotionJson, '$.promotions')
		WITH (
			ID_PRM_HDR_OX int,
			NMB_PRM int '$.lineNumber',
			FO_PRM nvarchar(20) '$.folio',
			CTGY_ITM nvarchar(10) '$.itemCategory',
			TYP_PRM nvarchar(10) '$.type',
			SRT_DT_PRM datetime2(7) '$.startDate',
			FNS_DT_PRM datetime2(7) '$.endDate',
			SKU nvarchar(20) '$.itemSku',
			CTGY_PRM nvarchar(50) '$.category',
			TYP_SPR nvarchar(10) '$.providerType',
			QTY_REQT_PRM decimal(10, 3) '$.requiredQuantity',
			DSC decimal(10, 3) '$.discount',
			TYP_DSC nvarchar(10) '$.discountType',
			OPT_PRM int '$.optative',
			DSCP_ITM nvarchar(255) '$.description',
			SUB_CTGY_ITM nvarchar(50) '$.itemSubcategory',
			SGMT_ITM nvarchar(50) '$.itemSegment',
			SUB_SGMT_ITM nvarchar(50) '$.itemSubsegment',
			DRN_CD nvarchar(20) '$.durationCode',
			GP_PRM decimal(10, 3) '$.grouper',
			PRCT_PRM decimal(5, 2) '$.percent',
			FS_OPTN_PRN nvarchar(50) '$.optativeOne',
			SND_PRM nvarchar(50) '$.sent',
			TYP_PRM_AD nvarchar(10) '$.action',
			CD_PRM nvarchar(50) '$.binCode',
			MSG_PRM nvarchar(255) '$.messagePsg',
			FL bit,
			LCNT int,
			ID_OPR_CRT uniqueidentifier,
			TM_CRT datetime2(7),
			ID_OPR_MDY uniqueidentifier,
			TM_MDY datetime2(7)
		) 
END
