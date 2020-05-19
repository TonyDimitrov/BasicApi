namespace CarsApi.Helpers
{
    using System;
    using System.ComponentModel;
    using System.Linq;
    using System.Reflection;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc.ModelBinding;

    public class ArrayModelBinder : IModelBinder
    {
        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            if (!bindingContext.ModelMetadata.IsEnumerableType)
            {
                bindingContext.Result = ModelBindingResult.Failed();

                return Task.CompletedTask;
            }

            var value = bindingContext.ValueProvider
                .GetValue(bindingContext.ModelName).ToString();

            if (value == null)
            {
                bindingContext.Result = ModelBindingResult.Success(null);

                return Task.CompletedTask;
            }

            var elementType = bindingContext.ModelType.GetTypeInfo().GenericTypeArguments[0];

            var converter = TypeDescriptor.GetConverter(elementType);

            var values = value.Split(',', StringSplitOptions.RemoveEmptyEntries)
                .Select(x => converter.ConvertFromString(x.Trim()))
                .ToArray();

            var arrayValues = Array.CreateInstance(elementType, values.Count());

            values.CopyTo(arrayValues, 0);

            bindingContext.Model = arrayValues;
            bindingContext.Result = ModelBindingResult.Success(bindingContext.Model);

            return Task.CompletedTask;
        }
    }
}
