using ConferenceApp.model.entity;

namespace ConferenceApp.model.dto;

public class UserDto
{
    public int? Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string Username { get; set; }
    public string conferenceRole { get; set; }

    public UserDto()
    {
    }

    public UserDto(User user, GatheringRoleEnum gr)
    {
        Id = user.Id;
        FirstName = user.FirstName;
        LastName = user.LastName;
        Email = user.Email;
        Username = user.Username;
        conferenceRole = gr.ToString();
    }
}