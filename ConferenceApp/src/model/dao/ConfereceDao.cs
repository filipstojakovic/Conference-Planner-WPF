namespace ConferenceApp.model.dao
{
	public class ConfereceDao
	{
		//TODO: maybe add transation logic in BaseDao

		//public Conference insertConference(Conference conference)
		//{
		//	using (SqlCommand geatheringCommand = new SqlCommand("INSERT INTO geathering (`description`) VALUES (@description); SELECT SCOPE_IDENTITY();", connection))
		//	{

		//	}
		//	geatheringCommand.Parameters.AddWithValue("@description", "Conference on AI and Machine Learning");
		//	int geatheringId = Convert.ToInt32(geatheringCommand.ExecuteScalar());

		//	// Insert a new conference record that's associated with the new geathering record
		//	SqlCommand conferenceCommand = new SqlCommand("INSERT INTO `conferencedb`.`conference` (`geathering_id`, `start_date`, `end_date`, `name`, `description`) VALUES (@geatheringId, @startDate, @endDate, @name, @description)", connection);
		//	conferenceCommand.Parameters.AddWithValue("@geatheringId", geatheringId);
		//	conferenceCommand.Parameters.AddWithValue("@startDate", new DateTime(2023, 5, 1, 9, 0, 0));
		//	conferenceCommand.Parameters.AddWithValue("@endDate", new DateTime(2023, 5, 3, 18, 0, 0));
		//	conferenceCommand.Parameters.AddWithValue("@name", "AI Conference");
		//	conferenceCommand.Parameters.AddWithValue("@description", "A conference on artificial intelligence and machine learning");
		//	conferenceCommand.ExecuteNonQuery();
		//	return null;
		//}
	}
}
