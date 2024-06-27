using System.Text.Json.Serialization;

namespace WebKassa.Models.Request
{
    public class WarehouseRequestOptions
    {
        /// <summary>
        /// Товар есть в списке товаров (0 - скрыт, 1 - доступен)
        /// </summary>
        [JsonPropertyName("isOpen")]
        public int IsOpen { get; set; } = 1;

        /// <summary>
        /// Регистрационный номер программной кассы
        /// </summary>
        [JsonPropertyName("cashRegisterId")]
        public int? CashRegisterId { get; set; } = null;

        /// <summary>
        /// Внутренний id товара 
        /// </summary>
        [JsonPropertyName("goodId")]
        public int? GoodId { get; set; }

        /// <summary>
        /// Артикул
        /// </summary>
        [JsonPropertyName("vendorCode")]
        public string? VendorCode { get; set; }

        /// <summary>
        /// Основной штрихкод
        /// </summary>
        [JsonPropertyName("gtin")]
        public string? Gtin { get; set; }
    }
}