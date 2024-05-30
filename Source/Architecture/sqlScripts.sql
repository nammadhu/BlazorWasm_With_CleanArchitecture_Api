--use [9katthe]
SELECT r.Name,ur.TenantId,t.Name,userid,RoleId,u.UserName  FROM [AspNetUserRoles] ur 
left join AspNetUsers u on ur.UserId=u.Id 
left join AspNetRoles r on ur.RoleId=r.Id
left join Tenants t on ur.TenantId=t.Id
--SELECT *  FROM [Tenants]
--SELECT *   FROM [AspNetRoles]
--SELECT *  FROM [AspNetUserRoles]
--select * from [AspNetUsers]

--update [AspNetUsers]
--set TenantId='b7384612-5768-445a-bf62-aeddd039cf9a' where id='b76c06e9-a767-4824-835f-8b4e86b52c90'

--select * FROM [AspNetRoleClaims]
--select * from [AspNetUserClaims]



--cleanup scripts
--complete cleanup scripts
select * from AspNetRoles
select * from AspNetRoleClaims
select * from AspNetUserClaims
select * from AspNetUserRoles
select * from AspNetUserTokens
--above has no data at all in vote system currently
--below has data,most danger... be cautious
select * FROM [dbo].[AspNetUsers]
select * from Votes
select * from VoteConstituencies--no need to delete data
select * from VoteSummaries
select * from VoteMessageSupportOpposes

--delete from VoteMessageSupportOpposes
--delete from VoteSummaries
--delete from Votes
--delete FROM [dbo].[AspNetUsers]


