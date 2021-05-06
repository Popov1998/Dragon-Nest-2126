Execute this query

use DNMembership
go
EXEC sp_change_users_login 'Update_One', 'DragonNest', 'DragonNest'
go


use DNWorld
go
EXEC sp_change_users_login 'Update_One', 'DragonNest', 'DragonNest'
go


NOTICE! Please use sql 2019