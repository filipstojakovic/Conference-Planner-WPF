using System.ComponentModel;
using ConferenceApp.model.entity;
using MySql.Data.MySqlClient;

namespace ConferenceApp.model.dao;

public class UserRoleDao : BaseDao
{
    public void insertUserRole(User user, MySqlTransaction transaction)
    {
        if (user.Roles == null || user.Roles.Count == 0)
        {
            insertDefaultRole(user, transaction);
            return;
        }

        const string userHasRoleSql = @"
				INSERT INTO user_has_role (user_id, role_id) 
				VALUES (@userId, @roleId)";

        foreach (var role in user.Roles)
        {
            using (var command = new MySqlCommand(userHasRoleSql, connection, transaction))
            {
                command.Parameters.AddWithValue("@userId", user.Id);
                command.Parameters.AddWithValue("@roleId", role.Id);

                command.ExecuteNonQuery();
            }
        }
    }

    private void insertDefaultRole(User user, MySqlTransaction transaction)
    {
        RoleDao roleDao = new RoleDao();
        Role role = roleDao.findByName(UserRoleEnum.user.ToString());
        user.Roles = new BindingList<Role> { role };
        insertUserRole(user, transaction);
    }

    public void deleteUserRole(int? userId, MySqlTransaction transaction)
    {
        string deleteUserRoleSql = $"DELETE FROM user_has_role WHERE user_id = {userId}";
        using (var command = new MySqlCommand(deleteUserRoleSql, connection, transaction))
        {
            command.ExecuteNonQuery();
        }
    }
}