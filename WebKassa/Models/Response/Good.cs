using System.Text.Json.Serialization;

namespace WebKassa.Models.Response
{
    public class Good
    {
        /// <summary>
        /// Внутренний id товара в АИС ПКС «Цифровые Кассы»
        /// </summary>
        [JsonPropertyName("goodId")]
        public int? Id { get; set; }

        /// <summary>
        /// Артикул
        /// </summary>
        [JsonPropertyName("vendorCode")]
        public string? Article { get; set; }

        /// <summary>
        /// Наименование товара
        /// </summary>
        [JsonPropertyName("goodName")]
        public string? Name { get; set; }

        /// <summary>
        /// Единица измерения 0 — штуки, 1 — килограммы, 2 —литры, 3 — метры
        /// </summary>
        [JsonPropertyName("unit")]
        public int? Unit { get; set; }

        /// <summary>
        /// Закупочная цена
        /// </summary>
        [JsonPropertyName("buyPrice")]
        public double? BuyPrice { get; set; }

        /// <summary>
        /// Цена продажи
        /// </summary>
        [JsonPropertyName("sellPrice")]
        public double? SellPrice { get; set; }

        /// <summary>
        /// Значение скидки
        /// </summary>
        [JsonPropertyName("discountValue")]
        public double? DiscountValue { get; set; }

        /// <summary>
        /// Количество (3 знака после запятой)
        /// </summary>
        [JsonPropertyName("quantity")]
        public double? Quantity { get; set; }

        /// <summary>
        /// Название категории (группы) товаров через слеш могут быть перечислены категории справочника для товара
        /// </summary>
        [JsonPropertyName("groupName")]
        public string? GroupName { get; set; }

        /// <summary>
        /// Тип товара 0 – товар, 1 – услуга, 2 – сертификат
        /// </summary>
        [JsonPropertyName("goodType")]
        public int? Type { get; set; }

        /// <summary>
        /// Основной штрих-код
        /// </summary>
        [JsonPropertyName("gtin")]
        public string? Barcode { get; set; }

        /// <summary>
        /// Дополнительные штрих-коды
        /// </summary>
        [JsonPropertyName("gtinAdditional")]
        public ICollection<string>? AdditionalBarcodes { get; set; }

        /// <summary>
        /// Открыт ли товар (услуга) 1 – открыт, 0 – закрыт(не продается)
        /// </summary>
        [JsonPropertyName("isOpen")]
        public int? IsOpen { get; set; }

        /// <summary>
        /// НДС ПРОДАЖИ 0 – 20%, 1 — 10%, 3 — Без НДС
        /// </summary>
        [JsonPropertyName("sellVat")]
        public int? SellVat { get; set; }

        /// <summary>
        /// Внутренний Id склада в АИС ПКС «Цифровые Кассы»
        /// </summary>
        [JsonPropertyName("stockId")]
        public int? StockId { get; set; }
    }
}