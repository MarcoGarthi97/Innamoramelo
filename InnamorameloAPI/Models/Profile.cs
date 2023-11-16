using MongoDB.Bson;
using MongoDB.Driver;
using Newtonsoft.Json;

namespace InnamorameloAPI.Models
{
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

        static private Mongo mongo = new Mongo();

        internal Profile? GetProfile(ObjectId id, int type = 0)
        {
            try
            {
                IMongoDatabase innamoramelo = mongo.GetDatabase();
                IMongoCollection<Profile> profiles = innamoramelo.GetCollection<Profile>("Profiles");

                FilterDefinition<Profile> filter;

                if (type == 0)
                    filter = Builders<Profile>.Filter.Eq(x => x.Id, id);
                else if (type == 1)
                    filter = Builders<Profile>.Filter.Eq(x => x.UserId, id);
                else
                    filter = Builders<Profile>.Filter.Empty;

                var find = profiles.Find(filter).FirstOrDefault();

                return find;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                return null;
            }
        }

        internal bool InsertProfile(Profile profile)
        {
            try
            {
                IMongoDatabase innamoramelo = mongo.GetDatabase();
                IMongoCollection<Profile> profiles = innamoramelo.GetCollection<Profile>("Profiles");

                profiles.InsertOne(profile);

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                return false;
            }
        }

        internal bool UpdateProfile(Profile profile)
        {
            try
            {
                IMongoDatabase innamoramelo = mongo.GetDatabase();
                IMongoCollection<Profile> profiles = innamoramelo.GetCollection<Profile>("Profiles");

                var filter = Builders<Profile>.Filter.Eq(x => x.Id, profile.Id);

                var updateDefinition = new List<UpdateDefinition<Profile>>();

                if (!string.IsNullOrEmpty(profile.Gender))
                    updateDefinition.Add(Builders<Profile>.Update.Set("Gender", profile.Gender));

                if (!string.IsNullOrEmpty(profile.SexualOrientation))
                    updateDefinition.Add(Builders<Profile>.Update.Set("SexualOrientation", profile.SexualOrientation));

                if (!string.IsNullOrEmpty(profile.LookingFor))
                    updateDefinition.Add(Builders<Profile>.Update.Set("LookingFor", profile.LookingFor));

                if (!string.IsNullOrEmpty(profile.School))
                    updateDefinition.Add(Builders<Profile>.Update.Set("School", profile.School));

                if (!string.IsNullOrEmpty(profile.Work))
                    updateDefinition.Add(Builders<Profile>.Update.Set("Work", profile.Work));

                if (!string.IsNullOrEmpty(profile.Bio))
                    updateDefinition.Add(Builders<Profile>.Update.Set("Bio", profile.Bio));

                if (profile.Passions != null)
                    updateDefinition.Add(Builders<Profile>.Update.Set("Passions", profile.Passions));

                if (profile.Location != null)
                    updateDefinition.Add(Builders<Profile>.Update.Set("Location", profile.Location));

                if (profile.Photos != null)
                    updateDefinition.Add(Builders<Profile>.Update.Set("Photos", profile.Photos));

                if (profile.RangeKm.HasValue)
                    updateDefinition.Add(Builders<Profile>.Update.Set("RangeKm", profile.RangeKm));

                var update = Builders<Profile>.Update.Combine(updateDefinition);

                var updateResult = profiles.UpdateOne(filter, update);

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                return false;
            }
        }

        internal bool DeleteProfile(ObjectId id)
        {
            try
            {
                IMongoDatabase innamoramelo = mongo.GetDatabase();
                IMongoCollection<Profile> profiles = innamoramelo.GetCollection<Profile>("Profiles");

                var filter = Builders<Profile>.Filter.Eq(x => x.Id, id);

                var delete = profiles.DeleteOne(filter);

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                return false;
            }
        }
    }
}
