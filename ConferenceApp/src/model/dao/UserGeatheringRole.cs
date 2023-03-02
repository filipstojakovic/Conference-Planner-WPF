using System;
using ConferenceApp.model.entity;
using ConferenceApp.src.model.entity;
using MySql.Data.MySqlClient;

namespace ConferenceApp.model.dao
{
    public class UserGatheringRoleDao : BaseDao
    {
        private readonly UserDao userDao;
        private readonly ConferenceDao conferenceDao;
        private readonly GatheringRoleDao gatheringRoleDao;

        public UserGatheringRoleDao()
        {
            userDao = new UserDao();
            conferenceDao = new ConferenceDao();
            gatheringRoleDao = new GatheringRoleDao();
        }

        public UserGatheringRole insertUserConferenceConferenceRole(User user, Conference conference,
            GatheringRoleEnum gatheringRoleEnum)
        {
            const string userGatheringRoleSql = @"
				INSERT INTO user_geathering_role (user_id, geathering_role_id, geathering_id) 
				VALUES (@userId, @geatheringRoleId, @geatheringId)";

            MySqlTransaction transaction = connection.BeginTransaction();
            GatheringRole gatheringRole = gatheringRoleDao.findByName(gatheringRoleEnum.ToString());
            Conference insertedConference;
            if (conference.Id == null)
                insertedConference = conferenceDao.insertConference(conference, transaction);
            else
                insertedConference = conference;

            UserGatheringRole userGatheringRole = null;
            try
            {
                using (var command = new MySqlCommand(userGatheringRoleSql, connection, transaction))
                {
                    command.Parameters.AddWithValue("@userId", 1); //user.Id);
                    command.Parameters.AddWithValue("@geatheringId", 1); // inseredConference.Id); //TODO: typo gathering
                    command.Parameters.AddWithValue("@geatheringRoleId", gatheringRole.Id); //Fk

                    command.ExecuteNonQuery();
                }

                transaction.Commit();
                userGatheringRole = new UserGatheringRole
                {
                    UserId = user.Id,
                    GatheringId = insertedConference.Id,
                    GatheringRoleId = gatheringRole.Id
                };
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                userGatheringRole = null;
                //TODO: handle exception
            }


            return userGatheringRole;
        }
    }
}