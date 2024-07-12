using System.Text.Json;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Options;

namespace Api.Endpoints;

/// <summary>
/// Inspired by MinimalApis.Extensions.ModelBinderOfT
/// </summary>
/// <typeparam name="TValue"></typeparam>
public class FromBody<TValue>
{
    public FromBody(TValue? model, ModelStateDictionary modelState)
    {
        ArgumentNullException.ThrowIfNull(modelState);

        Model = model;
        ModelState = modelState;
    }

    public TValue? Model { get; }

    public ModelStateDictionary ModelState { get; }

    public static implicit operator TValue?(FromBody<TValue> d) => d.Model;

    public void Deconstruct(out TValue? model, out ModelStateDictionary modelState)
    {
        model = Model;
        modelState = ModelState;
    }

    public static async ValueTask<FromBody<TValue>> BindAsync(HttpContext context /*, ParameterInfo parameter*/)
    {
        var jsonOptions = context.RequestServices.GetRequiredService<IOptions<Microsoft.AspNetCore.Http.Json.JsonOptions>>().Value;
        var model = await JsonSerializer.DeserializeAsync<TValue>(context.Request.Body, jsonOptions.SerializerOptions);

        var modelState = new ModelStateDictionary();

        return new FromBody<TValue>(model, modelState);
    }
}
