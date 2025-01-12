-- Must be run on master
select l.name as [login name],u.name as [user name] from sysusers u inner join sys.sql_logins l on u.sid=l.sid
