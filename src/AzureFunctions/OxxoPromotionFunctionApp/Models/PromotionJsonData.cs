using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OxxoPromotionFunctionApp
{
    public class StorePromotionData
    {
        public Promotiondata[] promotionData { get; set; }
    }

    public class Promotiondata
    {
        public float? BATCH_ID { get; set; }
        public float? LOCATION { get; set; }
        public string? ARCHIVO { get; set; }
        public DateTime? LOAD_DATE { get; set; }
        public float? LOAD_WEEK { get; set; }
        public float? LOAD_BATCH_ID { get; set; }
        public string? WM_PRM_STATUS { get; set; }
        public string? WM_TARGET_PRM { get; set; }
        public float? STORE { get; set; }
        public float? UPDATE_TYPE { get; set; }
        public DateTime? START_DATE { get; set; }
        public DateTime? END_DATE { get; set; }
        public DateTime? TIME { get; set; }
        public DateTime? END_TIME { get; set; }
        public float? TRAN_TYPE { get; set; }
        public string? ITEM { get; set; }
        public string? ITEM_NUMBER_TYPE { get; set; }
        public string? FORMAT_ID { get; set; }
        public float? PREFIX { get; set; }
        public string? REF_ITEM { get; set; }
        public string? REF_ITEM_NUMBER_TYPE { get; set; }
        public string? REF_FORMAT_ID { get; set; }
        public float? REF_PREFIX { get; set; }
        public string? ITEM_SHORT_DESC { get; set; }
        public string? ITEM_LONG_DESC { get; set; }
        public string? DEPT { get; set; }
        public string? CLASS { get; set; }
        public string? SUBCLASS { get; set; }
        public string? NEW_PRICE { get; set; }
        public string? NEW_SELLING_UOM { get; set; }
        public string? NEW_MULTI_UNITS { get; set; }
        public string? NEW_MULTI_UNIT_RETAIL { get; set; }
        public string? NEW_MULTI_SELLING_UOM { get; set; }
        public string? STATUS { get; set; }
        public string? TAXABLE_IND { get; set; }
        public float? PROMOTION { get; set; }
        public string? MIX_MATCH_NO { get; set; }
        public string? MIX_MATCH_TYPE { get; set; }
        public string? THRESHOLD_NO { get; set; }
        public string? LAUNCH_DATE { get; set; }
        public string? QTY_KEY_OPTIONS { get; set; }
        public string? MANUAL_PRICE_ENTRY { get; set; }
        public string? DEPOSIT_CODE { get; set; }
        public string? FOOD_STAMP_IND { get; set; }
        public string? WIC_IND { get; set; }
        public float? PROPORTIONAL_TARE_PCT { get; set; }
        public float? FIXED_TARE_VALUE { get; set; }
        public string? FIXED_TARE_UOM { get; set; }
        public string? REWARD_ELIGIBLE_IND { get; set; }
        public string? ELECT_MTK_CLUBS { get; set; }
        public string? RETURN_POLICY { get; set; }
        public string? STOP_SALE_IND { get; set; }
        public string? RETURNABLE_IND { get; set; }
        public string? REFUNDABLE_IND { get; set; }
        public string? BACK_ORDER_IND { get; set; }
        public string? VAT_CODE { get; set; }
        public float? VAT_RATE { get; set; }
        public string? CLASS_VAT_IND { get; set; }
        public float? LINE_ID { get; set; }
        public DateTime? LOAD_TIMESTAMP { get; set; }
        public string? MSG_POS { get; set; }
        public string? PROM_BIN_CODE { get; set; }
        public string? PROM_NAME { get; set; }
        public string? PROM_TRAN_TYPE { get; set; }
        public string? START_TIME { get; set; }
        public string? APPLY_TO { get; set; }
        public string? BUY_TYPE { get; set; }
        public string? BUY_AMT { get; set; }
        public string? GET_TYPE { get; set; }
        public string? GET_AMT { get; set; }
        public string? SELLING_UOM { get; set; }
        public string? THRESHOLD_AMT { get; set; }
        public string? DISCOUNT_AMT { get; set; }
        public string? DISCOUNT_TYPE { get; set; }
        public string? ORACLE_CR { get; set; }
        public string? ORACLE_CR_DESC { get; set; }
        public string? ORACLE_CR_SUPERIOR { get; set; }
        public string? ORACLE_CR_TYPE { get; set; }
        public string? ESTADO { get; set; }
        public float? RETEK_CR { get; set; }
        public float? RETEK_ASESOR { get; set; }
        public string? RETEK_ASESOR_NOMBRE { get; set; }
        public float? RETEK_DISTRITO { get; set; }
        public float? RETEK_PLAZA { get; set; }
        public string? RETEK_STATUS { get; set; }
        public string? SURH_CR { get; set; }
        public string? SURH_FLAG { get; set; }
        public float? CR_FLEX_VALUE_ID { get; set; }
        public string? ORACLE_EF { get; set; }
        public string? ORACLE_EF_DESC { get; set; }
        public float? EF_FLEX_VALUE_ID { get; set; }
        public string? ORACLE_CIA { get; set; }
        public string? ORACLE_CIA_DESC { get; set; }
        public float? CIA_FLEX_VALUE_ID { get; set; }
        public string? ID_ESTADO_FINANCIERO { get; set; }
        public float? ID_CENTRO_RESPONSABILIDAD { get; set; }
        public float? ID_COMPANIA { get; set; }
        public string? LEGACY_EF { get; set; }
        public string? LEGACY_CR { get; set; }
    }
}
