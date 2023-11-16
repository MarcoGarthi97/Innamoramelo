using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Innamoramelo.Models
{
    public class User
    {
        [BsonIgnoreIfDefault]
        [JsonConverter(typeof(ObjectIdConverter))]
        public ObjectId Id { get; set; }
        public string? Name { get; set; }
        public string? Phone { get; set; }
        public string? Password { get; set; }
        public string? Email { get; set; }
        public DateTime? Birthday { get; set; }
        public SecretCode? SecretCode { get; set; }
        public bool? IsActive { get; set; }
        public bool? CreateProfile { get; set; }

        public User() { }
        public User(bool? isActive)
        {
            IsActive = isActive;
        }
        public User(string? phone, string? password)
        {
            Phone = phone;
            Password = password;
        }
        public User(string? phone, string? password, string? email)
        {
            Phone = phone;
            Password = password;
            Email = email;
        }
    }

    public class SecretCode
    {
        public string? Code { get; set; }
        public DateTime? Created { get; set; }

        public SecretCode() { }
        public SecretCode(string? code, DateTime? created)
        {
            Code = code;
            Created = created;
        }
    }

    public class Profile
    {
        [JsonConverter(typeof(ObjectIdConverter))]
        public ObjectId Id { get; set; }
        [JsonConverter(typeof(ObjectIdConverter))]
        public ObjectId UserId { get; set; }
        public string? Gender { get; set; }
        public string? SexualOrientation { get; set; }
        public string? LookingFor { get; set; }
        public string? School { get; set; }
        public string? Work { get; set; }
        public string? Bio { get; set; }
        public List<string>? Passions { get; set; }
        public Location? Location { get; set; }
        public List<Photo>? Photos { get; set; }
        public int? RangeKm { get; set; }

        public Profile() { }
        public Profile(ObjectId userId, string? gender, string? sexualOrientation, string? lookingFor, string? school, string? work, string? bio, List<string>? passions, Location? location, List<Photo>? photos, int? rangeKm)
        {
            UserId = userId;
            Gender = gender;
            SexualOrientation = sexualOrientation;
            LookingFor = lookingFor;
            School = school;
            Work = work;
            Bio = bio;
            Passions = passions;
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
        [JsonConverter(typeof(ObjectIdConverter))]
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
        [JsonConverter(typeof(ObjectIdConverter))]
        public ObjectId UsernameId { get; set; }
        public bool? Like { get; set; }

        public Matches() { }
        public Matches(ObjectId usernameId, bool? like)
        {
            UsernameId = usernameId;
            Like = like;
        }
    }

    public class ChatInfos
    {
        [JsonConverter(typeof(ObjectIdConverter))]
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
        [JsonConverter(typeof(ObjectIdConverter))]
        public ObjectId UserId { get; set; }
        public string? Name { get; set; }
        public byte[]? Bytes { get; set; }

        public ChatInfosUser() { }
        public ChatInfosUser(ObjectId userId, string? name, byte[]? bytes)
        {
            UserId = userId;
            Name = name;
            Bytes = bytes;
        }
    }

    public class Chat
    {
        [JsonConverter(typeof(ObjectIdConverter))]
        public ObjectId Id { get; set; }
        [JsonConverter(typeof(ObjectIdConverter))]
        public ObjectId? SenderId { get; set; }
        [JsonConverter(typeof(ObjectIdConverter))]
        public ObjectId? ReceiverId { get; set; }
        public string? Content { get; set; }
        public DateTime? Timestamp { get; set; }

        public Chat() { }
        public Chat(ObjectId? senderId, ObjectId? receiverId)
        {
            SenderId = senderId;
            ReceiverId = receiverId;
        }

        public Chat(ObjectId? senderId, ObjectId? receiverId, string? content, DateTime? timestamp)
        {
            SenderId = senderId;
            ReceiverId = receiverId;
            Content = content;
            Timestamp = timestamp;
        }
    }

    public class Job
    {
        [JsonConverter(typeof(ObjectIdConverter))]
        public ObjectId Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
    }

    public class Municipality
    {
        public ObjectId id { get; set; }
        public string Name { get; set; }
        public string Province { get; set; }
        public string Region { get; set; }
        public string ProvinceTitle { get; set; }

        public Municipality() { }
        public Municipality(string name, string province, string region, string provinceTitle)
        {
            Name = name;
            Province = province;
            Region = region;
            ProvinceTitle = provinceTitle;
        }

    }
}

public class ObjectIdConverter : JsonConverter<ObjectId>
{
    public override ObjectId ReadJson(JsonReader reader, Type objectType, ObjectId existingValue, bool hasExistingValue, JsonSerializer serializer)
    {
        var token = JToken.Load(reader);
        if (token.Type == JTokenType.String)
        {
            return ObjectId.Parse(token.Value<string>());
        }
        return ObjectId.Empty;
    }

    public override void WriteJson(JsonWriter writer, ObjectId value, JsonSerializer serializer)
    {
        writer.WriteValue(value.ToString());
    }
}