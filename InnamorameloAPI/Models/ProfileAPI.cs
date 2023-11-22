using AutoMapper;
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

        private ProfileDTO? GetProfile(FilterDefinition<ProfileAPI> filter)
        {
            try
            {
                IMongoDatabase innamoramelo = mongo.GetDatabase();
                IMongoCollection<ProfileAPI> profiles = innamoramelo.GetCollection<ProfileAPI>("Profiles");

                var find = profiles.Find(filter).FirstOrDefault();

                var config = new MapperConfiguration(cfg => {
                    cfg.CreateMap<ProfileAPI, ProfileDTO>();
                });

                IMapper mapper = config.CreateMapper();

                var profile = mapper.Map<ProfileDTO>(find);

                return profile;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                return null;
            }
        }

        internal ProfileDTO? GetProfileById(ObjectId id)
        {
            try
            {
                FilterDefinition<ProfileAPI> filter = Builders<ProfileAPI>.Filter.Eq(x => x.Id, id);
                var find = GetProfile(filter);

                return find;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                return null;
            }
        }

        internal ProfileDTO? GetProfileByUserId(ObjectId userId)
        {
            try
            {
                FilterDefinition<ProfileAPI> filter = Builders<ProfileAPI>.Filter.Eq(x => x.UserId, userId);
                var find = GetProfile(filter);

                return find;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                return null;
            }
        }

        internal ProfileDTO? InsertProfile(ProfileDTO profile)
        {
            try
            {
                IMongoDatabase innamoramelo = mongo.GetDatabase();
                IMongoCollection<ProfileDTO> profiles = innamoramelo.GetCollection<ProfileDTO>("Profiles");

                profiles.InsertOne(profile);

                return profile;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                return null;
            }
        }

        internal ProfileDTO? UpdateProfile(ProfileDTO profile)
        {
            try
            {
                IMongoDatabase innamoramelo = mongo.GetDatabase();
                IMongoCollection<ProfileDTO> profiles = innamoramelo.GetCollection<ProfileDTO>("Profiles");

                var filter = Builders<ProfileDTO>.Filter.Eq(x => x.Id, profile.Id);

                var updateDefinition = new List<UpdateDefinition<ProfileDTO>>();

                if (!string.IsNullOrEmpty(profile.Gender))
                    updateDefinition.Add(Builders<ProfileDTO>.Update.Set("Gender", profile.Gender));

                if (!string.IsNullOrEmpty(profile.SexualOrientation))
                    updateDefinition.Add(Builders<ProfileDTO>.Update.Set("SexualOrientation", profile.SexualOrientation));

                if (!string.IsNullOrEmpty(profile.LookingFor))
                    updateDefinition.Add(Builders<ProfileDTO>.Update.Set("LookingFor", profile.LookingFor));

                if (!string.IsNullOrEmpty(profile.School))
                    updateDefinition.Add(Builders<ProfileDTO>.Update.Set("School", profile.School));

                if (!string.IsNullOrEmpty(profile.Work))
                    updateDefinition.Add(Builders<ProfileDTO>.Update.Set("Work", profile.Work));

                if (!string.IsNullOrEmpty(profile.Bio))
                    updateDefinition.Add(Builders<ProfileDTO>.Update.Set("Bio", profile.Bio));

                if (profile.Passions != null)
                    updateDefinition.Add(Builders<ProfileDTO>.Update.Set("Passions", profile.Passions));

                if (profile.Location != null)
                    updateDefinition.Add(Builders<ProfileDTO>.Update.Set("Location", profile.Location));

                if (profile.Photos != null)
                    updateDefinition.Add(Builders<ProfileDTO>.Update.Set("Photos", profile.Photos));

                if (profile.RangeKm.HasValue)
                    updateDefinition.Add(Builders<ProfileDTO>.Update.Set("RangeKm", profile.RangeKm));

                var updateProfile = Builders<ProfileDTO>.Update.Combine(updateDefinition);
                var update = profiles.UpdateOne(filter, updateProfile);

                var profileUpdated = GetProfileById(profile.Id);
                return profileUpdated;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                return null;
            }
        }

        private bool DeleteProfile(FilterDefinition<ProfileAPI> filter)
        {
            try
            {
                IMongoDatabase innamoramelo = mongo.GetDatabase();
                IMongoCollection<ProfileAPI> profiles = innamoramelo.GetCollection<ProfileAPI>("Profiles");

                var delete = profiles.DeleteOne(filter);

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                return false;
            }
        }

        internal bool DeleteProfileById(ObjectId id)
        {
            try
            {
                var filter = Builders<ProfileAPI>.Filter.Eq(x => x.Id, id);
                var delete = DeleteProfile(filter);

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                return false;
            }
        }

        internal bool DeleteProfileByUserId(ObjectId userId)
        {
            try
            {
                var filter = Builders<ProfileAPI>.Filter.Eq(x => x.UserId, userId);
                var delete = DeleteProfile(filter);

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                return false;
            }
        }
    }

    public class ProfileDTO
    {
        public ObjectId Id { get; set; }
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
    }
}
