-- Articles:
--    https://stackoverflow.com/questions/74055585/restrict-user-to-certain-tables-in-sql
--    https://learn.microsoft.com/en-us/sql/t-sql/statements/grant-transact-sql?view=sql-server-ver16

-- Must be run on master
/*
DROP USER [rprado]
DROP USER [rhurtado]
DROP USER [elopez]
DROP USER [sojendiz]
DROP USER [jibarra]
DROP USER [jcarmona]
DROP USER [rhaddad]
DROP USER [dguajardo]
DROP USER [jaguirre]
DROP USER [qa1]
DROP USER [qa2]

ALTER LOGIN [jcarmona] DISABLE
SELECT session_id
FROM sys.dm_exec_sessions
WHERE login_name = 'jcarmona'
KILL 96
KILL 148
KILL 150
KILL 152
*/

-- Must be run on MDB_ERDD_PromotionCloud
--------------------------
-- Access to all tables --
--------------------------
CREATE USER [jibarra] FOR LOGIN [jibarra] WITH DEFAULT_SCHEMA=[dbo]
GRANT ALL ON [dbo].[CO_PRM_HEADP] TO [jibarra]
GRANT ALL ON [dbo].[CO_PRM_DCM_STS] TO [jibarra]
GRANT ALL ON [dbo].[CO_PRM_STR_DCM_CFG] TO [jibarra]
GRANT ALL ON [dbo].[CO_PRM_DAT] TO [jibarra]
GRANT ALL ON [dbo].[CO_PRM_DAT_DTL] TO [jibarra]
GRANT ALL ON [dbo].[CO_PRM_DAT_HDR] TO [jibarra]
GRANT ALL ON [dbo].[CO_PRM_PRCS] TO [jibarra]
GRANT ALL ON [dbo].[CO_ERDD_MCRS] TO [jibarra]
GRANT ALL ON [dbo].[CO_PRM_HEADD] TO [jibarra]
GRANT ALL ON[dbo].[CO_PRM_CONFIGD] TO [jibarra]

CREATE USER [jcarmona] FOR LOGIN [jcarmona] WITH DEFAULT_SCHEMA=[dbo]
GRANT ALL ON [dbo].[CO_PRM_HEADP] TO [jcarmona]
GRANT ALL ON [dbo].[CO_PRM_DCM_STS] TO [jcarmona]
GRANT ALL ON [dbo].[CO_PRM_STR_DCM_CFG] TO [jcarmona]
GRANT ALL ON [dbo].[CO_PRM_DAT] TO [jcarmona]
GRANT ALL ON [dbo].[CO_PRM_DAT_DTL] TO [jcarmona]
GRANT ALL ON [dbo].[CO_PRM_DAT_HDR] TO [jcarmona]
GRANT ALL ON [dbo].[CO_PRM_PRCS] TO [jcarmona]
GRANT ALL ON [dbo].[CO_ERDD_MCRS] TO [jcarmona]
GRANT ALL ON [dbo].[CO_PRM_HEADD] TO [jcarmona]
GRANT ALL ON[dbo].[CO_PRM_CONFIGD] TO [jcarmona]

CREATE USER [rhaddad] FOR LOGIN [rhaddad] WITH DEFAULT_SCHEMA=[dbo]
GRANT ALL ON [dbo].[CO_PRM_HEADP] TO [rhaddad]
GRANT ALL ON [dbo].[CO_PRM_DCM_STS] TO [rhaddad]
GRANT ALL ON [dbo].[CO_PRM_STR_DCM_CFG] TO [rhaddad]
GRANT ALL ON [dbo].[CO_PRM_DAT] TO [rhaddad]
GRANT ALL ON [dbo].[CO_PRM_DAT_DTL] TO [rhaddad]
GRANT ALL ON [dbo].[CO_PRM_DAT_HDR] TO [rhaddad]
GRANT ALL ON [dbo].[CO_PRM_PRCS] TO [rhaddad]
GRANT ALL ON [dbo].[CO_ERDD_MCRS] TO [rhaddad]
GRANT ALL ON [dbo].[CO_PRM_HEADD] TO [rhaddad]
GRANT ALL ON[dbo].[CO_PRM_CONFIGD] TO [rhaddad]

CREATE USER [dguajardo] FOR LOGIN [dguajardo] WITH DEFAULT_SCHEMA=[dbo]
GRANT ALL ON [dbo].[CO_PRM_HEADP] TO [dguajardo]
GRANT ALL ON [dbo].[CO_PRM_DCM_STS] TO [dguajardo]
GRANT ALL ON [dbo].[CO_PRM_STR_DCM_CFG] TO [dguajardo]
GRANT ALL ON [dbo].[CO_PRM_DAT] TO [dguajardo]
GRANT ALL ON [dbo].[CO_PRM_DAT_DTL] TO [dguajardo]
GRANT ALL ON [dbo].[CO_PRM_DAT_HDR] TO [dguajardo]
GRANT ALL ON [dbo].[CO_PRM_PRCS] TO [dguajardo]
GRANT ALL ON [dbo].[CO_ERDD_MCRS] TO [dguajardo]
GRANT ALL ON [dbo].[CO_PRM_HEADD] TO [dguajardo]
GRANT ALL ON[dbo].[CO_PRM_CONFIGD] TO [dguajardo]

