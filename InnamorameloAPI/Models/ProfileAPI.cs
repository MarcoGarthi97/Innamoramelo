using MongoDB.Bson;
using MongoDB.Driver;
using Newtonsoft.Json;

namespace InnamorameloAPI.Models
{
    public class ProfileAPI
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
        public LocationAPI? Location { get; set; }
        public List<PhotoAPI>? Photos { get; set; }
        public int? RangeKm { get; set; }

        public ProfileAPI() { }
        public ProfileAPI(ObjectId userId, string? gender, string? sexualOrientation, string? lookingFor, string? school, string? work, string? bio, List<string>? passions, LocationAPI? location, List<PhotoAPI>? photos, int? rangeKm)
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

        static private MongoAPI mongo = new MongoAPI();

        internal ProfileAPI? GetProfile(ObjectId id, int type = 0)
        {
            try
            {
                IMongoDatabase innamoramelo = mongo.GetDatabase();
                IMongoCollection<ProfileAPI> profiles = innamoramelo.GetCollection<ProfileAPI>("Profiles");

                FilterDefinition<ProfileAPI> filter;

                if (type == 0)
                    filter = Builders<ProfileAPI>.Filter.Eq(x => x.Id, id);
                else if (type == 1)
                    filter = Builders<ProfileAPI>.Filter.Eq(x => x.UserId, id);
                else
                    filter = Builders<ProfileAPI>.Filter.Empty;

                var find = profiles.Find(filter).FirstOrDefault();

                return find;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                return null;
            }
        }

        internal bool InsertProfile(ProfileAPI profile)
        {
            try
            {
                IMongoDatabase innamoramelo = mongo.GetDatabase();
                IMongoCollection<ProfileAPI> profiles = innamoramelo.GetCollection<ProfileAPI>("Profiles");

                profiles.InsertOne(profile);

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                return false;
            }
        }

        internal bool UpdateProfile(ProfileAPI profile)
        {
            try
            {
                IMongoDatabase innamoramelo = mongo.GetDatabase();
                IMongoCollection<ProfileAPI> profiles = innamoramelo.GetCollection<ProfileAPI>("Profiles");

                var filter = Builders<ProfileAPI>.Filter.Eq(x => x.Id, profile.Id);

                var updateDefinition = new List<UpdateDefinition<ProfileAPI>>();

                if (!string.IsNullOrEmpty(profile.Gender))
                    updateDefinition.Add(Builders<ProfileAPI>.Update.Set("Gender", profile.Gender));

                if (!string.IsNullOrEmpty(profile.SexualOrientation))
                    updateDefinition.Add(Builders<ProfileAPI>.Update.Set("SexualOrientation", profile.SexualOrientation));

                if (!string.IsNullOrEmpty(profile.LookingFor))
                    updateDefinition.Add(Builders<ProfileAPI>.Update.Set("LookingFor", profile.LookingFor));

                if (!string.IsNullOrEmpty(profile.School))
                    updateDefinition.Add(Builders<ProfileAPI>.Update.Set("School", profile.School));

                if (!string.IsNullOrEmpty(profile.Work))
                    updateDefinition.Add(Builders<ProfileAPI>.Update.Set("Work", profile.Work));

                if (!string.IsNullOrEmpty(profile.Bio))
                    updateDefinition.Add(Builders<ProfileAPI>.Update.Set("Bio", profile.Bio));

                if (profile.Passions != null)
                    updateDefinition.Add(Builders<ProfileAPI>.Update.Set("Passions", profile.Passions));

                if (profile.Location != null)
                    updateDefinition.Add(Builders<ProfileAPI>.Update.Set("Location", profile.Location));

                if (profile.Photos != null)
                    updateDefinition.Add(Builders<ProfileAPI>.Update.Set("Photos", profile.Photos));

                if (profile.RangeKm.HasValue)
                    updateDefinition.Add(Builders<ProfileAPI>.Update.Set("RangeKm", profile.RangeKm));

                var update = Builders<ProfileAPI>.Update.Combine(updateDefinition);

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
                IMongoCollection<ProfileAPI> profiles = innamoramelo.GetCollection<ProfileAPI>("Profiles");

                var filter = Builders<ProfileAPI>.Filter.Eq(x => x.Id, id);

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
