using ServerAPI;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;
using Newtonsoft.Json.Schema.Generation;

namespace Tests.SchemaTest
{
    [TestClass]
    public class SchemaTests
    {
        [TestMethod]
        public void Harmonization()
        {
            JSchemaGenerator generator = new();
            JSchema schema = new JSchema
            {
                Type = JSchemaType.Object,
                Properties =
                {
                    ["ChooseCandidateCommand"] = generator.Generate(typeof(ChooseCandidateCommand)),
                    ["MoreInfoCandidateCommand"] = generator.Generate(typeof(MoreInfoCandidateCommand)),
                    ["CandidateDTO"] = generator.Generate(typeof(CandidateDTO)),
                    ["UpdateCandidatesResponse"] = generator.Generate(typeof(UpdateCandidatesResponse)),
                    ["TimerResponse"] = generator.Generate(typeof(TimerResponse)),
                    ["CandidateInfoResponse"] = generator.Generate(typeof(CandidateInfoResponse)),
                }
            };

            string ProjectDir = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.Parent.FullName;
            //string ProjectDir = Directory.GetCurrentDirectory();
            string SchemaPath = Path.Combine(ProjectDir, "Schema.json");

            File.WriteAllText(SchemaPath, schema.ToString());

            JSchema loadedSchema = JSchema.Parse(File.ReadAllText(SchemaPath));
            var chooseCandidateCommand = JObject.FromObject(new ChooseCandidateCommand());
            var moreInfoCandidateCommand = JObject.FromObject(new MoreInfoCandidateCommand());
            var candidateDTO = JObject.FromObject(new CandidateDTO());
            var updateCandidatesResponse = JObject.FromObject(new UpdateCandidatesResponse());
            var timerResponse = JObject.FromObject(new TimerResponse());
            var candidateInfoResponse = JObject.FromObject(new CandidateInfoResponse());

            Assert.IsTrue(chooseCandidateCommand.IsValid(loadedSchema.Properties["ChooseCandidateCommand"]), "A property is invalid with respect to schema!");
            Assert.IsTrue(moreInfoCandidateCommand.IsValid(loadedSchema.Properties["MoreInfoCandidateCommand"]), "A property is invalid with respect to schema!");
            Assert.IsTrue(candidateDTO.IsValid(loadedSchema.Properties["CandidateDTO"]), "A property is invalid with respect to schema!");
            Assert.IsTrue(updateCandidatesResponse.IsValid(loadedSchema.Properties["UpdateCandidatesResponse"]), "A property is invalid with respect to schema!");
            Assert.IsTrue(timerResponse.IsValid(loadedSchema.Properties["TimerResponse"]), "A property is invalid with respect to schema!");
            Assert.IsTrue(candidateInfoResponse.IsValid(loadedSchema.Properties["CandidateInfoResponse"]), "A property is invalid with respect to schema!");
        }
    }
}
