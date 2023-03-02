using System;
using ConferenceApp.model.entity;
using ConferenceApp.src.model.entity;
using MySql.Data.MySqlClient;

namespace ConferenceApp.model.dao
{
	public class UserGeatheringRoleDao : BaseDao
	{
		private readonly UserDao userDao;
		private readonly ConferenceDao conferenceDao;
		private readonly GeatheringRoleDao geatheringRoleDao;

		public UserGeatheringRoleDao()
		{
			userDao = new UserDao();
			conferenceDao = new ConferenceDao();
			geatheringRoleDao = new GeatheringRoleDao();
		}

		public UserGeatheringRole insertUserConferenceConferenceRole(User user, Conference conference, GeatheringRoleEnum geatheringRoleEnum)
		{
			string userGeatheringRoleSql = @"
				INSERT INTO user_geathering_role (user_id, geathering_role_id, geathering_id) 
				VALUES (@userId, @geatheringRoleId, @geatheringId)";


			MySqlTransaction transaction = connection.BeginTransaction();
			GeatheringRole geatheringRole = geatheringRoleDao.findByName(geatheringRoleEnum.ToString());
			Conference inseredConference = conferenceDao.insertConference(conference, transaction);

			UserGeatheringRole userGeatheringRole = null;
			try
			{
				using (var command = new MySqlCommand(userGeatheringRoleSql, connection, transaction))
				{
					command.Parameters.AddWithValue("@userId", 1);//user.Id);
					command.Parameters.AddWithValue("@geatheringId", 1);// inseredConference.Id);
					command.Parameters.AddWithValue("@geatheringRoleId", geatheringRole.Id); //Fk

					command.ExecuteNonQuery();
				}

				transaction.Commit();
				userGeatheringRole = new UserGeatheringRole
				{
					UserId = user.Id,
					GeatheringId = inseredConference.Id,
					GeatheringRoleId = geatheringRole.Id
				};
			}
			catch (Exception ex)
			{
				transaction.Rollback();
				userGeatheringRole = null;
				//TODO: handle exception
			}


			return userGeatheringRole;
		}
	}
}
