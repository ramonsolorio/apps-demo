/*
IF OBJECT_ID (N'PROMOTION_HEADER_ARCHIVE', N'U') IS NOT NULL
BEGIN
  DROP TABLE [PROMOTION_HEADER_ARCHIVE] ;
END

IF OBJECT_ID (N'PROMOTION_DATA_ARCHIVE', N'U') IS NOT NULL
BEGIN
  DROP TABLE [PROMOTION_DATA_ARCHIVE] ;
END

IF OBJECT_ID (N'PROMOTION_DATA_DETAILS_ARCHIVE', N'U') IS NOT NULL
BEGIN
  DROP TABLE [PROMOTION_DATA_DETAILS_ARCHIVE] ;
END
*/

CREATE TABLE [dbo].[PROMOTION_HEADER_ARCHIVE](
	[ID] [int] IDENTITY(1,1) PRIMARY KEY,
	[ARCHIVED_DATE] [datetime2] CONSTRAINT PRMH_DATE DEFAULT GETDATE(),
	[LOAD_BATCH_ID] DECIMAL NULL,
	[LOCATION] DECIMAL NULL
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[PROMOTION_DATA_ARCHIVE](
	[ID] [int] IDENTITY(1,1) PRIMARY KEY,
	[ARCHIVED_DATE] [datetime2] CONSTRAINT PRMD_DATE DEFAULT GETDATE(),
	[STORE] DECIMAL(10) NULL,
	[UPDATE_TYPE] DECIMAL NULL,
	[START_DATE] [datetime2] NULL,
	[END_DATE] [datetime2] NULL,
	[TIME] [datetime2] NULL,
	[END_TIME] [datetime2] NULL,
	[TRAN_TYPE] DECIMAL(2) NULL,
	[ITEM] [nvarchar](25) NULL,
	[ITEM_NUMBER_TYPE] [nvarchar](6) NULL,
	[FORMAT_ID] [nvarchar](1) NULL,
	[PREFIX] DECIMAL(2) NULL,
	[REF_ITEM] [nvarchar](25) NULL,
	[REF_ITEM_NUMBER_TYPE] [nvarchar](6) NULL,
	[REF_FORMAT_ID] [nvarchar](1) NULL,
	[REF_PREFIX] DECIMAL(2) NULL,
	[ITEM_SHORT_DESC] [nvarchar](20) NULL,
	[ITEM_LONG_DESC] [nvarchar](100) NULL,
	[DEPT] DECIMAL(4) NULL,
	[CLASS] DECIMAL(4) NULL,
	[SUBCLASS] DECIMAL(4) NULL,
	[NEW_PRICE] DECIMAL(20,4) NULL,
	[NEW_SELLING_UOM] [nvarchar](4) NULL,
	[NEW_MULTI_UNITS] DECIMAL(12,4) NULL,
	[NEW_MULTI_UNIT_RETAIL] DECIMAL(12,4) NULL,
	[NEW_MULTI_SELLING_UOM] [nvarchar](4) NULL,
	[STATUS] [nvarchar](1) NULL,
	[TAXABLE_IND] [nvarchar](1) NULL,
	[PROMOTION] DECIMAL(10) NULL,
	[MIX_MATCH_NO] DECIMAL(10) NULL,
	[MIX_MATCH_TYPE] [nvarchar](1) NULL,
	[THRESHOLD_NO] DECIMAL(10) NULL,
	[LAUNCH_DATE] [nvarchar](8) NULL,
	[QTY_KEY_OPTIONS] [nvarchar](6) NULL,
	[MANUAL_PRICE_ENTRY] [nvarchar](6) NULL,
	[DEPOSIT_CODE] [nvarchar](6) NULL,
	[FOOD_STAMP_IND] [nvarchar](1) NULL,
	[WIC_IND] [nvarchar](2) NULL,
	[PROPORTIONAL_TARE_PCT] DECIMAL(12,4) NULL,
	[FIXED_TARE_VALUE] DECIMAL(12,4) NULL,
	[FIXED_TARE_UOM] [nvarchar](4) NULL,
	[REWARD_ELIGIBLE_IND] [nvarchar](1) NULL,
	[ELECT_MTK_CLUBS] [nvarchar](6) NULL,
	[RETURN_POLICY] [nvarchar](6) NULL,
	[STOP_SALE_IND] [nvarchar](1) NULL,
	[RETURNABLE_IND] [nvarchar](1) NULL,
	[REFUNDABLE_IND] [nvarchar](1) NULL,
	[BACK_ORDER_IND] [nvarchar](1) NULL,
	[VAT_CODE] [nvarchar](6) NULL,
	[VAT_RATE] DECIMAL(20,10) NULL,
	[CLASS_VAT_IND] [nvarchar](1) NULL,
	[LINE_ID] DECIMAL NULL,
	[LOAD_TIMESTAMP] [datetime2] NULL,
	[LOAD_WEEK] DECIMAL NULL,
	[LOAD_BATCH_ID] DECIMAL NULL,
	[MSG_POS] [nvarchar](38) NULL,
	[PROM_BIN_CODE] [nvarchar](11) NULL,
	[PROM_NAME] [nvarchar](40) NULL
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[PROMOTION_DATA_DETAILS_ARCHIVE](
	[ID] [int] IDENTITY(1,1) PRIMARY KEY,
	[ARCHIVED_DATE] [datetime2] CONSTRAINT PRMDD_DATE DEFAULT GETDATE(),
	[STORE] DECIMAL(10) NULL,
	[PROMOTION] DECIMAL(10) NULL,
	[PROM_TRAN_TYPE] [nvarchar](6) NULL,
	[START_TIME] [datetime2] NULL,
	[END_TIME] [datetime2] NULL,
	[MIX_MATCH_NO] DECIMAL(10) NULL,
	[THRESHOLD_NO] DECIMAL(10) NULL,
	[APPLY_TO] [nvarchar](1) NULL,
	[BUY_TYPE] [nvarchar](1) NULL,
	[BUY_AMT] DECIMAL(20,4) NULL,
	[GET_TYPE] [nvarchar](1) NULL,
	[GET_AMT] DECIMAL(20,4) NULL,
	[SELLING_UOM] [nvarchar](2) NULL,
	[THRESHOLD_AMT] DECIMAL(20,4) NULL,
	[DISCOUNT_AMT] DECIMAL(20,4) NULL,
	[DISCOUNT_TYPE] [nvarchar](1) NULL,
	[LOAD_TIMESTAMP] [datetime2] NULL,
	[LOAD_WEEK] DECIMAL NULL,
	[LOAD_BATCH_ID] DECIMAL NULL

) ON [PRIMARY]
GO

