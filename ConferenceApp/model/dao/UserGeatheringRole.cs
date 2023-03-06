using System;
using ConferenceApp.model.entity;
using ConferenceApp.model.entity;
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
            GatheringRoleEnum gatheringRoleEnum, MySqlTransaction transaction)
        {
            const string userGatheringRoleSql = @"
				INSERT INTO user_gathering_role (user_id, gathering_role_id, gathering_id) 
				VALUES (@userId, @gatheringRoleId, @gatheringId)";

            GatheringRole gatheringRole = gatheringRoleDao.findByName(gatheringRoleEnum.ToString());
            Conference insertedConference = null;
            if (conference.Id == null)
                insertedConference = conferenceDao.insertConference(conference, transaction);
            else
                insertedConference = conference;

            UserGatheringRole userGatheringRole = null;
            using (var command = new MySqlCommand(userGatheringRoleSql, connection, transaction))
            {
                command.Parameters.AddWithValue("@userId", user.Id);
                command.Parameters.AddWithValue("@gatheringId", insertedConference.Id);
                command.Parameters.AddWithValue("@gatheringRoleId", gatheringRole.Id); //FK

                command.ExecuteNonQuery();
            }

            userGatheringRole = new UserGatheringRole
            {
                UserId = user.Id,
                GatheringId = insertedConference.Id,
                GatheringRoleId = gatheringRole.Id
            };

            return userGatheringRole;
        }
       
    }
}