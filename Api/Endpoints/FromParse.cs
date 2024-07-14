using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Options;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Http.Json;

namespace Api.Endpoints
{
    public class FromParse<TValue>
    {
        public FromParse(TValue? model, ModelStateDictionary modelState)
        {
            ArgumentNullException.ThrowIfNull(modelState);

            Model = model;
            ModelState = modelState;
        }

        public TValue? Model { get; }

        public ModelStateDictionary ModelState { get; }

        public static implicit operator TValue?(FromParse<TValue> d) => d.Model;

        public void Deconstruct(out TValue? model, out ModelStateDictionary modelState)
        {
            model = Model;
            modelState = ModelState;
        }

        public static bool TryParse(string? value, IFormatProvider? provider, out FromParse<TValue>? result)
        {
            var jsonOptions = new JsonOptions { SerializerOptions = { DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull } };
            var model = JsonSerializer.Deserialize<TValue>(value, jsonOptions.SerializerOptions);

            var modelState = new ModelStateDictionary();

            result = new FromParse<TValue>(model, modelState);
            return true;
        }
    }
}
