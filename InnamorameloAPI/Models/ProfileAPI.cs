using AutoMapper;
using MongoDB.Bson;
using MongoDB.Driver;

namespace InnamorameloAPI.Models
{
    public class ProfileAPI
    {
        static private MongoAPI mongo = new MongoAPI();

        private ProfileDTO? GetProfile(FilterDefinition<ProfileMongoDB> filter)
        {
            try
            {
                IMongoDatabase innamoramelo = mongo.GetDatabase();
                IMongoCollection<ProfileMongoDB> profiles = innamoramelo.GetCollection<ProfileMongoDB>("Profiles");

                var find = profiles.Find(filter).FirstOrDefault();
                if(find != null)
                {
                    var profile = new ProfileDTO();
                    Validator.CopyProperties(find, profile);

                    return profile;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return null;
        }

        internal ProfileDTO? GetProfileById(string id)
        {
            try
            {
                FilterDefinition<ProfileMongoDB> filter = Builders<ProfileMongoDB>.Filter.Eq(x => x.Id, new ObjectId(id));
                var find = GetProfile(filter);

                return find;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                return null;
            }
        }

        internal ProfileDTO? GetProfileByUserId(string userId)
        {
            try
            {
                FilterDefinition<ProfileMongoDB> filter = Builders<ProfileMongoDB>.Filter.Eq(x => x.UserId, new ObjectId(userId));
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
                var profileInsert = new ProfileMongoDB();
                Validator.CopyProperties(profile, profileInsert);

                IMongoDatabase innamoramelo = mongo.GetDatabase();
                IMongoCollection<ProfileMongoDB> profiles = innamoramelo.GetCollection<ProfileMongoDB>("Profiles");

                profiles.InsertOne(profileInsert);

                var profileInserted = GetProfileByUserId(profile.UserId);
                return profileInserted;
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
                IMongoCollection<ProfileMongoDB> profiles = innamoramelo.GetCollection<ProfileMongoDB>("Profiles");

                var filter = Builders<ProfileMongoDB>.Filter.Eq(x => x.UserId, new ObjectId(profile.UserId));

                var updateDefinition = new List<UpdateDefinition<ProfileMongoDB>>();

                if (!string.IsNullOrEmpty(profile.Gender))
                    updateDefinition.Add(Builders<ProfileMongoDB>.Update.Set("Gender", profile.Gender));

                if (!string.IsNullOrEmpty(profile.SexualOrientation))
                    updateDefinition.Add(Builders<ProfileMongoDB>.Update.Set("SexualOrientation", profile.SexualOrientation));

                if (profile.LookingFor != null && profile.LookingFor.Length > 0)
                    updateDefinition.Add(Builders<ProfileMongoDB>.Update.Set("LookingFor", profile.LookingFor));

                if (!string.IsNullOrEmpty(profile.Education))
                    updateDefinition.Add(Builders<ProfileMongoDB>.Update.Set("Education", profile.Education));

                if (!string.IsNullOrEmpty(profile.Job))
                    updateDefinition.Add(Builders<ProfileMongoDB>.Update.Set("Job", profile.Job));

                if (!string.IsNullOrEmpty(profile.Bio))
                    updateDefinition.Add(Builders<ProfileMongoDB>.Update.Set("Bio", profile.Bio));

                if (profile.Passions != null)
                    updateDefinition.Add(Builders<ProfileMongoDB>.Update.Set("Passions", profile.Passions));

                if (profile.Location != null)
                    updateDefinition.Add(Builders<ProfileMongoDB>.Update.Set("Location", profile.Location));

                if (profile.RangeKm.HasValue)
                    updateDefinition.Add(Builders<ProfileMongoDB>.Update.Set("RangeKm", profile.RangeKm));

                var updateProfile = Builders<ProfileMongoDB>.Update.Combine(updateDefinition);
                var update = profiles.UpdateOne(filter, updateProfile);

                var profileUpdated = GetProfileByUserId(profile.UserId);
                return profileUpdated;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                return null;
            }
        }

        private bool DeleteProfile(FilterDefinition<ProfileMongoDB> filter)
        {
            try
            {
                IMongoDatabase innamoramelo = mongo.GetDatabase();
                IMongoCollection<ProfileMongoDB> profiles = innamoramelo.GetCollection<ProfileMongoDB>("Profiles");

                var delete = profiles.DeleteOne(filter);

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                return false;
            }
        }

        internal bool DeleteProfileById(string id)
        {
            try
            {
                var filter = Builders<ProfileMongoDB>.Filter.Eq(x => x.Id, new ObjectId(id));
                var delete = DeleteProfile(filter);

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                return false;
            }
        }

        internal bool DeleteProfileByUserId(string userId)
        {
            try
            {
                var filter = Builders<ProfileMongoDB>.Filter.Eq(x => x.UserId, new ObjectId(userId));
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
}
