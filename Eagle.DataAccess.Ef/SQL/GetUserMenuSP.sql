USE [Kingofday_Eagle]
GO
/****** Object:  StoredProcedure [Auth].[GetUserMenu]    Script Date: 2019-12-23 15:18:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		M.Norouzi
-- Create date: 2018-07-01
-- Modified Date: 2019-05-26
-- Description:	Get User Role & Menu
-- =============================================
Alter   PROCEDURE [Auth].[GetUserMenu]
	@UserId AS UNIQUEIDENTIFIER
AS
BEGIN
	declare @rolIds as table(id int)
	insert into		@rolIds			select			RoleId
					from			[Auth].[UserInRole] uir
					inner Join		[Base].[User] u
					On				uir.UserId = u.UserId
					Where			u.UserId=@UserId
									And	u.IsActive = 1
	select			A.[ActionId], 
					A.ParentId,
					A.ShowInMenu,
					A.OrderPriority,
					A.ControllerName,
					A.ActionName,
					A.[Name],
					A.Icon,
					IsDefault = cast(AIR.IsDefault as bit),--CASE WHEN (SELECT TOP(1) ActionInRoleId FROM [Auth].[ActionInRole]  WHERE IsDefault = 1) IS NULL THEN cast(0 as bit) ELSE  cast(1 as bit) END
					RoleId=R.RoleId,
					RoleNameFa = R.[RoleNameFa],
					IsAction = CASE WHEN A.ControllerName IS NULL THEN cast(0 as bit) ELSE cast(1 as bit) END,
					[Actions] = (SELECT				DISTINCT 
													IsDefault = cast(AIR2.IsDefault as bit),
													ActionId = A2.ActionId,
													A2.ControllerName,
													A2.ActionName,
													A2.[Name],
													A2.OrderPriority,
													A2.ShowInMenu,
													A2.Icon, 
													R2.RoleId,
													R2.RoleNameFa
								From				[Auth].[Action] A2
								INNER JOIN			[Auth].[ActionInRole] AIR2	
								ON					A2.ActionId = AIR2.ActionId
								Inner Join			[Auth].[Role] R2
								On					AIR2.RoleId = R2.RoleId
								Where				R2.RoleId		in
													(Select			id					
													from			@rolIds)
													And				A2.ParentId = A.ActionId
								FOR JSON PATH)


					from			[Auth].[Action] A
					Inner Join		[Auth].[ActionInRole] AIR
					On				A.ActionId = AIR.ActionId
					Inner Join		[Auth].[Role] R
					On				AIR.RoleId = R.RoleId
					Where			R.RoleId		in
									(Select			id					
													from			@rolIds)
									AND				ParentId		Is Null

END