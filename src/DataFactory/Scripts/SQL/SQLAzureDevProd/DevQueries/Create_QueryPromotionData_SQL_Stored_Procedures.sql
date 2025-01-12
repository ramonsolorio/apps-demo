IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND OBJECT_ID = OBJECT_ID('dbo.spGetStorePromotionData'))
	DROP PROCEDURE [dbo].[spGetStorePromotionData]
GO

CREATE PROCEDURE [dbo].[spGetStorePromotionData]
	 @LoadBatchId DECIMAL = 122176113,
	 @Location DECIMAL(10) = 924,
	 @JsonResult nvarchar(max) out
AS
BEGIN
	DECLARE @Json nvarchar(max) =
	(
		SELECT
		 JSON_QUERY((
			SELECT
				[LOAD_BATCH_ID],
				[LOCATION]
			FROM [dbo].[PROMOTION_HEADER] h
			WHERE h.[LOCATION] = @Location and h.[LOAD_BATCH_ID] = @LoadBatchId
			FOR JSON PATH,
				INCLUDE_NULL_VALUES
		)) AS [PROMOTION_HEADER],
		 JSON_QUERY((
			SELECT
				p.[STORE],
				p.[UPDATE_TYPE],
				p.[START_DATE],
				p.[END_DATE],
				p.[TIME],
				p.[END_TIME],
				p.[TRAN_TYPE],
				p.[ITEM],
				p.[ITEM_NUMBER_TYPE],
				p.[FORMAT_ID],
				p.[PREFIX],
				p.[REF_ITEM],
				p.[REF_ITEM_NUMBER_TYPE],
				p.[REF_FORMAT_ID],
				p.[REF_PREFIX],
				p.[ITEM_SHORT_DESC],
				p.[ITEM_LONG_DESC],
				p.[DEPT],
				p.[CLASS],
				p.[SUBCLASS],
				p.[NEW_PRICE],
				p.[NEW_SELLING_UOM],
				p.[NEW_MULTI_UNITS],
				p.[NEW_MULTI_UNIT_RETAIL],
				p.[NEW_MULTI_SELLING_UOM],
				p.[STATUS],
				p.[TAXABLE_IND],
				p.[PROMOTION],
				p.[MIX_MATCH_NO],
				p.[MIX_MATCH_TYPE],
				p.[THRESHOLD_NO],
				p.[LAUNCH_DATE],
				p.[QTY_KEY_OPTIONS],
				p.[MANUAL_PRICE_ENTRY],
				p.[DEPOSIT_CODE],
				p.[FOOD_STAMP_IND],
				p.[WIC_IND],
				p.[PROPORTIONAL_TARE_PCT],
				p.[FIXED_TARE_VALUE],
				p.[FIXED_TARE_UOM],
				p.[REWARD_ELIGIBLE_IND],
				p.[ELECT_MTK_CLUBS],
				p.[RETURN_POLICY],
				p.[STOP_SALE_IND],
				p.[RETURNABLE_IND],
				p.[REFUNDABLE_IND],
				p.[BACK_ORDER_IND],
				p.[VAT_CODE],
				p.[VAT_RATE],
				p.[CLASS_VAT_IND],
				p.[LINE_ID],
				p.[LOAD_TIMESTAMP],
				p.[LOAD_WEEK],
				p.[LOAD_BATCH_ID],
				p.[MSG_POS],
				p.[PROM_BIN_CODE],
				p.[PROM_NAME],
				d.[PROM_TRAN_TYPE],
				d.[START_TIME],
				d.[APPLY_TO],
				d.[BUY_TYPE],
				d.[BUY_AMT],
				d.[GET_TYPE],
				d.[GET_AMT],
				d.[SELLING_UOM],
				d.[THRESHOLD_AMT],
				d.[DISCOUNT_AMT],
				d.[DISCOUNT_TYPE]
			FROM [dbo].[PROMOTION_DATA] p
			JOIN [dbo].[PROMOTION_DATA_DETAILS] d ON p.[STORE] = d.STORE and p.[LOAD_BATCH_ID] = d.[LOAD_BATCH_ID] and p.[PROMOTION] = d.[PROMOTION]
			WHERE p.[LOAD_BATCH_ID] = @LoadBatchId AND p.[STORE] = @Location
			FOR JSON PATH,
				INCLUDE_NULL_VALUES
		)) AS [STORE_PROMOTION_DATA]
		FOR JSON PATH,
			INCLUDE_NULL_VALUES,
			WITHOUT_ARRAY_WRAPPER
	);
	SELECT @JsonResult = @Json;
END
