// ***********************************************************************
// Assembly         : JGP.Core
// Author           : Joshua Gwynn-Palmer
// Created          : 08-27-2022
//
// Last Modified By : Joshua Gwynn-Palmer
// Last Modified On : 08-27-2022
// ***********************************************************************
// <copyright file="ActionReceiptConverter.cs" company="Joshua Gwynn-Palmer">
//     Joshua Gwynn-Palmer
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace JGP.Core.Serialization
{
    using System.Text.Json;
    using System.Text.Json.Serialization;
    using Services;

    /// <summary>
    ///     Class ActionReceiptConverter.
    ///     Implements the <see cref="JsonConverter{ActionReceipt}" />
    /// </summary>
    /// <seealso cref="JsonConverter{ActionReceipt}" />
    public class ActionReceiptConverter : JsonConverter<ActionReceipt>
    {
        /// <summary>
        ///     Reads and converts the JSON to type <see cref="ActionReceipt" />.
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <param name="typeToConvert">The type to convert.</param>
        /// <param name="options">An object that specifies serialization options to use.</param>
        /// <returns>The converted value.</returns>
        public override ActionReceipt? Read(ref Utf8JsonReader reader, Type typeToConvert,
            JsonSerializerOptions options)
        {
            var model = JsonSerializer.Deserialize<ActionReceiptModel>(ref reader, options);

            return model?.GetActionReceipt();
        }

        /// <summary>
        ///     Writes a specified value as JSON.
        /// </summary>
        /// <param name="writer">The writer to write to.</param>
        /// <param name="value">The value to convert to JSON.</param>
        /// <param name="options">An object that specifies serialization options to use.</param>
        public override void Write(Utf8JsonWriter writer, ActionReceipt value, JsonSerializerOptions options)
        {
            var model = new ActionReceiptModel(value);
            JsonSerializer.Serialize(writer, model, options);
        }
    }
}