execute as user = 'qa2'
select * from [dbo].[CO_PRM_DCM_STS]
REVERT;

execute as user = 'rprado'
select * from [dbo].[CO_PRM_DCM_STS]
REVERT;