CREATE USER [qa1] FOR LOGIN [qa1] WITH DEFAULT_SCHEMA=[dbo]
GRANT ALL ON [dbo].[CO_PRM_HEADP] TO [qa1]
GRANT ALL ON [dbo].[CO_PRM_DCM_STS] TO [qa1]
GRANT ALL ON [dbo].[CO_PRM_STR_DCM_CFG] TO [qa1]
GRANT ALL ON [dbo].[CO_PRM_DAT] TO [qa1]
GRANT ALL ON [dbo].[CO_PRM_DAT_DTL] TO [qa1]
GRANT ALL ON [dbo].[CO_PRM_DAT_HDR] TO [qa1]
GRANT ALL ON [dbo].[CO_PRM_PRCS] TO [qa1]
GRANT ALL ON [dbo].[CO_ERDD_MCRS] TO [qa1]
GRANT ALL ON [dbo].[CO_PRM_HEADD] TO [qa1]
GRANT ALL ON[dbo].[CO_PRM_CONFIGD] TO [qa1]

CREATE USER [qa2] FOR LOGIN [qa2] WITH DEFAULT_SCHEMA=[dbo]
GRANT ALL ON [dbo].[CO_PRM_HEADP] TO [qa2]
GRANT ALL ON [dbo].[CO_PRM_DCM_STS] TO [qa2]
GRANT ALL ON [dbo].[CO_PRM_STR_DCM_CFG] TO [qa2]
GRANT ALL ON [dbo].[CO_PRM_DAT] TO [qa2]
GRANT ALL ON [dbo].[CO_PRM_DAT_DTL] TO [qa2]
GRANT ALL ON [dbo].[CO_PRM_DAT_HDR] TO [qa2]
GRANT ALL ON [dbo].[CO_PRM_PRCS] TO [qa2]
GRANT ALL ON [dbo].[CO_ERDD_MCRS] TO [qa2]
GRANT ALL ON [dbo].[CO_PRM_HEADD] TO [qa2]
GRANT ALL ON[dbo].[CO_PRM_CONFIGD] TO [qa2]

-------------------------------------
-- Access to physical store tables --
-------------------------------------
CREATE USER [rprado] FOR LOGIN [rprado] WITH DEFAULT_SCHEMA=[dbo]
GRANT ALL ON [dbo].[CO_PRM_HEADP] TO [rprado]
GRANT ALL ON [dbo].[CO_PRM_HEADD] TO [rprado]

CREATE USER [rhurtado] FOR LOGIN [rhurtado] WITH DEFAULT_SCHEMA=[dbo]
GRANT ALL ON [dbo].[CO_PRM_HEADP] TO [rhurtado]
GRANT ALL ON [dbo].[CO_PRM_HEADD] TO [rhurtado]

CREATE USER [elopez] FOR LOGIN [elopez] WITH DEFAULT_SCHEMA=[dbo]
GRANT ALL ON [dbo].[CO_PRM_HEADP] TO [elopez]
GRANT ALL ON [dbo].[CO_PRM_HEADD] TO [elopez]

CREATE USER [sojendiz] FOR LOGIN [sojendiz] WITH DEFAULT_SCHEMA=[dbo]
GRANT ALL ON [dbo].[CO_PRM_HEADP] TO [sojendiz]
GRANT ALL ON [dbo].[CO_PRM_HEADD] TO [sojendiz]

CREATE USER [jaguirre] FOR LOGIN [jaguirre] WITH DEFAULT_SCHEMA=[dbo]
GRANT ALL ON [dbo].[CO_PRM_HEADP] TO [jaguirre]
GRANT ALL ON [dbo].[CO_PRM_HEADD] TO [jaguirre]

-- Must be run on MDB_ERDD_PromotionMockCloud
--------------------------
-- Access to all tables --
--------------------------
CREATE USER [jibarra] FOR LOGIN [jibarra] WITH DEFAULT_SCHEMA=[dbo]
GRANT ALL ON [dbo].[FEM_POS_MODS_PRM_STG] TO [jibarra]
GRANT ALL ON [dbo].[FEM_POS_PROM_DETAIL_STG] TO [jibarra]
GRANT ALL ON [dbo].[POS_PRM_HEAD] TO [jibarra]
GRANT ALL ON [dbo].[XXFC_MAESTRO_DE_CRS_V] TO [jibarra]

CREATE USER [rprado] FOR LOGIN [rprado] WITH DEFAULT_SCHEMA=[dbo]
GRANT ALL ON [dbo].[FEM_POS_MODS_PRM_STG] TO [rprado]
GRANT ALL ON [dbo].[FEM_POS_PROM_DETAIL_STG] TO [rprado]
GRANT ALL ON [dbo].[POS_PRM_HEAD] TO [rprado]
GRANT ALL ON [dbo].[XXFC_MAESTRO_DE_CRS_V] TO [rprado]

CREATE USER [qa1] FOR LOGIN [qa1] WITH DEFAULT_SCHEMA=[dbo]
GRANT ALL ON [dbo].[FEM_POS_MODS_PRM_STG] TO [qa1]
GRANT ALL ON [dbo].[FEM_POS_PROM_DETAIL_STG] TO [qa1]
GRANT ALL ON [dbo].[POS_PRM_HEAD] TO [qa1]
GRANT ALL ON [dbo].[XXFC_MAESTRO_DE_CRS_V] TO [qa1]

CREATE USER [qa2] FOR LOGIN [qa2] WITH DEFAULT_SCHEMA=[dbo]
GRANT ALL ON [dbo].[FEM_POS_MODS_PRM_STG] TO [qa2]
GRANT ALL ON [dbo].[FEM_POS_PROM_DETAIL_STG] TO [qa2]
GRANT ALL ON [dbo].[POS_PRM_HEAD] TO [qa2]
GRANT ALL ON [dbo].[XXFC_MAESTRO_DE_CRS_V] TO [qa2]
