using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Innamoramelo.Models
{
    public class User
    {
        [BsonIgnoreIfDefault]
        public ObjectId Id { get; set; }
        public string? Username { get; set; }
        public string? Password { get; set; }
        public string? Email { get; set; }

        public User() { }
        public User(string? username, string? password, string? email)
        {
            Username = username;
            Password = password;
            Email = email;
        }
    }

    public class Profile
    {
        [BsonIgnoreIfDefault]
        public ObjectId Id { get; set; }
        public ObjectId UserId { get; set; }
        public bool IsActive { get; set; }
        public string? Name { get; set; }
        public string? Gender { get; set; }
        public string? SexualOrientation { get; set; }
        public string? LookingFor { get; set; }
        public string? School { get; set; }
        public string? Work { get; set; }
        public string? Bio { get; set; }
        public List<string>? Passions { get; set; }
        public DateTime? Birthday { get; set; }
        public Location? Location { get; set; }
        public List<Photo>? Photos { get; set; }
        public int? RangeKm { get; set; }

        public Profile() { }
        public Profile(ObjectId userId, bool isActive, string? name, string? gender, string? sexualOrientation, string? lookingFor, string? school, string? work, string? bio, List<string>? passions, DateTime? birthday, Location? location, List<Photo>? photos, int? rangeKm)
        {
            UserId = userId;
            IsActive = isActive;
            Name = name;
            Gender = gender;
            SexualOrientation = sexualOrientation;
            LookingFor = lookingFor;
            School = school;
            Work = work;
            Bio = bio;
            Passions = passions;
            Birthday = birthday;
            Location = location;
            Photos = photos;
            RangeKm = rangeKm;
        }
    }

    public class Location
    {
        public double Latitude { get; set; }
        public double Longitude { get; set; }

        public Location() { }
        public Location(double latitude, double longitude)
        {
            Latitude = latitude;
            Longitude = longitude;
        }
    }

    public class Photo
    {
        public string? NameFile { get; set; }
        public string? Path { get; set; }
        public byte[]? Bytes { get; set; }

        public Photo() { }
        public Photo(string? nameFile, string? path, byte[]? bytes)
        {
            NameFile = nameFile;
            Path = path;
            Bytes = bytes;
        }
    }

    public class Match
    {
        [BsonIgnoreIfDefault]
        public ObjectId Id { get; set; }
        public List<Matches>? Matches { get; set; }
        public bool? IsMatch { get; set; }

        public Match() { }
        public Match(List<Matches>? matches, bool? isMatch)
        {
            Matches = matches;
            IsMatch = isMatch;
        }
    }

    public class Matches
    {
        public string? Username { get; set; }
        public bool Like { get; set; }

        public Matches() { }
        public Matches(string? username, bool like)
        {
            Username = username;
            Like = like;
        }
    }

    public class ChatInfos
    {
        [BsonIgnoreIfDefault]
        public ObjectId Id { get; set; }
        public bool IsActive { get; set; }
        public List<ChatInfosUser>? ChatInfosUsers { get; set; }

        public ChatInfos() { }
        public ChatInfos(bool isActive, List<ChatInfosUser>? chatInfosUsers)
        {
            IsActive = isActive;
            ChatInfosUsers = chatInfosUsers;
        }
    }

    public class ChatInfosUser
    {
        public string? Username { get; set; }
        public string? Name { get; set; }
        public byte[]? Bytes { get; set; }

        public ChatInfosUser() { }
        public ChatInfosUser(string? username, string? name, byte[]? bytes)
        {
            Username = username;
            Name = name;
            Bytes = bytes;
        }
    }

    public class Chat
    {
        [BsonIgnoreIfDefault]
        public ObjectId Id { get; set; }
        public string? SenderId { get; set; }
        public string? ReceiverId { get; set; }
        public string? Content { get; set; }
        public DateTime Timestamp { get; set; }

        public Chat() { }
        public Chat(string? senderId, string? receiverId, string? content, DateTime timestamp)
        {
            SenderId = senderId;
            ReceiverId = receiverId;
            Content = content;
            Timestamp = timestamp;
        }
    }
}
