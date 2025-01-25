using Newtonsoft.Json.Linq;
using SmallTests.Entities;
using System.Linq;

namespace SmallTests.Helpers.Serialization
{
    public static class SerializationValueDependenciesHelper
    {

        public static ValueDependencies WireUsingNewton(ValueDependencies input)
        {


            ValueDependencies deserialized = SerializationHelper.WireUsingNewton(input);

            if (deserialized != null || !deserialized.EntityDependencies.IsNullOfEmpty())
            {
                for (int i = 0; i < deserialized.EntityDependencies.Count; i++)
                {

                    var entityDependency = deserialized.EntityDependencies[i];

                    if (entityDependency.Ids.Count == 0) continue; //No Ids, this should not happen

                    var id = entityDependency.Ids[0] as JObject;


                    if (id != null && id.Children<JProperty>().Count() > 1) //Composite key, turn it into ValueTuple or Compoite Data
                    {

                        var newIds = entityDependency.Ids.Select(id =>
                        {
                            JObject idObject = id as JObject;

                            var children = idObject.Children<JProperty>();



                            return new DynamicTuple(children.Select(p => ((JValue)p.Value).Value<object>()));

                        }).ToList();//.Distinct()

                        entityDependency.ReSetIds(newIds);
                    }
                }

            }

            return deserialized;
        }


        public static ValueDependencies WireUsingMessagePack(ValueDependencies input)
        {
            var deserialized = SerializationHelper.WireUsingMessagePack(input);

            if (!deserialized.EntityDependencies.IsNullOfEmpty())
            {
                for (int i = 0; i < deserialized.EntityDependencies.Count; i++)
                {
                    var entityDependency = deserialized.EntityDependencies[i];

                    if (entityDependency.Ids.Count == 0) continue; //No Ids, this should not happen

                    if (entityDependency.Ids[0].GetType().IsArray) //Composite key, turn it into ValueTuple or CompoiteData
                    {
                        var newIds = entityDependency.Ids.Select(id =>
                        {
                            dynamic[] idParts = (dynamic[])id;

                            return new DynamicTuple(idParts);

                        }).ToList();//.Distinct()

                        entityDependency.ReSetIds(newIds);
                    }
                }
            }

            return deserialized;
        }
    }
}
