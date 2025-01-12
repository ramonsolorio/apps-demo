SELECT *
FROM [dbo].[POS_PRM_HEAD] b
JOIN [dbo].[PROMOTION_DATA] a ON b.LOAD_BATCH_ID = a.LOAD_BATCH_ID
WHERE b.batch_id = 182599
AND a.load_batch_id = b.load_batch_id
AND a.load_week = b.load_week
AND b.LOCATION = 924
AND a.STORE = b.LOCATION
AND b.wm_prm_status = 'L'
ORDER BY a.load_batch_id, load_timestamp, line_id